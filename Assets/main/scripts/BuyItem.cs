using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;

public class BuyItem : MonoBehaviour
{
    public BallObject ballObject;
    public LineObject lineObject;
    public int ObjectIndex;
    public ItemType itemType;
    public int Cost;
    private Money _money;
    [SerializeField] private Button _button;
    [SerializeField] private ParticleSystem _particle;
    private SliderItem _sliderItem;

    private void Awake()
    {
        if (itemType == ItemType.Ball)
        {
            Cost = ballObject.BallCost;
        }
        else
        {
            Cost = lineObject.LineCost;
        }
    }

    private void Start()
    {
        _money = Money.Instance;
        _sliderItem = GetComponent<SliderItem>();
        _button.interactable = false;
    }

    public bool IsTaken()
    {
        if (itemType == ItemType.Ball)
        {
            if (ballObject.BallIndex == 2 || ballObject.IsTaken) return true;
            ballObject.IsTaken = YandexPrefs.GetInt("BallSave" + ballObject.BallIndex, 0) == 1 ? true : false;
            return ballObject.IsTaken;
        }
        else
        {
            if (lineObject.LineIndex == 0 || lineObject.IsTaken) return true;
            lineObject.IsTaken = YandexPrefs.GetInt("LineSave" + lineObject.LineIndex, 0) == 1 ? true : false;
            return lineObject.IsTaken;
        }
    }

    public void SetSkin()
    {
        switch (itemType)
        {
            case ItemType.Ball:
                ActiveBall.Instance.ActiveBallObject = ballObject;
                break;

            case ItemType.Line:
                ActiveBall.Instance.ActiveLineObject = lineObject;
                break;
        }
    }

    public void Activate()
    {
        if (!IsTaken())
        {
            _button.interactable = true;
        }
    }

    public void Diactivate()
    {
        if (!IsTaken())
        {
            _button.interactable = false;
        }
    }

    public void Buy()
    {
        if (_money.money - Cost >= 0)
        {
            _particle.Play();
            _button.interactable = false;
            _money.money -= Cost;
            SetSkin();
            _sliderItem.OffBuy();

            if (itemType == ItemType.Ball)
            {
                YandexPrefs.SetInt("BallSave" + ballObject.BallIndex, 1);
            }
            else
            {
                YandexPrefs.SetInt("LineSave" + lineObject.LineIndex, 1);
            }
        }
        
    }

    public enum ItemType
    {
        Ball,
        Line
    }
}
