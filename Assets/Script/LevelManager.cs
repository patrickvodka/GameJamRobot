using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private static int previousSceneIndex = -1;

    public Scene[] levels;




    public static void LoadLevel(int index)
    {
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public static void LoadNextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
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