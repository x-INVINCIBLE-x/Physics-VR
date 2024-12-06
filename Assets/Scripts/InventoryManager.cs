using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<XRSocketTagInteractor, Weapons> socketItems = new();

    public void AddItemFromSocket(Weapons item, XRSocketTagInteractor socket)
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