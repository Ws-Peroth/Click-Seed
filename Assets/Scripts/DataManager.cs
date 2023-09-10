using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#region DATA_DEFINED

[Serializable]
public class BuffData
{
    public string id;
    public DataManager.BuffType buff;
    public int buffCount;
    // buff is AdditionalClick: buffCount = Additional Click Count
    // buff is AutoClick: buffCount = click count per 1 sec
    public int leftBuffSec;

    public static BuffData Dummy()
    {
        return new BuffData("", DataManager.BuffType.None, 0, 0);
    }

    public BuffData(string id, DataManager.BuffType buff, int buffCount, int leftBuffSec)
    {
        this.id = id;
        this.buff = buff;
        this.buffCount = buffCount;
        this.leftBuffSec = leftBuffSec;
    }
}

[Serializable]
public class DefaultData
{
    public static DefaultData Dummy()
    {
        return new DefaultData("", "", 0);
    }

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
    public List<DefaultData> plant;
    public List<QuestData> quest;
    public List<string> shelf;
    public DefaultData growing;
    public List<BuffData> buffs;
    public List<BuffData> buffSettingData;

    public void Init(
        List<DefaultData> currency,
        List<DefaultData> elixer,
        List<DefaultData> inventory,
        List<ProductData> seedShop,
        List<ProductData> elixerShop,
        List<DefaultData> shopList,
        List<DefaultData> plant,
        DefaultData growing,
        List<QuestData> quest, 
        List<string> shelf, 
        List<BuffData> buffs,
        List<BuffData> buffSettingData)
    {
        this.currency = currency;
        this.elixer = elixer;
        this.inventory = inventory;
        this.seedShop = seedShop;
        this.elixerShop = elixerShop;
        this.shopList = shopList;
        this.plant = plant;
        this.growing = growing;
        this.quest = quest;
        this.shelf = shelf;
        this.buffs = buffs;
        this.buffSettingData = buffSettingData;
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
    public bool RESET_JSON_ON = false;

    public enum BuffType
    {
        None,
        AdditionalClick,
        AutoClick
    }

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
        Plant,
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
        0,
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
        if(MstData == null || RESET_JSON_ON)
        {
            // Data Init

            MstData = new JsonData();
            var currency = new List<DefaultData> {
                // currencyID, currencyName, havingCount
                new DefaultData ("currency", "Crescent", 1000),
                new DefaultData ("currency2", "Lunar Shard ", 10),
            };
            var elixer = new List<DefaultData> {
                // elixerID, elixerName, havingCount
                new DefaultData ("Elixir1", "Plant Aid", 0),
                new DefaultData ("Elixir2", "Plant Supplements", 0),
                new DefaultData ("Elixir3", "Sprite Helper", 0),
                new DefaultData ("Elixir4", "Elven Blessing", 0),
            };
            var inventory = new List<DefaultData> {
                // seedID, seedName, havingCount
                new DefaultData ("Seed1", "Slime Seed", 0),
                new DefaultData ("Seed2", "Crystal Seed", 0),
                new DefaultData ("Seed3", "Floresent Seed", 0),
                new DefaultData ("Seed4", "Mushroom Seed", 0),
                new DefaultData ("Seed5", "Tentaclel Seed", 0),
                new DefaultData ("Seed6", "Sunflower Seed", 0),
                new DefaultData ("Seed7", "Seed on Fire", 0),
                new DefaultData ("Seed8", "Carnivorous Seed", 0),
                new DefaultData ("Seed9", "Lettuce Flower Seed", 0),
                new DefaultData ("Seed10", "Cactus Seed", 0),
            }; 
            var shopList = new List<DefaultData> {
                // seedID, seedName, --
                new DefaultData ("ShopIconElixir", "Elixer Shop", 0),
                new DefaultData ("ShopIconSeed", "Seed Shop", 0),
            };
            var plant = new List<DefaultData> {
                // plantID, plantName, TotalClickCount
                new DefaultData ("plant1", "Slime Seed", 50),
                new DefaultData ("plant2", "Crystal Seed", 60),
                new DefaultData ("plant3", "Floresent Seed", 70),
                new DefaultData ("plant4", "Mushroom Seed", 80),
                new DefaultData ("plant5", "Tentaclel Seed", 90),
                new DefaultData ("plant6", "Sunflower Seed", 100),
                new DefaultData ("plant7", "Seed on Fire", 120),
                new DefaultData ("plant8", "Carnivorous Seed", 140),
                new DefaultData ("plant9", "Lettuce Flower Seed", 200),
                new DefaultData ("plant10", "Cactus Seed", 250),
            };
            var seedShop = new List<ProductData> {
                // seedID, seedName, price[currency1, currency2]
                new ProductData ("Seed1", "Slime Seed",         new int[]{ 0, 50}),
                new ProductData ("Seed2", "Crystal Seed",       new int[]{ 2, 0}),
                new ProductData ("Seed3", "Floresent Seed",     new int[]{ 0, 100}),
                new ProductData ("Seed4", "Mushroom Seed",      new int[]{ 0, 150}),
                new ProductData ("Seed5", "Tentaclel Seed",     new int[]{ 0, 200}),
                new ProductData ("Seed6", "Sunflower Seed",     new int[]{ 0, 10}),
                new ProductData ("Seed7", "Seed on Fire",       new int[]{ 3, 0}),
                new ProductData ("Seed8", "Carnivorous Seed",   new int[]{ 0, 300}),
                new ProductData ("Seed9", "Lettuce Flower Seed",new int[]{ 1, 0}),
                new ProductData ("Seed10", "Cactus Seed",       new int[]{ 0, 30}),
            };
            var elixerShop = new List<ProductData> {
                // elixerID, elixerName, price[currency1, currency2]
                new ProductData ("Elixir1", "Plant Aid",        new int[]{ 0, 500}),
                new ProductData ("Elixir2", "Plant Supplements",new int[]{ 0, 1000 }),
                new ProductData ("Elixir3", "Sprite Helper",    new int[]{ 0, 2500 }),
                new ProductData ("Elixir4", "Elven Blessing",    new int[]{ 1, 0 }),
            };
            // plantID, plantName, TotalClickCount
            var growing = DefaultData.Dummy();
            var quest = new List<QuestData> {
            };
            var shelf = new List<string>() {
            };
            var buffs = new List<BuffData>()
            {
                new BuffData("Elixir1", BuffType.AdditionalClick, 5, 0),
                new BuffData("Elixir2", BuffType.AdditionalClick, 10, 0),
                new BuffData("Elixir3", BuffType.AutoClick, 5, 0),
                new BuffData("Elixir4", BuffType.AutoClick, 10, 0)
            };
            var buffSettingData = new List<BuffData>()
            {
                /*
                new BuffData("Elixir1", BuffType.AdditionalClick, 5, 5 * 60),
                new BuffData("Elixir2", BuffType.AdditionalClick, 10, 10 * 60),
                new BuffData("Elixir3", BuffType.AutoClick, 5, 3 * 60),
                new BuffData("Elixir4", BuffType.AutoClick, 10, 5 * 60)
                */

                new BuffData("Elixir1", BuffType.AdditionalClick, 5, 1 * 60),
                new BuffData("Elixir2", BuffType.AdditionalClick, 10, 2 * 60),
                new BuffData("Elixir3", BuffType.AutoClick, 5, 1 * 60),
                new BuffData("Elixir4", BuffType.AutoClick, 10, 2 * 60)
                
            };
            MstData.Init(
                currency:   currency,
                elixer:     elixer,
                inventory:  inventory,
                shopList:   shopList,
                plant:      plant,
                growing:    growing,
                quest:      quest,
                shelf:      shelf,
                seedShop:   seedShop,
                elixerShop: elixerShop,
                buffs:      buffs,
                buffSettingData: buffSettingData
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

    public BuffData GetBuffData(string id)
    {
        return GetMstData().buffs.Find((m) => m.id == id);
    }
    public BuffData GetSetBuffData(string id)
    {
        return GetMstData().buffSettingData.Find((m) => m.id == id);
    }

    public DefaultData GetDefaultData(DataType type, string id)
    {
        if (type == DataType.Inventory)
        {
            return GetMstData().inventory.Find((m) => m.id == id);
        }
        else if (type == DataType.Currency)
        {
            return GetMstData().currency.Find((m) => m.id == id);
        }
        else if (type == DataType.Elixer)
        {
            return GetMstData().elixer.Find((m) => m.id == id);
        }
        else if (type == DataType.ShopList)
        {
            return GetMstData().shopList.Find((m) => m.id == id);
        }
        else if (type == DataType.Plant)
        {
            return GetMstData().plant.Find((m) => m.id == id);
        }
        else if (type == DataType.Growing)
        {
            return GetMstData().growing;
        }
        else
        {
            Debug.LogError($"Type Undefined: {type} is not defined in Default Data Types");
            return null;
        }
    }
    public ProductData GetShopData(DataType type, string id)
    {
        if (type == DataType.ElixerShop)
        {
            return GetMstData().elixerShop.Find((m) => m.id == id);
        }
        else if (type == DataType.SeedShop)
        {
            return GetMstData().seedShop.Find((m) => m.id == id);
        }
        else
        {
            Debug.LogError($"Type Undefined: {type} is not defined in Shop Data Types");
            return null;
        }
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
   
    public bool AddShelf(string plantId)
    {
        // If plantId is in MstData.plant
        if (MstData.plant.Exists((m) => m.id == plantId))
        {
            MstData.shelf.Add(plantId);
            MstData.growing = DefaultData.Dummy();
            return true;
        }
        Debug.LogError($"DataManager.AddShelf(): {plantId} is not in MstData.plant");
        return false;
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

    public void SetGrowingCount(int clickCount)
    {
        MstData.growing.count = clickCount;
    }
    public void SetGrowingData(DefaultData data)
    {
        MstData.growing = data;
    }

    public void SetBuffTime(string id, int time)
    {
        var targetItem = GetBuffData(id);

        if (targetItem == null)
        {
            Debug.LogError($"buff에서 {id}을(를) 찾을 수 없습니다");
            return;
        }
        if (time < 0)
        {
            // Can't Use this Seed
            Debug.LogError($"버프 지속 시간은 0 미만이 될 수 없습니다.");
            return;
        }
        targetItem.leftBuffSec = time;
        SaveMstData();
    }
}