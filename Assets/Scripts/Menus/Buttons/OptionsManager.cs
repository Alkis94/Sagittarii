using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class OptionsManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundEffectsSlider;

    private void OnEnable()
    {
        if(ES3.FileExists("Options"))
        {
            masterSlider.value = ES3.Load<float>("MasterVolume", "Options");
            musicSlider.value = ES3.Load<float>("MusicVolume", "Options");
            soundEffectsSlider.value = ES3.Load<float>("SoundEffectsVolume", "Options");
        }
    }

    public void SetMasterVolume(float volume)
    {
        ES3.Save<float>("MasterVolume", volume, "Options");
        volume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        ES3.Save<float>("MusicVolume", volume, "Options");
        volume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        ES3.Save<float>("SoundEffectsVolume", volume, "Options");
        volume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("SoundEffects", volume);
    }

}
