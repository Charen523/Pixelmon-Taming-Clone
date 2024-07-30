using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static bool isInit;

    public event Action OnPlayerDie;
    public event Action OnStageTimeOut;
    public event Action OnStageStart;
    public event Action OnStageClear;


    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 30;
    }

    public void OnInit()
    {
        StartCoroutine(OnManagerInit());
    }

    public IEnumerator OnManagerInit()
    {
        UILoading.Show();
        DataManager.Instance.Init();
        yield return new WaitUntil(() => DataManager.Instance.isInit);
        ResourceManager.Instance.Init();
        yield return new WaitUntil(() => ResourceManager.Instance.isInit);
        InitData();
        yield return new WaitUntil(() => DataManager.Instance.isPxmInit);
        isInit = true;
    }
    public async void InitData()
    {
        await DataManager.Instance.SetPixelmonData();
    }

    //public async void InitWorld()
    //{
    //    player = Instantiate(await ResourceManager.instance.LoadAsset<Player>("CHA00001", eAddressableType.prefab));
    //    player.Init(null);
    //    player.equipment.equipParent = CameraContainer.instance.EquipSlot;

    //    var datas = DataManager.instance.mapData.data;
    //    datas.ForEach(async data =>
    //    {
    //        for (int i = 0; i < data.count; i++)
    //        {
    //            var width = Environments.instance.terrain.localBounds.size.x / 2;
    //            var height = Environments.instance.terrain.localBounds.size.z / 2;
    //            var x = UnityEngine.Random.Range(-width, width);
    //            var z = UnityEngine.Random.Range(-height, height);
    //            if (!await SpawnManager.instance.SpawnObject(DataManager.instance.GetCloneData<WorldObjectData>(data.target), data.isRandom ? new Vector3(x, 0, z) : data.pos, data.isRandom))
    //            {
    //                i--;
    //            }
    //        }
    //    });
    //}


    public void NotifyPlayerDie()
    {
        OnPlayerDie?.Invoke();
    }

    public void NotifyStageTimeOut()
    {
        OnStageTimeOut?.Invoke();
    }

    public void NotifyStageStart()
    {
        OnStageStart?.Invoke();
    }

    public void NotifyStageClear()
    {
        OnStageClear?.Invoke();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        OnPlayerDie = null;
        OnStageTimeOut = null;
        OnStageStart = null;
        OnStageClear = null;
    }
}
