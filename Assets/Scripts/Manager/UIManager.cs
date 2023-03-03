using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] View _menuView, _gameView, _endGameView;

    View _currentView;

    public MenuView MenuView { get => (MenuView)_menuView; }
    public GameView GameView { get => (GameView)_gameView; }
    public EndGameView EndGameView { get => (EndGameView)_endGameView; }

    public void Init()
    {
        GameManager.Instance.OnGameStateChanged += HandleStateChange;

        MenuView.Init();
        GameView.Init();
        EndGameView.Init();
    }

    public void SwitchView(View newView)
    {
        if (_currentView != null && _currentView != newView)
        {
            _currentView.CG.DOFade(0, .2f);

            _currentView.CG.interactable = false;
            _currentView.CG.blocksRaycasts = false;

            _currentView.CloseView();

        }

        _currentView = newView;

        _currentView.OpenView();

        _currentView.CG.DOFade(1, .2f);

        _currentView.CG.interactable = true;
        _currentView.CG.blocksRaycasts = true;
    }

    void HandleStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.MENU:
                SwitchView(_menuView);
                break;
            case GameState.PLAY:
                SwitchView(_gameView);
                break;
            case GameState.END:
                SwitchView(_endGameView);
                break;
            default:
                break;
        }
    }
}