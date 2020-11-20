using UnityEngine;
using Cinemachine;

public class MushroomTower : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField]
    private AudioClip breakSound;
    private bool isBroken = false;
    private int numberOfHitsToDestroy = 5;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBroken && numberOfHitsToDestroy <= 0)
        {
            animator.SetTrigger("Break");
            audioSource.PlayOneShot(breakSound);
            cinemachineImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 1;
            cinemachineImpulseSource.GenerateImpulse();
            isBroken = true;
            //ES3.Save<bool>("isEnabled" + transform.GetSiblingIndex(), false, "Levels/" + mapType + "/Room" + roomKey + "/Props");
            transform.gameObject.layer = 14;
        }
        else
        {
            numberOfHitsToDestroy--;
            cinemachineImpulseSource.GenerateImpulse();
            audioSource.Play();
        }
    }
}

