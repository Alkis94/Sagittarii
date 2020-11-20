using System;
using UnityEngine;

public class BossDoorOutside : BossDoor, IInteractable
{
    [SerializeField]
    private bool isLocked;

    protected override void Start()
    {
        base.Start();
        isOpen = !isLocked;
        animator.SetBool("isOpen", isOpen);
    }

    public void Interact()
    {
        if (isOpen)
        {
            DoorEnter(bossName);
        }
    }
}
