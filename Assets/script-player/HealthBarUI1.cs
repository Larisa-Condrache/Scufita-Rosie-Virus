using UnityEngine;
using TMPro;

public class HealthBarUI1 : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TMP_Text healthText;

    void Update()
    {
        healthText.text = playerHealth.currentHealth + " %";
    }
}