using UnityEngine;

public class DeactivateParentButton: MonoBehaviour
{
    public void OnButtonClick ()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
