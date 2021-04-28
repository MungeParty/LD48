using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRoom : MonoBehaviour
{
    public float RoomIdleTimeout = 5f;
    public bool isAwake = true;

    public Rooms room;
    public ShipFloor floor;
    public ShipEnvironment environment;

    private List<RoomLight> roomLights = new List<RoomLight>();
    private float nextIdleCheck;

    void Start()
    {
        if (environment == null) environment = GetComponent<ShipEnvironment>();
        if (transform.parent != null) floor = transform.parent.GetComponent<ShipFloor>();
        roomLights.Clear();
        GetComponentsInChildren<RoomLight>(roomLights);
    }

    private void FixedUpdate()
    {
        if (Time.time > nextIdleCheck)
        {
            nextIdleCheck = Time.time + RoomIdleTimeout;
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
            light.intensity = 1.0f;
    }

    public void SleepRoom()
    {
        if (!isAwake) return;
        isAwake = false;

        foreach (RoomLight light in roomLights)
            light.intensity = 0.2f;
    }
}
