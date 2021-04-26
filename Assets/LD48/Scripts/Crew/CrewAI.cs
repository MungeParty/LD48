using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    None = 0,
    Wander = 1,
    Traverse = 2,
    Occupy = 3
}

public class CrewAI : MonoBehaviour
{
    public Crew crew;
    public Objective objective;

    // settings
    public float wanderScratchPercent;
    public float wanderCheckMin;
    public float wanderCheckMax;
    public float minWanderStopDistance;

    private AIState state = AIState.None;

    float nextActionTime;

    private void Start()
    {
        crew = GetComponent<Crew>();
        CheckState();
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    public void CheckState()
    {
        if (state == AIState.None)
            state = AIState.Wander;
    }

    public void SetState(AIState newState)
    {
        if (state == newState) return;
        state = newState;
    }

    void UpdateState()
    {
        switch (state)
        {
            case AIState.Wander:
                Wander();
                break;
            case AIState.Traverse:
                Traverse();
                break;
            case AIState.Occupy:
                Occupy();
                break;
        }
    }

    //void NoticeTasks()
    //{
    //    List<Objective> nearby = Objective.GetNearby(transform.position, 20, crew.room.room, crew.room.floor.floor);
    //    Objective winner;
    //    foreach (Objective space in nearby)
    //    {
    //        if (space.assignment == null || !space.isOccupied)
    //        {

    //        }
    //    }
    //}

    public void Wander()
    {
        if (Time.time > nextActionTime)
        {
            Crew nearest = crew.GetNearest();
            List<Hazard> hazards = Hazard.GetNearby(transform.position, 4f);
            float nearestDistSq = nearest == null ? Mathf.Infinity : (nearest.transform.position - crew.transform.position).sqrMagnitude;
            float waitScale = crew.wantsToSprint ? 0.25f : 1f;
            if (Mathf.Abs(crew.moveDirection) > Mathf.Epsilon)
            {
                if (hazards.Count > 0 ||
                    (nearest != null
                    && nearest.moveDirection != crew.moveDirection
                    && nearestDistSq < minWanderStopDistance * minWanderStopDistance))
                {
                    nextActionTime = Time.time + wanderCheckMin * waitScale;
                    return;
                }
                crew.moveDirection = 0f;
            }
            else
            {
                float choice = Random.Range(0f, 1f);
                if (choice < wanderScratchPercent)
                    crew.DoScratch();
                else
                    crew.moveDirection = ((int)(choice * 10f) % 2 == 0) ? -1f : 1f;
            }
            nextActionTime = Time.time + Random.Range(wanderCheckMin, wanderCheckMax) * waitScale;
        }
    }

    public void Traverse()
    {
    }

    public void Occupy()
    {
    }
}
