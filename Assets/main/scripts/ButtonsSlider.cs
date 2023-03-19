using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonsSlider : MonoBehaviour
{
    [SerializeField] private Button _BntLeft;
    [SerializeField] private Button _BntRight;
    [SerializeField] private SliderContainer _sliderContainer;
    private int _itemIndex = 2;

    private void Start()
    {
        _BntLeft.onClick.AddListener(LeftClick);
        _BntRight.onClick.AddListener(RightClick);
    }

    public void RightClick()
    {
        _sliderContainer.RightSlide(_itemIndex);
        StartCoroutine(EnableButtons());
        _BntLeft.interactable = false;
        _BntRight.interactable = false;
    }

    public void LeftClick()
    {
        _sliderContainer.LeftSlide(_itemIndex);
        StartCoroutine(EnableButtons());
        _BntLeft.interactable = false;
        _BntRight.interactable = false;
    }

    public IEnumerator EnableButtons()
    {
        yield return new WaitForSeconds(_sliderContainer._scrollSpeedInSeconds);

        _BntLeft.interactable = true;
        _BntRight.interactable = true;
    }
}
