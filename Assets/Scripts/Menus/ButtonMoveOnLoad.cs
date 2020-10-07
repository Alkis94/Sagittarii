using UnityEngine;
using System.Collections;

public class ButtonMoveOnLoad : MonoBehaviour
{
    [SerializeField]
    private float waitDelay = 0.1f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(waitDelay + 0.1f);
        LeanTween.moveLocalX(gameObject, -245, 0.2f).setEaseLinear();   
    }
}
