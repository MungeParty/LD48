using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFloor : MonoBehaviour
{
    public int floor;
    public Ship ship;
    public ShipEnvironment environment;

    private void Start()
    {
        if (environment == null) environment = GetComponent<ShipEnvironment>();
        if (transform.parent != null) ship = transform.parent.GetComponent<Ship>();
    }
}
