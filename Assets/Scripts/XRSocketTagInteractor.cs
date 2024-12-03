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
        Debug.Log(args.interactableObject.transform.name);
        //args.interactableObject.transform.parent = this.transform;
        args.interactableObject.transform.localPosition = transform.localPosition;
        currentWeapon = args.interactableObject.transform.gameObject;
        inventoryManager.slot1Weapon = args.interactableObject.transform.gameObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        //inventoryManager.slot1Weapon = null;
        if (targetTag != TargetTag.Weapon) return;
        currentWeapon = null;
    }

    protected override void OnEnable()
    {
        if (targetTag == TargetTag.Weapon)
        {
        if (inventoryManager.slot1Weapon != null)
        {
            currentWeapon = inventoryManager.slot1Weapon;
            currentWeapon.SetActive(true);
            //currentWeaapon.transform.localPosition = transform.localPosition;
            if (currentWeapon != null)
            {
                // Use the attachTransform of the socket if available; otherwise, default to the socket's transform
                Transform attachTransform = this.attachTransform != null ? this.attachTransform : this.transform;

                // Set the weapon's parent to the socket's attachTransform
                currentWeapon.transform.SetParent(attachTransform);
                currentWeapon.transform.localPosition = Vector3.zero; // Adjust position relative to the socket
                currentWeapon.transform.localRotation = Quaternion.identity; // Adjust rotation relative to the socket
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
            currentWeapon.SetActive(false);
            //inventoryManager.slot1Weapon = currentWeaapon;
        }

        }
        base.OnDisable();
    }
}
