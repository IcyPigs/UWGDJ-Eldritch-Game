using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField]
    public Line lines;

    public struct Line{
        public Character character;
        public string statement;
        public float timeToRead;
    }
}
