using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

enum MenuState { centered, around }

public class MenuView : View
{
    [SerializeField] MenuPanel _menuPanel;
    [SerializeField] SettingPanel _settingPanel;
    [SerializeField] LeaderBoardPanel _leaderBoardPanel;
    [SerializeField] TMP_InputField _nameInputField;

    [SerializeField] float _speedToSwitchPanel = .2f;

    MenuState _state;

    Panel _currentPanel;

    public override void Init()
    {
        base.Init();

        _menuPanel.Init();
        _settingPanel.Init();
        _leaderBoardPanel.Init();
    }

    public override void OpenView()
    {
        base.OpenView();
    }

    public override void CloseView()
    {
        base.CloseView();
    }

    public void SwitchPanel(Panel newPanel)
    {
        if (_currentPanel == null) _currentPanel = _menuPanel;

        _currentPanel.CG.interactable = false;
        _currentPanel.CG.blocksRaycasts = false;

        if (_state == MenuState.centered)
        {
            _currentPanel.RT.DOAnchorMin(new Vector2(newPanel._mainAnchorMin.x, newPanel._mainAnchorMin.y), _speedToSwitchPanel);
            _currentPanel.RT.DOAnchorMax(new Vector2(newPanel._mainAnchorMax.x, newPanel._mainAnchorMax.y), _speedToSwitchPanel);
        }
        else
        {
            _currentPanel.RT.DOAnchorMin(new Vector2(_currentPanel._anchorMin.x, _currentPanel._anchorMin.y), _speedToSwitchPanel);
            _currentPanel.RT.DOAnchorMax(new Vector2(_currentPanel._anchorMax.x, _currentPanel._anchorMax.y), _speedToSwitchPanel);
        }

        _currentPanel = newPanel;

        newPanel.RT.DOAnchorMin(new Vector2(0, 0), _speedToSwitchPanel);
        newPanel.RT.DOAnchorMax(new Vector2(1, 1), _speedToSwitchPanel).OnComplete(() =>
        {
            _currentPanel.CG.interactable = true;
            _currentPanel.CG.blocksRaycasts = true;
        });

        if (_state == MenuState.centered) _state = MenuState.around;
        else _state = MenuState.centered;
    }

    public void HandleStartButton()
    {
        AudioManager.Instance.PlaySound("StartGame");

        PlayerManager.Instance.Name = _nameInputField.text;

        GameManager.Instance.StartGame();
    }

}
