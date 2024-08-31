using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    InGame,
    GameOver
}
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    private GameState _gameState;
    public GameState GameState { get { return _gameState; } }

    private void Start()
    {
        _gameState = GameState.MainMenu;
    }

    public void OpenMenu()
    {
        _gameState = GameState.MainMenu;
    }

    public void CloseMenu()
    {
        _gameState = GameState.InGame;
    }
    public void IsDeath()
    {
        _gameState = GameState.GameOver;
    }
}
