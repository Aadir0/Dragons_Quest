using System.Collections;
using System.Threading;
using UnityEngine;

public class Health : MonoBehaviour
{
    // To make a header in unity editor
    [Header("Health")]

    //Initial health of the player
    [SerializeField] private float startingHealth;

    //Public property for current hp which can be changed by any script like traps, enemys or collectibles.
    public float currentHealth { get; private set; }

    //Refrence to animator
    private Animator anim;

    //Tracks if the character is dead or not
    private bool dead;

    [Header("iFrames or Invincibility")]

    //How long invincibility would be
    [SerializeField] private float iFramesDuration;

    //How many times flashes during i-frames
    [SerializeField] private int numberOfFlashes;

    //For flashing
    private SpriteRenderer SpriteRen;
    [Header ("Components")]
    [SerializeField] private Behaviour[] components;
    [Header ("Audio Clips")]
    [SerializeField] private AudioClip Deathsound;
    [SerializeField] private AudioClip hurtsound;
    private PlayerMovement player;

    private void Awake()
    {
        //Set starting health
        currentHealth = startingHealth;

        //To get components
        anim = GetComponent<Animator>();
        SpriteRen = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerMovement>();
    }

    //-------------Take-Damage------------------
    public void TakeDamage(float _damage)
    {
        //If already dead, don't take any more damage
        if (dead) return;

        //Subtract health but restrict to so it never goes below 0
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        //  Character is still alive
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");            //Play hurt animation
            StartCoroutine(Invernability());    //Start i-frames so player can't get damage for some times
            if (SoundManager.instance != null)
                SoundManager.instance.Playsound(hurtsound);
        }
        else
        {
            Die();
        }
    }

    //----------------Healing-----------------
    public void AddHealth(float _value)
    {
        //Add health but clamp it so it does not exceed startingHealth
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("idle");
        StartCoroutine(Invernability());

        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
    }

    // ---------------Invincibility Duration------------------
    private IEnumerator Invernability()
    {
        //Disable collison between player layer 8 and Enemy/Trap layer 10
        Physics2D.IgnoreLayerCollision(8, 10, true);
        player.speed = 4; //Reduce player speed during invincibility

        //Flashes the color several time
        for (int i = 0; i < numberOfFlashes; i++)
        {
            //Make sprite semi-transparent of any color
            SpriteRen.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / numberOfFlashes * 3);

            //return to normal color
            SpriteRen.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / numberOfFlashes * 3);
        }

        //After invincibility ends enable the collision between layer 8 and 10
        Physics2D.IgnoreLayerCollision(8, 10, false);
        player.speed = 5; //Return to normal speed
    }

    private void Die()
    {
        //Character just reached 0 health - trigger death
        dead = true;  //Mark player as dead FIRST to prevent multiple death triggers
        anim.SetTrigger("die");         //Play death animation

        //Disable components but NOT the animator
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.Playsound(Deathsound);
        }
    }
}