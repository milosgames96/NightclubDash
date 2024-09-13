using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerStartScene : MonoBehaviour
{
    public Button GameStartButton;
    public Button ExitGameButton;

    void Start()
    {
        GameStartButton.onClick.AddListener(GameStart);
        ExitGameButton.onClick.AddListener(ExitGame);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
