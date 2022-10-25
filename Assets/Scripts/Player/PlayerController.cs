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

    Vector2 moveInput;

    public GameObject Avatar { get; private set; }

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
        if (!Avatar) return;

        if (Avatar.TryGetComponent(out CharacterMovement movement))
        {
            movement.JumpState = value.Get<float>() > 0.5f;
        }
    }

    public void OnShoot(InputValue value)
    {
        if (!Avatar) return;

        if (Avatar.TryGetComponent(out WeaponManager weaponManager))
        {
            weaponManager.UseState = value.Get<float>() > 0.5f;
        }
    }

    public void OnReload(InputValue value)
    {
        if (!Avatar) return;

        if (Avatar.TryGetComponent(out WeaponManager weaponManager))
        {
            weaponManager.ReloadState = value.Get<float>() > 0.5f;
        }
    }

    private void Update()
    {
        UpdateAvatar();

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
