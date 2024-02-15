using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class  LevelManager : MonoBehaviour
{
   
    
    public static List<Scene> levels;
    public static void LoadLevel(int index)
    {
        //SceneManager.LoadScene(levels[index].name);
    }
    public static void LoadNextLevel()
    {
        LoadLevel( + 1);
    }
    
    public static void LoadFirstLevel()
    {
        LoadLevel(1);
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
        LoadLevel( 0);
    }
    


    
}
