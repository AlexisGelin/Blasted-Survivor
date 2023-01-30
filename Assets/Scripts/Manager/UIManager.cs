using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] View _menuView, _gameView;

    View _currentView;

    public void Init()
    {
        SwitchView(_menuView);
    }

    public void SwitchView(View newView)
    {
        if (_currentView != null && _currentView != newView)
        {
            _currentView.CG.DOFade(0, .2f);

            _currentView.CG.interactable = false;
            _currentView.CG.blocksRaycasts = false;
        }

        _currentView = newView;

        _currentView.CG.DOFade(1, .2f);

        _currentView.CG.interactable= true;
        _currentView.CG.blocksRaycasts= true;
    }
}
