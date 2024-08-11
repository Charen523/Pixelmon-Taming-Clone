using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class DropItem : SerializedMonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    Sequence mySequence;
    Vector3 itemScale;
    [SerializeField] string itemName;
    [SerializeField] int amount;
    [SerializeField] float waitTime = 1.25f;
    [SerializeField] WaitForSeconds delayTime;
    private void Awake()
    {
        itemScale = transform.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        delayTime = new WaitForSeconds(waitTime);
        mySequence = DOTween.Sequence()
        .SetAutoKill(false) //추가
        .Prepend(sr.DOFade(0, 0))
        .Append(sr.DOFade(1, 1f))
        .Join(transform.DOShakeScale(0.5f, itemScale.x, 5, 1f, false))
        .Append(sr.DOFade(0, 1.5f))
        .OnComplete(() => gameObject.SetActive(false));

    }

    [ContextMenu("Test")]
    public void ExeCuteSequence(GameObject _enemy, int _amount)
    {
        amount = _amount;
        sr.color = Color.white;
        transform.position = _enemy.transform.position;    
        StartCoroutine(MoveToPlayer());
    }
    IEnumerator MoveToPlayer()
    {
        mySequence.Restart();
        yield return delayTime;
        float time = 0;
        while (time <= waitTime)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, Player.Instance.gameObject.transform.position, time / 2);
            yield return null;
            GetReward();
        }
    }

    public void GetReward()
    {
        RewardManager.Instance.GetReward(itemName, amount);
        amount = 0;
    }
}
