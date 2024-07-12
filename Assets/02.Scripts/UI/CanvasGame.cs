using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasGame : MonoBehaviour
{
    [SerializeField] private List<Transform> parents;

    public void Awake()
    {
        if (!GameManager.isInit) SceneManager.LoadScene("StartScene");
    }

    IEnumerator Start()
    {
        UILoading.Hide();
        //yield return SceneManager.LoadSceneAsync("WorldScene", LoadSceneMode.Additive);
        UIManager.SetParents(parents);
        yield return UIManager.Show<UIGame>();
    }
}
