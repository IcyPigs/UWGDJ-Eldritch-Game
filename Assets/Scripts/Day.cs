using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Scriptable Objects/Day")]
public class Day : ScriptableObject
{
    public Character[] characters;
    public Dialogue dialogue;
    [Space]
    [Header ("Day Settings")]
    public float leniency;
    public float patienceDecayRate;
}
