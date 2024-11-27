using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swing : MonoBehaviour
{
    public Transform startSwingPoint;
    public float maxDistance;
    public LayerMask swingLayer;

    public Transform predictionPoint;

    public InputActionProperty swingAction;
    public InputActionProperty pullAction;

    public float pullingStrngth = 500f;

    public Rigidbody playerRb;
    public LineRenderer lineRenderer;
    public SpringJoint joint;

    private Vector3 swingPoint;
    private bool hasHit;

    private void Update()
    {
        if (swingAction.action.WasPerformedThisFrame())
        {
            StartSwing();
        }
        else
        {
            StopSwing();
        }

        PullRope();
        DrawRope();
    }

    private void StartSwing()
    {
        if (!hasHit) return;

        joint = playerRb.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distance = Vector3.Distance(playerRb.position, swingPoint);
        joint.maxDistance = distance;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;
    }

    private void StopSwing()
    {
        Destroy(joint);
    }

    public void PullRope()
    {
        if (!joint) return;

        if (!pullAction.action.IsPressed()) return;

        Vector3 direction = (startSwingPoint.position - swingPoint).normalized;
        playerRb.AddForce(direction * pullingStrngth * Time.deltaTime);

        float distance = Vector3.Distance(playerRb.position, swingPoint);
        joint.maxDistance = distance;
    }

    private void GeSwingPoint()
    {
        if (joint)
        {
            predictionPoint.gameObject.SetActive(false);
            return;
        }

        RaycastHit raycastHit;

        hasHit = Physics.Raycast(startSwingPoint.position, startSwingPoint.forward, out raycastHit, maxDistance, swingLayer);

        if (hasHit)
        {
            swingPoint = raycastHit.point;
            predictionPoint.position = swingPoint;
            predictionPoint.gameObject.SetActive(true);
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }
    }

    private void DrawRope()
    {
        if (joint)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startSwingPoint.position);
            lineRenderer.SetPosition(1, swingPoint);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
