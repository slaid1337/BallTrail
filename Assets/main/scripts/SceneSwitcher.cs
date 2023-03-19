using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadNextLevel()
    {
        StartCoroutine(StartSwitchingScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadMenu()
    {
        StartCoroutine(StartSwitchingScene(0));
        LvlTransition.Instance.CloseLvl();
    }

    public void LoadScene(int index)
    {
        StartCoroutine(StartSwitchingScene(index));
    }

    private IEnumerator StartSwitchingScene(int index)
    {
        yield return new WaitForSeconds(1.5f);

        if (SceneManager.sceneCountInBuildSettings == index)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(index);
        }
    }
}
