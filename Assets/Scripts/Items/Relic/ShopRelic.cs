using UnityEngine;
using System.Collections;

public class ShopRelic: MonoBehaviour
{
    [SerializeField]
    private int cost = 100;
    public int Cost { get =>  cost ; private set => cost = value; }
}
