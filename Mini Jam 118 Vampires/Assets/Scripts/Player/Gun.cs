using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform rayPivot;
    [SerializeField] private float shootCoolDown = 0;
    [SerializeField] private LayerMask enemyLayerMask;
    private EnemyHealth enemyHealth;

    private void Update(){
        ShootGun();
        ReloadGun();
    }

    private void ShootGun(){
        if(Input.GetMouseButton(0) && shootCoolDown == 0){
            if(Physics.Raycast(rayPivot.position, rayPivot.forward, out RaycastHit hit, Mathf.Infinity, enemyLayerMask)){
                enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                enemyHealth.ReceiveDamage();
            }
            playerHealth.ReduceAmmo();
        }
    }

    private void ReloadGun(){
        if(Input.GetKeyDown(KeyCode.R)){
            playerHealth.Reload();
        }
    }
}
