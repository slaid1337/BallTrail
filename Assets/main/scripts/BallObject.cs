using UnityEngine;

[CreateAssetMenu(fileName = "BallObject", menuName = "Balls/BallObject")]
public class BallObject : ScriptableObject
{
    public Sprite BallSprite;
    public bool IsTaken;
    public int BallIndex;
    public int BallCost;
}
