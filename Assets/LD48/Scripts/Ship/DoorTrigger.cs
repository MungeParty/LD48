using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private static RaycastHit2D[] doorHits = new RaycastHit2D[6];

    Door door;
    Collider2D trigger;

    private void Start()
    {
        if (transform.parent != null)
            door = transform.parent.GetComponent<Door>();
        if (transform.parent != null)
            trigger = transform.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (door == null) return;
        Crew crew = collision.gameObject.GetComponent<Crew>();
        if (crew != null) door.DoorEntered(crew);
    }

    public bool PlayerCheck(RaycastHit2D[] doorHits)
    {
        int count = trigger.Cast(Vector2.zero, door.filter, doorHits);
        return count > 0;
    }
}
