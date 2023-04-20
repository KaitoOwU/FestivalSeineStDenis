using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GAMESTATE
    {
        Menu,
        Playing,
        Dialogue
    };

    private GAMESTATE _gameState;

    public static GameManager instance;

    public Action OnGamePause;
    public Action OnGameUnPause;

    public Action OnDialogue;
    public Action OnStopDialogue;

    [Scene]
    public int _gameScene;

    [Scene]
    public int _menuScene;

    public GAMESTATE GameState { get => _gameState; set => _gameState = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        GameState = GAMESTATE.Playing;
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void OpenMenuScene()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("Tom"));
        SceneManager.MoveGameObjectToScene(PlayerManager.instance.gameObject, SceneManager.GetSceneByName("Tom"));
    }
    public void QuitGame() => Application.Quit();


}
