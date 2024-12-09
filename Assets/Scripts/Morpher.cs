using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morpher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.TryGetComponent(out InventoryItem item))
        {
            Debug.Log("Got component");
            if (item.isInventoryItem) return;
            
            item.isInventoryItem = true;
            item.inventoryModel.SetActive(true);
            item.inGameModel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out InventoryItem item))
        {
            if (!item.isInventoryItem) return;

            item.isInventoryItem = false;
            item.inGameModel.SetActive(true);
            item.inventoryModel.SetActive(false);
        }
    }
}
