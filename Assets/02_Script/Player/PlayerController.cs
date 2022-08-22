using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MagicShield magicShield;

    [Header("For Debug")] 
    [SerializeField] private bool pcMode = true;
    [SerializeField] private float handPosZ = 0.3f;
    [SerializeField] private float pressedHandPosZ = 0.5f;
    [SerializeField] private float releasedHandPosZ = 0.3f;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

    private Camera _main;
    private RaycastHit hit;
    
    private Vector2 m_Look;
    private Vector2 m_Move;

    private PlayerInput playerInput;
    private PlayerMoveRotate playerMoveRotate;
    private PlayerMagic playerMagic;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMoveRotate = GetComponent<PlayerMoveRotate>();
        playerMagic = GetComponent<PlayerMagic>();
        Debug.Assert(magicShield, "Error : magic shield not set");
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

    public void OnShield(InputAction.CallbackContext context)
    {
        print("Shield ");
    }

    public void Update()
    {
        if (pcMode)
        {
            MousePosToHandPos();
            ShootMagic();
        }

        // 플레이어 마법
        ChangeElement();
        Grip();
        Charge();

        // 투사체 방어
        Shield();

        Look(m_Look);
        Move(m_Move);
        Teleport();
    }
    
    // 마우스 위치를 손 위치로 적용
    private void MousePosToHandPos()
    {
#if ENABLE_INPUT_SYSTEM
        Vector2 mousePosition = Mouse.current.position.ReadValue();

#else
        Vector2 mousePosition = Input.mousePosition;
#endif

        // 깊이에 따라
        float h = Screen.height;
        float w = Screen.width;
        float screenSpacePosX = (mousePosition.x - (w * 0.5f)) / w * 2;
        float screenSpacePosY = (mousePosition.y - (h * 0.5f)) / h * 2;
        leftHandTransform.localPosition = new Vector3(screenSpacePosX * handPosZ, screenSpacePosY * handPosZ, handPosZ);
        rightHandTransform.localPosition = new Vector3(screenSpacePosX * handPosZ, screenSpacePosY * handPosZ, handPosZ);

        // 눈 위치를 기준으로 손의 방향을 정함
        Vector3 eyePos = _main.transform.position;
        leftHandTransform.forward = (leftHandTransform.position - eyePos).normalized;
        rightHandTransform.forward = (rightHandTransform.position - eyePos).normalized;
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

    private void Grip()
    {
        if (playerInput.actions["Grip"].WasPressedThisFrame())
        {
            playerMagic.TurnOnGrip();
        }
        if (playerInput.actions["Grip"].WasReleasedThisFrame())
        {
            playerMagic.TurnOffGrip();
        }
    }

    private void Charge()
    {
        if (playerInput.actions["Charge"].WasPressedThisFrame())
        {
            playerMagic.StartCharge();
        }
        if (playerInput.actions["Charge"].IsPressed())
        {
            playerMagic.OnCharge();
        }
        if (playerInput.actions["Charge"].WasReleasedThisFrame())
        {
            playerMagic.EndCharge();
        }
    }

    private void ChangeElement()
    {
        if (playerInput.actions["Change Prev Element"].WasPressedThisFrame())
        {
            playerMagic.ChangeElement(false);
        }
        if (playerInput.actions["Change Next Element"].WasPressedThisFrame())
        {
            playerMagic.ChangeElement(true);
        }
    }

    private void Shield()
    {
        if (playerInput.actions["Shield"].WasPressedThisFrame())
        {
            magicShield.ActiveShield(true);
        }
        if (playerInput.actions["Shield"].WasReleasedThisFrame())
        {
            magicShield.ActiveShield(false);
        }
    }

    #region Movement
    private void Teleport()
    {
        if (playerInput.actions["Teleport"].WasPressedThisFrame())
        {
            playerMoveRotate.StartTeleport();
        }
        if (playerInput.actions["Teleport"].IsPressed())
        {
            playerMoveRotate.OnTeleport();
        }
        if (playerInput.actions["Teleport"].WasReleasedThisFrame())
        {
            playerMoveRotate.EndTeleport();
        }
    }

    private void Move(Vector2 direction)
    {
        playerMoveRotate.Move(direction);
    }

    private void Look(Vector2 rotate)
    {
        playerMoveRotate.Look(rotate);
    }
    #endregion
}
