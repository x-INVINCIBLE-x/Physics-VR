using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public GameObject inGameModel;
    public GameObject inventoryModel;
}