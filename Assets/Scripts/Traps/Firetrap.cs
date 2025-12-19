using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header ("Firetrap timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteren;
    private bool triggerd;
    private bool active;
    [Header ("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteren = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggerd)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if (active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
    
    private IEnumerator ActivateFiretrap()
    {
        //To change the object color to red when triggered and trigger the trap
        triggerd = true;
        spriteren.color = Color.red;  

        //Wait for the delay 
        yield return new WaitForSeconds(activationDelay);
        if (SoundManager.instance != null)
            SoundManager.instance.Playsound(firetrapSound);
        //To change the object color to default and activate the trap
        spriteren.color = Color.white;  
        active = true;
        //Turn on the animation
        anim.SetBool("activate", true);

        //Wait until x seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        triggerd = false;
        active = false;
        anim.SetBool("activate", false);
    }
}
