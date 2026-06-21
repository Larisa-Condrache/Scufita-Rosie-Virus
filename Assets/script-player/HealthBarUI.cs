using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;

    void Update()
    {
        fillImage.fillAmount =
            (float)playerHealth.currentHealth / playerHealth.maxHealth;
    }
}