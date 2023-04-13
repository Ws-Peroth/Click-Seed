using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamedata : MonoBehaviour
{
    private const string MOONFRAGMENT = "MoonFragment";
    private const string MOONSHARD = "MoonShard";

    private int MoonFragment = 0;
    private int MoonShard = 0;

    void Start()
    {
        MoonFragment = PlayerPrefs.GetInt(MOONFRAGMENT);
        MoonShard = PlayerPrefs.GetInt(MOONSHARD);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
