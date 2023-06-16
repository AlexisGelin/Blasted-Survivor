using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingPanel : Panel
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown resolutionDropdown;

    //Cache
    Resolution[] resolutions;

    public override void Init()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionList = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolution = resolutions[i].width + "x" + resolutions[i].height;
            resolutionList.Add(resolution);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        base.Init();
    }
    public void SetMainVolume(float volume) => audioMixer.SetFloat("MainVolume", volume);
    public void SetMusicVolume(float volume) => audioMixer.SetFloat("MusicVolume", volume);
    public void SetSFXVolume(float volume) => audioMixer.SetFloat("SFXVolume", volume);
    public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);
    public void SetFullScreen(bool isFullscreen)
    {
        AudioManager.Instance.PlaySound("ToggleButton");

        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution newResolution = resolutions[resolutionIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
    }


}
