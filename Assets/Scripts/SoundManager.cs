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

    private AudioClip mainMusic;
    private AudioClip partidaCarro;
    private AudioClip bigBoom;
    private AudioClip ninaTheme;
    private AudioClip openSong;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainMusic = Resources.Load<AudioClip>("");
        partidaCarro = Resources.Load<AudioClip>("INSANE NINA_SFX_PARTIDA PLAY B");
        bigBoom = Resources.Load<AudioClip>("INSANE NINA_BIG BOOM");
        ninaTheme = Resources.Load<AudioClip>("INSANE NINA_NINA THEME");
        openSong = Resources.Load<AudioClip>("INSANE NINE_OPEN SONG_02");
        musicVolume = PlayerPrefsController.GetMusicVolume();
        SFXVolume = PlayerPrefsController.GetSFXVolume();
        audioSource.volume = musicVolume;
        ChangeMusic("Main Music");
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "PartidaCarro":
                audioSource.PlayOneShot(partidaCarro, SFXVolume);
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
            case "Big Boom":
                audioSource.clip = bigBoom;
                audioSource.Play();
                break;
            case "Nina Theme":
                audioSource.clip = ninaTheme;
                audioSource.Play();
                break;
            case "Open Song":
                audioSource.clip = openSong;
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
