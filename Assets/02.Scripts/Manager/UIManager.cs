using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public enum eUIPosition
{
    UI,
    Popup,
    Navigator
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<Transform> parents;
    private List<UIBase> uiList = new List<UIBase>();

    public static void SetParents(List<Transform> parents)
    {
        Instance.parents = parents;
    }

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
        ui.gameObject.SetActive(true);
        return (T)ui;
    }

    public static void Hide<T>(params object[] param) where T : UIBase
    {
        var ui = Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
        if (ui != null)
        {
            Instance.uiList.Remove(ui);
            if (ui.uiPosition == eUIPosition.UI)
            {
                var prevUI = Instance.uiList.FindLast(obj => obj.uiPosition == eUIPosition.UI);
                prevUI.SetActive(true);
            }
            ui.closed.Invoke(param);
            Destroy(ui.gameObject);
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
}