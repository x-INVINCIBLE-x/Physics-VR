using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinousMovementPhysics : MonoBehaviour
{
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;

    public LayerMask groundLayer;
    public Rigidbody rb;


    public Transform directionSource;
    public Transform turnSource;
    public CapsuleCollider bodyCollider;

    private Vector2 inputMoveAxis;
    private float inputTurnAxis;

    [Space]
    private bool isGrounded;
    public bool onlyMoveWhenGrounded = false;

    public float speed = 1;
    public float turnSpeed = 60f;

    private float jumpVelocity;
    public float jumpHeight = 1.5f;

    private void Update()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;

        bool jump = jumpInputSource.action.WasPerformedThisFrame();

        if (isGrounded && jump)
        {
            jumpVelocity = Mathf.Sqrt(1 * -Physics.gravity.y * jumpHeight);
            rb.velocity = Vector3.up * jumpVelocity;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

        if(!onlyMoveWhenGrounded || (onlyMoveWhenGrounded && isGrounded))
            return;

        Quaternion raw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = raw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        Vector3 targetMovePosition =  (rb.position + direction * Time.fixedDeltaTime * speed);

        float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;
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
