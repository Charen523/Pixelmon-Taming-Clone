using UnityEngine;
using UnityEngine.Audio;

public enum BgmIndex
{
    Intro
}

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip[] bgmClip;

    public bool[] isMuted = new bool[3];
    private float[] preVolumes = new float[3];

    protected override void Awake()
    {
        isDontDestroyOnLoad = true;
        base.Awake();

        bgmAudioSource = GetComponent<AudioSource>();
        if (bgmAudioSource == null)
        {
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
        }

        // BGM 믹서 그룹 설정
        bgmAudioSource.outputAudioMixerGroup = bgmMixerGroup;
        bgmAudioSource.loop = true;
    }

    private void Start()
    {
        ChangeBackGroundMusic(BgmIndex.Intro);
    }

    public void ChangeBackGroundMusic(BgmIndex clip)
    {
        AudioClip music = bgmClip[(int)clip];
        bgmAudioSource.Stop();
        bgmAudioSource.clip = music;
        bgmAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        //효과음.
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void ToggleMasterMute()
    {
        isMuted[0] = !isMuted[0];

        if (isMuted[0])
        {
            audioMixer.GetFloat("MasterVolume", out preVolumes[0]);
            audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", preVolumes[0]);
        }
    }

    public void ToggleBGMMute()
    {
        isMuted[1] = !isMuted[1];

        if (isMuted[1])
        {
            audioMixer.GetFloat("BGMVolume", out preVolumes[1]);
            audioMixer.SetFloat("BGMVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("BGMVolume", preVolumes[1]);
        }
    }

    public void ToggleSFXMute()
    {
        isMuted[2] = !isMuted[2];

        if (isMuted[2])
        {
            audioMixer.GetFloat("SFXVolume", out preVolumes[2]);
            audioMixer.SetFloat("SFXVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", preVolumes[2]);
        }
    }
}