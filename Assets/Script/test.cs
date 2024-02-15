using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class test : MonoBehaviour
{
    [CanBeNull] public static test Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;
    private bool CanPlayerSpawn;
    private Vector2 PlayerSpawnPos;
    public float respawnTIme;
    private GameObject Player;

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

    public IEnumerator SpawnPlayer(float Time)
    {
        yield return new WaitForSeconds(Time);
        
    }

    public void PlayerStarted(GameObject player , Vector2 playerSpawn)
    {
        Player = player;
        PlayerSpawnPos = playerSpawn;
        Debug.Log(playerSpawn);
    }
    public void RespawnPlayer()
    {

    }

    public enum GameState
    {
        SelectLevel, 
        Victory, 
        Lose, 
        ReloadLevel
    }
}

