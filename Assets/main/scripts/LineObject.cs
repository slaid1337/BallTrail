using UnityEngine;

[CreateAssetMenu(fileName = "LineObject", menuName = "Balls/LineObject")]
public class LineObject : ScriptableObject
{
    public Gradient gradient;
    public bool IsTaken;
    public int LineIndex;
    public int LineCost;
}
