
using BaseTemplate.Behaviours;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public enum GameState { MENU, PLAY, END }

public class GameManager : MonoSingleton<GameManager>
{

    GameState _gameState;
    bool _isGamePause;

    public GameState GameState { get => _gameState; }
    public bool IsGamePause { get => _isGamePause; }

    public event Action<GameState> OnGameStateChanged;


    void Awake()
    {
        Time.timeScale = 1;

        DOTween.SetTweensCapacity(2000,200);

        AudioManager.Instance.Init();

        PoolManager.Instance.Init();
        EnemyPoolManager.Instance.Init();

        TankManager.Instance.Init();

        UIManager.Instance.Init();

        PostProcessManager.Instance.Init();

        CameraManager.Instance.Init();

        WorldManager.Instance.Init();

        SpawnPointEnemyManager.Instance.Init();

        UpdateGameState(GameState.MENU);

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            ReloadScene();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameState == GameState.PLAY)
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        _isGamePause = !_isGamePause;
        if (_isGamePause)
        {
            Time.timeScale = 0;
            UIManager.Instance.HandleEnterPause();
        }
        else
        {
            Time.timeScale = 1;
            UIManager.Instance.HandleExitPause();
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

                HighscoreTable.Instance.AddHighscoreEntry(PlayerManager.Instance.TotalEnemyKilled, PlayerManager.Instance.Name);
                break;
        }

        OnGameStateChanged?.Invoke(_gameState);
    }


    public void UpdateStateToMenu() => UpdateGameState(GameState.MENU);
    public void UpdateStateToPlay() => UpdateGameState(GameState.PLAY);
    public void UpdateStateToEnd() => UpdateGameState(GameState.END);
}