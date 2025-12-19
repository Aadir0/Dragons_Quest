using UnityEngine;

public class CameraController : MonoBehaviour
{
    //-----------------Room camera-------------
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //----------------Follow player--------------
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        //------------Room camera--------------

        /* Smothly moves camera to next room and find center position for that room camera placment */

        // transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        //---------------Follow player--------------

        //Follow players X positiona and look ahead 
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);

        //Lerp just smoothly adjust lookahead to face the direction player is moving
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    //Called by the Door script to move it to new room
    public void MoveToNewRoom(Transform _newRoom)
    {
        //Store the X position of the room the camera should move to
        currentPosX = _newRoom.position.x;
    }
}