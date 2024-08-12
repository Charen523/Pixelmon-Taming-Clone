using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasIntro : MonoBehaviour
{
    [SerializeField] private List<Transform> parents;

    [SerializeField] private GameObject pxmBg;
    [SerializeField] private GameObject logo;

    private void Start()
    {
        StartCoroutine(AppVersionCheck(OnVersionCheckCompleted));
    }
    private IEnumerator StartGame()
    {
        yield return SceneManager.LoadSceneAsync("DontDestroy", LoadSceneMode.Additive);
        UIManager.SetParents(parents);
        yield return new WaitForSeconds(2);
        GameManager.Instance.OnInit();
        yield return new WaitUntil(() => GameManager.isInit);
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void OnClickExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnVersionCheckCompleted(bool isUpdated)
    {
        if (!isUpdated)
        {
            // 업데이트가 필요할 경우
            Debug.Log("업데이트가 필요합니다.");
            logo.SetActive(false);
            pxmBg.SetActive(true);
        }
        else
        {
            // 업데이트가 필요하지 않을 경우
            Debug.Log("최신 버전을 사용 중입니다. 메인 화면으로 이동합니다.");
            // 예: 앱의 메인 화면으로 이동
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator AppVersionCheck(Action<bool> _State)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
    string _AppID = "https://play.google.com/store/apps/details?id=앱id";

            UnityWebRequest _WebRequest = UnityWebRequest.Get(_AppID);
        yield return _WebRequest.SendWebRequest();

        // 네트워크 요청에 실패했을 경우 처리
        if (_WebRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get version info from Google Play Store: " + _WebRequest.error);

            if (_State != null)
                _State(false);
            yield break;
        }


        // 정규식으로 전채 문자열중 버전 정보가 담겨진 태그를 검색한다.
        string _Pattern = @"<span class=""htlgb"">[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}<";
        Regex _Regex = new Regex(_Pattern, RegexOptions.IgnoreCase);
        Match _Match = _Regex.Match(_WebRequest.downloadHandler.text);

        if (_Match.Success)
        {
            // 버전 정보가 담겨진 태그를 찾음
            // 해당 태그에서 버전 넘버만 가져온다
            _Match = Regex.Match(_Match.Value, "[0-9]+\\.[0-9]+\\.[0-9]+");

            try
            {
                int[] _ClientVersion = VersionPaser(Application.version);
                int[] _AppStoreVersion = VersionPaser(_Match.Value);

                Debug.Log("  Application.version : " + Application.version + ", AppStore version :" + _Match.Value);

                // 버전 비교
                if (_AppStoreVersion[0] > _ClientVersion[0] ||
                    (_AppStoreVersion[0] == _ClientVersion[0] && _AppStoreVersion[1] > _ClientVersion[1]) ||
                    (_AppStoreVersion[0] == _ClientVersion[0] && _AppStoreVersion[1] 
                    == _ClientVersion[1] && _AppStoreVersion[2] > _ClientVersion[2]))
                {
                    _State?.Invoke(true);
                }
                else
                {
                    _State?.Invoke(false);
                }
            }
            catch (Exception Ex)
            {
                // 비정상 버전정보 파싱중 Exception처리
                Debug.LogError("비정상 버전 정보 Exception : " + Ex);
                Debug.LogError("  Application.version : " + Application.version + ", AppStore version :" + _Match.Value);
                _State?.Invoke(false);
            }
        }
        else
        {
            Debug.LogError("Not Found AppStoreVersion Info");
            _State?.Invoke(false);
        }
#elif UNITY_EDITOR
        _State?.Invoke(true);
        yield break;
#endif

    }

    private int[] VersionPaser(string version)
    {
        string[] parts = version.Split('.');
        return new int[] { int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]) };
    }
}