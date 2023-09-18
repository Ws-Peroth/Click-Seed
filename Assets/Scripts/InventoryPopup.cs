using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : Popup
{
    public GameObject content;
    public GameObject cell;
    public List<GameObject> cellList = new();

    public enum InventoryType
    {
        Inventory,
        ShopList,
        Shop,
        Quest
    }

    public class InventoryData
    {
        public string name;
        public string id;
        public Sprite image;
        public InventoryType inventoryType;
        public int count;

        public InventoryData(string name, string id, Sprite image, InventoryType inventoryType, int count)
        {
            this.name = name;
            this.id = id;
            this.image = image;
            this.inventoryType = inventoryType;
            this.count = count;
        }
    }
    public class QuestData
    {
        public string id;
        public string npcName;
        public string questStory;
        public Sprite image;
        public InventoryType inventoryType;
        public int[] rewards;

        public QuestData(string id, string npcName, string questStory, Sprite image, InventoryType inventoryType, int[] rewards)
        {
            this.id = id;
            this.npcName = npcName;
            this.questStory = questStory;
            this.inventoryType = inventoryType;
            this.rewards = rewards;
            this.image = image;
        }
    }
    public class ShopData
    {
        public string name;
        public string id;
        public Sprite image;
        public InventoryType inventoryType;
        public int[] price;

        public ShopData(string name, string id, Sprite image, InventoryType inventoryType, int[] price)
        {
            this.name = name;
            this.id = id;
            this.image = image;
            this.inventoryType = inventoryType;
            this.price = price;
        }
    }
    public void Init(InventoryData[] inventoryDatas)
    {
        foreach(var cell in cellList){
            Destroy(cell);
        }
        cellList = new List<GameObject>();
        for (int i = 0; i < inventoryDatas.Length; i++)
        {
            var newCell = Instantiate(cell, Vector2.zero, Quaternion.identity, content.transform).GetComponent<InventoryCell>();
            newCell.Init(inventoryDatas[i]);
            newCell.gameObject.SetActive(true);
            cellList.Add(newCell.gameObject);
        }
    }

    public void Init(ShopData[] shopDatas)
    {
        foreach (var cell in cellList)
        {
            Destroy(cell);
        }
        cellList = new List<GameObject>();
        for (int i = 0; i < shopDatas.Length; i++)
        {
            var newCell = Instantiate(cell, Vector2.zero, Quaternion.identity, content.transform).GetComponent<InventoryCell>();
            newCell.Init(shopDatas[i]);
            newCell.gameObject.SetActive(true);
            cellList.Add(newCell.gameObject);
        }
    }

    public void Init(QuestData[] questDatas)
    {
        foreach (var cell in cellList)
        {
            Destroy(cell);
        }
        cellList = new List<GameObject>();
        for (int i = 0; i < questDatas.Length; i++)
        {
            var newCell = Instantiate(cell, Vector2.zero, Quaternion.identity, content.transform).GetComponent<QuestCell>();
            newCell.Init(questDatas[i], QuestRemoveAndUpdateCellIndex);
            newCell.gameObject.SetActive(true);
            cellList.Add(newCell.gameObject);
        }
        UpdateQuestCellIndex();
    }

    public void QuestRemoveAndUpdateCellIndex(int cellIndex)
    {
        cellList.RemoveAt(cellIndex);
        UpdateQuestCellIndex();
    }
    public void UpdateQuestCellIndex()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            cellList[i].GetComponent<QuestCell>().SetCellIndex(i);
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
