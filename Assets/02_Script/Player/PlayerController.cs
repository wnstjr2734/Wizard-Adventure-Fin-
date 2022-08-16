using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

/// <summary>
/// 플레이어 조작 처리를 담당하는 클래스
/// 주의 : 컨트롤러에 기능을 다 넣으면 컨트롤러 클래스가 매우 무거워지므로
/// 관련 기능끼리 묶어서 클래스로 따로 만들고 PlayerController에는 입력 관련 코드만 넣기 바람
/// </summary>
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
    [SerializeField] private float pressedHandPosZ = 0.5f;
    [SerializeField] private float releasedHandPosZ = 0.3f;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

    [SerializeField]
    private MagicShield magicShield;

    private Camera _main;
    private RaycastHit hit;

    private Vector2 rotation;
    private Vector2 look;
    private Vector2 move;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        Debug.Assert(magicShield, "Error : magic shield not set");
    }

    private void Start()
    {
        _main = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
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
            ShootMagic();
        }
        

        Look(look);
        Move(move);

        Teleport();
    }

    // 마우스 위치를 손(양손) 위치로 변환
    private void MousePosToHandPos()
    {
#if ENABLE_INPUT_SYSTEM
        Vector2 mousePosition = Mouse.current.position.ReadValue();

#else
        Vector2 mousePosition = Input.mousePosition;
#endif

        // 손 위치 조정
        float h = Screen.height;
        float w = Screen.width;
        float screenSpacePosX = (mousePosition.x - (w * 0.5f)) / w * 2;
        float screenSpacePosY = (mousePosition.y - (h * 0.5f)) / h * 2;
        leftHandTransform.localPosition = new Vector3(screenSpacePosX * handPosZ, screenSpacePosY * handPosZ, handPosZ);
        rightHandTransform.localPosition = new Vector3(screenSpacePosX * handPosZ, screenSpacePosY * handPosZ, handPosZ);

        // 손 각도 설정
        Vector3 eyePos = _main.transform.position;
        leftHandTransform.forward = leftHandTransform.position - eyePos;
        rightHandTransform.forward = rightHandTransform.position - eyePos;
    }

    private void ShootMagic()
    {
        if (playerInput.actions["Shoot Magic"].WasPressedThisFrame())
        {
            handPosZ = pressedHandPosZ;
        }
        if (playerInput.actions["Shoot Magic"].WasReleasedThisFrame())
        {
            handPosZ = releasedHandPosZ;
        }
    }

    private void Shield()
    {
        if (playerInput.actions["Shoot Magic"].WasPressedThisFrame())
        {
            //magicShield.
        }
        if (playerInput.actions["Shoot Magic"].WasReleasedThisFrame())
        {
        }
    }

    #region Movement
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
            // 왼손 컨트롤러를 기준으로 텔레포트를 쏜다
            // 지형을 대상으로만 움직일 수 있음
            if (Physics.Raycast(leftHandTransform.position, leftHandTransform.forward, 
                    out hit, 15, LayerMask.NameToLayer("Default") ))
            {
                // 해당 지형이 서있을 수 없는 곳이면(벽 등) 무시한다
                if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
                {
                    teleportTarget.position = hit.point + Vector3.up * 0.05f;
                    DrawTeleportLineCurve(footPos.localPosition, teleportTarget.localPosition);
                }
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

    // 텔레포트 시각화 - 이차 베지어 커브로 위치 보간
    private void DrawTeleportLineCurve(Vector3 startPos, Vector3 endPos)
    {
        // 베지어 커브 제어점 위치 = 둘 사이 거리 중간 위치 + 거리 사이 중간
        Vector3 mid = (endPos - startPos) * 0.5f;
        Vector3 controlPointPos = mid + Vector3.up * mid.magnitude;

        for (int i = 0; i < line.positionCount; i++)
        {
            float t = (float)i / line.positionCount;
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
    #endregion

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        rotation.y += rotate.x * scaledRotateSpeed;
        rotation.x = Mathf.Clamp(rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = rotation;
    }
}
