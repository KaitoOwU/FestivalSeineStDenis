using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private PlayerInputManager _playerInputManager;
    [SerializeField] private CameraBehaviour _camera;
    [SerializeField] private DialogueManager _dialogueManager;

    [SerializeField] private GameObject _p1Image;
    [SerializeField] private GameObject _p2Image;
    [SerializeField] private GameObject _p1ContolerImage;
    [SerializeField] private GameObject _p2ContolerImage;
    [SerializeField] private GameObject _startGame;

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
        DontDestroyOnLoad(this);
    }

    #endregion




    private void Start()
    {
        _playerInputManager =  GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += ff => AddNewPlayer();
        SceneManager.activeSceneChanged += SceneChange;
    }



    private void SceneChange(Scene s1, Scene s2)
    {
        if(SceneManager.GetActiveScene().name == "Tom")
        {
            Camera.StartCameraMove();
            _players[0].transform.position = new Vector2(_players[0].transform.position.x, _players[0].transform.position.y + 1.5f);
        }
    }
    public void SkipDialogue()
    {
        if(GameManager.instance.GameState == GameManager.GAMESTATE.Dialogue)
        {
            DialogueManager.Skip = true;
        }
    }

    public void AddNewPlayer()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            if(index == 0)
            {
                _p1Image.SetActive(true);
                _p1ContolerImage.SetActive(false);
            }
            else
            {
                _p2ContolerImage.SetActive(false);
                _p2Image.SetActive(true);
                _startGame.SetActive(true);
            }
            index++;
        }
    }
}
