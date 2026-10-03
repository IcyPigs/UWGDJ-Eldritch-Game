using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField]
    public Line[] lines;
}

[System.Serializable]
public struct Line{
    public Character character;
    [TextArea(3,10)]
    public string statement;
    public float timeToRead;
}
