using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GAMESTATE GameState { get => _gameState; set => _gameState = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        GameState = GAMESTATE.Playing;
    }


}
