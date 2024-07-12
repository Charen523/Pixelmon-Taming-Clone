using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class CanvasIntro : MonoBehaviour
{
    [SerializeField] private List<Transform> parents;
    [SerializeField] private Sprite TitleBg;
    public Image logo;

    IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync("DontDestroy", LoadSceneMode.Additive);
        UILoading.Instance.SetBG(TitleBg);
        UIManager.SetParents(parents);
        yield return new WaitForSeconds(2);
        GameManager.Instance.OnInit();
        yield return new WaitUntil(() => GameManager.isInit);
        SceneManager.LoadSceneAsync("MainScene");
    }
}