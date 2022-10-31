using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform rayPivot;
    [SerializeField] private LayerMask enemyLayerMask;
    private Animator gunAnimator;
    private float shootCoolDown = 0;
    private EnemyHealth enemyHealth;

    private void Start(){
        gunAnimator = gameObject.GetComponent<Animator>();
    }

    private void Update(){
        ShootGun();
        ReloadGun();
        shootCoolDown -= Time.deltaTime;
    }

    private void ShootGun(){
        if(Input.GetMouseButton(0) && shootCoolDown <= 0){
            if(Physics.Raycast(rayPivot.position, rayPivot.forward, out RaycastHit hit, Mathf.Infinity, enemyLayerMask)){
                enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                enemyHealth.ReceiveDamage();
            }
            gunAnimator.SetTrigger("Shoot");
            playerHealth.ReduceAmmo();
            shootCoolDown = 2;
        }
    }

    private void ReloadGun(){
        if(Input.GetKeyDown(KeyCode.R) && playerHealth.UseBlood()){
            playerHealth.Reload();
        }
    }
}
