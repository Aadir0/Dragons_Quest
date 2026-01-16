using System.Collections;
using System.Threading;
using NUnit.Framework.Constraints;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
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

    [Header ("Components")]
    [SerializeField] private Behaviour[] components;
    [Header ("Audio Clips")]
    [SerializeField] private AudioClip Deathsound;
    [SerializeField] private AudioClip hurtsound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        //If already dead, don't take any more damage
        if (dead) return;

        //Subtract health but restrict to so it never goes below 0
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        //  Character is still alive
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            if (SoundManager.instance != null)
                SoundManager.instance.Playsound(hurtsound);
        }
        else
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
                SoundManager.instance.Playsound(Deathsound);
        }
    }
    private void deactivate()
    {
        gameObject.SetActive(false);
    }
}