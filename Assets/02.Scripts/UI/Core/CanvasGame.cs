using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGame : MonoBehaviour
{
    [SerializeField] private List<Transform> parents;

    IEnumerator Start()
    {
        UILoading.Hide();
        UIManager.SetParents(parents);
        UIManager.SetCanvas(transform);
        yield return new WaitUntil(() => DataManager.Instance.isInit);
        yield return UIManager.Show<UIMiddleBar>();
    }
}
