using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPanelControls : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCanvas;
    private bool inventortStatus = false;

    public void ToggleInventory()
    {
        inventortStatus = !inventortStatus;
        inventoryCanvas.SetActive(inventortStatus);
    }

    public void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
    }
}
