using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Cinemachine;

public class CharacterChooser : MonoBehaviour
{

    [SerializeField]
    private CharacterClass characterClass = CharacterClass.thief;

    public CharacterClass CharacterClassChosen
    {
        get => characterClass;
        set
        {
            characterClass = value;
            ChooseChild();
        }
    }

    private void ChooseChild()
    {
        List<int> childrenToDestroy = new List<int>();
        Transform chosenChild = transform.GetChild(0);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<PlayerStats>().CharacterClass != CharacterClassChosen)
            {
                childrenToDestroy.Add(i);
            }
            else
            {
                chosenChild = child;
            }
        }

        for (int i = 0; i < childrenToDestroy.Count; i++)
        {
            Destroy(transform.GetChild(childrenToDestroy[0]).gameObject);
        }
    }
}
