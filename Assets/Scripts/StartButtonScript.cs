using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject MainMenuScreen;
    public GameObject CreditsScreen;
    public int Level;

    private void Start()
    {
        LoadingScreen.SetActive(false);
    }

    public void StartGame()
    {
        LoadingScreen.SetActive(true);
        GameManager.Lives = GameManager.MaxLives;
        SceneManager.LoadScene("Gameplay");
    }


    public void ShowCredits()
    {
        MainMenuScreen.SetActive(false);
        GameManager.Lives = GameManager.MaxLives;
        CreditsScreen.SetActive(true);
    }


    public void BackToMainMenu()
    {

        CreditsScreen.SetActive(false);
        MainMenuScreen.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartLevel()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Level_0" + Level);
    }
}
