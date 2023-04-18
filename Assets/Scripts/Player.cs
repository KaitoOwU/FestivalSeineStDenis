using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PLAYER
    {
        PLAYER1,
        PLAYER2,
    };

    [SerializeField] private PLAYER playerType;

    private PlayerManager _playerManager;
    
    public PlayerManager PlayerManager { get => _playerManager; }
    public PLAYER PlayerType { get => playerType; set => playerType = value; }

    private void Start()
    {
        _playerManager = PlayerManager.instance;
        transform.parent = _playerManager.transform;
        _playerManager.Players.Add(gameObject);
    }
}
