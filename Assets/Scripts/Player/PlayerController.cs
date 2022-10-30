using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject avatarPrefab;

    PlayerInput input;
    Vector2 moveInput;
    
    public GameObject Avatar { get; private set; }
    public static HashSet<object> InputBlockers { get; } = new HashSet<object>();

    public void Spawn (Vector3 point, bool force = false)
    {
        if (Avatar)
        {
            if (force)
            {
                Destroy(Avatar);
            }
            else return;
        }

        Avatar = Instantiate(avatarPrefab, point, Quaternion.identity);
    }

    public void OnMove (InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    public void OnJump (InputValue value)
    {
        SetStateOnAvatarComponent<CharacterMovement>((m, v) => m.JumpState = v, value);
    }

    public void OnShoot(InputValue value)
    {
        SetStateOnAvatarComponent<WeaponManager>((wm, v) => wm.UseState = v, value);
    }

    public void OnReload(InputValue value)
    {
        SetStateOnAvatarComponent<WeaponManager>((wm, v) => wm.ReloadState = v, value);
    }

    public void OnAim(InputValue value)
    {
        SetStateOnAvatarComponent<WeaponManager>((wm, v) => wm.AimState = v, value);
    }

    public void SetStateOnAvatarComponent<T> (System.Action<T, bool> method, InputValue value)
    {
        if (!Avatar) return;

        T component = Avatar.GetComponentInChildren<T>();
        if (component != null)
        {
            method(component, value.Get<float>() > 0.5f);
        }
    }

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        UpdateAvatar();

        if (input)
        {
            input.enabled = InputBlockers.Count == 0;
        }

        if (Avatar)
        {
            if (Avatar.TryGetComponent(out FPCameraController cameraController))
            {
                cameraController.enabled = InputBlockers.Count == 0;
            }
        }

        if (Keyboard.current.numpad7Key.wasPressedThisFrame)
        {
            Spawn(Vector3.zero);
        }
    }

    private void UpdateAvatar()
    {
        if (!Avatar) return;

        if (Avatar.TryGetComponent(out CharacterMovement movement))
        {
            movement.MoveDirection = Avatar.transform.TransformDirection(moveInput.x, 0.0f, moveInput.y);
        }
    }
}
