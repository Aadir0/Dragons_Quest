using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float distance;   // how far it moves left & right
    [SerializeField] private float speed;      // movement speed

    private Vector3 startPos;
    private bool movingRight = true;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (movingRight)
            transform.position += Vector3.right * speed * Time.deltaTime;
        else
            transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x >= startPos.x + distance)
            movingRight = false;

        if (transform.position.x <= startPos.x - distance)
            movingRight = true;
    }
}