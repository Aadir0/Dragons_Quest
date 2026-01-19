using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("Spikehead Attribute")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private Vector2 destination;
    private Vector3[] directions = new Vector3[4];
    private float checkTimer;
    private bool attacking;
    [Header ("SFX")]
    [SerializeField] private AudioClip impactSound;
    
    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        //If the spikehead is attacking move him to final destination
        if (attacking)
        {
            transform.Translate(destination * Time.deltaTime * speed);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();

        //Check if spikehead sees player in all 4 directions
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
                break;
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.right * range;    //To calculate right direction
        directions[1] = -transform.right * range;   //To calculate left direction
        directions[2] = transform.up * range;       //To calculate up 
        directions[3] = -transform.up * range;    //To calculate down
    }

    private void Stop()
    {
        destination = transform.position; //Sets destination as current position so it doesn't move
        attacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SoundManager.instance != null)
            SoundManager.instance.Playsound(impactSound);
        base.OnTriggerEnter2D(collision);

        Stop();   //Stop spikehead ones it hit something
    }
}
