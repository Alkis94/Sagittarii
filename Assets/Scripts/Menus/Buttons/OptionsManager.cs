using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class OptionsManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master",Mathf.Log10(volume) * 20);
    }

}
