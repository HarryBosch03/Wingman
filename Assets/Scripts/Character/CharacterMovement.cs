using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public sealed class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;

    [Space]
    [SerializeField] float jumpHeight;
    [SerializeField] float upGravity;
    [SerializeField] float downGravity;

    [Space]
    [SerializeField] float groundCheckRadius;
    [SerializeField] float groundCheckOffset;
    [SerializeField] LayerMask groundCheckMask;

    [Space]
    [SerializeField] Transform top;
    [SerializeField] float topTargetHeight;
    [SerializeField] float topSpring;

    float topHeight;
    bool previousJumpState;

    public float MoveSpeed => moveSpeed;
    public Rigidbody DrivingRigidbody { get; private set; }

    public Vector3 MoveDirection { get; set; }
    public bool JumpState { get; set; }
    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        DrivingRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        IsGrounded = GetIsGrounded();

        MoveCharacter();

        if (JumpState && !previousJumpState)
        {
            Jump();
        }
        previousJumpState = JumpState;

        ApplyGravity();
    }

    private void Update()
    {
        topHeight += (transform.position.y + topTargetHeight - topHeight) * topSpring * Time.deltaTime;
        top.transform.position = new Vector3(transform.position.x, topHeight, transform.position.z);
    }

    private void ApplyGravity()
    {
        DrivingRigidbody.useGravity = false;
        DrivingRigidbody.velocity += GetGravity() * Time.deltaTime;
    }

    private void MoveCharacter()
    {
        Vector3 target = MoveDirection * moveSpeed;
        Vector3 current = DrivingRigidbody.velocity;

        Vector3 delta = Vector3.ClampMagnitude(target - current, moveSpeed);
        delta.y = 0.0f;

        Vector3 force = delta / moveSpeed * acceleration;

        DrivingRigidbody.velocity += force * Time.deltaTime;
    }

    private void Jump ()
    {
        if (IsGrounded)
        {
            float gravity = Vector3.Dot(Vector3.down, GetGravity());
            float jumpForce = Mathf.Sqrt(2.0f * gravity * jumpHeight);
            DrivingRigidbody.velocity = new Vector3(DrivingRigidbody.velocity.x, jumpForce, DrivingRigidbody.velocity.z);
        }
    }

    private Vector3 GetGravity()
    {
        float scale = upGravity;
        if (!JumpState)
        {
            scale = downGravity;
        }
        else if (DrivingRigidbody.velocity.y < 0.0f)
        {
            scale = downGravity;
        }

        return Physics.gravity * scale;
    }

    public bool GetIsGrounded ()
    {
        foreach (var query in Physics.OverlapSphere(transform.position + Vector3.up * groundCheckOffset, groundCheckRadius, groundCheckMask))
        {
            if (query.transform.root != transform.root)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GetIsGrounded() ? Color.green : Color.red;

        Gizmos.DrawSphere(transform.position + Vector3.up * groundCheckOffset, groundCheckRadius);

        Gizmos.color = Color.white;
    }
}
