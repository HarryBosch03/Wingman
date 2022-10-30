using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public class WeaponAnimator : MonoBehaviour
{
    public Transform root;

    [Space]
    public OccilatorV3 idleTranslation;
    public OccilatorV3 idleRotation;

    [Space]
    public OccilatorV3 movingTranslation;
    public OccilatorV3 movingRotation;

    [Space]
    public float weaponSway;
    public float weaponSwaySmoothing;

    [Space]
    public float groundedSmoothing;

    [Space]
    public Vector3 shootImpulseA;
    public Vector3 shootImpulseB;

    [Space]
    public Vector2 shootTorqueA;
    public Vector2 shootTorqueB;

    [Space]
    public float recenteringForce;
    public float recenteringDrag;

    [Space]
    public Animator animator;
    public string reloadAnimationName;
    public float reloadAnimationBlend;

    FPCameraController cameraController;
    CharacterMovement movement;

    protected WeaponEffect effect;
    protected WeaponAmmo ammo;

    Vector2 lastCamRotation;
    Vector2 swayPosition;
    Vector2 swayVelocity;

    float grounded;
    float groundedVelocity;

    Vector3 shootOffset;
    Vector2 shootRotation;

    Vector3 shootVelocity;
    Vector2 shootTorque;

    protected virtual void Awake()
    {
        cameraController = GetComponentInParent<FPCameraController>();
        movement = GetComponentInParent<CharacterMovement>();

        effect = GetComponent<WeaponEffect>();
        ammo = GetComponent<WeaponAmmo>();
    }

    protected virtual void OnEnable()
    {
        effect.ExecuteEvent += OnUse;
        ammo.ReloadEvent += OnReload;
    }

    protected virtual void OnDisable()
    {
        effect.ExecuteEvent -= OnUse;
        ammo.ReloadEvent -= OnReload;
    }

    protected virtual void OnUse()
    {
        shootVelocity += new Vector3
        {
            x = Random.Range(shootImpulseA.x, shootImpulseB.x),
            y = Random.Range(shootImpulseA.y, shootImpulseB.y),
            z = Random.Range(shootImpulseA.z, shootImpulseB.z),
        };

        shootTorque += new Vector2
        {
            x = Random.Range(shootTorqueA.x, shootTorqueB.x),
            y = Random.Range(shootTorqueA.y, shootTorqueB.y),
        };
    }

    protected virtual void OnReload()
    {
        animator.CrossFade(reloadAnimationName, reloadAnimationBlend);
    }

    protected virtual void LateUpdate()
    {
        ApplyMovementAnimation();

        root.localPosition += shootOffset;
        root.localRotation *= Quaternion.Euler(shootRotation.y, 0.0f, -shootRotation.x);

        shootOffset += shootVelocity * Time.deltaTime;
        shootRotation += shootTorque * Time.deltaTime;

        shootVelocity += -shootOffset * recenteringForce * Time.deltaTime;
        shootTorque += -shootRotation * recenteringForce * Time.deltaTime;

        shootVelocity -= shootVelocity * recenteringDrag * Time.deltaTime;
        shootTorque -= shootTorque * recenteringDrag * Time.deltaTime;
    }

    private void ApplyMovementAnimation()
    {
        Vector3 velocity = movement.DrivingRigidbody.velocity;
        float normalizedSpeed = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z) / movement.MoveSpeed;

        grounded = Mathf.SmoothDamp(grounded, movement.IsGrounded ? 1.0f : 0.0f, ref groundedVelocity, groundedSmoothing);

        root.localPosition += Vector3.Lerp(idleTranslation, movingTranslation, normalizedSpeed) * grounded;
        root.rotation *= Quaternion.Slerp(Quaternion.Euler(idleTranslation.Get() * grounded), Quaternion.Euler(movingTranslation.Get() * grounded), normalizedSpeed);

        Vector2 camDelta = cameraController.ScreenSpaceRotation - lastCamRotation;
        swayPosition = Vector2.SmoothDamp(swayPosition, camDelta * weaponSway, ref swayVelocity, weaponSwaySmoothing);

        root.rotation *= Quaternion.Euler(swayPosition.y, 0.0f, swayPosition.x);
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