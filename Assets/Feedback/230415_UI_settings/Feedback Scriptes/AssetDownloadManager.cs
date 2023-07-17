using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetDownloadManager : Singleton<AssetDownloadManager>
{
    public enum AssetTag
    {
        Elixir,
        Seed,
        Plant,
        PlantIcon
    };
    public Dictionary<AssetTag, string> AssetsTagPaths = new Dictionary<AssetTag, string>()
    {
        {AssetTag.Elixir, "Elixir"},
        {AssetTag.Seed, "Seed"},
        {AssetTag.Plant, "Plant"},
        {AssetTag.PlantIcon, "PlantIcon"},
    };

    public T[] GetAssetsWithPath<T> (string path) where T : Object
    {
        if (Resources.LoadAll<T>(path) is not T[] data)
        {
            Debug.Log($"data not found :: {path}");
            return null;
        }
        if(data.Length == 0)
        {
            Debug.Log($"data length is 0 :: {path}");
            return null;
        }
        return data;
    }

    public List<T[]> GetAllAssetsWithTag<T>(AssetTag tag) where T : Object
    {
        var loadData = new List<T[]>();
        var index = 1;
        var defaultPath = AssetsTagPaths[tag];
        while (true)
        {
            var getAsset = GetAssetsWithPath<T>($"{defaultPath}{index}");
            if(getAsset == null)
            {
                break;
            }
            index++;
            loadData.Add(getAsset);
        }
        return loadData;
    }
}
