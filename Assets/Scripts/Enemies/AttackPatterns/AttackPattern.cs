using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    [SerializeField]
    protected List<AttackData> attackData;
    protected AudioSource audioSource;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public virtual void Attack(int index)
    {
        if (attackData[index].projectileSpawnPositionOffset.Count == 0)
        {
            for (int i = 0; i < attackData[index].projectileAmount; i++)
            {
                attackData[index].projectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }

        if (attackData[index].attackSound != null)
        {
            audioSource.PlayOneShot(attackData[index].attackSound);
        }
    }

}
