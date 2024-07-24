using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SerializedMonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    public float projectileDamage;

    private Vector2 startPosition;
    private Vector2 shootDirection;
    private float flyDistance;
    private float flySpeed;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        rb.velocity = shootDirection.normalized * flySpeed;

        // 발사체가 일정 거리 이상 날아가면 비활성화
        if (Vector2.Distance(startPosition, rb.position) >= flyDistance)
        {
            ResetProjectile();
        }
    }

    //Invoke쪽에서 전달해야 할 변수들.
    public void GetAttackSign(Vector3 startPos, Vector2 direction, float damage, float bulletRange, float speed)
    {
        startPosition = startPos; //날아가기 시작하는 지점.
        shootDirection = direction; //날아갈 방향.
        projectileDamage = damage; //전달할 데이터.
        projectileDamage = 100f; //테스트용. 지워야 함.
        flyDistance = bulletRange; //날아갈 거리.
        flySpeed = speed; //날아갈 속도.

        rb.position = startPosition;
    }

    // 발사체의 위치와 속도를 초기화하는 메서드
    public void ResetProjectile()
    {
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(-1000, 0, 0);
        gameObject.SetActive(false);
    }
}
