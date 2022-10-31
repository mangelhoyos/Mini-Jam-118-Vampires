using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 objective;
    private static float SPEED = 20f;

    private PlayerHealth playerHealth;

    public void SetObjective(Vector3 targetObjective)
    {
        objective = targetObjective;
    }

    void Update()
    {
        if(objective != null)
        {
            transform.position += objective * SPEED * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Shoot");
            playerHealth = other.GetComponentInParent<PlayerSmoothMovement>().ReturnPlayerHealth();;
            playerHealth.PlayerRecieveDamage();
        }
    }
}
