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

    // 이벤트 수신을 위한 이벤트 키 값들
    private readonly string[] EventIds = new string[]
    {
            "seedShop",
            "elixirShop",
    };

    // Start is called before the first frame update
    private void Start()
    {
        popup.gameObject.SetActive(false);
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
                    new InventoryPopup.InventoryData(data.name, sprite[0], InventoryPopup.InventoryType.Inventory, data.count)
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
                    new InventoryPopup.InventoryData(data.name, sprite[0], InventoryPopup.InventoryType.Inventory, data.count)
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
                    new InventoryPopup.InventoryData(data.name, sprite[0], InventoryPopup.InventoryType.ShopList, data.count)
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

    public void ReceiveEvent(string EventId)
    {
        throw new System.NotImplementedException();
    }

    public object GetOriginObject()
    {
        return this;
    }
}
