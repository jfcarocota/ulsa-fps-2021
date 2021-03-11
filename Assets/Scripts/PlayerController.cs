using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    PlayerInputs playerInputs;
    [SerializeField, Range(0.1f, 10f)]
    float moveSpeed = 5f;
    [SerializeField]
    Transform camTrs;
    [SerializeField, Range(0.1f, 180f)]
    float camRotSpeed = 90f;
    float camRotationAmounthY;
    float camRotationAmounthX;
    [SerializeField, Range(-150, 150)]
    float camMaxRotation = 45f;
    [SerializeField, Range(0.1f, 15f)]
    float jumpForce = 5f;
    [SerializeField, Range(0.01f, 10f)]
    float rayLength = 1f;
    [SerializeField]
    Color rayColor = Color.magenta;
    [SerializeField]
    LayerMask detectionLayer;
    [SerializeField]
    Vector3 rayPosition;
    [SerializeField, Range(1f, 5f)]
    float augmentedFactor = 1f;
    float augmentedSpeed = 1f;
    float baseSpeed = 1f;

    void Awake()
    {
        rb ??= GetComponent<Rigidbody>();
        playerInputs ??= new PlayerInputs();
    }

    void OnEnable()
    {
        playerInputs?.Enable();
    }

    void OnDisable()
    {
        playerInputs?.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInputs.Gameplay.Jump.performed += _=> Jump();
        playerInputs.Gameplay.Run.performed += _=> augmentedSpeed = augmentedFactor;
        playerInputs.Gameplay.Run.canceled += _=> augmentedSpeed = baseSpeed;
    }

    void Update()
    {
        camRotationAmounthX -= CamAxis.y * camRotSpeed * Time.deltaTime;
        camRotationAmounthX = Mathf.Clamp(camRotationAmounthX, -camMaxRotation, camMaxRotation);
        camTrs.localRotation = Quaternion.Euler(camRotationAmounthX, camTrs.rotation.y, camTrs.rotation.z);
        camRotationAmounthY += CamAxis.x * camRotSpeed * Time.deltaTime;
        rb.rotation = Quaternion.Euler(rb.rotation.x, camRotationAmounthY, rb.rotation.z);
        rb.position += Forward * moveSpeed * augmentedSpeed * Time.deltaTime;
    }

    void Jump()
    {
        if(!Grounding) return;
        rb.AddForce(JumpDirection, ForceMode.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(RelativeRayPosition, -transform.up * rayLength);
    }

    bool Grounding => Physics.Raycast(RelativeRayPosition, -transform.up, rayLength, detectionLayer);

    Vector2 Axis => playerInputs.Gameplay.Movement.ReadValue<Vector2>();

    float CamAxisX => playerInputs.Gameplay.CamAxisX.ReadValue<float>();
    float CamAxisY => playerInputs.Gameplay.CamAxisY.ReadValue<float>();

    Vector2 CamAxis => new Vector2(CamAxisX, CamAxisY);

    Vector3 MovementAxis => new Vector3(Axis.x, 0f, Axis.y);

    Vector3 Forward => rb.rotation * MovementAxis;
    Vector3 JumpDirection => Vector3.up * jumpForce;

    Vector3 RelativeRayPosition => rayPosition + transform.position;
}
