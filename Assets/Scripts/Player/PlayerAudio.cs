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
    private AudioClip playerJumpSound;
    [SerializeField]
    private AudioClip playerGotHitSound;
    [SerializeField]
    private List<AudioClip> playerSteps;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGroundImpactSound ()
    {
        audioSource.clip = playerGroundImpactSound;
        audioSource.Play();
    }

    public void PlayGotHitSound()
    {
        audioSource.clip = playerGotHitSound;
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        audioSource.clip = playerDeathSound;
        audioSource.Play();
    }

    public void PlayJumpSound()
    {
        audioSource.clip = playerJumpSound;
        audioSource.Play();
    }

    //Gets called from animation event
    public void PlayRandomStepSound()
    {
        audioSource.clip = playerSteps[Random.Range(0, playerSteps.Count)];
        audioSource.Play();
    }




}
