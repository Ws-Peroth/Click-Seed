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

    public void BuyItem(DataManager.DataType type, string id, string keyType)
    {
        DataManager.DataType buyType;
        var buyData = DataManager.Instance.GetShopData(type, id);

        if(buyData == null)
        {
            Debug.LogError($"Buy Item: {id} Not Found");
            return;
        }

        var havingCrystal = DataManager.Instance.GetDefaultData(DataManager.DataType.Currency, "currency");
        var havingFragment = DataManager.Instance.GetDefaultData(DataManager.DataType.Currency, "currency2");
        ulong totalBuyPrice = (ulong)buyData.prices[0] * DataManager.FragmentToCrystalValue + (ulong)buyData.prices[1];
        ulong havingCurrency = (ulong)havingCrystal.count * DataManager.FragmentToCrystalValue + (ulong)havingFragment.count;

        if(totalBuyPrice > havingCurrency)
        {
            Debug.LogError($"BuyItem: {type}의 개수는 0 미만이 될 수 없습니다.");
            return;
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
            return;
        }
        var targetInventorData = DataManager.Instance.GetDefaultData(buyType, id);
        targetInventorData.count += 1;
        DataManager.Instance.SaveMstData();
        Debug.Log($"Purchase Process Successed");
    }
}
