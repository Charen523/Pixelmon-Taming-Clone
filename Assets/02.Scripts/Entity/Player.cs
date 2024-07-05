using UnityEngine;

public class Player : Singleton<Player>
{
    public float DetectionRadius = 6.0f; // 탐지 반경 설정

    // Gizmos를 사용하여 탐지 반경을 시각적으로 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }

}