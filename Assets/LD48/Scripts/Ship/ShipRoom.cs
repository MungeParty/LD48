using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRoom : MonoBehaviour
{
    public Rooms room;
    public ShipFloor floor;
    public ShipEnvironment environment;

    void Start()
    {
        if (environment == null) environment = GetComponent<ShipEnvironment>();
        if (transform.parent != null) floor = transform.parent.GetComponent<ShipFloor>();
    }
}
