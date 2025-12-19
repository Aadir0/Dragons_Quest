using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    //To set Amount of health restored by collectible
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //To check if object entering the trigger is player
        if (collision.CompareTag("Player"))
        {
            if (SoundManager.instance != null)
                SoundManager.instance.Playsound(pickupSound);
            //Restore player's health
            collision.GetComponent<Health>().AddHealth(healthValue);
            
            //Disable the collectible after one use
            gameObject.SetActive(false);
        }
    }
}