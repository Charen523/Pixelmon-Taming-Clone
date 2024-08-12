using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public ActiveData data;
    public MyAtvData myData;
    public Pixelmon owner;
    public Enemy enemy;
    public GameObject target;
    public virtual void InitInfo(Pixelmon pxm, GameObject _target, ActiveData atvData, MyAtvData myAtvData)
    {
        owner = pxm;
        data = atvData;
        myData = myAtvData;
        target = _target;
        Setprojectile();
        ExecuteSkill();
    }

    protected virtual void ExecuteSkill() { }
    protected virtual void Setprojectile() { }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out enemy))
        {
            //Debug.Log(target);
            var result = owner.status.GetTotalDamage(owner.myData, true, data.maxRate + myData.lv);
            enemy.healthSystem.TakeDamage(result.Item1, result.Item2);
        }
    }

    protected virtual void CloseSkill()
    {
        gameObject.SetActive(false);
    }
}
