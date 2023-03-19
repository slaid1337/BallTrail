using UnityEngine;

[CreateAssetMenu(fileName = "ActiveBallObject", menuName = "Balls/ActiveBallObject")]
public class ActiveBallObject : ScriptableObject
{
    public BallObject _activeBallObject;
}
