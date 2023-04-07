using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerManager _playerManager;
    
    public PlayerManager PlayerManager { get => _playerManager; }

    private void Start()
    {
        _playerManager = PlayerManager.instance;
        transform.parent = _playerManager.transform;
        _playerManager.Players.Add(gameObject);
    }
}
