using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.XR;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class PlayerController : Singleton<PlayerController>
{
    public enum MagicAbility
    {
        Base = 1,
        Shield = 2,
        Grip = 4,
        Charge = 8,
        ChangeElement = 16,
    }

    [SerializeField]
    private MagicShield magicShield;

    [Header("For Debug")] 
    private bool isVR;    
    [SerializeField] private float handPosZ = 0.3f;
    [SerializeField] private float pressedHandPosZ = 0.5f;
    [SerializeField] private float releasedHandPosZ = 0.3f;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    

    private Camera _main;
    private RaycastHit hit;
    
    private Vector2 m_Look;
    private Vector2 m_Move;
    

    private int previousChangeElementInput = 0;

    private PlayerInput playerInput;
    private PlayerMoveRotate playerMoveRotate;
    private PlayerMagic playerMagic;
    private GameObject properties;

    // 배운 능력(비트마스크 방식)
    public int learnedAbility = 0;

    public bool CanControlPlayer => playerInput.currentActionMap.name.Equals("Player");

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMoveRotate = GetComponent<PlayerMoveRotate>();
        playerMagic = GetComponent<PlayerMagic>();
        properties = transform.FindChildRecursive("Properties").gameObject;
        Debug.Assert(magicShield, "Error : magic shield not set");
    }

    private void Start()
    {
        _main = Camera.main;
        Debug.Log(playerInput.currentActionMap.name);

        isVR = IsPresent();
        OVRManager.display.RecenterPose();
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
        // 디버그 테스트
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    playerInput.SwitchCurrentActionMap("Test");
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    playerInput.SwitchCurrentActionMap("Player");
        //}

        if (!isVR)
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

        //속성 선택 창 On/Off
        //PropertieseAcitve();
    }

    public static bool IsPresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                return true;
            }
        }
        return false;
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
        if (!CheckLearnedAbility(MagicAbility.Grip))
        {
            return;
        }

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
        if (!CheckLearnedAbility(MagicAbility.Charge))
        {
            return;
        }

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
        if (!CheckLearnedAbility(MagicAbility.ChangeElement))
        {
            return;
        }
        
        int input = Mathf.RoundToInt(playerInput.actions["Change Element"].ReadValue<float>());
        if (input != previousChangeElementInput)
        {
            properties.SetActive(true);
            playerMagic.ChangeElement(input);
        }
        previousChangeElementInput = input;
       
    }

    private void Shield()
    {
        if (!CheckLearnedAbility(MagicAbility.Shield))
        {
            return;
        }

        if (playerInput.actions["Shield"].WasPressedThisFrame())
        {
            magicShield.gameObject.SetActive(true);
        }
        if (playerInput.actions["Shield"].WasReleasedThisFrame())
        {
            magicShield.gameObject.SetActive(false);
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

    public bool CheckLearnedAbility(MagicAbility ability)
    {
        return (learnedAbility & (int)ability) > 0;
    }

    public void LearnAbility(MagicAbility ability)
    {
        learnedAbility |= (int)ability;
    }

    public void PropertieseAcitve()
    {
        #region 입력시 창 표시
        //int input = Mathf.RoundToInt(playerInput.actions["PropertiesSelect"].ReadValue<float>());
        //print("현재 썸스틱 클릭 : " + input);

        //if (0 != input)
        //{
        //    if (isActive == false)
        //    {
        //        properties.SetActive(true);
        //        isActive = true;
        //    }           
        //}
        //print(isActive);
        #endregion
        properties.SetActive(false);
        
    }

}
