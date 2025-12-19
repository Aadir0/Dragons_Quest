using UnityEngine;

public class WinningScene : MonoBehaviour
{
    [SerializeField] private GameObject Winningscene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Winningscene.SetActive(true);
        }
    }
}
