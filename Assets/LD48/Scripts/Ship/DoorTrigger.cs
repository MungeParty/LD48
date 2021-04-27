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

    //private void OnTrigger(Collider other)
    //{
    //    Crew crew = other.gameObject.GetComponent<Crew>();
    //    if (crew != null)
    //    {
    //        crew.room = crew.transform.position.x < transform.position.x
    //            ? door.left 
    //            : door.right;
    //        crew.transform.SetParent(crew.room.transform, true);
    //    }
    //}
}
