using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс управления сценами игры
/// </summary>
public class ScenesManager
{

    public ScenesManager()
    {
        if (SceneManager.sceneCount < 0)
        {
            throw new Exception("Выберите сцены в Build Settings");
        }
    }

    /// <summary>
    /// Перезагрузить текущий уровень
    /// </summary>
    public void RestatrLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Загрузить следующий уровень
    /// </summary>
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Перейти на сцену главного меню
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}