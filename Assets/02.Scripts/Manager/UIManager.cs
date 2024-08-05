using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum eUIPosition
{
    Navigator,
    Tab,
    UI,
    Popup
}

public class UIManager : Singleton<UIManager>
{
    public event Action<DirtyUI> UpdateUI;

    [SerializeField] private List<Transform> parents;
    public Transform canvas;
    public Button tabOverlay;
    
    private List<UIBase> uiList = new List<UIBase>();

    public static void SetParents(List<Transform> parents)
    {
        Instance.parents = parents;
    }

    public static void SetCanvas(Transform canvas)
    {
        Instance.canvas = canvas;
    }

    public static void SetTabOverlayBtn(Button btn)
    {
        Instance.tabOverlay = btn;
    }

    /// <summary>
    /// async 함수에서만 사용가능한 메서드.
    /// </summary>
    /// <typeparam name="T">UIBase를 상속받은 클래스 이름</typeparam>
    /// <param name="param">()안에 뭐 없어도 됨</param>
    /// <returns></returns>
    public async static Task<T> Show<T>(params object[] param) where T : UIBase
    {
        var ui = Instance.uiList.Find(obj => obj.name == typeof(T).ToString());

        if (ui == null)
        {
            var prefab = await ResourceManager.Instance.LoadAsset<T>(typeof(T).ToString(), eAddressableType.ui);
            ui = Instantiate(prefab, Instance.parents[(int)prefab.uiPosition]);
            ui.name = ui.name.Replace("(Clone)", "");
            if (ui.uiPosition == eUIPosition.UI)
            {
                Instance.uiList.ForEach(obj =>
                {
                    if (obj.uiPosition == eUIPosition.UI) obj.gameObject.SetActive(false);
                });
            }
            Instance.uiList.Add(ui);
        }
        ui.opened.Invoke(param);
        ui.gameObject.SetActive(ui.isActiveInCreated);
        ui.isActiveInCreated = true;
        return (T)ui;
    }

    public async static Task<UIBase> Show(string name, params object[] param)
    {
        var ui = Instance.uiList.Find(obj => obj.name == name);

        if (ui == null)
        {
            var prefab = await ResourceManager.Instance.LoadAsset<UIBase>(name, eAddressableType.ui);
            ui = Instantiate(prefab, Instance.parents[(int)prefab.uiPosition]);
            ui.name = ui.name.Replace("(Clone)", "");
            if (ui.uiPosition == eUIPosition.UI)
            {
                Instance.uiList.ForEach(obj =>
                {
                    if (obj.uiPosition == eUIPosition.UI) obj.gameObject.SetActive(false);
                });
            }
            Instance.uiList.Add(ui);
        }
        ui.opened?.Invoke(param);
        ui.gameObject.SetActive(ui.isActiveInCreated);
        ui.isActiveInCreated = true;
        return ui;
    }

    public static void Hide<T>(params object[] param) where T : UIBase
    {
        var ui = Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
        if (ui != null)
        {
            ui.closed.Invoke(param);
            if (ui.isDestroyAtClosed)
            {
                Instance.uiList.Remove(ui);
                Destroy(ui.gameObject);
            }
            else
            {
                ui.SetActive(false);
            }
        }
    }

    public static void Hide(string name)
    {
        var ui = Instance.uiList.Find(obj => obj.name == name);
        if (ui != null)
        {
            if (ui.isDestroyAtClosed)
            {
                Instance.uiList.Remove(ui);
                Destroy(ui.gameObject);
            }
            else
            {
                ui.SetActive(false);
            }
        }
    }

    public static T Get<T>() where T : UIBase
    {
        return (T)Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
    }

    public static bool IsOpened<T>() where T : UIBase
    {
        return Instance.uiList.Exists(obj => obj.name == typeof(T).ToString());
    }

    public void InvokeUIChange(DirtyUI dirtyUI)
    {
        UpdateUI?.Invoke(dirtyUI);
    }
}