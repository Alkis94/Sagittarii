using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyData))]
[RequireComponent(typeof(EnemyCollision))]
public class EnemyDropItemSpecial : MonoBehaviour
{
    private EnemyCollision enemyCollision;
    private EnemyData enemyData;
    private ItemData itemData;
   

    private void OnEnable()
    {
        enemyData = GetComponent<EnemyData>();
        itemData = enemyData.SpecialItem.GetComponent<ItemData>();
        enemyCollision = GetComponent<EnemyCollision>();
        enemyCollision.OnDeath += CancelInvoke;
    }

    private void OnDisable()
    {
        enemyCollision.OnDeath -= CancelInvoke;
    }

    private void DropItem()
    {
        float randomNumber;
        randomNumber = Random.Range(0f, 1f);
        if (randomNumber < itemData.DropRate && !ItemHandler.ItemDropped[itemData.ItemId])
        {
            ObjectFactory.Instance.CreatePickup(transform, enemyData.SpecialItem);
            ItemHandler.ItemDropped[itemData.ItemId] = true;
        }
    }


}
