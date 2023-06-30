using BaseTemplate.Behaviours;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoSingleton<PostProcessManager>
{
    [SerializeField] Volume _volume;
    [SerializeField] AnimationCurve _vignetteCurve;

    Vignette _vignette;
    Tweener _vignetteTween;

    const float BASE_VIGNETTE_INTENSITY = .2f;

    public void Init()
    {
        _volume.profile.TryGet(out _vignette);
    }

    public void DoVignetteFlash(float intensity = 1f, float time = .5f)
    {
        if (_vignetteTween.IsActive()) _vignetteTween.Kill();

        _vignetteTween = DOVirtual.Float(BASE_VIGNETTE_INTENSITY, BASE_VIGNETTE_INTENSITY + intensity, time, x =>
        {
            _vignette.intensity.value = x;
        }).SetEase(_vignetteCurve);
    }
}
