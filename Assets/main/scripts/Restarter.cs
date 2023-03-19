using System.Collections;
using Coffee.UIEffects;
using UnityEngine;

public class Restarter : Singletone<Restarter>
{
    [SerializeField] private Ball[] _balls;
    [SerializeField] private RectTransform[] _spawnPoints;
    [SerializeField] private GameObject _lineParent;
    [SerializeField] private GameObject _startBtn;
    [SerializeField] private GameObject _restartBtn;

    public void Restart()
    {
        for (int i = 0; i < _lineParent.transform.childCount; i++)
        {
            Destroy(_lineParent.transform.GetChild(i).gameObject);
        }

        _startBtn.SetActive(true);
        _restartBtn.SetActive(false);

        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        foreach (var ball in _balls)
        {
            var rb = ball.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.simulated = false;
            var uid = ball.GetComponent<UIDissolve>();
            uid.Reverse = false;
            uid.Play();
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < _balls.Length; i++)
        {
            _balls[i].GetComponent<RectTransform>().anchoredPosition = _spawnPoints[i].anchoredPosition;
            var uid = _balls[i].GetComponent<UIDissolve>();
            uid.Reverse = true;
            uid.Play();
        }
    }
}
