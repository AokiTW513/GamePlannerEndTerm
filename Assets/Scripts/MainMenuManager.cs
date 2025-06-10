using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButton);
        quitButton.onClick.AddListener(OnQuitButton);
    }

    private void OnStartButton()
    {
        SceneManager.LoadScene("Level1");
    }

    private void OnQuitButton()
    {
        Application.Quit();
    }
}