using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SliderBar : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Slider slider;
    [SerializeField] float TimeToSlide;

    public void SetMaxBar(int maxValue, int value, string prefix = "", string suffix = "")
    {
        slider.maxValue = maxValue;
        slider.value = value;
        _text.text = prefix + value + suffix;
    }

    public void SetMaxBarWithText(int maxValue, int value, string text, string prefix = "", string suffix = "")
    {
        slider.maxValue = maxValue;
        slider.value = value;
        _text.text = prefix + text + suffix;
    }

    public void SetMaxBar(int maxValue, int value, bool separated, string separator = " / ")
    {
        slider.maxValue = maxValue;
        slider.value = value;
        _text.text = value + separator + maxValue;
    }

    public void SetBar(int value)
    {
        float currentValue = slider.value;

        slider.DOValue(value, TimeToSlide);
    }

    public void SetBar(int value, string prefix = "", string suffix = "")
    {
        float currentValue = slider.value;

        slider.DOValue(value, TimeToSlide);

        _text.text = prefix + value + suffix;
    }

    public void SetBar(int value, bool separated = false, string separator = " / ")
    {
        float currentValue = slider.value;

        slider.DOValue(value, TimeToSlide);

        _text.text = value + separator + slider.maxValue;
    }

    public void UpdateMaxBar(int value)
    {
        slider.maxValue = value;
    }

    public void UpdateMaxBar(int value, string prefix = "", string suffix = "")
    {
        float currentValue = slider.value;
        slider.maxValue = value;

        slider.DOValue(currentValue, TimeToSlide);

        _text.text = prefix + currentValue + suffix;
    }

    public void UpdateMaxBar(int value, bool separated = false, string separator = " / ")
    {
        float currentValue = slider.value;
        slider.maxValue = value;

        slider.DOValue(currentValue, TimeToSlide);

        _text.text = currentValue + separator + slider.maxValue;
    }
}
