using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private Resolution[] resolutions;

    [SerializeField] private Dropdown resolutionsDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle isFullScreenToggle;
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectdSlider;

    public AudioMixer mainAudioMixer;

    private float mainVolume = 0;
    private float effectVolume = 0;
    private float musicVolume = 0;

    private bool isFullscreen = true;
    private int qualityLevel = 2;
    private int resolutionLevel = 0;


    private void Awake()
    {
        mainVolume = PlayerPrefs.HasKey("mainVolume") ? PlayerPrefs.GetFloat("mainVolume") : 0;
        effectVolume = PlayerPrefs.HasKey("effectVolume") ? PlayerPrefs.GetFloat("effectVolume") : 0;
        musicVolume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 0;

        qualityLevel = PlayerPrefs.HasKey("qualityLevel") ? PlayerPrefs.GetInt("qualityLevel") : 2;
        resolutionLevel = PlayerPrefs.HasKey("resolutionLevel") ? PlayerPrefs.GetInt("resolutionLevel") : 0;

        if (PlayerPrefs.GetString("isFullscreen") == "True")
        {
            isFullscreen = true;
        }
        else
        {
            isFullscreen = false;
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
                if (resolutionLevel == 0)
                {
                    resolutionLevel = i;
                }
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.RefreshShownValue();
        resolutionsDropdown.value = resolutionLevel;
        SetResolution(resolutionLevel);

        isFullScreenToggle.isOn = isFullscreen;
        SetFullScreen(isFullscreen);

        qualityDropdown.RefreshShownValue();
        qualityDropdown.value = qualityLevel;

        mainSlider.value = mainVolume;
        musicSlider.value = musicVolume;
        effectdSlider.value = effectVolume;
    }


    public void SetMainVolume(float volume)
    {
        mainAudioMixer.SetFloat("MainAudio", volume);
        PlayerPrefs.SetFloat("mainVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mainAudioMixer.SetFloat("MusicAudio", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetEffectVolume(float volume)
    {
        mainAudioMixer.SetFloat("EffectAudio", volume);
        PlayerPrefs.SetFloat("effectVolume", volume);
    }

    public void SetGraphicQuality(int quantityIndex)
    {
        QualitySettings.SetQualityLevel(quantityIndex);
        PlayerPrefs.SetInt("qualityLevel", quantityIndex);
    }

    public void SetFullScreen(bool isFullScreenMode)
    {
        Screen.fullScreen = isFullScreenMode;
        PlayerPrefs.SetString("isFullScreen", isFullScreenMode ? "True" : "False");
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionLevel", resolutionIndex);
    }
}
