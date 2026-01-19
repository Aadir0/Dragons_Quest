using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    private void LateUpdate()
    {
        Vector3 desiredPosition = transform.position;
        desiredPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = desiredPosition;
    }
}