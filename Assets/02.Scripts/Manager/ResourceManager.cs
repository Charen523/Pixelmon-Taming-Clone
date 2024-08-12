using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SocialPlatforms;

public class ResourceManager : Singleton<ResourceManager>
{
    public bool isAutoLoading = false;
    [NonSerialized] public bool isInit = false;

    public async void Init()
    {
        await LoadAddressable();
    }

    #region Use Resources
    public Dictionary<string, object> assetPools = new Dictionary<string, object>();

    public T LoadAsset<T>(string key) where T : UnityEngine.Object
    {
        if (assetPools.ContainsKey(key)) return (T)assetPools[key];
        var asset = Resources.Load<T>(key);
        if (asset != null) assetPools.Add(key, asset);
        return asset;
    }

    public T LoadThumbnail<T>(string key) where T : UnityEngine.Object
    {
        return LoadAsset<T>("Thumbnail/" + key);
    }

    public T LoadUI<T>(string key) where T : UnityEngine.Object
    {
        return LoadAsset<T>("UI/" + key);
    }

    public async Task<TextAsset> LoadData(string key)
    {
        return await LoadAsset<TextAsset>(key, eAddressableType.data);
    }
    #endregion

    #region Addressable Async

    void Start()
    {
        if (isAutoLoading)
            Init();
    }

    private Dictionary<eAddressableType, Dictionary<string, AddressableMap>> addressableMap = new Dictionary<eAddressableType, Dictionary<string, AddressableMap>>();

    private async void InitAddressableMap()
    {
        await Addressables.LoadAssetsAsync<TextAsset>("AddressableMap", (text) =>
        {
            var map = JsonUtility.FromJson<AddressableMapData>(text.text);
            var key = eAddressableType.prefab;
            Dictionary<string, AddressableMap> mapDic = new Dictionary<string, AddressableMap>();
            foreach (var data in map.list)
            {
                key = data.addressableType;
                if (!mapDic.ContainsKey(data.key))
                    mapDic.Add(data.key, data);
            }
            if (!addressableMap.ContainsKey(key)) addressableMap.Add(key, mapDic);

        }).Task;
        isInit = true;
    }

    public async Task LoadAddressable()
    {
        var init = await Addressables.InitializeAsync().Task;
        var handle = Addressables.DownloadDependenciesAsync("InitDownload");
        UILoading.Instance.SetProgress(handle, "리소스 불러오는 중");
        //StartCoroutine(SetProgress(handle));
        await handle.Task;
        switch (handle.Status)
        {
            case AsyncOperationStatus.None:
                break;
            case AsyncOperationStatus.Succeeded:
                //Debug.Log("다운로드 성공!");
                break;
            case AsyncOperationStatus.Failed:
                //Debug.Log("다운로드 실패 : " + handle.OperationException.Message);
                //Debug.LogError(handle.OperationException.ToString());
                break;
            default:
                break;
        }
        Addressables.Release(handle);
        InitAddressableMap();
    }

    public List<string> GetPaths(string key, eAddressableType addressableType, eAssetType assetType)
    {
        var keys = new List<string>(addressableMap[addressableType].Keys);
        keys.RemoveAll(obj => !obj.Contains(key));
        List<string> retList = new List<string>();
        keys.ForEach(obj =>
        {
            if (addressableMap[addressableType][obj].assetType == assetType)
                retList.Add(addressableMap[addressableType][obj].path);
        });
        return retList;
    }
    public string GetPath(string key, eAddressableType addressableType)
    {
        var map = addressableMap[addressableType][key.ToLower()];
        return map.path;
    }

    public async Task<List<T>> LoadAssets<T>(string key, eAddressableType addressableType, eAssetType assetType)
    {
        try
        {
            var paths = GetPaths(key, addressableType, assetType);
            List<T> retList = new List<T>();
            foreach (var path in paths)
            {
                retList.Add(await LoadAssetAsync<T>(path));
            }
            return retList;
        }
        catch (Exception e)
        {
            //Debug.LogError(e.Message);
        }
        return default;
    }

    public async Task<List<T>> LoadDataAssets<T>()
    {
        try
        {
            var ao = Addressables.LoadAssetsAsync<TextAsset>("JsonData", null);
            UILoading.Instance.SetProgress(ao, "데이터 불러오는 중");
            var jsonData = await ao.Task;

            List<T> retList = new List<T>();
            foreach (var data in jsonData)
            {
                var obj = JsonUtility.FromJson<T>(data.text);
                retList.Add(obj);
            }
            return retList;
        }
        catch (Exception e)
        {
            //Debug.LogError(e.Message);
        }
        return default;
    }


    public async Task<T> LoadAsset<T>(string key, eAddressableType addressableType)
    {
        try
        {
            var path = GetPath(key, addressableType);
            return await LoadAssetAsync<T>(path);
        }
        catch (Exception e)
        {
            //Debug.LogWarning(e.Message);
        }
        return default;
    }
    private async Task<T> LoadAssetAsync<T>(string path)
    {
        try
        {
            if (path.Contains(".prefab") && typeof(T) != typeof(GameObject) || path.Contains("UI/"))
            {
                var obj = await Addressables.LoadAssetAsync<GameObject>(path).Task;
                return obj.GetComponent<T>();
            }
            else if (path.Contains(".json"))
            {
                var textAsset = await Addressables.LoadAssetAsync<TextAsset>(path).Task;
                return JsonUtility.FromJson<T>(textAsset.text);
            }
            else
            {
                return await Addressables.LoadAssetAsync<T>(path).Task;
            }
        }
        catch (Exception e)
        {
            //Debug.LogError(e.Message);
        }
        return default;
    }
    #endregion
}
