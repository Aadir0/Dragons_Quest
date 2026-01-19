using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float windForce = 10f;
    private Rigidbody2D playerRigidbody;
    private bool playerInWind = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerInWind = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerInWind && playerRigidbody != null)
        {
            //Apply upward force continuously
            playerRigidbody.AddForce(Vector2.up * windForce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerRigidbody != null)
        {
            playerInWind = false;
            playerRigidbody = null;
        }
    }
}