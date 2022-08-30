using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public Vector3 lastCheckPointPos;
    public static GameObject player;

    public Portal latestPoral = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RestartGame()
    {

    }
}
