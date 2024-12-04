using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum TargetTag
{
    EnergyKey,
    Weapon
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
        if (targetTag != TargetTag.Weapon) return;
        //args.interactableObject.transform.localPosition = transform.localPosition;
        if (args.interactableObject.transform.TryGetComponent(out Weapons weapons))
        {
            item = weapons.inventoryItem;
            if (weapons.isInventoryItem) return;
        }
        
        currentWeapon = args.interactableObject.transform.gameObject;
        Debug.Log(args.interactableObject.transform.gameObject.transform.localScale);
        args.interactableObject.transform.gameObject.transform.localScale = (currentWeapon.transform.localScale / 10);
        Debug.Log(args.interactableObject.transform.gameObject.transform.localScale);

        //inventoryManager.slot1Weapon = args.interactableObject.transform.gameObject;
        //Debug.Log(item.inventoryModel.name); 
        //inventoryManager.AddItemFromSocket(item, this);
        //ReplaceObject(args.interactableObject.transform.gameObject, item.inventoryModel);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        //inventoryManager.slot1Weapon = null;
        if (targetTag != TargetTag.Weapon) return;

        if (args.interactorObject.transform.TryGetComponent(out Weapons weapons))
        {
            if (!weapons.isInventoryItem) return;
        }
        Debug.Log(inventoryManager.GetItem(this).inGameModel);
        ReplaceObject(args.interactableObject.transform.gameObject, inventoryManager.GetItem(this).inGameModel);
        
        inventoryManager.RemoveItemFromSocket(this);
        currentWeapon = null;
    }

    private void ReplaceObject(GameObject oldObject, GameObject newObjectPrefab)
    {
        //Transform oldTransform = oldObject.transform;
        //GameObject newObject = Instantiate(newObjectPrefab, oldTransform.position, oldTransform.rotation, oldTransform.parent);
        //Destroy(oldObject);

        oldObject.transform.localScale = oldObject.transform.localScale / 10;
    }

    protected override void OnEnable()
    {
        if (targetTag == TargetTag.Weapon)
        {
            if (inventoryManager.slot1Weapon != null)
            {
                //currentWeapon = inventoryManager.slot1Weapon;
                item = inventoryManager.GetItem(this);

                if (item != null)
                    currentWeapon = Instantiate(item.inventoryModel);
                //currentWeapon.SetActive(true);
                //currentWeaapon.transform.localPosition = transform.localPosition;
                if (currentWeapon != null)
                {
                    Transform attachTransform = this.attachTransform ?? this.transform;

                    currentWeapon.transform.SetParent(attachTransform);
                    currentWeapon.transform.localPosition = Vector3.zero; 
                    currentWeapon.transform.localRotation = Quaternion.identity;
                }

            }

        }

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        if (targetTag == TargetTag.Weapon)
        {
            if (currentWeapon != null)
            {
                Destroy(currentWeapon);
            }

        }
        base.OnDisable();
    }
}
