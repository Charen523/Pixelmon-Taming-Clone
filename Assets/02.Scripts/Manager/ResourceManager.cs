using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SocialPlatforms;

public class ResourceManager : Singleton<ResourceManager>
{
    public bool isAutoLoading = false;
    [NonSerialized] public bool isInit = false;

    public async void Init()
    {
        //await LoadAddressable();
    }

    #region Use Resources
#if !USE_COROUTINE && !USE_ASYNC
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
#endif
    #endregion

    #region Addressable Callback
#if USE_COROUTINE
    private Dictionary<eAddressableType, Dictionary<string, AddressableMap>> addressableMap = new Dictionary<eAddressableType, Dictionary<string, AddressableMap>>();

    void Start()
    {
        LoadAddressable();
    }

    private void InitAddressableMap()
    {
        Addressables.LoadAssetsAsync<TextAsset>("AddressableMap", (text) =>
        {
            var map = JsonUtility.FromJson<AddressableMapData>(text.text);
            var key = eAddressableType.prefab;
            Dictionary<string, AddressableMap> mapDic = new Dictionary<string, AddressableMap>();
            foreach (var data in map.list)
            {
                key = data.addressableType;
                if(!mapDic.ContainsKey(data.key))
                    mapDic.Add(data.key, data);
            }
            if (!addressableMap.ContainsKey(key)) addressableMap.Add(key, mapDic);

        });
    }

    public void LoadAddressable()
    {
        StartCoroutine(CLoadAddressable());
    }

    public IEnumerator CLoadAddressable()
    {
        yield return Addressables.InitializeAsync();
        var handle = Addressables.DownloadDependenciesAsync("InitDownload");
        yield return SetProgress(handle);
        switch (handle.Status)
        {
            case AsyncOperationStatus.None:
                break;
            case AsyncOperationStatus.Succeeded:
                Ironcow.Logger.Log("다운로드 성공!");
                break;
            case AsyncOperationStatus.Failed:
                Ironcow.Logger.Log("다운로드 실패 : " + handle.OperationException.Message);
                Ironcow.Logger.LogError(handle.OperationException.ToString());
                break;
            default:
                break;
        }
        Addressables.Release(handle);
        InitAddressableMap();
    }

    public IEnumerator SetProgress(AsyncOperationHandle handle)
    {
        while (!handle.IsDone)
        {
            //UILoading.instance.SetProgress(handle.GetDownloadStatus().Percent, "Resource Download...");
            yield return new WaitForEndOfFrame();
        }
        //UILoading.instance.SetProgress(1);

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

    public void LoadAssets<T>(string key, eAddressableType addressableType, eAssetType assetType, Action<List<T>> callback)
    {
        StartCoroutine(CLoadAssets(key, addressableType, assetType, callback));
    }

    IEnumerator CLoadAssets<T>(string key, eAddressableType addressableType, eAssetType assetType, Action<List<T>> callback)
    {
        var paths = GetPaths(key, addressableType, assetType);
        List<T> retList = new List<T>();
        foreach (var path in paths)
        {
            yield return CLoadAsset<T>(path, obj =>
            {
                retList.Add(obj);
            });
        }
        yield return new WaitUntil(() => paths.Count == retList.Count);
        callback.Invoke(retList);
    }

    public void LoadAsset<T>(string key, eAddressableType addressableType, Action<T> callback)
    {
        var path = GetPath(key, addressableType);
        LoadAsset<T>(path, callback);
    }

    public void LoadAsset<T>(string path, Action<T> callback)
    {
        StartCoroutine(CLoadAsset(path, callback));
    }

    public IEnumerator CLoadAsset<T>(string path, Action<T> callback)
    {
        if (path.Contains(".prefab") && typeof(T) != typeof(GameObject) || path.Contains("UI/"))
        {
            var handler = Addressables.LoadAssetAsync<GameObject>(path);
            handler.Completed += (op) =>
            {
                callback.Invoke(op.Result.GetComponent<T>());
            };
            yield return handler;
        }
        else
        {
            var handler = Addressables.LoadAssetAsync<T>(path);
            handler.Completed += (op) =>
            {
                callback.Invoke(op.Result);
            };
            yield return handler;
        }
    }
#endif
    #endregion

    #region Addressable Async
#if USE_ASYNC

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
                if(!mapDic.ContainsKey(data.key))
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
        UILoading.instance.SetProgress(handle, "리소스 로딩 중...");
        //StartCoroutine(SetProgress(handle));
        await handle.Task;
        switch (handle.Status)
        {
            case AsyncOperationStatus.None:
                break;
            case AsyncOperationStatus.Succeeded:
                Ironcow.Logger.Log("다운로드 성공!");
                break;
            case AsyncOperationStatus.Failed:
                Ironcow.Logger.Log("다운로드 실패 : " + handle.OperationException.Message);
                Ironcow.Logger.LogError(handle.OperationException.ToString());
                break;
            default:
                break;
        }
        Addressables.Release(handle);
        InitAddressableMap();
    }

    public IEnumerator SetProgress(AsyncOperationHandle handle)
    {
        while (!handle.IsDone)
        {
            UILoading.instance.SetProgress(handle.GetDownloadStatus().Percent, "Resource Download...");
            yield return new WaitForEndOfFrame();
        }
        UILoading.instance.SetProgress(1);

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
            Debug.LogError(e.Message);
        }
        return default;
    }

    public async Task<List<T>> LoadDataAssets<T>()
    {
        try
        {
            int idx = 0;
            var ao = Addressables.LoadAssetsAsync<T>("JsonData", (datas) => // ��Ʈ���� Addressable Label�� ������ ���̺�
            {

            });
            UILoading.instance.SetProgress(ao, "������ �ε���...");
            var retList = new List<T>(await ao.Task);
            return retList;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
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
            Debug.LogWarning(e.Message);
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
            else
                return await Addressables.LoadAssetAsync<T>(path).Task;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return default;
    }
#endif
    #endregion
}
