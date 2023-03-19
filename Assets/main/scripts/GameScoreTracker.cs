using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;

public class GameScoreTracker : Singletone<GameScoreTracker>
{
    private int _ballCount;
    [SerializeField] private MovePanel _movePanel;
    
    public int Score { get; set; }

    [SerializeField] private GameSimulation _gameSimulation;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _restartBtn;

    private void Start()
    {
        _ballCount = _gameSimulation.Balls.Length;
        StartCoroutine(WaitUntilScore());
    }

    private IEnumerator WaitUntilScore()
    {
        yield return new WaitUntil(() => Score == _ballCount);

        _restartBtn.gameObject.SetActive(false);
        _nextBtn.gameObject.SetActive(true);
        int money = YandexPrefs.GetInt("Money", 0);
        YandexPrefs.SetInt("Money", money + 15);
        YandexPrefs.SetInt("LvlSave" + SceneManager.GetActiveScene().buildIndex, 1);

        try
        {
            YandexSDK.instance.ShowInterstitial();
        }
        catch
        {

        }

        FadeBG.Instance.Fade();
        _movePanel.Open();
    }
}
