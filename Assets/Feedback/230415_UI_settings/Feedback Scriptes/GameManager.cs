using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GameManager : Singleton<GameManager>
{
    // 서버에서의 로직을 처리하도록 하고 싶은데...
    // [DataManager <--> GameManager] <===> Client Scripts

    public bool Harvest(string plantId)
    {
        if (DataManager.Instance.AddShelf(plantId))
        {
            DataManager.Instance.SetGrowingData(DefaultData.Dummy());
            DataManager.Instance.SaveMstData();
            return true;
        }
        return false;
    }

    public void PotClicked(Action<DataManager.GrowupType> growUpAction, Action<bool> activeHarvest, bool isAutoClick = false)
    {   
        var growing = DataManager.Instance.GetMstData().growing;
        var isPlanted = !(growing == null || string.IsNullOrEmpty(growing.name));
        if (!isPlanted)
        {
            // plant가 없는 경우에는 무시
            return;
        }

        var clickCount = 1;

        if (isAutoClick == false)
        {
            // Check Buff
            var clickBuffs = DataManager.Instance.GetMstData().buffs
                .Where((v, idx) => v.buff == DataManager.BuffType.AdditionalClick && v.leftBuffSec > 0)
                .Select((v) => v.buffCount)
                .ToList();
            var isEmpty = clickBuffs == null || clickBuffs.Count == 0;
            var sum = clickBuffs.Sum();
            var addClick = isEmpty ? 0 : sum;

            clickCount += addClick;
        }

        var step = GetGrowStepByClickCount(growing.id, DataManager.Instance.GetMstData().growing.count + clickCount);

        // 이미 Max일 경우
        var maxCount = DataManager.Instance.GetDefaultData(DataManager.DataType.Plant, growing.id).count;
        if (growing.count + clickCount < maxCount)
        {
            DataManager.Instance.GetMstData().growing.count += clickCount;
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
            Debug.LogError($"BuyItem: {type}의 개수는 0 미만이 될 수 없습니다.");
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
    public IEnumerator AutoClickBuff(int clickCount)
    {
        var restTime =  1.0f / clickCount;
        Debug.Log($"AutoClickBuff: {1.0f} / {clickCount} => restTime = {restTime}");
        for(int i = 0; i < clickCount; i++)
        {
            // Add Click Count
            PotClicked(
                (growupType) => {
                    GlobalEventController.Instance.SendEvent("GrowUp", "GrowUp", new object[] { growupType });
                }, 
                (isHarvestOn) => {
                    GlobalEventController.Instance.SendEvent("Harvest", "Harvest", new object[] { isHarvestOn });
                }, true);
            yield return new WaitForSecondsRealtime(restTime);
        }
        yield return null;
    }

    public IEnumerator UpdateBuffTime()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            var buffInfos = DataManager.Instance.GetMstData().buffs;
            foreach (var buff in buffInfos)
            {
                if (buff.leftBuffSec > 0)
                {
                    DataManager.Instance.SetBuffTime(buff.id, buff.leftBuffSec - 1);
                    if (buff.buff == DataManager.BuffType.AutoClick)
                    {
                        StartCoroutine(AutoClickBuff(buff.buffCount));
                    }
                }
                else
                {
                    buff.leftBuffSec = 0;
                }
            }
            DataManager.Instance.GetMstData().buffs = buffInfos;
            DataManager.Instance.SaveMstData();
        }
    }

    public bool UseElixir(string id)
    {
        var targetBuff = DataManager.Instance.GetMstData().buffs
            .Where((v) => v.id == id)
            .ToList();
        if(targetBuff == null || targetBuff.Count == 0)
        {
            Debug.LogError($"GameManager.UseElixir(): {id}버프를 찾을 수 없습니다");
            return false;
        }
        var buff = targetBuff[0];
        if(buff.leftBuffSec > 0)
        {
            Debug.Log($"GameManager.UseElixir(): {id}는 이미 사용중인 버프입니다");
            return false;
        }

        var time = DataManager.Instance.GetSetBuffData(id).leftBuffSec;
        DataManager.Instance.SetBuffTime(id, time);
        DataManager.Instance.ChangeElixirCount(id, -1);
        DataManager.Instance.SaveMstData();
        return true;
    }

    private void Start()
    {
        StartCoroutine(UpdateBuffTime());
    }
}
