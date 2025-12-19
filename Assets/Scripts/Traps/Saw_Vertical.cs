using UnityEngine;

public class Saw_Vertical : MonoBehaviour
{
    //To set trap movement distance left to right from starting position
    [SerializeField] private float movementDistance;

    //To set trap speed
    [SerializeField] private float speed;

    //To set trap damage
    [SerializeField] private float damage;

    //Current movement direction
    private bool movingup;

    //position limits where the trap will turn back
    private float upEdge;
    private float downEdge;

    private void Awake()
    {
        //Calculate movement baundries based on starting point
        upEdge = transform.position.y - movementDistance;
        downEdge = transform.position.y + movementDistance;
    }

    private void Update()
    {
        //----------For moving left-----------
        if (movingup)
        {
            //If did't reached left limit
            if (transform.position.y > upEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                //Reached left edge - switching direction
                movingup = false;
            }
        }

        //----------For moving right----------
        else
        {
            //If did't reached right limit
            if (transform.position.y < downEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                //reached right edge - switching direction
                movingup = true;
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