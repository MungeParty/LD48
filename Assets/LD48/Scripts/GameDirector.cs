using System;
using System.Collections.Generic;
using UnityEngine;

public enum CrewMembers
{
    Captain,
    Pilot,
    Doctor,
    Soldier,
    Engineer,
    Technician,
    Chef
}

public enum ProfficiencyTypes
{
    // education
    Medical,        // doc 3
    Technical,      // tech 3, eng 1,  1
    Mechanical,     // eng 3
    Navigation,     // pilot 2, cpt 1?
    History,        // random 
    Languages,      // random

    // experience
    Piloting,       // pilot 3
    Salvage,        // soldier 2, eng?, tech?
    Strategy,       // cpt 3, soldier
    Cooking,        // chef 3
    Bartering,      // soldier, random
    Cleaning,       // random
    Repair,         // random

    // traits
    Smart,          // tech, cpt, doc, mech
    Strong,         // soldier 3, random
    Lucky,          // chef, random
    Likeable,       // cpt, chef, random
    Intimidatingm,  // soldier 2, random
    Resourceful,    // eng, random
    Persistent,     // doc, random
    Curious,        // doc, random
}

public enum TaskPriority
{
    Busywork = 1,
    Menial = 2,
    Normal = 3,
    Rushed = 4,
    High = 5,
    Hurried = 6,
    Urgent = 7,
    Critical = 8,
    Emergency = 9,
    Catastrophe = 10,
}

[Serializable]
public struct ProfficiencyLevel
{
    public int level;
    public ProfficiencyTypes type;

    public ProfficiencyLevel(ProfficiencyTypes _type) : this(_type, 1)
    {
    }

    public ProfficiencyLevel(ProfficiencyTypes _type, int _level)
    {
        type = _type;
        level = _level;
    }
}


[Serializable]
public struct TaskCost
{
    public int baseCost;
    public List<ProfficiencyLevel> bonuses;

    public TaskCost(int _baseCost, List<ProfficiencyLevel> _bonuses)
    {
        baseCost = _baseCost;
        bonuses = _bonuses;
    }

    public int Calculate(List<ProfficiencyLevel> profficiencies)
    {
        // return if no bonuses available
        if (bonuses == null) return baseCost;
        // each bonus allows removal of [level] turns
        // from the base cost...
        int cost = baseCost;
        foreach (ProfficiencyLevel bonus in bonuses)
        {
            // count down from max bonus for this profficiency
            int bonusLevel = bonus.level;
            foreach (ProfficiencyLevel profficiency in profficiencies)
            {
                // if no bonus left for this profficiency, go to next
                if (bonusLevel <= 0 || cost <= 1) break;
                // skip type mismatch
                if (profficiency.type != bonus.type) continue;
                // get bonus amount from this skill
                int bonusDelta = Math.Min(profficiency.level, bonusLevel);
                // subtract from cost and bonus
                bonusLevel -= bonusDelta;
                cost = Mathf.Max(1, cost - bonusDelta);
            }
            if (cost <= 1) break;
        }
        // return cost
        return cost;
    }
}


[Serializable]
public struct CrewTask
{
    public string name;
    public TaskPriority priority;
    public TaskCost cost;

    public CrewTask(string _name, TaskPriority _priority, TaskCost _cost)
    {
        name = _name;
        priority = _priority;
        cost = _cost;
    }
}

public class TaskAssignment
{
    public Crew assignee;
    public CrewTask task;
    public int progress;
    public int cost;

    public TaskAssignment(CrewTask _task)
    {
        task = _task;
        progress = 0;
        cost = _task.cost.baseCost;
    }

    public void UpdateCost(Crew crew)
    {
        cost = task.cost.Calculate(crew.profficiencies);
    }
};

public enum GamePhases
{
    None,
    Loading,
    Warping,
    Planning,
    Executing
}

public class GameDirector : MonoBehaviour
{
    public GamePhases phase { get; private set; }
    public GamePhases nextPhase { get; private set; }

    public bool isLoaded;

    public delegate GamePhases UpdatePhase(GamePhases phase);

    Dictionary<GamePhases, UpdatePhase> phaseUpdates = new Dictionary<GamePhases, UpdatePhase>();

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        isLoaded = false;
        UpdateCurrentPhase();
    }

    public void RequestPhase(GamePhases _phase)
    {
        if (_phase == phase) return;
        nextPhase = phase;
    }

    private void UpdateCurrentPhase()
    {
        if (nextPhase == phase) nextPhase = GamePhases.None;
        switch (phase)
        {
            case GamePhases.None:
                nextPhase = UpdateNone(phase);
                break;
            case GamePhases.Loading:
                nextPhase = UpdateLoading(phase);
                break;
            case GamePhases.Planning:
                nextPhase = UpdatePlanning(phase);
                break;
            case GamePhases.Executing:
                nextPhase = UpdateExecuting(phase);
                break;
            case GamePhases.Warping:
                nextPhase = UpdateWarping(phase);
                break;
        }
        if (nextPhase == GamePhases.None) return;
        phase = nextPhase;
        nextPhase = GamePhases.None;
    }

    public GamePhases UpdateNone(GamePhases phase)
    {
        return GamePhases.Loading;
    }

    public GamePhases UpdateLoading(GamePhases phase)
    {
        // enter a new area, randomly choose from
        if (!isLoaded)
            return GamePhases.None;
        return GamePhases.Planning;
    }

    public GamePhases UpdatePlanning(GamePhases phase)
    {
        // do planning phase...
        // return phase;
        return GamePhases.Executing;
    }

    public GamePhases UpdateExecuting(GamePhases phase)
    {
        // crew executes tasks...
        // return phase;
        /// ready to warp?
        // return GamePhases.Warping;
        /// if not...
        return GamePhases.Planning;
    }

    public GamePhases UpdateWarping(GamePhases phase)
    {
        // increase warp depth
        // do cool warp
        // switch to loading phase to enter a new area
        return GamePhases.Loading;
    }
}
