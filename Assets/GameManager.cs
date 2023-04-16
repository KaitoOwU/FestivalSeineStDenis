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

    public GAMESTATE GameState { get => _gameState; set => _gameState = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
