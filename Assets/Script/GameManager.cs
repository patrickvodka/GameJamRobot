using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    [CanBeNull] public static GameManager Instance;
    public GameState State; 
    private GameObject Player;
    public float respawnTIme;
    private Vector2 PlayerSpawnPos;
    private bool CanPlayerSpawn;
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
            case GameState.NewGame:
                HandNewGame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newsState), newsState, null);
        }
        OnGameStateChanged?.Invoke(newsState);
    }

    private void HandNewGame()
    {
        LevelManager.LoadFirstLevel();
    }

    private void HandReloadLevel()
    {
        LevelManager.RestartLevel();
    }

    private void HandLose()
    {
        LevelManager.RestartLevel();
    }

    private void HandVictory()
    {
        LevelManager.LoadNextLevel();
    }

    private void HandSelectLevel()
    {
        LevelManager.LoadMainMenu();
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
        ReloadLevel,
        NewGame
    }
}
