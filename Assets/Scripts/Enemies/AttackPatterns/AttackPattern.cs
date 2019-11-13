using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPattern : MonoBehaviour
{
    [SerializeField]
    protected AttackData attackData;
    [SerializeField]
    protected AudioClip attackSound = null;
    protected AudioSource audioSource;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public virtual void Attack()
    {
        if(attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

}
