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
    public GameObject priceObjectGroup;
    public TextMeshProUGUI crystalPriceText;
    public TextMeshProUGUI fragmentPriceText;


    public Dictionary<InventoryPopup.InventoryType, string> preText = new()
    {
        { InventoryPopup.InventoryType.Inventory, "count : " },
    };

    public void Init(InventoryPopup.InventoryData data)
    {
        cellType = data.inventoryType;
        cellNameText.text = data.name;
        cellImage.sprite = data.image;
        UpdateCellCount(data.count);
        priceObjectGroup.SetActive(false);

        var mstData = DataManager.Instance.GetMstData();

        button.onClick.AddListener(() =>
        {
            Debug.Log($"Click Cell : {data.name} ({data.id})");
            GlobalEventController.Instance.SendEvent("Selected", data.id, new object[]{ data });
        });
    }

    public void Init(InventoryPopup.ShopData data)
    {
        cellType = data.inventoryType;
        cellNameText.text = data.name;
        cellImage.sprite = data.image;
        UpdateCellCount(data.price);
        priceObjectGroup.SetActive(true);
        var mstData = DataManager.Instance.GetMstData();

        button.onClick.AddListener(() =>
        {
            Debug.Log($"Buy Cell : {data.name} ({data.id})");
            GlobalEventController.Instance.SendEvent("BuyItem", data.id, new object[] { data });
        });
    }

    public void UpdateCellCount(int count)
    {
        var isTypeFind = preText.ContainsKey(cellType);
        cellCountText.text = "";
        if (isTypeFind)
        {
            cellCountText.text = $"{preText[cellType]}{count}";
        }
        if(cellType == InventoryPopup.InventoryType.Shop)
        {
            cellCountText.text = "";
            crystalPriceText.text = $"{count}";
            fragmentPriceText.text = $"{count}";
        }
    }
    public void UpdateCellCount(int[] count)
    {
        var isTypeFind = preText.ContainsKey(cellType);
        cellCountText.text = "";
        if (isTypeFind)
        {
            cellCountText.text = $"{preText[cellType]}{count}";
        }
        if (cellType == InventoryPopup.InventoryType.Shop)
        {
            cellCountText.text = "";
            crystalPriceText.text = count[0].ToString();
            fragmentPriceText.text = count[1].ToString();
        }
    }
}
