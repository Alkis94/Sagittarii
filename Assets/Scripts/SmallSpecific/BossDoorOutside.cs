using System;
using UnityEngine;

public class BossDoorOutside : BossDoor, IInteractable
{
    [SerializeField]
    private string levelToLoad;

    protected override void Start()
    {
        base.Start();
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
    }

    public void Interact()
    {
        if (isOpen)
        {
            DoorEnter(levelToLoad);
        }
    }
}
