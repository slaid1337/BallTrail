using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;

public class Money : Singletone<Money>
{
    [SerializeField] private Text _moneyText;
    private int _money;

    public int money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            Refresh();
        }
    }

    private void Start()
    {
        if (YandexSDK.instance.IsFirstOpen)
        {
            YandexSDK.instance.onInitializeData += LoadSave;
        }
        else
        {
            LoadSave();
        }
    }

    public void Refresh()
    {
        _moneyText.text = _money.ToString();
        YandexPrefs.SetInt("Money", _money);
    }

    public void LoadSave()
    {
        _money = YandexPrefs.GetInt("Money", 0);
        _moneyText.text = _money.ToString();
    }
}
