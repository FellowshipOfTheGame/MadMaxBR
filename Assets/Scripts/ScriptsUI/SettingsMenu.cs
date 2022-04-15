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
    [SerializeField] private Slider effectsSlider;

    public AudioMixer mainAudioMixer;

    private float mainVolume = 0;
    private float effectVolume = 0;
    private float musicVolume = 0;

    private bool isFullscreen = true;
    private int qualityLevel = 2;
    private int resolutionLevel = 0;

    [SerializeField] private KeyCode slot1UseButton = KeyCode.I;
    [SerializeField] private KeyCode slot2UseButton = KeyCode.J;
    [SerializeField] private KeyCode slot3UseButton = KeyCode.K;
    [SerializeField] private KeyCode slot4UseButton = KeyCode.L;
    [SerializeField] private int lastKeyCode = 0;

    [SerializeField] private Text slot1UseButtonText;
    [SerializeField] private Text slot2UseButtonText;
    [SerializeField] private Text slot3UseButtonText;
    [SerializeField] private Text slot4UseButtonText;


    private void Awake()
    {
        mainVolume = PlayerPrefs.HasKey("mainVolume") ? PlayerPrefs.GetFloat("mainVolume") : 0;
        effectVolume = PlayerPrefs.HasKey("effectVolume") ? PlayerPrefs.GetFloat("effectVolume") : 0;
        musicVolume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 0;
       
        slot1UseButton = PlayerPrefs.HasKey("Slot1UseButtonIndex") ? (KeyCode) PlayerPrefs.GetInt("Slot1UseButtonIndex") : KeyCode.I;
        slot2UseButton = PlayerPrefs.HasKey("Slot2UseButtonIndex") ? (KeyCode) PlayerPrefs.GetInt("Slot2UseButtonIndex") : KeyCode.J;
        slot3UseButton = PlayerPrefs.HasKey("Slot3UseButtonIndex") ? (KeyCode) PlayerPrefs.GetInt("Slot3UseButtonIndex") : KeyCode.K;
        slot4UseButton = PlayerPrefs.HasKey("Slot4UseButtonIndex") ? (KeyCode) PlayerPrefs.GetInt("Slot4UseButtonIndex") : KeyCode.L;

        qualityLevel = PlayerPrefs.HasKey("qualityLevel") ? PlayerPrefs.GetInt("qualityLevel") : 2;
        resolutionLevel = PlayerPrefs.HasKey("resolutionLevel") ? PlayerPrefs.GetInt("resolutionLevel") : 0;

        if (PlayerPrefs.GetInt("isFullscreenIndex") == 1)
        {
            isFullscreen = true;
        }
        else
        {
            isFullscreen = false;
        }
    }


    private void Start()
    {
        lastKeyCode = 0;
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
        effectsSlider.value = effectVolume;

        slot1UseButtonText.text = slot1UseButton.ToString();
        slot2UseButtonText.text = slot2UseButton.ToString();
        slot3UseButtonText.text = slot3UseButton.ToString();
        slot4UseButtonText.text = slot4UseButton.ToString();
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                if (e.keyCode == KeyCode.W || e.keyCode == KeyCode.A || e.keyCode == KeyCode.S || e.keyCode == KeyCode.D)
                {
                    return;
                }

                if (lastKeyCode == 1 && !(e.keyCode == slot2UseButton || e.keyCode == slot3UseButton || e.keyCode == slot4UseButton))
                {
                    slot1UseButton = e.keyCode;
                    slot1UseButtonText.text = e.keyCode.ToString();
                    PlayerPrefs.SetInt("Slot1UseButtonIndex", (int) e.keyCode);
                }
                else if (lastKeyCode == 2 && !(e.keyCode == slot1UseButton || e.keyCode == slot3UseButton || e.keyCode == slot4UseButton))
                {
                    slot2UseButton = e.keyCode;
                    slot2UseButtonText.text = e.keyCode.ToString();
                    PlayerPrefs.SetInt("Slot2UseButtonIndex", (int) e.keyCode);
                }
                else if (lastKeyCode == 3 && !(e.keyCode == slot1UseButton || e.keyCode == slot2UseButton || e.keyCode == slot4UseButton))
                {
                    slot3UseButton = e.keyCode;
                    slot3UseButtonText.text = e.keyCode.ToString();
                    PlayerPrefs.SetInt("Slot3UseButtonIndex", (int) e.keyCode);
                }
                else if (lastKeyCode == 4 && !(e.keyCode == slot1UseButton || e.keyCode == slot2UseButton || e.keyCode == slot3UseButton))
                {
                    slot4UseButton = e.keyCode;
                    slot4UseButtonText.text = e.keyCode.ToString();
                    PlayerPrefs.SetInt("Slot4UseButtonIndex", (int) e.keyCode);
                }

                lastKeyCode = 0;
            }
        }
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
        if (isFullScreenMode)
        {
            PlayerPrefs.SetInt("isFullScreenIndex", 1);
        }
        else
        {
            PlayerPrefs.SetInt("isFullScreenIndex", 0);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionLevel", resolutionIndex);
    }

    public void f1(int buttonIndex)
    {
        lastKeyCode = buttonIndex;
    }
}
