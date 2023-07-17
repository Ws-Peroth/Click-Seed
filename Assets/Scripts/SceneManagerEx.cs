using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : Singleton<SceneManagerEx>
{
    public enum Scenes
    {
        Init,
        Title,
        Main
    };

    public void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
