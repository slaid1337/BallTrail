using UnityEngine;

public class GameSimulation : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] _balls;

    public Rigidbody2D[] Balls
    {
        get
        {
            return _balls;
        }
    }

    public void SimulateBalls()
    {
        foreach (var ball in _balls)
        {
            var rb = ball.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.simulated = true;
        }
    }
}
