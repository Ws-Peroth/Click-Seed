using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class DefaultData{
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
    public int questId;

    public QuestData(string npcId, string npcName, int questId)
    {
        this.npcId = npcId;
        this.npcName = npcName;
        this.questId = questId;
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

public class DataManager : Singleton<DataManager>
{
    // 서버의 DB와 RPC정보의 제공 역할
    // 데이터를 Json으로 저장하며, 갖고있는 데이터를 인스턴스를 통해 준다


    private JsonData MstData;
    public JsonData GetMstData() { return MstData; }

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
    public enum SeedType
    {
        SlimeSeed,
        CrystalSeed,
        FloresentSeed,
        MushroomSeed,
        TentacleSeed,
    }

    public void Start()
    {
        Debug.Log(Path.Combine(Application.persistentDataPath, "mstData.json"));
        MstData = null;
        MstData = JsonManager.Instance.LoadJsonData<JsonData>(Path.Combine(Application.persistentDataPath, "mstData.json"));
        if(MstData == null)
        {
            // Data Init

            MstData = new JsonData();
            var currency = new List<DefaultData> {
                new DefaultData ("currency", "Crescent", 0),
                new DefaultData ("currency2", "Lunar Shard ", 0),
            };
            var elixer = new List<DefaultData> {
                new DefaultData ("Elixer1", "Plant Aid", 0),
                new DefaultData ("Elixer2", "Plant Supplements", 0),
                new DefaultData ("Elixer3", "Sprite Helper", 0),
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
                new DefaultData ("ElixirIcon", "Elixer Shop", 0),
                new DefaultData ("smallpot", "Seed Shop", 0),
            };
            var seedShop = new List<ProductData> {
                new ProductData ("Seed1", "Slime Seed",         new int[]{ 10, 1000}),
                new ProductData ("Seed2", "Crystal Seed",       new int[]{ 10, 1000}),
                new ProductData ("Seed3", "Floresent Seed",     new int[]{ 10, 1000}),
                new ProductData ("Seed4", "Mushroom Seed",      new int[]{ 10, 1000}),
                new ProductData ("Seed5", "Tentaclel Seed",     new int[]{ 10, 1000}),
                new ProductData ("Seed6", "Sunflower Seed",     new int[]{ 10, 1000}),
                new ProductData ("Seed7", "Seed on Fire",       new int[]{ 10, 1000}),
                new ProductData ("Seed8", "Carnivorous Seed",   new int[]{ 10, 1000}),
                new ProductData ("Seed9", "Lettuce Flower Seed",new int[]{ 10, 1000}),
            };
            var elixerShop = new List<ProductData> {
                new ProductData ("Elixer1", "Plant Aid",        new int[]{ 10, 1000}),
                new ProductData ("Elixer2", "Plant Supplements",new int[]{ 10, 1000 }),
                new ProductData ("Elixer3", "Sprite Helper",    new int[]{ 10, 1000 }),
            };
            DefaultData growing = null;
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
        }

        var data = JsonManager.Instance.ReadJson(Path.Combine(Application.persistentDataPath, "mstData.json"));
        GameManager.Instance.LoadFirstScene();
        Debug.Log(data);
    }
    public void SaveMstData()
    {
        var json = JsonUtility.ToJson(MstData);
        // System.IO.File.WriteAllText(Path.Combine(Application.persistentDataPath, "file.txt"), jsonTextFile);
    }
}
