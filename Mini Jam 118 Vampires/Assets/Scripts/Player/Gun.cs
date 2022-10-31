using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private LayerMask enemyLayerMask;
    private int bullets = 12;
    private Animator gunAnimator;
    private float shootCoolDown = 0;
    private EnemyHealth enemyHealth;

    [Header("Laser")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform rayPivot;
    [SerializeField] private float laserDuration = 0.05f;
    [SerializeField] private LayerMask rayLayerMask;
    Vector3 cameraFowardPos;

    private void Start(){
        gunAnimator = gameObject.GetComponent<Animator>();
    }

    private void Update(){
        ShootGun();
        ReloadGun();
        shootCoolDown -= Time.deltaTime;
    }

    private void ShootGun(){
        if(Input.GetMouseButton(0) && shootCoolDown <= 0 && bullets > 0){
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, Mathf.Infinity, enemyLayerMask)){
                enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                enemyHealth.ReceiveDamage();
            }
            Physics.Raycast(cameraPivot.transform.position, cameraPivot.transform.forward, out RaycastHit hit2, Mathf.Infinity);
            lineRenderer.SetPosition(0, rayPivot.position);
            lineRenderer.SetPosition(1, hit2.point);
            bullets -= 1;
            gunAnimator.SetTrigger("Shoot");
            playerHealth.ReduceAmmo();
            shootCoolDown = 2;
            StartCoroutine(ShootLaser());
        }
    }

    private void ReloadGun(){
        if(Input.GetKeyDown(KeyCode.R) && playerHealth.UseBlood()){
            bullets = 12;
            playerHealth.Reload();
        }
    }

    private IEnumerator ShootLaser(){
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;
    }
}
