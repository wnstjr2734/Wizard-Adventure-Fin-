using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public static GameObject player;
    public static PlayerInput playerInput;
    public static PlayerController Controller { get; private set; }

    private CharacterStatus playerStatus;
    private PlayerMoveRotate playerMoveRotate;

    #region Map

    // ������ ���� ��ġ
    private Vector3 lastCheckPoint;
    private Portal latestRoomPortal = null;

    #endregion

    protected override void OnAwake()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerInput = player.GetComponent<PlayerInput>();
        Debug.Assert(playerInput, "Error : Player Input not set");

        Controller = player.GetComponent<PlayerController>();

        playerStatus = player.GetComponent<CharacterStatus>();
        playerMoveRotate = player.GetComponent<PlayerMoveRotate>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GoNextRoom();
        }
    }

    public void SetCheckPoint(Vector3 checkPoint, Portal roomPortal)
    {
        lastCheckPoint = checkPoint;
        latestRoomPortal = roomPortal;
    }

    public void RestartGame()
    {
        playerStatus.ResetStatus();
        playerMoveRotate.SetPos(lastCheckPoint);
        latestRoomPortal.ResetRoom();
    }

    // ����� - ���� ������ ���� �̵�
    public void GoNextRoom()
    {
        latestRoomPortal.UsePortal();
    }
}
