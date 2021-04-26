using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TaskSO : ScriptableObject
{
    public string Name;
    public ProfficiencyLevel ProfficiencyLevel;
    public int ClearanceLevel;
    public Rooms taskRoom;
    public ShipSystems system;
}
