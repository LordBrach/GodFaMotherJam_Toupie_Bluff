using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
    private const string KeyGeneral = "MasterVolume";
    private const string KeyMusic = "MusicVolume";
    private const string KeySfx = "SFXVolume";

    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public float GeneralVolume { get; private set; } = 1f;
    public float MusicVolume { get; private set; } = 1f;
    public float SfxVolume { get; private set; } = 1f;

    private void Awake()
    {
        Instance = this;

        GeneralVolume = PlayerPrefs.GetFloat(KeyGeneral, 1f);
        MusicVolume = PlayerPrefs.GetFloat(KeyMusic, 1f);
        SfxVolume = PlayerPrefs.GetFloat(KeySfx, 1f);

        Apply();
    }

    public void SetGeneralVolume(float value)
    {
        GeneralVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(KeyGeneral, GeneralVolume);
        Apply();
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(KeyMusic, MusicVolume);
        Apply();
    }

    public void SetSfxVolume(float value)
    {
        SfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(KeySfx, SfxVolume);
        Apply();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    private void Apply()
    {
        if (musicSource != null) musicSource.volume = GeneralVolume * MusicVolume;
        if (sfxSource != null) sfxSource.volume = GeneralVolume * SfxVolume;
    }
}