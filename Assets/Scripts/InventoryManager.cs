using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<XRSocketTagInteractor, InventoryItem> socketItems;
    public GameObject slot1Weapon;
    public GameObject slot2Weapon;
    public GameObject slot3Weapon;
    public GameObject slot4Weapon;

    private void Awake()
    {
        socketItems = new();
    }

    public void AddItemFromSocket(InventoryItem item, XRSocketTagInteractor socket)
    {
        socketItems.Add(socket, item);
    }

    public void RemoveItemFromSocket(XRSocketTagInteractor socket)
    {
        if (!socketItems.ContainsKey(socket))
        {
            Debug.Log("Item to remove is not in Inventory");
            return;
        }

        socketItems.Remove(socket);
    }

    public InventoryItem GetItem(XRSocketTagInteractor item)
    {
        if (!socketItems.ContainsKey(item))
        {
            Debug.Log("Socket with no item requesting Inventory Item");
            return null;
        }

        return socketItems[item];
    }
}