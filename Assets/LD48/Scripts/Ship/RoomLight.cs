using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLight : MonoBehaviour
{
    private List<Light> lights;
    private float baseIntensity;
    private float currentIntenity;
    private float targetIntenstiy;

    private float actualIntensity;

    private ShipRoom room;

    public float intensity
    {
        get { return targetIntenstiy; }
        set { targetIntenstiy = value; }
    }

    private void Start()
    {
        lights = new List<Light>(GetComponentsInChildren<Light>());
        baseIntensity = lights.Count > 0 ? lights[0].intensity : 1f;
        if (room == null && transform.parent != null)
            room = transform.parent.GetComponentInParent<ShipRoom>();
        currentIntenity = 0f;
        targetIntenstiy = 1f;
    }

    private void FixedUpdate()
    {
        if (room == null && transform.parent != null)
            room = transform.parent.GetComponentInParent<ShipRoom>();

        if (room == null) return;
        if (!room.environment.isPowered)
        {
            actualIntensity = 0f;
        }
        else
        {
            if (currentIntenity != targetIntenstiy)
            {
                float diff = targetIntenstiy - currentIntenity;
                float rate = 100f * Time.deltaTime;
                if ((diff > 0 && rate > diff) || (diff < 0 && rate < diff))
                    currentIntenity = targetIntenstiy;
                else
                    currentIntenity += diff;
            }
            else
            {
                if (Random.value > room.environment.powerPercent * 1.33f)
                    currentIntenity *= 0.6f;
            }

            actualIntensity = baseIntensity * currentIntenity;
        }

        foreach (Light light in lights)
        {
            light.intensity = actualIntensity;
        }
    }
}
