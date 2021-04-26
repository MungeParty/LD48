using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipEnvironment))]
public class ShipRoom : MonoBehaviour
{
    public Rooms room;
    public ShipFloor floor;
    public ShipEnvironment environment;

    public List<Objective> objectives = new List<Objective>();

    void Start()
    {
        environment = GetComponent<ShipEnvironment>();
        if (transform.parent != null)
            floor = GetComponentInParent<ShipFloor>();
    }
}
