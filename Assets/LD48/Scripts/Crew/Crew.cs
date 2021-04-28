using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Crew : MonoBehaviour
{
    public static List<Crew> GetRoomies(Rooms room)
    {
        List<Crew> results = new List<Crew>();
        foreach (Crew crew in instances)
        {
            if (crew == null || crew.room == null
                || crew.room.room != room) continue;
            results.Add(crew);
        }
        return results;
    }

    // crew profile
    public CrewProfileSO profile;
    public GameObject CrewUIPanel;

    // active profficiencies
    public List<ProficiencyLevel> profficiencies;

    // list of assignments
    public List<TaskAssignment> assignments = new List<TaskAssignment>();

    public static string Tag = "Crew";
    private static List<Crew> instances = new List<Crew>();

    // animation clip names
    private readonly string IdleStand = "crew_idle_stand";
    private readonly string IdleScratch = "crew_idle_scratch";
    private readonly string Walk = "crew_walk";
    private readonly string Hurt = "crew_hurt";
    private readonly string Dead = "crew_dead";
    private readonly string Float = "crew_float";
    private readonly string FloatKick = "crew_float_kick";
    private readonly string FloatDead = "crew_dead_zerog";
    private readonly string Land = "crew_land";
    private readonly string Work = "crew_working";

    #region settings
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private LayerMask wallMask;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float walkAccel;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float sprintAccel;

    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float floatAccel;

    [SerializeField]
    private float floatSpeed;

    [SerializeField]
    private float liftZeroG;

    [SerializeField]
    private float torqueZeroG;

    [SerializeField]
    private float maxVeloctiy;

    [SerializeField]
    private float stepDownTolerance;

    [SerializeField]
    private float walkAnimScale;

    [SerializeField]
    private float walkAnimScaleMin;

    [SerializeField]
    private float walkAnimScaleMax;

    [SerializeField]
    private float groundFriction;
    #endregion

    #region control
    public float moveDirection;

    public bool wantsToSprint = false;

    public bool isWorkign = false;

    public bool isHurt = false;

    public bool isDead = false;

    public bool isLanding = true;
    #endregion

    #region internal
    private Vector2 velocity;

    private Vector2 position;

    private bool hasGravity;

    private bool hadGravity;

    private bool isGrounded;

    private bool wasGrounded;

    private float groundY;

    private Vector2 groundNormal;

    private Vector2 groundDirection;

    public ShipRoom room;

    private Rigidbody2D body;

    private Animation anim;

    private SpriteRenderer sprite;
    #endregion

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        sprite = GetComponent<SpriteRenderer>();

        // remove alpha from crew color
        Color profileColor = profile.CrewColor;
        profileColor.a = 1.0f;
        sprite.material.color = profileColor;

        // roll profficiencies wat wat
        RollProfficiencies();
        instances.Add(this);
        SetUI();
        CrewUIPanel.GetComponent<Button>().onClick.AddListener(OnClickPanel);
    }

    private void OnDestroy()
    {
        instances.Remove(this);
    }

    private void FixedUpdate()
    {
        // env update
        UpdateEnvironment();

        // void move if hurt or dead
        if (isHurt || isDead || isLanding || isWorkign)
            moveDirection = 0f;

        // movement
        if (hasGravity)
            UpdateWithGravity();
        else
            UpdateZeroG();

        // update animation
        UpdateAnimation();

        // update historical fields
        hadGravity = hasGravity;
        wasGrounded = isGrounded;
    }

    private void UpdateEnvironment()
    {
        if (room == null)
            DoRoomCheck();

        gravity = room.environment.gravityForce;
        hasGravity = gravity > Mathf.Epsilon;
    }

    private void UpdateWithGravity()
    {
        // slam constraints
        position = transform.position;
        if (!hadGravity) body.velocity = Vector2.zero;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.isKinematic = true;
        RaycastHit2D hit;

        // apply gravity acceleration
        velocity.y -= gravity * Time.deltaTime;

        // ground check
        // recovering from zerog
        Vector2 up2d = new Vector2(transform.up.x, transform.up.y);
        Vector2 center = position + up2d * 3.5f;
        hit = Physics2D.CircleCast(
            center,
            0.5f,
            Vector2.down,
            3f + Mathf.Max(0f, -velocity.y * Time.deltaTime) + stepDownTolerance * 2f,
            groundMask
        );

        // set grounded
        isGrounded = hit.collider != null
            && center.y - 3.5 + (velocity.y * Time.deltaTime) < hit.point.y + stepDownTolerance;
        groundY = isGrounded ? hit.point.y : float.MinValue;
        groundNormal = isGrounded ? hit.normal : groundNormal;

        // apply lateral acceleration
        float accelX = isGrounded ? (wantsToSprint ? sprintAccel : walkAccel) : floatAccel;
        float maxVelX = isGrounded ? (wantsToSprint ? sprintSpeed : walkSpeed) : floatSpeed;
        bool isMoving = Mathf.Abs(moveDirection) > 0f;
        if (isMoving && !isLanding)
        {
            // which direction along hit normal
            groundDirection = !isGrounded
                ? new Vector2(Mathf.Sign(velocity.x), 0f)
                : (moveDirection < 0
                    ? new Vector2(-groundNormal.y, groundNormal.x)
                    : new Vector2(groundNormal.y, -groundNormal.x));

            velocity += groundDirection * accelX * Time.deltaTime;
            if (velocity.x > maxVelX) velocity.x = maxVelX;
            else if (velocity.x < -maxVelX) velocity.x = -maxVelX;
        }
        else
        {
            velocity.x *= 1f - groundFriction;
        }

        // wall check
        if (Mathf.Abs(velocity.x) > 0f)
        {
            // start ray at knee
            float distance = velocity.magnitude + 3f;
            hit = Physics2D.CircleCast(
                center,
                0.5f,
                groundDirection,
                distance,
                wallMask
            );
            if (hit.collider != null && Mathf.Abs(hit.point.x - position.x) < 1.5f)
            {
                float dirSign = Mathf.Sign(hit.point.x - position.x);
                float velSign = Mathf.Sign(groundDirection.x);
                float moveSign = Mathf.Sign(moveDirection);
                if (dirSign == velSign)
                {
                    if (moveSign == velSign)
                        moveDirection *= -1;
                    position.x = hit.point.x + ((velocity.x > 0) ? -1.45f : 1.45f);
                    velocity.x = 0;
                }
            }
        }

        // update falling
        if (isGrounded)
        {
            position.y = groundY;
            velocity.y = 0f;
            transform.up = Vector3.up;
        }

        // clamp overall vel
        if (velocity.sqrMagnitude > maxVeloctiy * maxVeloctiy)
            velocity = velocity.normalized * maxVeloctiy;

        // apply velocity
        position.x += velocity.x * Time.deltaTime;
        position.y = isGrounded ? groundY : position.y + velocity.y * Time.deltaTime;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    private void UpdateZeroG()
    {
        // reset landing flag
        isLanding = true;

        // set body params
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.isKinematic = false;

        // update local vel from body
        velocity = body.velocity;
        position = transform.position;

        // todo...
        if (hadGravity)
        {
            if (wasGrounded)
            {
                body.AddForce(
                    Vector2.up * liftZeroG * Random.Range(0.25f, 1f)
                    + Vector2.right * liftZeroG * Random.Range(-1f, 1f));
            }
            body.AddTorque(Random.Range(torqueZeroG * 0.1f, torqueZeroG * 0.1f));
            isGrounded = false;
        }
    }

    private void UpdateAnimation()
    {
        // get clip name
        string clipName = "";
        if (anim.clip != null) clipName = anim.clip.name;

        AnimationState state = anim[clipName];

        if (Mathf.Abs(moveDirection) > Mathf.Epsilon)
            sprite.flipX = moveDirection < 0f;

        // get desired clip
        string targetClip = clipName;
        if (isDead)
        {
            // play dead
            if (hasGravity && isGrounded)
            {
                if (clipName != FloatDead)
                {
                    if (clipName != Dead && clipName != Hurt)
                        targetClip = Hurt;
                    else if (!state.enabled)
                        targetClip = Dead;
                }
                else
                {
                    targetClip = Dead;
                }
            }
            else
            {
                targetClip = FloatDead;
            }
        }
        else if (isHurt && isGrounded)
        {
            // play hurt
            targetClip = Hurt;
        }
        else if (isWorkign)
        {
            // play work loop
            targetClip = Work;
        }
        else if (!hasGravity)
        {
            // play float
            if (clipName != FloatKick || state.normalizedTime >= 1.0f)
                targetClip = Float;
        }
        else
        {

            // check run direction
            if (!isGrounded)
            {
                targetClip = Float;
            }
            else if (Mathf.Abs(moveDirection) < Mathf.Epsilon)
            {
                if (isLanding)
                {
                    // play land
                    if (clipName != Land)
                    {
                        targetClip = Land;
                        float landSpeed = wantsToSprint ? 1f : 0.25f;
                        anim[Land].speed = landSpeed + 0.5f + landSpeed * Random.value;
                    }
                    else if (!state.enabled || state.normalizedTime >= 1f || state.normalizedTime <= 0f)
                    {
                        isLanding = false;
                    }
                }
                else if (clipName != IdleScratch || state.normalizedTime >= 1.0f)
                {
                    targetClip = IdleStand;
                }
            }
            else
            {
                // walking
                targetClip = Walk;
                anim[Walk].speed =
                    Mathf.Clamp(
                        velocity.magnitude * walkAnimScale,
                        walkAnimScaleMin,
                        walkAnimScaleMax);
            }
        }

        // transition if needed
        if (clipName != targetClip)
        {
            anim.clip = anim[targetClip].clip;
            anim.Play(targetClip);
        }
    }

    public Crew GetNearest()
    {
        Crew nearest = null;
        float nearDistSq = float.MaxValue;
        foreach (Crew crew in instances)
        {
            if (crew == this) continue;

            float distSq = (crew.transform.position - transform.position).sqrMagnitude;
            if (nearest == null || distSq < nearDistSq)
            {
                nearest = crew;
                nearDistSq = distSq;
            }
        }
        return nearest;
    }

    public void DoScratch()
    {
        if (isGrounded && !isDead && !isHurt)
        {
            moveDirection = 0f;
            anim[IdleScratch].speed = wantsToSprint ? 2f : 1f;
            anim.clip = anim[IdleScratch].clip;
            anim.Play(IdleScratch);
        }
    }

    public void DoRoomCheck()
    {
        if (transform.parent != null)
            room = transform.parent.GetComponent<ShipRoom>();
    }

    public void NoticeAssignment(TaskAssignment assignment)
    {
        //if (assignments)
        //int sortIndex = assignments.FindIndex(0, other => other.task.priority < assignment.task.priority);
        //if (sortIndex > -1) assignments.Insert(sortIndex, assignment);
    }

    public void DoHurt(Vector2 force)
    {
        if (isHurt) return;
        isHurt = true;
        isGrounded = false;
        isLanding = true;
        transform.position += new Vector3(force.x, force.y);
        velocity = force.normalized * floatSpeed;
    }

    public void RollProfficiencies()
    {
        if (!profile) return;
        List<ProficiencyLevel> result = new List<ProficiencyLevel>();
        foreach (ProfileProficiencies roll in profile.proficiencies)
        {
            int level = 0;
            if (!roll.isRandom)
                level = Mathf.Max(1, roll.level);
            else
                level = Mathf.Max(0, Random.Range(roll.min, roll.max));

            if (level > 0)
            {
                int sortIndex = result.FindIndex(0, other => other.level <= level);
                if (sortIndex > -1) result.Insert(sortIndex, new ProficiencyLevel(roll.type, level));
            }
        }
        profficiencies = result;
    }

    private void SetUI()
    {
        TextMeshProUGUI name = CrewUIPanel.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.name == "Name");
        name.text = profile.Name;
        name.color = profile.CrewColor;
    }

    private void OnClickPanel()
    {
        Debug.Log(profile.Name);
        GameObject detailedList = CrewUIPanel.transform.Find("DetailedTaskLists").gameObject;
        detailedList.SetActive(!detailedList.activeSelf);
    }
}
