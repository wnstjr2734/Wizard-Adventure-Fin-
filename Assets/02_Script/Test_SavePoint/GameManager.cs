using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public static GameObject player;

    // 마지막 시작 위치
    private Vector3 lastCheckPoint;
    private Portal latestRoomPortal = null;

    //private PlayerController
    private PlayerMoveRotate playerMoveRotate;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerMoveRotate = player.GetComponent<PlayerMoveRotate>();
    }

    public void SetCheckPoint(Vector3 checkPoint, Portal roomPortal)
    {
        lastCheckPoint = checkPoint;
        latestRoomPortal = roomPortal;
    }

    public void RestartGame()
    {
        playerMoveRotate.SetPos(lastCheckPoint);
        latestRoomPortal.ResetRoom();
    }
}
