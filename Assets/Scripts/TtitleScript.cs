using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TtitleScript : MonoBehaviour
{
    public Button startButtton;
    void Start()
    {
        startButtton.onClick.AddListener(() =>
        {
            SceneManagerEx.Instance.LoadScene(SceneManagerEx.Scenes.Main);
        });
    }
}
