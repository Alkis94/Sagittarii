using System;
using UnityEngine;

public class BossDoorOutside : BossDoor, IInteractable
{
    [SerializeField]
    private bool isAlwaysUnlocked = false;
    private bool isLocked = true;

    protected override void Start()
    {
        base.Start();

        if (isAlwaysUnlocked)
        {
            isLocked = false;
        }
        else if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + bossName))
        {
            if (ES3.KeyExists("isLocked", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + bossName))
            {
                isLocked = ES3.Load<bool>("isLocked", "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + bossName);
            }
            else
            {
                isLocked = true;
            }
        }
        else
        {
            isLocked = true;
        }

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
