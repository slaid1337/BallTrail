using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Ball : MonoBehaviour {

    private GameScoreTracker _tracker;
    [SerializeField] private float _jumpForce;
    private Restarter _restarter;
    [SerializeField] private ParticleSystem _dieEffect;
    private AudioSource _dieSound;
    [SerializeField] private ParticleSystem _winEffect;
    private AudioSource _winSound;

    private void Start()
    {
        _tracker = GameScoreTracker.Instance;
        _restarter = Restarter.Instance;

        try
        {
            GetComponent<Image>().sprite = ActiveBall.Instance.ActiveBallObject.BallSprite;
        }
        catch
        {

        }

        if (_dieEffect != null)
        {
            _dieSound = _dieEffect.GetComponent<AudioSource>();
        }
        if (_winEffect != null)
        {
            _winSound = _winEffect.GetComponent<AudioSource>();
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Target")
        {
            _tracker.Score++;
            PlayWinEffect();
        }
        else if (collider.tag == "jumper")
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        else if (collider.tag == "Spike")
        {
            _restarter.Restart();
            PlayDieEffect();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Target")
        {
            _tracker.Score--;
        }
    }

    public void PlayWinEffect()
    {
        if (_winEffect != null) _winEffect.Play();
        if (_winSound != null) _winSound.Play();
    }

    public void PlayDieEffect()
    {
        if (_dieEffect != null) _dieEffect.Play();
        if (_dieSound != null) _dieSound.Play();
    }
}
