using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProfileProficiencies
{
    public ProfficiencyTypes type;
    public int level;
    public bool isRandom;
    public int min;
    public int max;
}

[CreateAssetMenu()]
public class CrewProfileSO : ScriptableObject
{
    public string Name;
    public Color CrewColor;
    public string Bio;
    public CrewMembers Job;
    public List<ProfileProficiencies> proficiencies;
}
