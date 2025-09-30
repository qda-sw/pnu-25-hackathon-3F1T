using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class MainStartButton : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    void OnQuitButtonClick()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
