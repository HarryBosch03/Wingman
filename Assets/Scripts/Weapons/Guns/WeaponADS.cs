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

    float adsPercent;

    public bool ADSState { get; set; }
    public float ADSPercent => curve.Evaluate(adsPercent);

    private void Update()
    {
        adsPercent = Mathf.MoveTowards(adsPercent, ADSState ? 1.0f : 0.0f, adsSpeed * Time.deltaTime);

        if (animator)
        {
            animator.SetFloat(adsParameterName, adsPercent);
        }
    }
}