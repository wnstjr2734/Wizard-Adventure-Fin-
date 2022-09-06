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

    public class Room
    {
        public Portal portal;
        public EnemySpawn startPoint;
    }

    [SerializeField, Tooltip("맵(방 전부 담고 있는) 트랜스폼")]
    private Transform map;

    private Room[] rooms;

    // 마지막 시작 위치
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

    private void SetStageInfo()
    {
        Debug.Assert(map, "Error : Map is not set");

        Transform[] childs = new Transform[map.childCount];
        rooms = new Room[childs.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            var room = map.GetChild(i);

            rooms[i].portal = room.GetComponentInChildren<Portal>();
            rooms[i].startPoint = room.GetComponentInChildren<EnemySpawn>();
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
}
