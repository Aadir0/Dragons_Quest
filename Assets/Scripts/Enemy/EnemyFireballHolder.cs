using UnityEngine;

public class EnemyFireballHolder : MonoBehaviour
{
    [SerializeField] private Transform Enemy;

    private void Update()
    {
        transform.localScale = Enemy.localScale;
    }
}
