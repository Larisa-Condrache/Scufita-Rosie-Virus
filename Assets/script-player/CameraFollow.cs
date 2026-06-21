using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public Transform player;

    public float smoothTime = 0.18f;
    public Vector3 cameraOffset = new Vector3(0f, 2f, -10f);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + cameraOffset;
        targetPosition.z = -10f;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}