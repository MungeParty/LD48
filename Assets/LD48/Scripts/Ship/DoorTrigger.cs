using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    Door door;

    private void Start()
    {
        if (transform.parent != null)
            door = transform.parent.GetComponent<Door>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (door == null) return;
        if (collision.gameObject.tag == Crew.Tag)
            door.DoorEntered(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (door == null) return;
        if (collision.gameObject.tag == Crew.Tag)
            door.DoorEntered();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Crew crew = collision.gameObject.GetComponent<Crew>();
        if (crew != null)
        {
            ShipRoom room = crew.transform.position.x < transform.position.x
                ? door.left
                : door.right;
            if (room == null && crew.room != null) return;
            crew.room = room;
            crew.transform.SetParent(crew.room.transform, true);
        }
    }
}
