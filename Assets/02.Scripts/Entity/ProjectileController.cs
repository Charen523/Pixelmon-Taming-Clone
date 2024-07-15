using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
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

    private void OnEnable()
    {
        rb.position = startPosition;
        rb.velocity = shootDirection.normalized * flySpeed;
    }

    private void Update()
    {
        // 발사체가 일정 거리 이상 날아가면 비활성화
        if (Vector2.Distance(startPosition, rb.position) >= flyDistance)
        {
            gameObject.SetActive(false);
        }
    }

    //Invoke쪽에서 전달해야 할 변수들.
    public void GetAttackSign(Vector3 startPos, Vector2 direction, float damage, float bulletRange, float speed)
    {
        startPosition = startPos; //날아가기 시작하는 지점.
        shootDirection = direction; //날아갈 방향.
        projectileDamage = damage; //전달할 데이터.
        flyDistance = bulletRange; //날아갈 거리.
        flySpeed = speed; //날아갈 속도.


    }
}
