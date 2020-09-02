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
        audioSource.PlayOneShot(buttonOverSound);
    }

    public void OnButtonPressed()
    {
        audioSource.PlayOneShot(buttonPressedSound);
    }
}
