using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRoom : MonoBehaviour
{
    public static readonly float RoomIdleTimeout = 2f;

    public Rooms room;
    public ShipFloor floor;
    public ShipEnvironment environment;

    private List<RoomLight> roomLights = new List<RoomLight>();
    private float nextIdleCheck;
    private bool isAwake = true;
    private float liveIntensity;

    void Start()
    {
        if (environment == null) environment = GetComponent<ShipEnvironment>();
        if (transform.parent != null) floor = transform.parent.GetComponent<ShipFloor>();
    }

    private void FixedUpdate()
    {
        if (Time.time > nextIdleCheck)
        {
            if (Crew.GetRoomies(room).Count >= 1)
            {
                WakeRoom();
            }
            else 
            {
                SleepRoom();
            }
        }
    }

    public void WakeRoom()
    {
        if (isAwake) return;
        isAwake = true;

        if (Random.value > environment.sensorsPercent * 4f)
            return;

        foreach (RoomLight light in roomLights)
            light.intensity = liveIntensity;
    }

    public void SleepRoom()
    {
        if (!isAwake) return;
        isAwake = false;
        foreach (RoomLight light in roomLights)
        {
            liveIntensity = 0.2f;
        }
    }
}
