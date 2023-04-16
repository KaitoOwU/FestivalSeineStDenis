using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private PlayerInputManager _playerInputManager;
    private CameraBehaviour _camera;

    private List<GameObject> _players = new List<GameObject>();
    private int index = 0;

    public CameraBehaviour Camera { get => _camera; set => _camera = value; }
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

    public void AddPlayerToCinemachineGroup(Player player)
    {
        Camera.GetComponent<CinemachineTargetGroup>().m_Targets.SetValue(player.gameObject, index);
        index++;
    }

    public void AddNewPlayer()
    {
        print("Player joined");
        //_playerInputManager.playerJoinedEvent.
    }
}
