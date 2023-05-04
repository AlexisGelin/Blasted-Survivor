using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    Tweener _lagTimeTweener;

    public void DoLagTime(float intensity = .2f, float duration = .075f)
    {
        if (_lagTimeTweener.IsActive()) _lagTimeTweener.Kill();

        Time.timeScale = intensity;

        _lagTimeTweener = DOVirtual.Float(Time.timeScale, 1, duration, x => Time.timeScale = x).SetUpdate(UpdateType.Normal, true);
    }

    public void SetTime(float intensity = .2f)
    {
        if (_lagTimeTweener.IsActive()) _lagTimeTweener.Kill();

        Time.timeScale = intensity;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}

