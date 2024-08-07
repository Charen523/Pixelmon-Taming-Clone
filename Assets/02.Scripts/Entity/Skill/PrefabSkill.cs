using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSkill : MonoBehaviour
{
    RandomSpotSkill randomSpotSkill;
    Enemy enemy;
    public void SetSkill(RandomSpotSkill randSkill)
    {
        randomSpotSkill = randSkill;
    }

    public void RandomAttack()
    {
        transform.position = (Vector2)randomSpotSkill.transform.position + Random.insideUnitCircle * 1.2f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out enemy))
        {
            enemy.healthSystem.TakeDamage(
                randomSpotSkill.owner.status.GetTotalDamage(randomSpotSkill.owner.myData, true, randomSpotSkill.data.maxRate));
        }
    }
}
