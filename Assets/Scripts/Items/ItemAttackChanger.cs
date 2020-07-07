using UnityEngine;
using Sirenix.OdinInspector;

public class ItemAttackChanger : MonoBehaviour
{
    [SerializeField]
    private bool changeMainAttack = false;
    [SerializeField]
    [ShowIf("@changeMainAttack")]
    private PlayerAttackData attackData;
    [SerializeField]
    private bool changeSecondaryAttack = false;
    [SerializeField]
    [ShowIf("@changeSecondaryAttack")]
    private PlayerAttackData secondaryAttackData;
    [SerializeField]
    private string relicName;
    [SerializeField]
    private bool isUnique = false;
    
    



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(changeMainAttack)
            {
                collision.GetComponentInChildren<PlayerAttackHandler>().AttackData = attackData;
            }

            if(changeSecondaryAttack)
            {
                collision.GetComponentInChildren<PlayerAttackHandler>().SecondaryAttackData = secondaryAttackData;
            }
            
            if(isUnique)
            {
                RelicFactory.PlayerGotUniqueRelic(relicName);
            }
            
        }
    }
}
