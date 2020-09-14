using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip playerGroundImpactSound;
    [SerializeField]
    private AudioClip playerDeathSound;
    [SerializeField]
    private AudioClip playerSplatterDeathSound;
    [SerializeField]
    private AudioClip playerJumpSound;
    [SerializeField]
    private AudioClip playerGotHitSound;
    [SerializeField]
    private List<AudioClip> playerSteps;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.priority = 0;
    }

    public void PlayGroundImpactSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = playerGroundImpactSound;
            audioSource.Play();
        }
    }

    public void PlayGotHitSound()
    {
        audioSource.PlayOneShot(playerGotHitSound);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(playerDeathSound);
    }

    public void PlaySplatterDeathSound()
    {
        audioSource.PlayOneShot(playerSplatterDeathSound);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(playerJumpSound);
    }

    //Gets called from animation event
    public void PlayRandomStepSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = playerSteps[Random.Range(0, playerSteps.Count)];
            audioSource.Play();
        }
    }




}
