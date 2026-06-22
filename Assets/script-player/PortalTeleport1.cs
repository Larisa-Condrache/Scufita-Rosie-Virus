using UnityEngine;

public class PortalTeleport1 : MonoBehaviour
{
    public Transform destination;

    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used)
            return;

        if (other.CompareTag("Player"))
        {
            other.transform.position = destination.position;
            used = true;
        }
    }
}