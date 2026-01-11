using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    //Reference to players health script
    [SerializeField] private Health playerHealth;

    //Total health bar 
    [SerializeField] private Image totalhealthBar;

    //currrent health bar
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        // Check if playerHealth is assigned
        if (playerHealth == null)
        {
            Debug.LogError("Player Health not assigned to Healthbar! Please assign it in the Inspector.");
            return;
        }
        
        //Set the total health bar's size based on max health
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
    
    private void Update()
    {
        // Check if playerHealth is assigned
        if (playerHealth == null)
            return;
            
        //update current health bar every frame
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}