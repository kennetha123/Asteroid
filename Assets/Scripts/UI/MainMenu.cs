using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuUI;
    public GameObject DifficultySelectUI;

    public Animator transition;

    public void Play(int difficulty)
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("ChangeScene");

        yield return new WaitForSeconds(1);

        AsyncOperation async = SceneManager.LoadSceneAsync("SampleScene");

        while (!async.isDone)
            yield return null;
    }

    public void GoToSelectDifficulty()
    {
        MenuUI.SetActive(false);
        DifficultySelectUI.SetActive(true);
    }
    
    public void GoToMainMenu()
    {
        DifficultySelectUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
