using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseProfile : MonoBehaviour
{
    [SerializeField]
    private int profileId;
    [SerializeField]
    private Button chooseButton;
    

    public static event Action OnProfileChanged = delegate { };

    private void OnEnable()
    {
        OnProfileChanged += ChangeButtons;
    }

    private void OnDisable()
    {
        OnProfileChanged -= ChangeButtons;
    }

    public void Start()
    {
        if (profileId == ProfileManager.Instance.ProfileId)
        {
            chooseButton.interactable = false;
        }
    }

    public void OnChoosePress()
    {
        ProfileManager.Instance.SetProfileId(profileId);
        OnProfileChanged.Invoke();
    }

    public void OnDeletePress()
    {
        ProfileManager.Instance.DeleteProfile(profileId);
    }

    private void ChangeButtons()
    {
        chooseButton.interactable = ProfileManager.Instance.ProfileId != profileId;
    }
}
