using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AssetDownloadManager : Singleton<AssetDownloadManager>
{
    public enum AssetTag
    {

    }
    public Dictionary<AssetTag, string> AssetsPaths = new Dictionary<AssetTag, string>()
    {

    };

    public T[] GetAssetsWithPath<T>(string path)
    {

        var loadData = AssetDatabase.LoadAllAssetsAtPath(path) as T[];
        return loadData;
    }
}
