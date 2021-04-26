using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private static List<Hazard> instances = new List<Hazard>();

    private void Start()
    {
        instances.Add(this);
    }

    private void OnDestroy()
    {
        instances.Remove(this);
    }

    public static List<Hazard> GetNearby(Vector3 pos, float radius = 5f)
    {
        List<Hazard> result = new List<Hazard>();
        float radiusSq = radius * radius;
        foreach (Hazard hazard in instances)
        {
            float distSq = (hazard.transform.position - pos).sqrMagnitude;
            if (distSq <= radiusSq) result.Add(hazard);
        }
        return result;
    }
}
