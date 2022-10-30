using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponADS : MonoBehaviour
{
    [SerializeField] float adsSpeed;
    [SerializeField] AnimationCurve curve;

    [Space]
    [SerializeField] Animator animator;
    [SerializeField] string adsParameterName;

    [Space]
    [SerializeField] float fovOverride;
    [SerializeField] float baseViewmodelFOV;
    [SerializeField] float aimViewmodelFOV;

    WeaponAmmo ammo;

    float adsPercent;
    FPCameraController cameraController;

    public bool ADSState { get; set; }
    public float ADSPercent => curve.Evaluate(adsPercent);

    private void OnEnable()
    {
        cameraController = GetComponentInParent<FPCameraController>();
        ammo = GetComponent<WeaponAmmo>();
    }

    private void Update()
    {
        bool adsState = ADSState;
        if (ammo)
        {
            if (ammo.Reloading)
            {
                adsState = false;
            }
        }

        adsPercent = Mathf.MoveTowards(adsPercent, adsState ? 1.0f : 0.0f, adsSpeed * Time.deltaTime);

        if (animator)
        {
            animator.SetFloat(adsParameterName, ADSPercent);
        }

        if (cameraController)
        {
            cameraController.FOVOverride = Mathf.Lerp(cameraController.FOVOverride, fovOverride, ADSPercent);
            cameraController.ViewmodelFOV = Mathf.Lerp(baseViewmodelFOV, aimViewmodelFOV, ADSPercent);
        }
    }
}