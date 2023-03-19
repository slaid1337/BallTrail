using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SliderContainer : MonoBehaviour
{
    public float _scrollSpeedInSeconds;
    public SliderItem[] SliderItems;
    private RectTransform _rectTransform;
    [SerializeField] private float _activeScale;
    [SerializeField] private float _diactiveScale;

    private void Start()
    {
        SliderItems = GetComponentsInChildren<SliderItem>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void LeftSlide(int index)
    {
        _rectTransform.DOAnchorPosX(_rectTransform.anchoredPosition.x + _rectTransform.sizeDelta.x , _scrollSpeedInSeconds);
        transform.GetChild(1).GetComponent<SliderItem>().Activate(_activeScale, _scrollSpeedInSeconds);
        transform.GetChild(2).GetComponent<SliderItem>().Diactivate(_diactiveScale, _scrollSpeedInSeconds);
        StartCoroutine(CLeft());
    }

    public void RightSlide(int index)
    {
        _rectTransform.DOAnchorPosX(_rectTransform.anchoredPosition.x - _rectTransform.sizeDelta.x, _scrollSpeedInSeconds);
        transform.GetChild(3).GetComponent<SliderItem>().Activate(_activeScale, _scrollSpeedInSeconds);
        transform.GetChild(2).GetComponent<SliderItem>().Diactivate(_diactiveScale, _scrollSpeedInSeconds);
        StartCoroutine(CRight());
    }

    private IEnumerator CRight()
    {
        yield return new WaitForSeconds(_scrollSpeedInSeconds);
        transform.GetChild(0).GetComponent<SliderItem>().MoveEnd(SliderItems.Length - 1);
        _rectTransform.anchoredPosition = _rectTransform.anchoredPosition + _rectTransform.sizeDelta;
    }

    private IEnumerator CLeft()
    {
        yield return new WaitForSeconds(_scrollSpeedInSeconds);
        transform.GetChild(SliderItems.Length - 1).GetComponent<SliderItem>().MoveStart();
        _rectTransform.anchoredPosition = _rectTransform.anchoredPosition - _rectTransform.sizeDelta;
    }
}
