using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private bool hit;
    private BoxCollider2D collide;
    private Animator anim;
    private float lifetime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        collide = GetComponent<BoxCollider2D>();
    }
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        collide.enabled = true;
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision);   //The base will call parent script codes first
        collide.enabled = false;
        if (anim != null)
        {
            anim.SetTrigger("explode");
        }
        gameObject.SetActive(false);        //After hitting the arrows will get deactivated
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
