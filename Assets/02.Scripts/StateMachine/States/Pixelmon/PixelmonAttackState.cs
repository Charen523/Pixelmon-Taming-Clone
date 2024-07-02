using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PixelmonAttackState
{

    //public override void Enter()
    //{
    //    base.Enter();
    //    //StartAnimation();
    //}

    //public override void Exit()
    //{
    //    base.Exit();
    //    //StopAnimation();
    //}

    //public override void Execute()
    //{
    //    base.Execute();
    //}

    public GameObject Search(GameObject pixelmon, List<GameObject> enemies)
    {
        GameObject target = new GameObject();
        // 5f => 탐색범위 길이로 변경
        float minDistance = 5f;
        enemies.ForEach(enemy =>
        {
            Vector3 pos = pixelmon.transform.position - enemy.transform.position;
            float distance = pos.sqrMagnitude;

            if(distance < minDistance * minDistance)
                target = enemy;
        });
        return target;
    }
}
