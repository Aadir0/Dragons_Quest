using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;

    //Object pool of fireballs
    [SerializeField] private GameObject[] fireballs;
    private Animator anim;
    private PlayerMovement playerMovement;
    [SerializeField] private AudioClip firballSound;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        //Left mouse click pressed + cooldown finished + movement allows attacking
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        //Increase cooldown timer every frame
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.Playsound(firballSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        //Move fireball to firepoint position
        fireballs[FindFireball()].transform.position = firePoint.position;
        //Activate and set direction of projectile
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    //Returns a list of the first available fireball from the pool
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy) // inactive = free to use
                return i;
        }
        //If all fireballs are active, return first one (means it cancels it and overwrites it)
        return 0;
    }
}