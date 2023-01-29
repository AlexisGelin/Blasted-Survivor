
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

        PoolManager.Instance.Init();

        WaveManager.Instance.Init();

        TankManager.Instance.Init();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            PlayerController.Instance.Evolve("DoubleShot");
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

}