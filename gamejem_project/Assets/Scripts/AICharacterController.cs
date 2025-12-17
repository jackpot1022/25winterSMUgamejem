// 2025-12-17 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;

public class AICharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // 기본 이동 속도
    public float jumpForce = 10f; // 점프 힘
    public float obstacleDetectionDistance = 2f; // 장애물 감지 거리
    public float sensorRange = 5f; // 추가 감지 센서 범위
    public Vector2 sensorSize = new Vector2(5f, 5f); // 정사각형 센서 크기
    public LayerMask obstacleLayer; // 장애물 레이어 설정

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 장애물 감지 및 이동 방향 결정
        Vector2 leftSensorPosition = transform.position + Vector3.left * sensorRange;
        Vector2 rightSensorPosition = transform.position + Vector3.right * sensorRange;

        int leftObstacleCount = CountObstacles(leftSensorPosition);
        int rightObstacleCount = CountObstacles(rightSensorPosition);

        // 장애물이 더 많은 방향으로 이동
        float moveDirection = rightObstacleCount > leftObstacleCount ? 1 : -1;
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        // 장애물 감지 및 점프
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * moveDirection, obstacleDetectionDistance, obstacleLayer);
        if (hit.collider != null && isGrounded)
        {
            // 장애물이 감지되었고, 바닥에 있을 때 점프
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private int CountObstacles(Vector2 sensorPosition)
    {
        // 정사각형 센서를 사용하여 장애물 개수 계산
        Collider2D[] obstacles = Physics2D.OverlapBoxAll(sensorPosition, sensorSize, 0, obstacleLayer);
        return obstacles.Length;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았는지 확인
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private void OnDrawGizmos()
    {
        // 장애물 감지 레이캐스트를 시각적으로 확인하기 위한 디버그용
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * obstacleDetectionDistance);

        // 추가 감지 센서 시각화 (정사각형)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.left * sensorRange, sensorSize);
        Gizmos.DrawWireCube(transform.position + Vector3.right * sensorRange, sensorSize);
    }

    //wow
}
