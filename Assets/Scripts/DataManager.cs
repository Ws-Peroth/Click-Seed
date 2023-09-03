using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#region DATA_DEFINED

[Serializable]
public class DefaultData
{
    public string id;
    public string name;
    public int count;

    public DefaultData(string id, string name, int count)
    {
        this.id = id;
        this.name = name;
        this.count = count;
    }
}

[Serializable]
public class ProductData
{
    public string id;
    public string name;
    public int[] prices;

    public ProductData(string id, string name, int[] prices)
    {
        this.id = id;
        this.name = name;
        this.prices = prices;
    }
}

[Serializable]
public class QuestData
{
    public string npcId;
    public string npcName;
    public string questId;
    public int questReward;

    public QuestData(string npcId, string npcName, string questId, int questReward)
    {
        this.npcId = npcId;
        this.npcName = npcName;
        this.questId = questId;
        this.questReward = questReward;
    }
}

[Serializable]
public class JsonData
{
    public List<DefaultData> currency;
    public List<DefaultData> elixer;
    public List<DefaultData> inventory;
    public List<DefaultData> shopList; 
    public List<ProductData> seedShop;
    public List<ProductData> elixerShop;
    public List<QuestData> quest;
    public string[] shelf;
    public DefaultData growing;

    public void Init(
        List<DefaultData> currency,
        List<DefaultData> elixer,
        List<DefaultData> inventory,
        List<ProductData> seedShop,
        List<ProductData> elixerShop,
        List<DefaultData> shopList,
        DefaultData growing,
        List<QuestData> quest, 
        string[] shelf)
    {
        this.currency = currency;
        this.elixer = elixer;
        this.inventory = inventory;
        this.seedShop = seedShop;
        this.elixerShop = elixerShop;
        this.shopList = shopList;
        this.growing = growing;
        this.quest = quest;
        this.shelf = shelf;
    }
}
#endregion

public class DataManager : Singleton<DataManager>
{
    // ������ DB�� RPC������ ���� ����
    // �����͸� Json���� �����ϸ�, �����ִ� �����͸� �ν��Ͻ��� ���� �ش�


    private JsonData MstData;
    public JsonData GetMstData() { return MstData; }
    public const int FragmentToCrystalValue = 5000;
    public enum DataType
    {
        Currency,
        Elixer,
        Inventory,
        ShopList,
        SeedShop,
        ElixerShop,
        Quest,
        Shelf,
        Growing
    }
    public enum GrowupType
    {
        Seed,
        Sprout1,
        Sprout2,
        Blooming,
        Flower
    }
    public enum CurrencyTypes
    {
        Crystal,
        CrystalFragment
    }
    public readonly int[] PotGrowupCount = new int[]
    {
        10,
        20,
        30,
        40
    };


    public void Start()
    {
        Debug.Log(Path.Combine(Application.persistentDataPath, "mstData.json"));
        MstData = null;
        MstData = JsonManager.Instance.LoadJsonData<JsonData>(Path.Combine(Application.persistentDataPath, "mstData.json"));
        if(MstData == null || Application.isEditor)
        {
            // Data Init

            MstData = new JsonData();
            var currency = new List<DefaultData> {
                new DefaultData ("currency", "Crescent", 1000),
                new DefaultData ("currency2", "Lunar Shard ", 10),
            };
            var elixer = new List<DefaultData> {
                new DefaultData ("Elixir1", "Plant Aid", 0),
                new DefaultData ("Elixir2", "Plant Supplements", 0),
                new DefaultData ("Elixir3", "Sprite Helper", 0),
            };
            var inventory = new List<DefaultData> {
                new DefaultData ("Seed1", "Slime Seed", 0),
                new DefaultData ("Seed2", "Crystal Seed", 0),
                new DefaultData ("Seed3", "Floresent Seed", 0),
                new DefaultData ("Seed4", "Mushroom Seed", 0),
                new DefaultData ("Seed5", "Tentaclel Seed", 0),
                new DefaultData ("Seed6", "Sunflower Seed", 0),
                new DefaultData ("Seed7", "Seed on Fire", 0),
                new DefaultData ("Seed8", "Carnivorous Seed", 0),
                new DefaultData ("Seed9", "Lettuce Flower Seed", 0),
            }; 
            var shopList = new List<DefaultData> {
                new DefaultData ("ShopIconElixir", "Elixer Shop", 0),
                new DefaultData ("ShopIconSeed", "Seed Shop", 0),
            };
            var seedShop = new List<ProductData> {
                new ProductData ("Seed1", "Slime Seed",         new int[]{ 0, 50}),
                new ProductData ("Seed2", "Crystal Seed",       new int[]{ 2, 0}),
                new ProductData ("Seed3", "Floresent Seed",     new int[]{ 0, 100}),
                new ProductData ("Seed4", "Mushroom Seed",      new int[]{ 0, 150}),
                new ProductData ("Seed5", "Tentaclel Seed",     new int[]{ 0, 200}),
                new ProductData ("Seed6", "Sunflower Seed",     new int[]{ 0, 10}),
                new ProductData ("Seed7", "Seed on Fire",       new int[]{ 3, 0}),
                new ProductData ("Seed8", "Carnivorous Seed",   new int[]{ 0, 300}),
                new ProductData ("Seed9", "Lettuce Flower Seed",new int[]{ 1, 0}),
            };
            var elixerShop = new List<ProductData> {
                new ProductData ("Elixir1", "Plant Aid",        new int[]{ 10, 1000}),
                new ProductData ("Elixir2", "Plant Supplements",new int[]{ 10, 1000 }),
                new ProductData ("Elixir3", "Sprite Helper",    new int[]{ 10, 1000 }),
            };
            DefaultData growing = new DefaultData("", "", 0);
            var quest = new List<QuestData> {
            };
            var shelf = new string[] {
            };

            MstData.Init(
                currency:   currency,
                elixer:     elixer,
                inventory:  inventory,
                shopList:   shopList,
                growing:    growing,
                quest:      quest,
                shelf:      shelf,
                seedShop:   seedShop,
                elixerShop: elixerShop
                );
            JsonManager.Instance.SaveJsonData(MstData, Path.Combine(Application.persistentDataPath, "mstData.json"));
            MstData = JsonManager.Instance.LoadJsonData<JsonData>(Path.Combine(Application.persistentDataPath, "mstData.json"));
        }

        var data = JsonManager.Instance.ReadJson(Path.Combine(Application.persistentDataPath, "mstData.json"));
        GameManager.Instance.LoadFirstScene();
        Debug.Log(data);
    }
    public void SaveMstData()
    {
        JsonManager.Instance.SaveJsonData(MstData, Path.Combine(Application.persistentDataPath, "mstData.json"));
        // System.IO.File.WriteAllText(Path.Combine(Application.persistentDataPath, "file.txt"), jsonTextFile);
    }

    public DefaultData GetDefaultData(DataType type, string id)
    {
        var targetItem = new DefaultData("", "", 0);

        if (type == DataType.Inventory)
        {
            targetItem = GetMstData().inventory.Find((m) => m.id == id);
        }
        else if (type == DataType.Currency)
        {
            targetItem = GetMstData().currency.Find((m) => m.id == id);
        }
        else if (type == DataType.Elixer)
        {
            targetItem = GetMstData().elixer.Find((m) => m.id == id);
        }
        else if (type == DataType.ShopList)
        {
            targetItem = GetMstData().shopList.Find((m) => m.id == id);
        }
        else if (type == DataType.Growing)
        {
            targetItem = GetMstData().growing;
        }
        else
        {
            Debug.LogError($"Type Undefined: {type} is not defined in Default Data Types");
            return null;
        }

        return targetItem;
    }
    public ProductData GetShopData(DataType type, string id)
    {
        var targetItem = new ProductData("", "", new[] { 0, 0 });

        if (type == DataType.ElixerShop)
        {
            targetItem = GetMstData().elixerShop.Find((m) => m.id == id);
        }
        else if (type == DataType.SeedShop)
        {
            targetItem = GetMstData().seedShop.Find((m) => m.id == id);
        }
        else
        {
            Debug.LogError($"Type Undefined: {type} is not defined in Shop Data Types");
            return null;
        }

        return targetItem;
    }
    private void AddDefaultDataValue(DataType type, string id, int count)
    {
        var targetItem = GetDefaultData(type, id);

        if (targetItem == null)
        {
            Debug.LogError($"{type}에서 {id}을(를) 찾을 수 없습니다");
            return;
        }
        SetDefaultDataValue(type, id, targetItem.count + count);
    }
    private void SetDefaultDataValue(DataType type, string id, int count)
    {
        var targetItem = GetDefaultData(type, id);

        if (targetItem == null)
        {
            Debug.LogError($"{type}에서 {id}을(를) 찾을 수 없습니다");
            return;
        }
        if (count < 0)
        {
            // Can't Use this Seed
            Debug.LogError($"{type}의 개수는 0 미만이 될 수 없습니다.");
            return;
        }
        targetItem.count = count;
        SaveMstData();
    }

    public void ChangeSeedCount(string id, int count)
    {
        AddDefaultDataValue(DataType.Inventory, id, count);
    }
    public void SetSeedCount(string id, int count)
    {
        SetDefaultDataValue(DataType.Inventory, id, count);
    }
    public void ChangeElixirCount(string id, int count)
    {
        AddDefaultDataValue(DataType.Elixer, id, count);
    }
    public void SetElixirCount(string id, int count)
    {
        SetDefaultDataValue(DataType.Elixer, id, count);
    }
    public void ChangeCurrencyCount(int changeCrystal, int changeFragment)
    {
        AddDefaultDataValue(DataType.Currency, "currency", changeCrystal);
        AddDefaultDataValue(DataType.Currency, "currency2", changeFragment);
    }
    public void SetCurrencyCount(int changeCrystal, int changeFragment)
    {
        SetDefaultDataValue(DataType.Currency, "currency", changeCrystal);
        SetDefaultDataValue(DataType.Currency, "currency2", changeFragment);
    }

    public void SetCurrencyCount(ulong totalCurrency)
    {
        if (totalCurrency < 0)
        {
            Debug.LogError($"{DataType.Currency}의 개수는 0 미만이 될 수 없습니다.");
            return;
        }

        int setCrystal = (int)(totalCurrency / FragmentToCrystalValue);
        int setFragment = (int)(totalCurrency % FragmentToCrystalValue);

        SetCurrencyCount(setCrystal, setFragment);
    }

    public void SetGrowingData(DefaultData growingData)
    {
        MstData.growing = growingData;
    }
    public void SetGrowingCount(int clickCount)
    {
        MstData.growing.count = clickCount;
    }
}