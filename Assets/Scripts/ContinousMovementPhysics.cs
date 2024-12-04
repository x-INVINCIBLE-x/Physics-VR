using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinousMovementPhysics : MonoBehaviour
{
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;

    public LayerMask groundLayer;
    public Rigidbody rb;

    public float speed = 1;
    public float turnSpeed = 60f;

    public Transform directionSource;
    public Transform turnSource;
    public CapsuleCollider bodyCollider;

    private Vector2 inputMoveAxis;
    private float inputTurnAxis;

    //private void Update()
    //{
        
    //}

    private void Update()
    {
        //if(!IsGrounded())
        //    return;

        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;
        Quaternion raw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = raw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        Vector3 targetMovePosition =  (rb.position + direction * Time.deltaTime * speed);

        float angle = turnSpeed * Time.deltaTime * inputTurnAxis;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);

        rb.MoveRotation(rb.rotation * q);

        Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

        rb.MovePosition(newPosition);
    }

    public bool IsGrounded()
    {
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.05f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
