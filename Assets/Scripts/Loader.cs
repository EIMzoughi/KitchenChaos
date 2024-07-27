using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }
    private static Scene _targetScene;

    public static void load(Scene taretScene)
    {
        _targetScene = taretScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        Thread.Sleep(1000);
        
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(_targetScene.ToString());
    }
}
