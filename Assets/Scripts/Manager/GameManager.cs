
using BaseTemplate.Behaviours;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MENU, PLAY, END }

public class GameManager : MonoSingleton<GameManager>
{

    GameState _gameState;

    public GameState GameState { get => _gameState; }

    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        AudioManager.Instance.Init();

        PoolManager.Instance.Init();

        TankManager.Instance.Init();

        UIManager.Instance.Init();

        PostProcessManager.Instance.Init();

        CameraManager.Instance.Init();

        UpdateGameState(GameState.MENU);

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
        PlayerController.Instance.Init();

        WaveManager.Instance.Init();

        UpdateGameState(GameState.PLAY);
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

    public void UpdateGameState(GameState newGameState)
    {
        _gameState = newGameState;

        switch (_gameState)
        {
            case GameState.MENU:
                break;
            case GameState.PLAY:
                break;
            case GameState.END:
                break;
        }

        OnGameStateChanged?.Invoke(_gameState);
    }

}