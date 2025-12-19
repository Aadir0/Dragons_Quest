using System.Collections;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    // ---------------- ROOM CAMERA ----------------
    [Header("Room Camera")]
    [SerializeField] private float roomMoveSmoothTime = 0.25f; // SmoothDamp time
    private Vector3 velocity = Vector3.zero;
    private float targetRoomPosX;
    private bool inRoomTransition = false; // true while SmoothDamp is running

    // ---------------- FOLLOW CAMERA ----------------
    [Header("Follow Camera")]
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance = 1f;
    [SerializeField] private float lookSmoothSpeed = 3f;
    private float lookAhead = 0f;
    private bool followEnabled = true; // toggle follow on/off

    // small threshold to decide when camera reached the target
    private const float roomSnapThreshold = 0.01f;

    private void Update()
    {
        // If we are in a room transition, the SmoothDamp coroutine handles movement.
        // If follow is enabled and we're NOT in a room transition, follow the player.
        if (followEnabled && !inRoomTransition)
        {
            // Update look ahead smoothly (based on player facing via localScale.x)
            lookAhead = Mathf.Lerp(lookAhead, aheadDistance * Mathf.Sign(player.localScale.x), Time.deltaTime * lookSmoothSpeed);

            // Move camera to player's x + lookAhead (keeps y and z unchanged)
            transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        }
    }

    // ---------------- PUBLIC API ----------------

    // Called by Door (or other systems) to request a move to a new room center.
    // This will cancel current follow temporarily, move smoothly to the room, then re-enable follow.
    public void MoveToNewRoom(Transform _newRoom, float holdTimeAfterArrive = 0.2f)
    {
        // Set the target X and start the transition coroutine
        targetRoomPosX = _newRoom.position.x;
        StopAllCoroutines(); // stop previous transitions if any
        StartCoroutine(SmoothMoveToRoom(targetRoomPosX, holdTimeAfterArrive));
    }

    // Optional: call to immediately enable/disable player-follow camera behavior.
    public void SetFollowEnabled(bool enabled)
    {
        followEnabled = enabled;
    }

    // ---------------- COROUTINES ----------------

    private IEnumerator SmoothMoveToRoom(float targetX, float holdTimeAfterArrive)
    {
        // Disable following while we move the camera to room center
        inRoomTransition = true;
        // Optionally disable input-based lookahead while in room mode
        float previousLookAhead = lookAhead;
        lookAhead = 0f;
        followEnabled = false;

        // SmoothDamp loop until camera x is close enough to targetX
        while (Mathf.Abs(transform.position.x - targetX) > roomSnapThreshold)
        {
            Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, roomMoveSmoothTime);
            yield return null;
        }

        // Snap exactly to avoid tiny differences
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        // Optional hold so the camera stays for a bit in the room before following again
        if (holdTimeAfterArrive > 0f)
            yield return new WaitForSeconds(holdTimeAfterArrive);

        // Re-enable follow and restore lookAhead smoothing
        lookAhead = previousLookAhead;
        followEnabled = true;
        inRoomTransition = false;
    }
}
