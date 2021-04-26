using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private static List<Objective> instances = new List<Objective>();

    public static List<Objective> GetNearby(Vector3 pos, float radius = 20f, Rooms room = Rooms.AnyRoom, int floor = -1)
    {
        List<Objective> result = new List<Objective>();
        float radiusSq = radius * radius;
        foreach (Objective objective in instances)
        {
            if (room != Rooms.AnyRoom && objective.room.room != room) continue;
            if (floor >= 0 && objective.room.floor.floor!= floor) continue;
            float distSq = (objective.transform.position - pos).sqrMagnitude;
            if (distSq <= radiusSq) result.Add(objective);
        }
        return result;
    }

    public ShipRoom room;


    private void Start()
    {
        instances.Add(this);
        //room = GetComponentInParent<ShipRoom>();
        //room?.AddObjective(this);
    }

    private void OnDestroy()
    {
        instances.Remove(this);
        //room?.RemoveObjective(this);
        //room = null;
    }

    [SerializeField]
    private Color enabledColor;

    [SerializeField]
    private Color disabledColor;

    [SerializeField]
    private SpriteRenderer stateSprite;

    public Crew occupant { get; private set; }


    public TaskAssignment assignment;

    public bool isOccupied
    {
        get
        {
            return occupant != null;
        }
    }

    public bool Occupy(Crew crew)
    {
        // report succes if already set
        if (occupant == crew) return true;

        // report fail if not available
        if (occupant != null) return false;

        // assign and report success
        occupant = crew;

        UpdateColor();
        return true;
    }

    public void Leave(Crew crew = null)
    {
        // if assigning null or current occupant
        if (crew == null || occupant == crew)
        {
            occupant = null;
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        if (stateSprite != null)
            stateSprite.material.color = assignment != null ? disabledColor : enabledColor;
    }
}

