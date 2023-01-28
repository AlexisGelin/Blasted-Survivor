
using BaseTemplate.Behaviours;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { START, PLAY, END }

public class GameManager : MonoSingleton<GameManager>
{
    public GameState gameState;

    void Awake()
    {
        gameState = GameState.START;

        UIManager.Instance.Init();

        AudioManager.Instance.Init();

        PlayerController.Instance.Init();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

}