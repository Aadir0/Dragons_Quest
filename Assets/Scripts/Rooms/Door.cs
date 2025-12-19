using UnityEngine;

public class Door : MonoBehaviour
{
    //The room on left side
    [SerializeField] private Transform previousRoom;

    //the room on ride
    [SerializeField] private Transform nextRoom;

    //The camera controller script
    [SerializeField] private CameraController cam;

    private void Awake()
    {
        //To get main camera's controller component
        cam = Camera.main.GetComponent<CameraController>();
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     //Only react when player enters the door trigger
    //     if (collision.tag == "Player")
    //     {
    //         //Check if player entered from the left side
    //         if (collision.transform.position.x > transform.position.x)
    //         {
    //             //Move camera to next room 
    //             cam.MoveToNewRoom(nextRoom);
    //             Debug.Log("Enterd next room");
    //             nextRoom.GetComponent<Room>().ActivateRoom(true);      //To activate enemies of next room
    //             previousRoom.GetComponent<Room>().ActivateRoom(false); //To deactivate enemies in previous room
    //         }
    //         else
    //         {
    //             //Move camera to previous room
    //             cam.MoveToNewRoom(previousRoom);
    //             Debug.Log("Entered previous room");
    //             previousRoom.GetComponent<Room>().ActivateRoom(true);
    //             nextRoom.GetComponent<Room>().ActivateRoom(false);
    //         }
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Try to determine movement direction using Rigidbody2D velocity
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            bool movingRight;

            if (rb != null)
            {
                // If velocity is small, fall back to position check
                if (Mathf.Abs(rb.linearVelocity.x) > 0.05f)
                    movingRight = rb.linearVelocity.x > 0f;
                else
                    movingRight = collision.transform.position.x > transform.position.x;
            }
            else
            {
                // No Rigidbody available — fallback to position check
                movingRight = collision.transform.position.x > transform.position.x;
            }

            // // Debug log to see what the system thinks
            // Debug.Log($"Door triggered. movingRight={movingRight} playerX={collision.transform.position.x} doorX={transform.position.x}");

            if (movingRight)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }
}