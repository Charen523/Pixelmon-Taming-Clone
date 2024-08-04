using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public Pixelmon owner;
    public ActiveData data;
    public MyAtvData myData;
    public GameObject target;
    public Enemy enemy;
    public virtual void InitInfo(Pixelmon pxm, GameObject target, ActiveData atvData, MyAtvData myAtvData)
    {
        owner = pxm;
        data = atvData;
        myData = myAtvData;
        gameObject.transform.position = target.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out enemy))
        {
            enemy.healthSystem.TakeDamage(owner.status.GetTotalDamage(owner.myData, true, data.maxRate));
            Debug.Log(target);
        }
    }

    private void CloseSkill()
    {
        gameObject.SetActive(false);
    }
}
