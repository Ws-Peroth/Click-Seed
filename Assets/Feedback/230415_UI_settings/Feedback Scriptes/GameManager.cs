using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : Singleton<GameManager>
{
    public enum GrowupType
    {
        Seed,
        Sprout,
        Blooming,
        Flower
    }
    public enum CurrencyTypes
    {
        Crystal,
        CrystalFragment
    }
    public bool isPlanted;
    public int potClickCount;
    public int[] currencyData = new int[2] { 0, 0 };
    public readonly int[] PotGrowupCount = new int[]
    {
        10,
        20,
        30,
        40
    };

    public void potClicked(Action<GrowupType> growUpAction )
    {
        if (!isPlanted)
        {
            return;
        }
        potClickCount++;
        for(int i = PotGrowupCount.Length -1; i >= 0; i--)
        {
            if(potClickCount == PotGrowupCount[i])
            {
                growUpAction((GrowupType)i);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
