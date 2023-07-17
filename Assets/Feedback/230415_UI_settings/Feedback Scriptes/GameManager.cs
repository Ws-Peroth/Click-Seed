using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : Singleton<GameManager>
{
    // 서버에서의 로직을 처리하도록 하고 싶은데...
    // [DataManager <--> GameManager] <===> Client Scripts

    public void PotClicked(Action<DataManager.GrowupType> growUpAction )
    {   
        var growing = DataManager.Instance.GetMstData().growing;
        var isPlanted = !(growing == null || string.IsNullOrEmpty(growing.name));

        if (!isPlanted)
        {
            // plant가 없는 경우에는 무시
            return;
        }
        DataManager.Instance.GetMstData().growing.count++;
        for(int i = DataManager.Instance.PotGrowupCount.Length -1; i >= 0; i--)
        {
            if(DataManager.Instance.GetMstData().growing.count == DataManager.Instance.PotGrowupCount[i])
            {
                growUpAction((DataManager.GrowupType)i);
            }
        }
    }

    // Start is called before the first frame update
    public void LoadFirstScene()
    {
        SceneManagerEx.Instance.LoadScene((SceneManagerEx.Scenes)1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
