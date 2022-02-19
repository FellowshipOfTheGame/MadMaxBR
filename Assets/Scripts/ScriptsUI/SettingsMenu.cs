using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private Resolution[] resolutions;

    public Dropdown resolutionsDropdown;
    public Dropdown qualityDropdown;

    public AudioMixer mainAudioMixer;

    private float mainVolume = 0;
    private float effectVolume = 0;
    private float musicVolume = 0;

    private bool isFullscreen = true;
    private int qualityLevel = 3;
    private int resolutionLevel = 0;


    private void Awake()
    {
        if (PlayerPrefs.GetFloat("mainVolume") != 0)
        {
            mainVolume = PlayerPrefs.GetFloat("mainVolume");
        }
        if (PlayerPrefs.GetFloat("effectVolume") != 0)
        {
            effectVolume = PlayerPrefs.GetFloat("effectVolume");
        }
        if (PlayerPrefs.GetFloat("musicVolume") != 0)
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        }

        //   isFullscreen = PlayerPrefs.GetBool
        if (PlayerPrefs.GetInt("qualityLevel") != 0)
        {
            qualityLevel = PlayerPrefs.GetInt("qualityLevel");
        }
        if (PlayerPrefs.GetInt("resolutionLevel") != 0)
        {
            resolutionLevel = PlayerPrefs.GetInt("resolutionLevel");
        }
    }


    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();


        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionLevel = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = resolutionLevel;
        resolutionsDropdown.RefreshShownValue();


        qualityDropdown.value = qualityLevel;
        qualityDropdown.RefreshShownValue();
    }


    public void SetMainVolume(float volume)
    {
        mainAudioMixer.SetFloat("MainAudio", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mainAudioMixer.SetFloat("MusicAudio", volume);
    }

    public void SetEffectVolume(float volume)
    {
        mainAudioMixer.SetFloat("EffectAudio", volume);
    }

    public void SetGraphicQuality(int quantityIndex)
    {
        QualitySettings.SetQualityLevel(quantityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
