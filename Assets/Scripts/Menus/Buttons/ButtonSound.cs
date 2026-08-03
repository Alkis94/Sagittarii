using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buttonOverSound;
    [SerializeField]
    private AudioClip buttonPressedSound;


    private void Start()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource == null || !audioSource.isActiveAndEnabled)
        {
            return;
        }

        audioSource.PlayOneShot(buttonOverSound);
    }

    public void OnButtonPressed()
    {
        if (audioSource == null || !audioSource.isActiveAndEnabled)
        {
            return;
        }

        audioSource.PlayOneShot(buttonPressedSound);
    }
}
