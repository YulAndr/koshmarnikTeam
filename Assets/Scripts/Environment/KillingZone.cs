using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingZone: MonoBehaviour {
    public GameObject player;
    public Transform respawnPoint;

    private void OnCollisionEnter2D (Collision2D other) // Важная пометка, на которую я убила 8 часов. еслииспользуем onCollision, то галочку isTrigger нужно обязательно снять. Если использем OnTrigger, галочка обязательна
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collision detected");
            player.transform.position = respawnPoint.position;
        }
    }
}
