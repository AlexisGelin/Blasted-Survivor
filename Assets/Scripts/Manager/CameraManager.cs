using BaseTemplate.Behaviours;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera MainCamera;
    [SerializeField] CinemachineShake _mainShake;
    [SerializeField] AnimationCurve ZoomEaseHit, ZoomEaseDie;

    Tweener _zoomTween;

    const float BASE_SIZE_CAMERA = 5f;

    public void Init()
    {

    }

    public void ZoomHit(float zoomAmount = 1, float zoomTime = .5f, bool isDie = false)
    {
        float originalZoomValue = MainCamera.m_Lens.OrthographicSize;

        if (_zoomTween.IsActive()) _zoomTween.Kill();

        _zoomTween = DOVirtual.Float(BASE_SIZE_CAMERA, BASE_SIZE_CAMERA + zoomAmount, zoomTime, x => MainCamera.m_Lens.OrthographicSize = x).SetEase(isDie ? ZoomEaseDie : ZoomEaseHit);
    }

    public void ShakeCamera(float intensity = 4, float duration = .125f)
    {
       _mainShake.ShakeCamera(intensity, duration);
    }
}
