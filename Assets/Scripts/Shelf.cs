using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    public static int PotMaxPlaceCount = 10;
    public static int DefaultShelfCount = 2;

    public Image[] potPlaces = new Image[10];

    public void Init()
    {
        for(int i = 0; i < potPlaces.Length; i++)
        {
            potPlaces[i].sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>("blank")[0];
        }
    }
    public void Init(string[] potIds)
    {
        if(potIds.Length > potPlaces.Length)
        {
            Debug.LogError($"PotIds Data Length is over {potPlaces.Length}: {potIds.Length}");
            Init();
            return;
        }
        for (int i = 0; i < potPlaces.Length; i++)
        {
            if (i < potIds.Length)
            {
                var path = potIds[i];
                path = path.Replace("plant", "planticon");
                path = string.IsNullOrEmpty(path) ? "blank" : path;
                Debug.Log($"Shelf.Init(): Get Image Path = {path}");
                potPlaces[i].sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>(path)[0];
            }
            else
            {
                potPlaces[i].sprite = AssetDownloadManager.Instance.GetAssetsWithPath<Sprite>("blank")[0];
            }
        }
    }
}
