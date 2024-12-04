using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinousMovementPhysics : MonoBehaviour
{
    private PhysicsRig physicsRig;

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
    [Header("Control Options")]
    private bool isGrounded;
    public bool onlyMoveWhenGrounded = false; // if false then player can move and rotate in air also
    public bool jumpWithHand = false; // jump using hand and not only on key press

    [Header("Position and Rotation")]
    public float speed = 1;
    public float turnSpeed = 60f;

    [Space]
    [Header("Jump Info")]
    private float jumpVelocity;
    public float jumpHeight = 1.5f;
    public float minHandSpeedForJump;
    public float maxHandSpeedForJump;

    private void Awake()
    {
        physicsRig = GetComponent<PhysicsRig>();
    }

    private void Update()
    {
        //if(!IsGrounded())
        //    return;

        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;
        HandleJump();
    }
    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation()
    {
        //if (!onlyMoveWhenGrounded || (onlyMoveWhenGrounded && isGrounded))
        //    return;

        Quaternion raw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = raw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        Vector3 targetMovePosition = (rb.position + direction * Time.fixedDeltaTime * speed);

        float angle = turnSpeed * Time.deltaTime * inputTurnAxis;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);

        rb.MoveRotation(rb.rotation * q);

        Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

        rb.MovePosition(newPosition);
    }

    private void HandleJump()
    {
        bool jump = jumpInputSource.action.WasPerformedThisFrame();

        if (jumpWithHand)
        {
            bool currentJumpInput = jumpInputSource.action.IsPressed();

            float handSpeed = ((physicsRig.leftHandJointRB.velocity - rb.velocity).magnitude
                                + (physicsRig.rightHandJointRB.velocity - rb.velocity).magnitude) / 2;

            if (currentJumpInput) Debug.Log("HandSpeed" + handSpeed); // For Debugging Only

            if (currentJumpInput && isGrounded && handSpeed > minHandSpeedForJump)
            {
                Debug.Log("enter");
                //rb.velocity = Vector3.up * //Mathf.Clamp(handSpeed, minHandSpeedForJump, maxHandSpeedForJump);
                jumpVelocity = Mathf.Sqrt(1 * -Physics.gravity.y * jumpHeight);
                rb.velocity = Vector3.up * jumpVelocity;
            }
        }
        else
        {
            if (isGrounded && jump)
            {
                jumpVelocity = Mathf.Sqrt(1 * -Physics.gravity.y * jumpHeight);
                rb.velocity = Vector3.up * jumpVelocity;
            }
        }
    }


    public bool IsGrounded()
    {
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.05f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
