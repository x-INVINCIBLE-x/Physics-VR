using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRig : MonoBehaviour
{
    public Transform playerHead;
    public Transform leftController;
    public Transform rightController;

    public ConfigurableJoint leftHandJoint;
    public ConfigurableJoint rightHandJoint;
    public ConfigurableJoint headJoint;

    public Rigidbody leftHandJointRB;
    public Rigidbody rightHandJointRB;

    public CapsuleCollider bodyColliser;

    public float minHeight;
    public float maxHeight;

    private void FixedUpdate()
    {
        bodyColliser.height = Mathf.Clamp(playerHead.localPosition.y, minHeight, maxHeight);
        bodyColliser.center = new Vector3 (playerHead.localPosition.x, bodyColliser.height/2, playerHead.localPosition.z);

        //leftHandJoint.targetPosition = leftController.localPosition;
        //leftHandJoint.targetRotation = leftController.localRotation;

        //rightHandJoint.targetPosition = rightController.localPosition;
        //rightHandJoint.targetRotation = rightController.localRotation;

        //headJoint.targetPosition = playerHead.localPosition;
    }
}
