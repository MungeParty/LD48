using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipFloor : MonoBehaviour
{
    public int floor;
    public Ship ship;
    public ShipEnvironment environment;
    //public List<ShipRoom> rooms = new List<ShipRoom>();

    private void Start()
    {
        environment = GetComponent<ShipEnvironment>();
        if (transform.parent != null)
            ship = GetComponentInParent<Ship>();
        //ship?.AddFloor(this);
    }


    //public void AddRoom(ShipRoom room)
    //{
    //    if (!rooms.Contains(room))
    //        rooms.Add(room);
    //}

    //public void RemoveRoom(ShipRoom room)
    //{
    //    if (rooms.Contains(room))
    //        rooms.Remove(room);
    //}
}
