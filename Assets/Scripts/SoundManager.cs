using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private float musicVolume = 1f;
    private float SFXVolume = 1f;

    private AudioClip  mainMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainMusic = Resources.Load<AudioClip>("");
        musicVolume = PlayerPrefsController.GetMusicVolume();
        SFXVolume = PlayerPrefsController.GetSFXVolume();
        audioSource.volume = musicVolume;
        ChangeMusic("Main Music");
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "clip":
                //audioSource.PlayOneShot(clip, SFXVolume);
                break;
            default:
                break;
        }
    }

    public void ChangeMusic(string song)
    {
        switch (song)
        {
            case "Main Music":
                audioSource.clip = mainMusic;
                audioSource.Play();
                break;
            default:
                break;
        }
    }

    public void ChangeSFXVolume(float value)
    {
        SFXVolume = value;
        PlayerPrefsController.SetSFXVolume(value);
    }

    public void ChangeMusicVolume(float value)
    {
        audioSource.volume = value;
        PlayerPrefsController.SetMusicVolume(value);
    }
}
