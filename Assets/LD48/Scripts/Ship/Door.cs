using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private static RaycastHit2D[] doorHits = new RaycastHit2D[6];

    //public ShipFloor floor;
    public ShipRoom left;
    public ShipRoom right;
    public Crew victim;

    [SerializeField]
    private Collider2D collider2d;

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
        //floor = left.floor;
        doorStartY = sprite.transform.position.y;
        targetY = doorStartY + doorOffsetMax;
    }

    public void DoorEntered(bool enter = false)
    {
        //if (floor == null ||  floor.environment.isPowered && collider2d.enabled && sprite.transform.position.y == doorStartY)
        //{
        //    if (enter || Random.value * 4f < floor.environment.sensorsPercent)
        //        closeTime = Time.time + enterOpenTime;
        //}
    }

    private void FixedUpdate()
    {
        //if (!floor.environment.isPowered) return;

        //Vector3 pos = sprite.transform.position;
        //if (Time.time < closeTime)
        //{
        //    float remaining = targetY - pos.y;
        //    float delta = Mathf.Sign(remaining) * doorSpeed * doorSpeedCurve.Evaluate(floor.environment.powerPercent) * Time.deltaTime;
        //    if (Mathf.Abs(remaining) < Mathf.Abs(delta))
        //    {
        //        pos.y = targetY;
        //        sprite.transform.position = pos;
        //        collider2d.enabled = false;
        //    }
        //    else
        //    {
        //        pos.y += delta;
        //        sprite.transform.position = pos;
        //        float offsetY = pos.y - doorStartY;
        //        if (offsetY > 8)
        //            collider2d.enabled = false;
        //    }
        //}
        //else if (victim == null)
        //{
        //    float remaining = doorStartY -pos.y;
        //    float delta = Mathf.Sign(remaining) * doorSpeed * doorSpeedCurve.Evaluate(floor.environment.powerPercent) * Time.deltaTime;
        //    if (Mathf.Abs(remaining) < Mathf.Abs(delta))
        //    {
        //        pos.y = doorStartY;
        //        sprite.transform.position = pos;
        //        EnableDoor();
        //    }
        //    else
        //    {
        //        pos.y += delta;
        //        sprite.transform.position = pos;
        //        float offsetY = pos.y - doorStartY;
        //        if (offsetY < 8)
        //            EnableDoor();
        //    }
        //}
        //else
        //{
        //    if (!victim.isHurt) victim = null;
        //    closeTime = Time.time + victimHealResetTime;
        //}
    }

    private void EnableDoor()
    {
        //    if (collider2d.enabled) return;
        //    collider2d.enabled = true;
        //    int count = collider2d.Cast(Vector2.zero, doorHits);
        //    for (int i = 0; i < count; i++)
        //    {
        //        RaycastHit2D hit = doorHits[i];
        //        Crew crew = hit.collider.GetComponent<Crew>();
        //        if (crew != null)
        //        {
        //            victim = crew;
        //            Vector3 dir = victim.transform.position - transform.position;
        //            victim.DoHurt(new Vector2(Mathf.Sign(dir.x) * 2f, 1f));
        //        }
        //    }
    }
}
