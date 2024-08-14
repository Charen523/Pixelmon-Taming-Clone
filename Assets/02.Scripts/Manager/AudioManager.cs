using UnityEngine;
using UnityEngine.Audio;

public enum BgmIndex
{
    Intro,
    Main
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

    private void Start()
    {
        //ChangeBackGroundMusic(BgmIndex.Intro);
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
        //효과음?
    }

    public void ToggleMute(int index)
    {
        string audioName;

        switch (index)
        {
            case 1:
                audioName = "BGMVolume";
                break;
            case 2:
                audioName = "SFXVolume";
                break;
            default:
                audioName = "MasterVolume";
                break;
        }

        isMuted[index] = !isMuted[index];

        if (isMuted[index])
        {
            audioMixer.GetFloat(audioName, out preVolumes[index]);
            audioMixer.SetFloat(audioName, -80);
        }
        else
        {
            audioMixer.SetFloat(audioName, preVolumes[index]);
        }
    }
}