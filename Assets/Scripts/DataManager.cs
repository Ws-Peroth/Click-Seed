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
    public string questId;
    public string npcName;
    public string questStory;
    public string plantId;
    public int[] questReward;

    public QuestData(string questId, string npcName, string questStory, string plantId, int[] questReward)
    {
        this.questId = questId;
        this.npcName = npcName;
        this.questStory = questStory;
        this.plantId = plantId;
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
    public List<QuestData> questSettingData;
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
        List<QuestData> questSettingData, 
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
        this.questSettingData = questSettingData;
        this.shelf = shelf;
        this.buffs = buffs;
        this.buffSettingData = buffSettingData;
    }
}
#endregion

public class DataManager : Singleton<DataManager>
{
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
                new DefaultData ("currency", "Crescent", 0),
                new DefaultData ("currency2", "Lunar Shard ", 0),
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
                new DefaultData ("Seed6", "Sunflower Seed", 0),
                new DefaultData ("Seed10", "Cactus Seed", 0),
                new DefaultData ("Seed1", "Slime Seed", 0),
                new DefaultData ("Seed3", "Floresent Seed", 0),
                new DefaultData ("Seed4", "Mushroom Seed", 0),
                new DefaultData ("Seed5", "Tentaclel Seed", 0),
                new DefaultData ("Seed8", "Carnivorous Seed", 0),
                new DefaultData ("Seed9", "Lettuce Flower Seed", 0),
                new DefaultData ("Seed2", "Crystal Seed", 0),
                new DefaultData ("Seed7", "Seed on Fire", 0),
            }; 
            var shopList = new List<DefaultData> {
                // seedID, seedName, --
                new DefaultData ("ShopIconElixir", "Elixer Shop", 0),
                new DefaultData ("ShopIconSeed", "Seed Shop", 0),
            };
            var plant = new List<DefaultData> {
                // plantID, plantName, TotalClickCount
                new DefaultData ("plant6", "Sunflower Seed", 500),
                new DefaultData ("plant10", "Cactus Seed", 1000),
                new DefaultData ("plant1", "Slime Seed", 2000),
                new DefaultData ("plant3", "Floresent Seed", 2500),
                new DefaultData ("plant4", "Mushroom Seed", 3000),
                new DefaultData ("plant5", "Tentaclel Seed", 3500),
                new DefaultData ("plant8", "Carnivorous Seed", 5000),
                new DefaultData ("plant9", "Lettuce Flower Seed", 5000),
                new DefaultData ("plant2", "Crystal Seed", 7000),
                new DefaultData ("plant7", "Seed on Fire", 10000),
            };
            var seedShop = new List<ProductData> {
                // seedID, seedName, price[currency1, currency2]
                new ProductData ("Seed6", "Sunflower Seed",     new int[]{ 0, 0}),
                new ProductData ("Seed10", "Cactus Seed",       new int[]{ 0, 30}),
                new ProductData ("Seed1", "Slime Seed",         new int[]{ 0, 50}),
                new ProductData ("Seed3", "Floresent Seed",     new int[]{ 0, 100}),
                new ProductData ("Seed4", "Mushroom Seed",      new int[]{ 0, 150}),
                new ProductData ("Seed5", "Tentaclel Seed",     new int[]{ 0, 200}),
                new ProductData ("Seed8", "Carnivorous Seed",   new int[]{ 0, 300}),
                new ProductData ("Seed9", "Lettuce Flower Seed",new int[]{ 1, 0}),
                new ProductData ("Seed2", "Crystal Seed",       new int[]{ 2, 0}),
                new ProductData ("Seed7", "Seed on Fire",       new int[]{ 3, 0}),
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
            var questSettingData = new List<QuestData> {
                 new QuestData("quest_01", "Wealthy Man", "I seek a plant with florescent blossoms that can create a dazzling display of colors in my conservatory, enchanting all who enter. would I be able to purchace that radiant flower?", "planticon3", new int[]{ 0, 700 }),
                 new QuestData("quest_02", "Wealthy Man", "I've heard rumors of a mushroom plant with unique properties, capable of creating an extraordinary visual experience. Is such a marvel available in your plant collection?", "planticon4", new int[]{ 0, 1000 }),
                 new QuestData("quest_03", "Wealthy Man", "I desire a sunflower plant of unparalleled size and vibrancy, one that can compete with the grandeur of my extensive gardens. Can you provide me with a sunflower that will leave onlookers in awe?", "planticon6", new int[]{ 0, 100 }),
                 new QuestData("quest_04", "Wealthy Man", "I'm looking for a plant that exudes opulence and elegance, something that would make a striking addition to my mansion's grand entrance. Could i be able to buy a crystal plant?", "planticon2", new int[]{ 5, 0 }),
                 new QuestData("quest_05", "Little boy", "My mother is suffering from a persistent cough. Is there a plant that can help alleviate her symptoms and bring her some relief?", "planticon1", new int[]{ 0, 500 }),
                 new QuestData("quest_06", "Little boy", "I'm searching for a plant with soothing properties to create a calming tea for my sick mother. Do you have anything that can provide comfort and healing?", "planticon6", new int[]{ 0, 100 }),
                 new QuestData("quest_07", "Little boy", "I've heard tales of a mystical plant known for its ability to boost the immune system. Can you recommend something that can help strengthen my mother's health?", "planticon7", new int[]{ 10, 0 }),
                 new QuestData("quest_08", "Dwarf", "I'm in search of a robust plant that can withstand the harsh conditions of the underground tunnels. Do you have anything resilient and low-maintenance?", "planticon5", new int[]{ 0, 2000 }),
                 new QuestData("quest_09", "Dwarf", "I've heard tales of a plant that can illuminate dark caverns with its radiant glow. Is there such a luminescent specimen available here?", "planticon7", new int[]{ 10, 0 }),
                 new QuestData("quest_10", "Dwarf", "Is there a plant with spiky, protective thorns that can ward off intruders from our mining encampments? I need something that screams \"keep out!\"", "planticon10", new int[]{ 0, 300 }),
                 new QuestData("quest_11", "Dwarf", "I seek a plant that can produce nourishing seeds or fruits, sustaining our mining community during long expeditions. Is there a plant that fits this description?", "planticon5", new int[]{ 0, 2000 }),
                 new QuestData("quest_12", "Dwarf", "I've heard rumors of a carnivorous plant that can defend our tunnels from pests and unwanted creatures. Can you guide me to such a fierce and voracious botanical marvel?", "planticon8", new int[]{ 0, 2500 }),
                 new QuestData("quest_13", "Elf", "I've heard tales of a plant that possesses healing properties, able to mend wounds swiftly. Can you guide me to such a miraculous herb?", "planticon1", new int[]{ 0, 500 }),
                 new QuestData("quest_14", "Elf", "I long for a plant with vibrant colors and delicate petals that can be used for crafting enchanting elixirs. Can you recommend one that fits the bill?", "planticon9", new int[]{ 3, 0 }),
                 new QuestData("quest_15", "Elf", "I'm on a quest for a rare plant said to possess ancient wisdom and the ability to communicate with nature. Is such a mystical specimen available here?", "planticon4", new int[]{ 0, 1000 }),
                 new QuestData("quest_16", "Elf", "Excuse me, I'm looking for a unique plant to enhance my garden. Do you have any recommendations?", "planticon2", new int[]{ 5, 0 }),
                 new QuestData("quest_17", "Elf", "Is the slime plant available for purchase? I'm captivated by its enchanting properties and would love to add it to my garden.", "planticon1", new int[]{ 0, 100 }),
                 new QuestData("quest_18", "Wizard", "I'm in need of a plant that can enhance the potency of my spells. Is there a botanical marvel that can amplify magical energies?", "planticon1", new int[]{ 0, 100 }),
                 new QuestData("quest_19", "Wizard", "I'm searching for a plant with mystical properties that can aid in divination and scrying. Do you have something that can heighten my powers of foresight?", "planticon4", new int[]{ 0, 1000 }),
                 new QuestData("quest_20", "Wizard", "Is there a plant with luminescent leaves or petals that can serve as a natural source of illumination for my enchanted laboratory?", "planticon7", new int[]{ 10, 0 }),
                 new QuestData("quest_21", "Wizard", "I'm in search of a plant that can create a protective barrier against dark forces and unwanted magical influences. Is there a plant that can act as a guardian for my spellcasting sanctum?", "planticon10", new int[]{ 0, 300 }),
                 new QuestData("quest_22", "Wizard", "I seek a plant with magical tentacles that can assist me in potion brewing, specifically for stirring and mixing ingredients with precision. Is there a plant that fits this description?", "planticon5", new int[]{ 0, 2000 }),
            };
            var quest = new List<QuestData>
            {
                new QuestData("quest_03", "Wealthy Man", "I desire a sunflower plant of unparalleled size and vibrancy, one that can compete with the grandeur of my extensive gardens. Can you provide me with a sunflower that will leave onlookers in awe?", "planticon6", new int[]{ 0, 100 }),
                new QuestData("quest_06", "Little boy", "I'm searching for a plant with soothing properties to create a calming tea for my sick mother. Do you have anything that can provide comfort and healing?", "planticon6", new int[]{ 0, 100 }),
                new QuestData("quest_05", "Little boy", "My mother is suffering from a persistent cough. Is there a plant that can help alleviate her symptoms and bring her some relief?", "planticon1", new int[]{ 0, 500 }),
                new QuestData("quest_17", "Elf", "Is the slime plant available for purchase? I'm captivated by its enchanting properties and would love to add it to my garden.", "planticon1", new int[]{ 0, 100 }),
                new QuestData("quest_01", "Wealthy Man", "I seek a plant with florescent blossoms that can create a dazzling display of colors in my conservatory, enchanting all who enter. would I be able to purchace that radiant flower?", "planticon3", new int[]{ 0, 700 }),
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
                new BuffData("Elixir1", BuffType.AdditionalClick, 5, 5 * 60),
                new BuffData("Elixir2", BuffType.AdditionalClick, 10, 10 * 60),
                new BuffData("Elixir3", BuffType.AutoClick, 5, 3 * 60),
                new BuffData("Elixir4", BuffType.AutoClick, 10, 5 * 60)
            };
            MstData.Init(
                currency:   currency,
                elixer:     elixer,
                inventory:  inventory,
                shopList:   shopList,
                plant:      plant,
                growing:    growing,
                quest: quest,
                questSettingData: questSettingData,
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
        var result = GameManager.CalcTotalCurrencyToCurrency(totalCurrency);

        SetCurrencyCount(result[0], result[1]);
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