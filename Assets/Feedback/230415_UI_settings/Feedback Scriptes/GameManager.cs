using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : Singleton<GameManager>
{
    // ���������� ������ ó���ϵ��� �ϰ� ������...
    // [DataManager <--> GameManager] <===> Client Scripts

    public void PotClicked(Action<DataManager.GrowupType> growUpAction, Action<bool> activeHarvest)
    {   
        var growing = DataManager.Instance.GetMstData().growing;
        var isPlanted = !(growing == null || string.IsNullOrEmpty(growing.name));
        if (!isPlanted)
        {
            // plant�� ���� ��쿡�� ����
            return;
        }
        var step = GetGrowStepByClickCount(growing.id, DataManager.Instance.GetMstData().growing.count + 1);

        // �̹� Max�� ���
        var maxCount = DataManager.Instance.GetDefaultData(DataManager.DataType.Plant, growing.id).count;
        if (growing.count + 1 < maxCount)
        {
            DataManager.Instance.GetMstData().growing.count++;
            activeHarvest(false);
        }
        else
        {
            Debug.Log("Activate Harvest");
            DataManager.Instance.GetMstData().growing.count = maxCount;
            activeHarvest(true);
        }
        DataManager.Instance.SaveMstData();
        growUpAction(step);
    }

    public DefaultData FindPlantWithSeedName(string seedName)
    {
        var plantData = DataManager.Instance.GetMstData().plant.Find((m) => m.name == seedName);
        return plantData;
    }
    public ProductData FindSeedShopWithPlantName(string plantName)
    {
        var seedData = DataManager.Instance.GetMstData().seedShop.Find((m) => m.name == plantName);
        return seedData;
    }

    public DataManager.GrowupType GetGrowStepByClickCount(string plantId, int clickCount)
    {
        // Max      : 50
        // 0 ~ 9    : Seed
        // 10 ~ 19  : Sprout1
        // 20 ~ 29  : Sprout2
        // 30 ~ 39  : Blooming
        // 40 ~ 50  : Flower
        // 50       => Harvest On

        var plantData = DataManager.Instance.GetDefaultData(DataManager.DataType.Plant, plantId);
        if(plantData == null)
        {
            Debug.LogError($"GameManager.GetGrowStepByClickCount(): not found default Data ({plantId})");
        }
        var maxCount = plantData.count;
        int stepCount = Math.Min(clickCount / (maxCount / 5), 4);
        return (DataManager.GrowupType)stepCount;
    }

    public void PlantSeed(string plantId, Action callback)
    {
        var growingData = DataManager.Instance.GetDefaultData(DataManager.DataType.Plant, plantId);
        if (growingData == null)
        {
            Debug.LogError($"GameManager.PlantSeed(): {plantId} Not foune in seedShop data");
            return;
        }
        var data = new DefaultData(growingData.id, growingData.name, 0);
        // DataManager.Instance.SetSeedCount()
        var seedId = FindSeedShopWithPlantName(growingData.name).id;
        DataManager.Instance.ChangeSeedCount(seedId, -1);
        DataManager.Instance.SetGrowingData(data);
        DataManager.Instance.SaveMstData();
        callback();
    }

    // Start is called before the first frame update
    public void LoadFirstScene()
    {
        SceneManagerEx.Instance.LoadScene((SceneManagerEx.Scenes)1);
    }


    public bool BuyItem(DataManager.DataType type, string id, string keyType)
    {
        DataManager.DataType buyType;
        var buyData = DataManager.Instance.GetShopData(type, id);

        if(buyData == null)
        {
            Debug.LogError($"Buy Item: {id} Not Found");
            return false;
        }

        var havingCrystal = DataManager.Instance.GetDefaultData(DataManager.DataType.Currency, "currency");
        var havingFragment = DataManager.Instance.GetDefaultData(DataManager.DataType.Currency, "currency2");
        ulong totalBuyPrice = (ulong)buyData.prices[0] * DataManager.FragmentToCrystalValue + (ulong)buyData.prices[1];
        ulong havingCurrency = (ulong)havingCrystal.count * DataManager.FragmentToCrystalValue + (ulong)havingFragment.count;

        if(totalBuyPrice > havingCurrency)
        {
            Debug.LogError($"BuyItem: {type}�� ������ 0 �̸��� �� �� �����ϴ�.");
            return false;
        }
        ulong resultCurrency = havingCurrency - totalBuyPrice;
        int setCrystal = (int)resultCurrency / DataManager.FragmentToCrystalValue;
        int setFragment = (int)resultCurrency % DataManager.FragmentToCrystalValue;
        DataManager.Instance.SetCurrencyCount(setCrystal, setFragment);

        if (keyType == "Seed")
        {
            buyType = DataManager.DataType.Inventory;
        }
        else if (keyType == "Elixir")
        {
            buyType = DataManager.DataType.Elixer;
        }
        else
        {
            Debug.LogError($"DataType Not Found\nPurchase Process Failed");
            return false;
        }
        var targetInventorData = DataManager.Instance.GetDefaultData(buyType, id);
        targetInventorData.count += 1;
        DataManager.Instance.SaveMstData();
        Debug.Log($"Purchase Process Successed");

        return true;
    }
}
