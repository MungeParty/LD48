using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private static RaycastHit2D[] doorHits = new RaycastHit2D[6];
    private static List<Crew> _removals = new List<Crew>();

    private List<Crew> occupants = new List<Crew>();

    public ShipFloor floor;
    public ShipRoom left;
    public ShipRoom right;
    public Crew victim;
    public LayerMask triggerLayer;

    [HideInInspector]
    public ContactFilter2D filter = new ContactFilter2D();

    [SerializeField]
    private Collider2D collider2d;

    [SerializeField]
    private DoorTrigger trigger;

    [SerializeField]
    private GameObject sprite;

    [SerializeField]
    private float enterOpenTime = 5f;


    [SerializeField]
    private float victimHealResetTime = 5f;


    [SerializeField]
    private float doorSpeed = 10f;

    [SerializeField]
    private float doorOffsetMax = 10f;

    [SerializeField]
    private AnimationCurve doorSpeedCurve = new AnimationCurve();

    private float closeTime;
    private float doorStartY;
    private float targetY;

    private void Start()
    {
        filter.useLayerMask = true;
        filter.layerMask = triggerLayer;
        if (floor == null && transform.parent != null)
            floor = transform.parent.GetComponent<ShipFloor>();
        if (trigger == null)
            trigger = GetComponentInChildren<DoorTrigger>();

        doorStartY = sprite.transform.position.y;
        targetY = doorStartY + doorOffsetMax;
    }

    public void DoorEntered(Crew crew)
    {
        if (!occupants.Contains(crew)) occupants.Add(crew);
        if (floor == null) return;
        if (floor.environment.isPowered && collider2d.enabled && sprite.transform.position.y == doorStartY)
            closeTime = Time.time + enterOpenTime;
    }

    private void FixedUpdate()
    {
        UpdateOccupants();
        UpdateDoorState();
    }

    private void UpdateOccupants()
    {
        _removals.Clear();
        foreach (Crew crew in occupants)
        {
            float offset = crew.transform.position.x - transform.position.x;
            crew.ChangeRoom(offset < -0f ? left : right);
            if (Mathf.Abs(offset) > 16f)
                _removals.Add(crew);
        }
        foreach (Crew crew in _removals)
        {
            occupants.Remove(crew);
        }
        _removals.Clear();
    }

    private void UpdateDoorState()
    {
        if (floor == null || !floor.environment.isPowered) return;

        Vector3 pos = sprite.transform.position;
        if (Time.time < closeTime)
        {
            float remaining = targetY - pos.y;
            float delta = Mathf.Sign(remaining) * doorSpeed * doorSpeedCurve.Evaluate(floor.environment.powerPercent) * Time.deltaTime;
            if (Mathf.Abs(remaining) < Mathf.Abs(delta))
            {
                pos.y = targetY;
                sprite.transform.position = pos;
                DisableDoor();
            }
            else
            {
                pos.y += delta;
                sprite.transform.position = pos;
                float offsetY = pos.y - doorStartY;
                if (offsetY > 8)
                    DisableDoor();
            }
        }
        else if (victim == null)
        {
            if (collider2d.enabled) return;
            if (trigger.PlayerCheck(doorHits))
            {
                closeTime = Time.time + enterOpenTime;
                return;
            }

            float remaining = doorStartY - pos.y;
            float delta = Mathf.Sign(remaining) * doorSpeed * doorSpeedCurve.Evaluate(floor.environment.powerPercent) * Time.deltaTime;
            if (Mathf.Abs(remaining) < Mathf.Abs(delta))
            {
                pos.y = doorStartY;
                sprite.transform.position = pos;
                EnableDoor();
            }
            else
            {
                pos.y += delta;
                sprite.transform.position = pos;
                float offsetY = pos.y - doorStartY;
                if (offsetY < 8)
                    DisableDoor();
            }
        }
        else
        {
            if (!victim.isHurt) victim = null;
            closeTime = Time.time + victimHealResetTime;
        }
    }

    private void EnableDoor()
    {
        collider2d.enabled = true;
        int count = collider2d.Cast(Vector2.zero, filter, doorHits);
        for (int i = 0; i < count; i++)
        {
            RaycastHit2D hit = doorHits[i];
            Crew crew = hit.collider.GetComponent<Crew>();
            if (crew != null)
            {
                victim = crew;
                Vector3 dir = victim.transform.position - transform.position;
                victim.DoHurt(new Vector2(Mathf.Sign(dir.x) * 2f, 1f));
            }
        }
    }

    private void DisableDoor()
    {
        if (!collider2d.enabled) return;
        collider2d.enabled = false;
    }
}
