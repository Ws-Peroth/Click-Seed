using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI cellNameText;
    public Image cellImage;
    public TextMeshProUGUI cellCountText;
    public InventoryPopup.InventoryType cellType;



    public Dictionary<InventoryPopup.InventoryType, string> preText = new()
    {
        { InventoryPopup.InventoryType.Inventory, "count : " },
        { InventoryPopup.InventoryType.Shop, "price : " },
    };

    public void Init(InventoryPopup.InventoryData data)
    {
        cellType = data.inventoryType;
        cellNameText.text = data.name;
        cellImage.sprite = data.image;
        UpdateCellCount(data.count);

        var mstData = DataManager.Instance.GetMstData();

        

        button.onClick.AddListener(() =>
        {
            Debug.Log($"Click Cell : {data.name} ({data.id})");
            GlobalEventController.Instance.SendEvent(data.id, new object[]{ data });
        });
    }

    public void UpdateCellCount(int count)
    {
        var isTypeFind = preText.ContainsKey(cellType);
        if (isTypeFind)
        {
            cellCountText.text = $"{preText[cellType]}{count}";
        }
        else
        {
            cellCountText.text = "";
        }
    }
}
