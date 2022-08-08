using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Teleport")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private Transform footPos;
    private Camera _main;
    private RaycastHit hit;

    private Vector2 m_Rotation;
    private Vector2 m_Look;
    private Vector2 m_Move;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        _main = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        m_Look = context.ReadValue<Vector2>();
    }

    public void OnTeleport(InputAction.CallbackContext context)
    {
        print("Teleport");
    }

    public void Update()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        Look(m_Look);
        Move(m_Move);

        Teleport();
    }

    private void Teleport()
    {
        if (Physics.Raycast(_main.transform.position, _main.transform.forward, out hit, 15))
        {
            line.SetPosition(0, _main.transform.position);
            line.SetPosition(1, hit.point);

            teleportTarget.position = hit.point + Vector3.up * 0.05f;
        }

        if (playerInput.actions["Teleport"].WasPressedThisFrame())
        {
            line.gameObject.SetActive(true);
            teleportTarget.gameObject.SetActive(true);
            print("Get Down Teleport");
        }
        if (playerInput.actions["Teleport"].WasReleasedThisFrame())
        {
            print("Get Up Teleport");
            line.gameObject.SetActive(false);
            teleportTarget.gameObject.SetActive(false);

            transform.position = line.GetPosition(1) - footPos.localPosition;
        }
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += rotate.x * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation;
    }
}
