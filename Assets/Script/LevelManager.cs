using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        levels = new Scene[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            levels[i] = SceneManager.GetSceneByBuildIndex(i);
        }
    }

    private static int previousSceneIndex = -1;

    public static Scene[] levels;
    public static int currentLevelIndex = 0;



    public static void LoadLevel(int index)
    {
       
        Debug.Log("index "+index);
        Debug.Log("level "+levels.Length);
        if (index < 0 || index >= levels.Length) return;

        SceneManager.LoadScene(levels[index].name);
        currentLevelIndex = index;
    }

    public static void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }

    public static void LoadFirstLevel()
    {
        LoadLevel(0);
    }

    public static void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void LoadMainMenu()
    {
        LoadLevel(0);
    }

    public static void ReturnToPreviousScene()
    {
        if (previousSceneIndex >= 0)
        {
            LoadLevel(previousSceneIndex);
        }
        else
        {
            Debug.LogWarning("No previous scene available.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ReturnToPreviousScene();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadNextLevel();
        }
    }
}