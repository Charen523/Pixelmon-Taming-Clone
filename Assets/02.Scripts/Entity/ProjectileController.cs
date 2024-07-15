using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    float projectileDamage;

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    //Invoke쪽에서 전달해야 할 변수들.
    public void OnAttackSign(Vector2 direction, float damage)
    {
        projectileDamage = damage;
    }
}
