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
    private Vector2 lastDeadPos;
    public float respawnTime=1.5f;
    private Vector2 PlayerSpawnPos;
    private bool CanPlayerSpawn;
    
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
        transform.position = new Vector3(-500, -500, -500);

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
    public IEnumerator SpawnPlayerTimer()
    {
        Player.transform.position = transform.position;
      yield return new WaitForSeconds(respawnTime);
        RespawnPlayer();
    }


    public void PlayerStarted(GameObject player , Vector2 playerSpawn)
    {
        
        Player = player;
        PlayerSpawnPos = playerSpawn;
    }
    private void RespawnPlayer()
    {
        Player.transform.position = PlayerSpawnPos;
        var move = Player.GetComponent<PlayerMovement>();
            move.StartCoroutine(move.StartMoving());
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
