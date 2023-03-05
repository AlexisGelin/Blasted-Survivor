
using BaseTemplate.Behaviours;
using DG.Tweening;
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

        PoolManager.Instance.Init();

        TankManager.Instance.Init();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            ReloadScene();
        }
    }

    public void StartGame()
    {
        gameState = GameState.PLAY;

        PlayerController.Instance.Init();

        WaveManager.Instance.Init();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        DOTween.KillAll();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}