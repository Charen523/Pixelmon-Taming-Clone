using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasGame : MonoBehaviour
{
    [SerializeField] private List<Transform> parents;

    IEnumerator Start()
    {
        UILoading.Hide();
        //yield return SceneManager.LoadSceneAsync("WorldScene", LoadSceneMode.Additive);
        UIManager.SetParents(parents);
        UIManager.SetCanvas(transform);
        yield return null;
        yield return new WaitUntil(() => DataManager.Instance.isInit);
        //GameManager.Instance.InitWorld();
        //yield return null;
        //yield return UIManager.Show<UIGame>();
        yield return UIManager.Show<UIMiddleBar>();
    }
}
