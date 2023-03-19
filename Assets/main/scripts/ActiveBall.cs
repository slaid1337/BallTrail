using UnityEngine;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;

public class ActiveBall : Singletone<ActiveBall>
{
    private BallObject _activeBallObject;
    [SerializeField] private BallObject[] _ballObjects;

    public BallObject ActiveBallObject
    {
        get
        {
            return _activeBallObject;
        }
        set
        {
            _activeBallObject = value;
            YandexPrefs.SetInt("ActiveBall", _activeBallObject.BallIndex);
            Debug.Log("asa");
        }
    }

    private LineObject _activeLineObject;
    [SerializeField] private LineObject[] _lineObjects;

    public LineObject ActiveLineObject
    {
        get
        {
            return _activeLineObject;
        }
        set
        {
            _activeLineObject = value;
            YandexPrefs.SetInt("ActiveLine", _activeLineObject.LineIndex);
            Debug.Log("asa1");
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

    public void LoadSave()
    {
        if (YandexSDK.instance.IsFirstOpen)
        {
            foreach (var item in _ballObjects)
            {
                if (item.BallIndex == YandexPrefs.GetInt("ActiveBall", 2))
                {
                    _activeBallObject = item;
                }
            }

            foreach (var item in _lineObjects)
            {
                if (item.LineIndex == YandexPrefs.GetInt("ActiveLine", 0))
                {
                    _activeLineObject = item;
                }
            }
        }
    }
}
