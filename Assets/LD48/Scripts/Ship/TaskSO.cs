using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TaskSO : ScriptableObject
{
    public string Name;
    public ProficiencyLevel ProficiencyLevel;
    public int ClearanceLevel;
    public TaskPriority Priority;
    public Rooms taskRoom;
    public ShipSystems system;
}
