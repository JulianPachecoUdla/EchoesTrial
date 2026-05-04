using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinuePanelController : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}