using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [CanBeNull] public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
        
    }

    private void Start()
    {
        UpdateGameState(GameState.SelectLevel);
        
    }

    public void UpdateGameState(GameState newsState)
    {
        State = newsState;
        switch (newsState)
        {
            case GameState.SelectLevel:
                HandSelectLevel();
                break;
            case GameState.Victory:
                HandVictory();
                break;
            case GameState.Lose:
                HandLose();
                break;
            case GameState.ReloadLevel:
                HandReloadLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newsState), newsState, null);
        }
        OnGameStateChanged?.Invoke(newsState);
    }

    private void HandReloadLevel()
    {
        throw new NotImplementedException();
    }

    private void HandLose()
    {
        throw new NotImplementedException();
    }

    private void HandVictory()
    {
        throw new NotImplementedException();
    }

    private void HandSelectLevel()
    {
        throw new NotImplementedException();
    }

    public enum GameState
    {
        SelectLevel, 
        Victory, 
        Lose, 
        ReloadLevel
    }
}
