using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Eiko.YaSDK.Data;
using System.Collections;

public class LvlItem : MonoBehaviour
{
    [SerializeField] private Button _lvlButton;
    [SerializeField] private GameObject _lock;
    [SerializeField] private Text _text;
    public int LvlIndex;

    private void Start()
    {
        _lvlButton.onClick.AddListener(LoadScene);
        _text.text = LvlIndex.ToString();

        if (LvlIndex != 1)
        {
            if (YandexPrefs.GetInt("LvlSave" + (LvlIndex - 1), 0) == 1)
            {
                _lvlButton.interactable = true;
                _lock.SetActive(false);
            }
            else
            {
                _lvlButton.interactable = false;
                _lock.SetActive(true);
            }
        }
    }

    private void LoadScene()
    {
        LvlTransition.Instance.CloseLvl();
        StartCoroutine(StartSwitchingScene(LvlIndex));
    }

    private IEnumerator StartSwitchingScene(int index)
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(index);
    }
}
