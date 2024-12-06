using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum TargetTag
{
    EnergyKey,
    Weapon,
    InventoryItem
}

public class XRSocketTagInteractor : XRSocketInteractor
{
    public TargetTag targetTag;
    public GameObject currentWeapon;
    public InventoryItem item;
    public InventoryManager inventoryManager;

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.tag == targetTag.ToString();
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.tag == targetTag.ToString();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (targetTag != TargetTag.InventoryItem) return;

        if (args.interactableObject.transform.TryGetComponent(out Weapons weapon))
        {
            Debug.Log("Can't find weapon from Inventory Item");
            return;
        }

        currentWeapon = args.interactableObject.transform.gameObject;
        inventoryManager.AddItemFromSocket(weapon, this);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (targetTag != TargetTag.InventoryItem) return;

        inventoryManager.RemoveItemFromSocket(this);
        currentWeapon = null;
    }

    protected override void OnEnable()
    {
        item = inventoryManager.GetItem(this);

        if (item != null)
        {
            currentWeapon = Instantiate(item.gameObject, transform.position, transform.rotation);
            item.inventoryModel.SetActive(true);
            item.inGameModel.SetActive(false);
            item.isInventoryItem = true;
        }

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        base.OnDisable();
    }
}
