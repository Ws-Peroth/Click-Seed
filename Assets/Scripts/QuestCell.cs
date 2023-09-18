using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    private int cellIndex;
    [SerializeField] private Image plantIcon;
    [SerializeField] private GameObject[] priceArea = new GameObject[2];
    [SerializeField] private TextMeshProUGUI[] priceTexts = new TextMeshProUGUI[2];
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI questStoryText;

    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button cancelButton;

    public InventoryPopup.InventoryType cellType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(InventoryPopup.QuestData data, Action<int> indexUpdateCallback )
    {
        cellType = data.inventoryType;
        npcNameText.text = data.npcName;
        questStoryText.text = data.questStory;
        plantIcon.sprite = data.image;

        UpdateCellCount(data.rewards);
        var mstData = DataManager.Instance.GetMstData();

        cancelButton.onClick.AddListener(() =>
        {
            Debug.Log($"Cancel Quest");
            GameManager.Instance.CancelQuest(cellIndex);

            GlobalEventController.Instance.SendEvent("Update", "Shelf", new object[] { data });
            indexUpdateCallback(cellIndex);
            gameObject.SetActive(false);
            Destroy(gameObject);
        });
        purchaseButton.onClick.AddListener(() =>
        {
            Debug.Log($"Purchase Quest");
            if (GameManager.Instance.ClearQuest(cellIndex))
            {
                indexUpdateCallback(cellIndex); 
                GlobalEventController.Instance.SendEvent("Update", "Shelf", new object[] { data });
                gameObject.SetActive(false);
                Destroy(gameObject);
                return;
            }

            GlobalEventController.Instance.SendEvent("Select", "QuestFailure", new object[] { data });
        });
    }

    public void SetCellIndex(int cellIndex)
    {
        this.cellIndex = cellIndex;
    }

    public void UpdateCellCount(int[] count)
    {
        for(int i = 0; i < priceArea.Length; i++)
        {
            priceArea[i].SetActive(count[i] > 0);
            priceTexts[i].text = count[i].ToString();
        }
    }
}
