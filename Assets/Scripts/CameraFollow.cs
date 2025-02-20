using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라다닐 대상 (로컬 플레이어)
    public Vector3 offset = new Vector3(0f, 0f, -10f); // 기본 카메라 오프셋
    public float lerpTime = 10f; 
    
    public Vector2 minPosition; // 최소 X, Y 좌표 (왼쪽 아래)
    public Vector2 maxPosition; // 최대 X, Y 좌표 (오른쪽 위)


    //캐릭터라 rigidbody2d로 움직여서 fixedUpdate에서 팔로우
    private void FixedUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산
        Vector3 targetPosition = target.position + offset;

        // 카메라 위치 계산
        float clampedX = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
        
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, offset.z);
        
        transform.position = Vector3.Lerp(transform.position, clampedPosition, Time.deltaTime * lerpTime);

    }
    public void SetTarget(Transform newTarget)
    {
        // 카메라가 따라다닐 대상을 설정
        target = newTarget;
    }
}