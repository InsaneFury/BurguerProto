using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    private void Start()
    {
        GetResolutions();
    }

    #region Graphics
    public void SetQuality(int quality) => QualitySettings.SetQualityLevel(quality);   
    public void SetResolution(int resolutionIndex) 
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetFullScreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;
    public void SetVSync(bool vSync) => QualitySettings.vSyncCount = vSync ? 1 : 0;
    public void SetAntiAliasing(int antiAliasing) => QualitySettings.antiAliasing = antiAliasing;
    private void GetResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{ resolutions[i].width}x{resolutions[i].height}";
            options.Add(option);

            bool isCurrentResolution = (resolutions[i].width == Screen.currentResolution.width) &&
                (resolutions[i].height == Screen.currentResolution.height);

            if (isCurrentResolution)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    #endregion

    #region Audio
    public void SetMasterVolume(float masterVolume) => Debug.Log($"MasterVolume: {masterVolume}");
    public void SetSFXVolume(float sfxVolume) => Debug.Log($"SFXVolume: {sfxVolume}");
    public void SetMusicVolume(float musicVolume) => Debug.Log($"MusicVolume: {musicVolume}");
    public void SetMute(bool mute) => Debug.Log($"Muted: {mute}");
    #endregion
}
