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
    [SerializeField] private CameraBehaviour _camera;
    [SerializeField] private DialogueManager _dialogueManager;

    private List<GameObject> _players = new List<GameObject>();
    private int index = 0;

    public CameraBehaviour Camera { get => _camera; set => _camera = value; }
    public DialogueManager DialogueManager { get => _dialogueManager; set => _dialogueManager = value; }
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



    public void SkipDialogue()
    {
        Debug.Log("ss");
        if(GameManager.instance.GameState == GameManager.GAMESTATE.Dialogue)
        {
            DialogueManager.Skip = true;
        }
    }

    private void Start()
    {
        _playerInputManager =  GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += ff => AddNewPlayer();
    }

    public void AddNewPlayer()
    {
        Camera.StartCameraMove(_players.Count);
    }
}
