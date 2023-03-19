using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MovePanel : MonoBehaviour
{
    private MainCanvas _mainCanvas;
    [SerializeField] private ParticleSystem _particle;

    public void Open()
    {
        _mainCanvas = MainCanvas.Instance;
        float startPlace = -(_mainCanvas.GetComponent<RectTransform>().sizeDelta.y / 2) - (GetComponent<RectTransform>().sizeDelta.y / 2);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startPlace);
        GetComponent<RectTransform>().DOLocalMoveY(0, 0.5f);

        if (_particle != null)
        {
            StartCoroutine(PlayParticles());
        }
    }

    private IEnumerator PlayParticles()
    {
        yield return new WaitForSeconds(0.5f);

        _particle.Play();
    }
}
