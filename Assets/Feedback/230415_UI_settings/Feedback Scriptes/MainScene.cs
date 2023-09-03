using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour, IGlobalEventReceiver
{
    [SerializeField] private Button elixirButton;
    [SerializeField] private Button seedButton;
    [SerializeField] private Button shelfButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button potButton;

    [SerializeField] private TextMeshProUGUI[] currencyText = new TextMeshProUGUI[2];
    [SerializeField] private TextMeshProUGUI clickCountText;

    [SerializeField] private Sprite[] popupContentImages;
    [SerializeField] private Sprite[] growSeedImages;
    [SerializeField] private Image seed;

    [SerializeField] private InventoryPopup popup;
    [SerializeField] private ConfirmPopup confirmPopup;

    // 이벤트 수신을 위한 이벤트 키 값들
    private readonly string[] EventIds = new string[]
    {
        // "ShopIconSeed",
        // "ShopIconElixir",
        // "Elixir",
        // "Seed",
        "SelectItem",
        "Selected",
        "BuyItem",
        "Buy",
        "UseItem"
    };

    private readonly string[] BtnNames = new string[]
    {
        "ShopIconSeed",
        "ShopIconElixir",
        "Elixir",
        "Seed",
    };

    // Start is called before the first frame update
    private void Start()
    {
        popup.gameObject.SetActive(false);
        confirmPopup.gameObject.SetActive(false);
        var isDataNull = DataManager.Instance.GetMstData().growing == null;
        var path = (isDataNull || string.IsNullOrEmpty(DataManager.Instance.GetMstData().growing.name)) ? "" : DataManager.Instance.GetMstData().growing.name;
        var plantSprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(path);
        seed.sprite = plantSprite != null && plantSprite[0] != null ? plantSprite[0] : null;
        seed.color = new Color(1, 1, 1, 0);

        clickCountText.text = "0";

        elixirButton.onClick.AddListener(() =>
        {
            popup.categoryImage.sprite = popupContentImages[0];
            popup.titleText.text = "Elixir Inventory";

            var inventoryData = new List<InventoryPopup.InventoryData>();
            for(int i = 0; i < DataManager.Instance.GetMstData().elixer.Count; i++)
            {
                var data = DataManager.Instance.GetMstData().elixer[i];
                if (data.count == 0)
                {
                    continue;
                }
                var sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(data.id);
                inventoryData.Add(
                    new InventoryPopup.InventoryData(data.name, data.id, sprite[0], InventoryPopup.InventoryType.Inventory, data.count)
                );
            }
            popup.Init(inventoryData.ToArray());
            popup.gameObject.SetActive(true);
        });

        seedButton.onClick.AddListener(() =>
        {
            popup.categoryImage.sprite = popupContentImages[1];
            popup.titleText.text = "Seed Inventory";

            var inventoryData = new List<InventoryPopup.InventoryData>();
            for (int i = 0; i < DataManager.Instance.GetMstData().inventory.Count; i++)
            {
                var data = DataManager.Instance.GetMstData().inventory[i];
                if (data.count == 0)
                {
                    continue;
                }
                var sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(data.id);
                inventoryData.Add(
                    new InventoryPopup.InventoryData(data.name, data.id, sprite[0], InventoryPopup.InventoryType.Inventory, data.count)
                );
            }
            popup.Init(inventoryData.ToArray());
            popup.gameObject.SetActive(true);
        });

        shopButton.onClick.AddListener(() =>
        {
            Debug.Log("Click Shop List");

            popup.categoryImage.sprite = popupContentImages[1];
            popup.titleText.text = "Shop List";

            var inventoryData = new List<InventoryPopup.InventoryData>();
            for (int i = 0; i < DataManager.Instance.GetMstData().shopList.Count; i++)
            {
                var data = DataManager.Instance.GetMstData().shopList[i];
                var sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(data.id);
                Debug.Log($"Is Sprite is NULL  : {sprite == null}");
                inventoryData.Add(
                    new InventoryPopup.InventoryData(data.name, data.id, sprite[0], InventoryPopup.InventoryType.ShopList, data.count)
                );
            }
            popup.Init(inventoryData.ToArray());
            popup.gameObject.SetActive(true);
            // var elixirShopIcon = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>("Elixir");
            // var seedShopIcon = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>("smallpot");

        });

        potButton.onClick.AddListener(() =>
        {
            Debug.Log("Clicked!");
            GameManager.Instance.PotClicked(GrowUp);
            clickCountText.text = DataManager.Instance.GetMstData().growing.count.ToString();
        });
    }

    private void OnEnable()
    {
        // 이벤트 등록을 위해 이벤트 ID 등록
        if (this is IGlobalEventReceiver Interface)
        {
            Interface.Regist(Interface, EventIds);
        }
    }

    private void OnDestroy()
    {
        // 해당 오브젝트의 파괴 이후에 해당 오브젝트에 대한 이벤트 호출 방지를 위해 이벤트 등록 해제
        if (this is IGlobalEventReceiver Interface)
        {
            Debug.Log("OnDestroy: Unregist()");
            Debug.Log($"Manager is null = {GlobalEventController.Instance == null}");
            Interface.Unregist(Interface, EventIds);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for(int i = 0; i < currencyText.Length; i++)
        {
            currencyText[i].text = DataManager.Instance.GetMstData().currency[i].count.ToString();
        }
    }

    private void GrowUp(DataManager.GrowupType type)
    {

    }

    private string ItemIdToBtnName (string itemId)
    {
        string key = itemId;

        foreach (var id in BtnNames)
        {
            if (key.StartsWith(id))
            {
                key = id;
            }
        }
        return key;
    }

    private void SelectShopListCell(string key)
    {
        if (key == "ShopIconSeed")
        {
            popup.titleText.text = "Seed Inventory";

            var inventoryData = new List<InventoryPopup.ShopData>();
            for (int i = 0; i < DataManager.Instance.GetMstData().seedShop.Count; i++)
            {
                var data = DataManager.Instance.GetMstData().seedShop[i];
                var sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(data.id);
                inventoryData.Add(
                    new InventoryPopup.ShopData(
                        data.name, data.id, sprite[0], InventoryPopup.InventoryType.Shop, data.prices
                        )
                );
            }
            popup.Init(inventoryData.ToArray());
            popup.gameObject.SetActive(true);
        }
        else if (key == "ShopIconElixir")
        {
            // Shop에서 Elixir 선택시, Elixir 구매 절차
            // popup.titleText.text = "Seed Inventory";

            var inventoryData = new List<InventoryPopup.ShopData>();
            for (int i = 0; i < DataManager.Instance.GetMstData().elixerShop.Count; i++)
            {
                var data = DataManager.Instance.GetMstData().elixerShop[i];
                var sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(data.id);
                inventoryData.Add(
                    new InventoryPopup.ShopData(
                        data.name, data.id, sprite[0], InventoryPopup.InventoryType.Shop, data.prices
                        )
                );
            }
            popup.Init(inventoryData.ToArray());
            popup.gameObject.SetActive(true);
        }
    }

    public void ReceiveEvent(string EventId, string name, object[] param)
    {
        string keyType = ItemIdToBtnName(name);
        if (EventId == "BuyItem")
        {
            if (param == null || param.Length <= 0) { return; }

            InventoryPopup.ShopData product = param[0] as InventoryPopup.ShopData;
            var price = product.price;
            var totalPrice = (ulong)(price[0] * DataManager.FragmentToCrystalValue + price[1]);
            var amount = DataManager.Instance.GetMstData().currency;
            var totalAmount = (ulong)amount[0].count * DataManager.FragmentToCrystalValue + (ulong)amount[1].count;

            confirmPopup.gameObject.SetActive(true);
            if (totalAmount < totalPrice)
            {
                // 구매불가 팝업
                confirmPopup.Init(
                    "Confirm", $"Insufficient resources to purchase {product.name }",
                    () => { }, () => { },
                    true, false
                );
                return;
            }
            // 구매 절차 팝업
            confirmPopup.Init(
                "Confirm", $"Purchase {product.name }",
                () =>
                {
                    // OK Callback
                    // var datas = param as InventoryPopup.ShopData[];
                    GlobalEventController.Instance.SendEvent("Buy", name, param);
                }
            );
        }
        else if (EventId == "SelectItem")
        {
            if(keyType == "ShopIconSeed" || keyType == "ShopIconElixir")
            {
                SelectShopListCell(keyType);
            }
            else if (keyType == "Seed" || keyType == "Elixir")
            {
                GlobalEventController.Instance.SendEvent("Selected", name, param);
                return;
            }
        }
        else if (EventId == "Selected")
        {

            if (param == null || param.Length <= 0) { return; }

            var product = param[0] as InventoryPopup.InventoryData;
            var need = product.count;
            int havingCount = 0;

            if (keyType == "Elixir")
            {
                foreach (var item in DataManager.Instance.GetMstData().elixer)
                {
                    if(item.id == name)
                    {
                        havingCount = item.count;
                    }
                }
                Debug.Log($"Use Elixir {product.name}");
            }
            else if (keyType == "Seed")
            {
                foreach (var item in DataManager.Instance.GetMstData().inventory)
                {
                    if (item.id == name)
                    {
                        havingCount = item.count;
                    }
                }
                Debug.Log($"Plant Seed {product.name}");
            }

            confirmPopup.gameObject.SetActive(true);
            if (havingCount < need)
            {
                // 사용불가 팝업
                confirmPopup.Init(
                    "Confirm", $"Not enough quantity for {product.name } to use",
                    () => { }, () => { },
                    true, false
                );
                return;
            }

            confirmPopup.Init(
                "Confirm", $"Do you want to { (keyType == "Seed" ? "plant" : "use")} {product.name}?" ,
                () =>
                {
                    GlobalEventController.Instance.SendEvent("UseItem", name, param);
                }
            );
        }
        else if (EventId == "Buy")
        {
            var product = param[0] as InventoryPopup.ShopData;
            DataManager.DataType buyType;
            // TODO: 1. GameManager에서 mstData의 "currency"와 "elixer" / "inventory" 데이터 부분을 수정하는 함수 작성
            // TODO: 2. 작성한 함수 호출
            if(keyType == "Seed")
            {
                buyType = DataManager.DataType.SeedShop;
            }
            else if(keyType == "Elixir")
            {
                buyType = DataManager.DataType.ElixerShop;
            }
            else
            {
                Debug.LogError($"DataType Not Found\nPurchase Process Failed");
                return;
            }
            if (GameManager.Instance.BuyItem(buyType, name, keyType))
            {
                Debug.Log($"{product.name}을(를) 구매하였습니다");
            }
        }
        else if (EventId == "UseItem")
        {
            var product = param[0] as InventoryPopup.InventoryData;
            // TODO: 1. GameManager에서 mstData의 "currency"와 "elixer" / "inventory" 데이터 부분을 수정하는 함수 작성
            // TODO: 2. 작성한 함수 호출
            if (keyType == "Seed")
            {
                // Plant
                Debug.Log($"{product.name}을(를) 심었습니다");
                PlantSeed(product);
            }
            else if (keyType == "Elixir")
            {
                // Use Buff Item
                Debug.Log($"{product.name}을(를) 사용하였습니다");

            }
        }
    }

    public void PlantSeed(InventoryPopup.InventoryData data)
    {

    }

    public object GetOriginObject()
    {
        return this;
    }
}
