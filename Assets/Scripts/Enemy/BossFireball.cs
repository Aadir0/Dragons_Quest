using UnityEngine;

public class BossFireball : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 3f;
    [SerializeField] private float lifetime = 5f;
    
    private float direction;
    private bool hit;
    private float lifeTimer;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hit = true;
            boxCollider.enabled = false;
            
            // Damage the player
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            // Play explosion animation
            if (anim != null)
            {
                anim.SetTrigger("explode");
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void ActivateFireball(float _direction)
    {
        lifeTimer = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        
        // Flip the fireball sprite based on direction
        float localScaleX = Mathf.Abs(transform.localScale.x);
        if (_direction < 0)
            localScaleX = -localScaleX;
        
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // Called by explosion animation using animation events
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
