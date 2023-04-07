using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private PlayerInputManager _playerInputManager;

    public List<GameObject> _players = new List<GameObject>();

    public List<GameObject> Players
    {
        get
        {
            return _players;
        }

        set
        {
            _players = value;
        }
    }

    #region singleton
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    #endregion


    private void Start()
    {
        _playerInputManager =  GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += ff => AddNewPlayer();
    }

    public void AddNewPlayer()
    {
        print("Player joined");
        //_playerInputManager.playerJoinedEvent.
    }
}
