using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public sealed class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float groundAcceleration;

    [Space]
    [SerializeField] float airMoveForce;

    [Space]
    [SerializeField] float jumpHeight;
    [SerializeField] float upGravity;
    [SerializeField] float downGravity;
    [SerializeField] float jumpSpringPauseTime;

    [Space]
    [SerializeField] float springDistance;
    [SerializeField] float springForce;
    [SerializeField] float springDamper;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundCheckMask;

    bool previousJumpState;
    float lastJumpTime;

    public float MoveSpeed => moveSpeed;
    public Rigidbody DrivingRigidbody { get; private set; }

    public Vector3 MoveDirection { get; set; }
    public bool JumpState { get; set; }
    public float DistanceToGround { get; private set; }
    public bool IsGrounded => DistanceToGround < springDistance;

    private void Awake()
    {
        DrivingRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        DistanceToGround = GetDistanceToGround();

        MoveCharacter();

        if (JumpState && !previousJumpState)
        {
            Jump();
        }
        previousJumpState = JumpState;

        ApplySpring();
        ApplyGravity();
    }

    private void ApplySpring()
    {
        if (IsGrounded && Time.time > lastJumpTime + jumpSpringPauseTime)
        {
            float contraction = 1.0f - (DistanceToGround / springDistance);
            DrivingRigidbody.velocity += Vector3.up * contraction * springForce * Time.deltaTime;
            DrivingRigidbody.velocity -= Vector3.up * DrivingRigidbody.velocity.y * springDamper * Time.deltaTime;
        }
    }

    private void ApplyGravity()
    {
        DrivingRigidbody.useGravity = false;
        DrivingRigidbody.velocity += GetGravity() * Time.deltaTime;
    }

    private void MoveCharacter()
    {
        if (IsGrounded)
        {
            Vector3 target = MoveDirection * moveSpeed;
            Vector3 current = DrivingRigidbody.velocity;

            Vector3 delta = Vector3.ClampMagnitude(target - current, moveSpeed);
            delta.y = 0.0f;

            Vector3 force = delta / moveSpeed * groundAcceleration;

            DrivingRigidbody.velocity += force * Time.deltaTime;
        }
        else
        {
            DrivingRigidbody.velocity += MoveDirection * airMoveForce * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (IsGrounded)
        {
            float gravity = Vector3.Dot(Vector3.down, GetGravity());
            float jumpForce = Mathf.Sqrt(2.0f * gravity * jumpHeight);
            DrivingRigidbody.velocity = new Vector3(DrivingRigidbody.velocity.x, jumpForce, DrivingRigidbody.velocity.z);

            lastJumpTime = Time.time;
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

    public float GetDistanceToGround()
    {
        if (Physics.SphereCast(DrivingRigidbody.position + Vector3.up * groundCheckRadius, groundCheckRadius, Vector3.down, out var hit, 1000.0f, groundCheckMask))
        {
            return hit.distance;
        }
        else return float.PositiveInfinity;
    }

    private void OnDrawGizmosSelected()
    {
        if (!DrivingRigidbody) DrivingRigidbody = GetComponent<Rigidbody>();
        float dist = GetDistanceToGround();
        Gizmos.color = dist < springDistance ? Color.green : Color.red;

        Gizmos.DrawRay(transform.position, Vector3.down * dist);

        Gizmos.color = Color.white;
    }
}
