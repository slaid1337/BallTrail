using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;

public class SliderItem : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _lock;
    [SerializeField] private Text _costText;
    private BuyItem _buyItem;
    public bool IsActive;

    private void Start()
    {
        _buyItem = GetComponent<BuyItem>();

        if (YandexSDK.instance.IsFirstOpen)
        {
            YandexSDK.instance.onInitializeData += LoadSave;
        }
        else
        {
            LoadSave();
        }
    }

    public void OffBuy()
    {
        _lock.SetActive(false);
        _costText.gameObject.SetActive(false);
    }

    public void LoadSave()
    {
        if (_buyItem.IsTaken())
        {
            OffBuy();
        }
        else
        {
            _costText.text = _buyItem.Cost.ToString();
        }

        switch (_buyItem.itemType)
        {
            case BuyItem.ItemType.Ball:
                if (_buyItem.ballObject.BallIndex == YandexPrefs.GetInt("ActiveBall", 2))
                {
                    transform.SetSiblingIndex(2);
                    IsActive = true;
                    _buyItem.Activate();
                    Activate(1f, 0.01f);
                }
                else
                {
                    Diactivate(0.6f, 0.01f);
                }
                break;
            case BuyItem.ItemType.Line:
                if (_buyItem.lineObject.LineIndex == YandexPrefs.GetInt("ActiveLine", 0))
                {
                    transform.SetSiblingIndex(2);
                    IsActive = true;
                    _buyItem.Activate();
                    Activate(1f, 0.01f);
                }
                else
                {
                    Diactivate(0.6f, 0.01f);
                }
                break;
        }
    }

    public void MoveEnd(int index)
    {
        transform.SetSiblingIndex(index);
    }

    public void MoveStart()
    {
        transform.SetSiblingIndex(0);
    }

    public void Diactivate(float size, float speed)
    {
        IsActive = false;
        _buyItem.Diactivate();
        _rectTransform.DOScale(size, speed);
        _image.DOColor(new Color(0.5f, 0.5f, 0.5f, 1f), speed);
    }

    public void Activate(float size, float speed)
    {
        IsActive = true;
        _buyItem.Activate();
        _rectTransform.DOScale(size, speed);
        _image.DOColor(new Color(1f, 1f, 1f, 1f), speed);

        if (_buyItem.IsTaken())
        {
            _buyItem.SetSkin();
        }
    }
}
