using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class SpriteAtlasChecker : EditorWindow
{
    [MenuItem("Tools/Sprite Atlas Checker")]
    public static void ShowWindow()
    {
        GetWindow<SpriteAtlasChecker>("Sprite Atlas Checker");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Check Scene for Non-Atlas Sprites"))
        {
            CheckSceneForNonAtlasSprites();
        }

        if (GUILayout.Button("Check All Prefabs for Non-Atlas Sprites"))
        {
            CheckPrefabsForNonAtlasSprites();
        }
    }

    private void CheckSceneForNonAtlasSprites()
    {
        // 현재 씬에서 모든 Image와 SpriteRenderer 컴포넌트를 가져옴
        Image[] images = FindObjectsOfType<Image>();
        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        // 모든 SpriteAtlas를 로드
        SpriteAtlas[] atlases = Resources.FindObjectsOfTypeAll<SpriteAtlas>();

        // 비아틀라스 스프라이트를 출력
        Debug.Log("Checking for non-atlas sprites in the scene...");

        foreach (var image in images)
        {
            if (image.sprite != null && !IsSpriteInAnyAtlas(image.sprite, atlases))
            {
                Debug.Log("Image not in any atlas: " + image.gameObject.name + " - " + AssetDatabase.GetAssetPath(image.sprite));
            }
        }

        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.sprite != null && !IsSpriteInAnyAtlas(spriteRenderer.sprite, atlases))
            {
                Debug.Log("SpriteRenderer not in any atlas: " + spriteRenderer.gameObject.name + " - " + AssetDatabase.GetAssetPath(spriteRenderer.sprite));
            }
        }

        Debug.Log("Scene check complete.");
    }

    private void CheckPrefabsForNonAtlasSprites()
    {
        // 프로젝트의 모든 프리팹 경로 가져오기
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        SpriteAtlas[] atlases = Resources.FindObjectsOfTypeAll<SpriteAtlas>();

        Debug.Log("Checking all prefabs for non-atlas sprites...");

        foreach (var guid in prefabGUIDs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null)
            {
                // 프리팹의 모든 Image와 SpriteRenderer 컴포넌트를 가져옴
                Image[] images = prefab.GetComponentsInChildren<Image>(true);
                SpriteRenderer[] spriteRenderers = prefab.GetComponentsInChildren<SpriteRenderer>(true);

                foreach (var image in images)
                {
                    if (image.sprite != null && !IsSpriteInAnyAtlas(image.sprite, atlases))
                    {
                        Debug.Log("Prefab Image not in any atlas: " + prefab.name + " - " + prefabPath + " - " + AssetDatabase.GetAssetPath(image.sprite));
                    }
                }

                foreach (var spriteRenderer in spriteRenderers)
                {
                    if (spriteRenderer.sprite != null && !IsSpriteInAnyAtlas(spriteRenderer.sprite, atlases))
                    {
                        Debug.Log("Prefab SpriteRenderer not in any atlas: " + prefab.name + " - " + prefabPath + " - " + AssetDatabase.GetAssetPath(spriteRenderer.sprite));
                    }
                }
            }
        }

        Debug.Log("Prefab check complete.");
    }

    private bool IsSpriteInAnyAtlas(Sprite sprite, SpriteAtlas[] atlases)
    {
        foreach (var atlas in atlases)
        {
            if (atlas.GetSprite(sprite.name) != null)
            {
                return true;
            }
        }
        return false;
    }
}
