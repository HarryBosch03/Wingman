using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class FPCameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform cameraRotor;

    [Space]
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] RenderObjects[] viewmodelRenderObjects;

    public Vector2 ScreenSpaceRotation { get; private set; }
    public float FOVOverride { get; set; }
    public float ViewmodelFOV { get; set; }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Vector2 ssr = ScreenSpaceRotation;

        if (Mouse.current != null) ssr += Mouse.current.delta.ReadValue() * mouseSensitivity;

        ssr.y = Mathf.Clamp(ssr.y, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(0.0f, ssr.x, 0.0f);
        cameraRotor.rotation = Quaternion.Euler(-ssr.y, ssr.x, 0.0f);

        ScreenSpaceRotation = ssr;
    }

    private void LateUpdate()
    {
        vcam.m_Lens.FieldOfView = FOVOverride;
        FOVOverride = 90.0f;

        foreach (var viewmodelRenderObject in viewmodelRenderObjects)
        {
            viewmodelRenderObject.settings.cameraSettings.cameraFieldOfView = ViewmodelFOV;
        }
    }
}
