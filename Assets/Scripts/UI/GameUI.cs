using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public GameManager manager;

    public GameObject PauseUI;
    public GameObject GameOverUI;
    public GameObject PlayingUI;

    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text healthText;

    public Animator transition;

    private void Update()
    {
        if (GameOverUI.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = PauseUI.activeSelf == true ? 1 : 0;
            PauseUI.SetActive(!PauseUI.activeSelf);
        }

        scoreText.text = string.Format("Score: {0}", manager.currentScore);
        healthText.text = string.Format("Health: {0}", manager.currentHealth);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("ChangeScene");

        yield return new WaitForSeconds(1);

        AsyncOperation async = SceneManager.LoadSceneAsync("MainMenu");

        while (!async.isDone)
            yield return null;
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
    }
}
