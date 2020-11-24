using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HowToPlay : MonoBehaviour
{
    [SerializeField]
    List<GameObject> tips;
    private int index;

    public void LeftArrow ()
    {

        tips[index].SetActive(false);

        if(index == 0)
        {
            index = tips.Count - 1;
        }
        else
        {
            index--;
        }

        tips[index].SetActive(true);
    }

    public void RightArrow()
    {
        tips[index].SetActive(false);

        if (index == tips.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        tips[index].SetActive(true);
    }

}
