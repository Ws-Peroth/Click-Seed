using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField] private Button elixirButton;
    [SerializeField] private Button seedButton;
    [SerializeField] private Button shelfButton;
    [SerializeField] private Button potButton;

    [SerializeField] private TextMeshProUGUI[] currencyText = new TextMeshProUGUI[2];
    [SerializeField] private TextMeshProUGUI clickCountText;

    [SerializeField] private Sprite[] popupContentImages;
    [SerializeField] private Sprite[] seedImages;
    [SerializeField] private Image seed;

    [SerializeField] private GameObject popup;


    // Start is called before the first frame update
    private void Start()
    {
        popup.SetActive(false);

        GameManager.Instance.isPlanted = false;
        GameManager.Instance.potClickCount = 0;

        seed.sprite = null;
        seed.color = new Color(1, 1, 1, 0);

        clickCountText.text = "0";

        elixirButton.onClick.AddListener(() =>
        {

            popup.SetActive(true);
        });

        seedButton.onClick.AddListener(() =>
        {

            popup.SetActive(true);
        });

        potButton.onClick.AddListener(() =>
        {
            GameManager.Instance.potClicked(GrowUp);
            clickCountText.text = GameManager.Instance.potClickCount.ToString();
        });
    }

    // Update is called once per frame
    private void Update()
    {
        for(int i = 0; i < currencyText.Length; i++)
        {
            currencyText[i].text = GameManager.Instance.currencyData[i].ToString();
        }
    }

    private void GrowUp(GameManager.GrowupType type)
    {

    }


}
