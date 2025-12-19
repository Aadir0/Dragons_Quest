using UnityEngine;

public class PlayerRespawn1 : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointAudio;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    public void checkRespawn()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
        }
        else
        {
            transform.position = currentCheckpoint.position;    //Move player to checkpoint position
            playerHealth.Respawn();                             //Restore player health and reset animation

            //move camera to the checkpoint room (for that we have to make the checkpoint room as child of the room object)
            Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;                    //Store the checkpoint that we activated as a main
            if (SoundManager.instance != null)
                SoundManager.instance.Playsound(checkpointAudio);
            collision.enabled = false;                                  //Deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear");    //Activate animation
        }
    }
}
