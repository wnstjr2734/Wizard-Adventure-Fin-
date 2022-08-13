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

    [Header("For Debug")] 
    [SerializeField] private bool pcMode = true;
    [SerializeField] private float handPosZ = 0.3f;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

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
        // Debug : Mouse To Hand
        if (pcMode)
        {
            MousePosToHandPos();
        }

        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        Look(m_Look);
        Move(m_Move);

        Teleport();
    }

    // ���콺 ��ġ�� ��(���) ��ġ�� ��ȯ
    private void MousePosToHandPos()
    {
#if ENABLE_INPUT_SYSTEM
        Vector2 mousePosition = Mouse.current.position.ReadValue();

#else
        Vector2 mousePosition = Input.mousePosition;
#endif

        // �� ��ġ ����
        float h = Screen.height;
        float w = Screen.width;
        float screenSpacePosX = (mousePosition.x - (w * 0.5f)) / w * 2;
        float screenSpacePosY = (mousePosition.y - (h * 0.5f)) / h * 2;
        leftHandTransform.localPosition = new Vector3(screenSpacePosX * handPosZ, screenSpacePosY * handPosZ, handPosZ);
        rightHandTransform.localPosition = new Vector3(screenSpacePosX * handPosZ, screenSpacePosY * handPosZ, handPosZ);

        // �� ���� ����
        Vector3 eyePos = _main.transform.position;
        leftHandTransform.forward = leftHandTransform.position - eyePos;
        rightHandTransform.forward = rightHandTransform.position - eyePos;
    }

    private void Teleport()
    {
        if (playerInput.actions["Teleport"].WasPressedThisFrame())
        {
            line.gameObject.SetActive(true);
            teleportTarget.gameObject.SetActive(true);
            print("Get Down Teleport");
        }
        if (playerInput.actions["Teleport"].IsPressed())
        {
            // �޼� ��Ʈ�ѷ��� �������� �ڷ���Ʈ�� ���
            if (Physics.Raycast(leftHandTransform.position, leftHandTransform.forward, out hit, 15,
                    ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                teleportTarget.position = hit.point + Vector3.up * 0.05f;
                DrawTeleportLineCurve(footPos.localPosition, teleportTarget.localPosition);
            }
        }
        if (playerInput.actions["Teleport"].WasReleasedThisFrame())
        {
            print("Get Up Teleport");
            line.gameObject.SetActive(false);
            teleportTarget.gameObject.SetActive(false);

            transform.position = teleportTarget.position - footPos.localPosition;
        }
    }

    // �ڷ���Ʈ �ð�ȭ - ���� ������ Ŀ��� ��ġ ����
    private void DrawTeleportLineCurve(Vector3 startPos, Vector3 endPos)
    {
        // ������ Ŀ�� ������ ��ġ = �� ���� �Ÿ� �߰� ��ġ + �Ÿ� ���� �߰�
        Vector3 mid = (endPos - startPos) * 0.5f;
        Vector3 controlPointPos = mid + Vector3.up * mid.magnitude;

        for (int i = 0; i < line.positionCount; i++)
        {
            float t = (float) i / line.positionCount;
            var m = Vector3.Lerp(startPos, controlPointPos, t);
            var n = Vector3.Lerp(controlPointPos, endPos, t);
            var b = Vector3.Lerp(m, n, t);

            line.SetPosition(i, b);
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
