using UnityEngine;

public class Saw : MonoBehaviour
{
    //To set trap movement distance left to right from starting position
    [SerializeField] private float movementDistance;

    //To set trap speed
    [SerializeField] private float speed;

    //To set trap damage
    [SerializeField] private float damage;

    //Current movement direction
    private bool movingLeft;

    //position limits where the trap will turn back
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        //Calculate movement baundries based on starting point
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        //----------For moving left-----------
        if (movingLeft)
        {
            //If did't reached left limit
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                //Reached left edge - switching direction
                movingLeft = false;
            }
        }

        //----------For moving right----------
        else
        {
            //If did't reached right limit
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                //reached right edge - switching direction
                movingLeft = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if we collided with player or not
        if (collision.CompareTag("Player"))
        {
            //Deal damage to player health script
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}