using System;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponAnimator : MonoBehaviour
{
    public Transform root;
    public Transform slide;

    [Space]
    public OccilatorV3 idleTranslation;
    public OccilatorV3 idleRotation;

    [Space]
    public OccilatorV3 movingTranslation;
    public OccilatorV3 movingRotation;

    [Space]
    public Vector3 slideRackOffset;

    [Space]
    public float weaponSway;
    public float weaponSwaySmoothing;

    [Space]
    public float groundedSmoothing;

    FPCameraController cameraController;
    CharacterMovement movement;

    Vector2 lastCamRotation;
    Vector2 swayPosition;
    Vector2 swayVelocity;

    float grounded;
    float groundedVelocity;

    private void Awake()
    {
        cameraController = GetComponentInParent<FPCameraController>();
        movement = GetComponentInParent<CharacterMovement>();
    }

    private void LateUpdate()
    {
        ApplyRootPostProcessing();
    }

    private void ApplyRootPostProcessing()
    {
        Vector3 velocity = movement.DrivingRigidbody.velocity;
        float normalizedSpeed = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z) / movement.MoveSpeed;

        grounded = Mathf.SmoothDamp(grounded, movement.IsGrounded ? 1.0f : 0.0f, ref groundedVelocity, groundedSmoothing);

        root.localPosition += Vector3.Lerp(idleTranslation, movingTranslation, normalizedSpeed) * grounded;
        root.rotation *= Quaternion.Slerp(Quaternion.Euler(idleTranslation.Get() * grounded), Quaternion.Euler(movingTranslation.Get() * grounded), normalizedSpeed);

        Vector2 camDelta = cameraController.ScreenSpaceRotation - lastCamRotation;
        swayPosition = Vector2.SmoothDamp(swayPosition, camDelta * weaponSway, ref swayVelocity, weaponSwaySmoothing);

        root.rotation *= Quaternion.Euler(swayPosition.y, swayPosition.x, swayPosition.x);
        lastCamRotation = cameraController.ScreenSpaceRotation;
    }
}

[System.Serializable]
public class OccilatorV3
{
    public Occilator x;
    public Occilator y;
    public Occilator z;

    public Vector3 Get() => Get(Time.time);

    public Vector3 Get(float t)
    {
        return new Vector3(x ? x.Get(t) : 0.0f, y ? y.Get(t) : 0.0f, z ? z.Get(t) : 0.0f);
    }

    public static implicit operator Vector3(OccilatorV3 occilator)
    {
        if (occilator == null) return Vector3.zero;
        return occilator.Get();
    }
}