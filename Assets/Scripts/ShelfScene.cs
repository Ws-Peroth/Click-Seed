using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShelfScene : MonoBehaviour, IGlobalEventReceiver
{
    [SerializeField] private Button questButton;
    [SerializeField] private Button moveToPotSceneButton;

    [SerializeField] private GameObject shelfContenArea;
    [SerializeField] private Shelf shelfOrigin;
    [SerializeField] private List<Shelf> shelfList = new List<Shelf>();

    [SerializeField] private TextMeshProUGUI[] currencyText = new TextMeshProUGUI[2];

    [SerializeField] private InventoryPopup popup;
    [SerializeField] private ConfirmPopup confirmPopup;

    readonly string[] EventIds = new string[]
    {
        "Update",
        "Select",
    };

    public object GetOriginObject()
    {
        return this;
    }

    public void InitShelf()
    {
        var shelfData = DataManager.Instance.GetMstData().shelf;
        int shelfCount = shelfData.Count / Shelf.PotMaxPlaceCount;  // return need shelf index
        // 1~9 / 10 = 0     // shelfData[0~9]: shelf[0]
        // 10~19 / 10 = 1   // shelfData[10~19]: shelf[1]
        // 20~29 / 10 = 2   // shelfData[20~29]: shelf[2]
        // ...

        for(var i = 0; i < shelfCount + 1; i++)
        {
            // 생성된 Shelf가 없다면
            if(i >= shelfList.Count)
            {
                var newShelf = Instantiate(shelfOrigin, Vector2.zero, Quaternion.identity, shelfContenArea.transform);
                shelfList.Add(newShelf);
                newShelf.gameObject.SetActive(true);
            }
            var st = i * Shelf.PotMaxPlaceCount;        // 0, 10, 20, 30 ...
            var ed = st + Shelf.PotMaxPlaceCount - 1;   // 9, 19, 29, 39 ...
                                                        // get:: index >= {0, 10, 20, 30...}  &&  index <= {9, 19, 29, 39...}
            var initData = shelfData.Where((value, index) => index >= st && index <= ed).ToArray();
            shelfList[i].Init(initData);
        }

    }

    public void ReceiveEvent(string EventId, string name, object[] param)
    {
        if(EventId == "Update")
        {
            // name == "Shelf"
            InitShelf();
        }
        if(EventId == "Select")
        {
            // name = "QuestFailure"
            var product = param[0] as InventoryPopup.QuestData;
            confirmPopup.gameObject.SetActive(true);
            // 구매불가 팝업
            confirmPopup.Init(
                "Confirm", $"Not enough plant to submit in quest",
                () => { }, () => { },
                true, false
            );
            return;
        }
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

    // Start is called before the first frame update
    void Start()
    {
        InitShelf();

        moveToPotSceneButton.onClick.AddListener(() =>
        {
            SceneManagerEx.Instance.LoadScene(SceneManagerEx.Scenes.Main);
        });

        questButton.onClick.AddListener(() =>
        {
            Debug.Log("Open Quest");
            popup.titleText.text = "Quest";

            var inventoryData = new List<InventoryPopup.QuestData>();
            var questData = DataManager.Instance.GetMstData().quest;
            if(questData == null)
            {
                Debug.LogError("questData is null");
            }
            else if (questData.Count == 0)
            {
                Debug.LogError("questData Count is 0");
            }
            else
            {
                Debug.Log($"questData Count is {questData.Count}");
            }
            for (int i = 0; i < DataManager.Instance.GetMstData().quest.Count; i++)
            {
                var data = DataManager.Instance.GetMstData().quest[i];
                var sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(data.plantId).First();
                inventoryData.Add(
                    new InventoryPopup.QuestData(data.questId, data.npcName, data.questStory, sprite, InventoryPopup.InventoryType.Quest, data.questReward)
                ); ;
            }
            popup.Init(inventoryData.ToArray());
            popup.gameObject.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < currencyText.Length; i++)
        {
            currencyText[i].text = DataManager.Instance.GetMstData().currency[i].count.ToString();
        }
    }
}
