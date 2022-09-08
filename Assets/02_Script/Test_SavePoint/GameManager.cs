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
    public static Transform eye;
    public static PlayerController Controller { get; private set; }

    private CharacterStatus playerStatus;
    private PlayerMoveRotate playerMoveRotate;

    #region Map

    // 마지막 시작 위치
    private Transform checkPointTr;
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
        eye = player.transform.GetChild(0);

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

    public void SetCheckPoint(Transform checkPoint, Portal roomPortal)
    {
        checkPointTr = checkPoint;
        latestRoomPortal = roomPortal;
    }

    public void RestartGame()
    {
        playerStatus.ResetStatus();
        playerMoveRotate.SetPos(checkPointTr.position, checkPointTr.forward);
        latestRoomPortal.ResetRoom();
    }

    // 디버그 - 다음 방으로 강제 이동
    public void GoNextRoom()
    {
        latestRoomPortal.UsePortal();
    }
}
