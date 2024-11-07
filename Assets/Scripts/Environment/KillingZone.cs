using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingZone: MonoBehaviour {
    public GameObject player;
    public Transform respawnPoint;

    private void OnCollisionEnter2D (Collision2D other) // ������ �������, �� ������� � ����� 8 �����. �������������� onCollision, �� ������� isTrigger ����� ����������� �����. ���� ��������� OnTrigger, ������� �����������
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collision detected");
            player.transform.position = respawnPoint.position;
        }
    }
}
