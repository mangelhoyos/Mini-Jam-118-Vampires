using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private float meleeCoolDown = 0;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip meleeClip;
    [SerializeField] private AudioClip souberBlood;
    private EnemyHealth enemyHealth;
    private Animator meleeAnimator;
    private Collider coll;

    private void Start(){
        meleeAnimator = gameObject.GetComponentInParent<Animator>();
    }

    private void Update(){
        meleeCoolDown -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E)){
            if(meleeCoolDown <= 0){
                meleeAnimator.SetTrigger("Melee");
                meleeCoolDown = 1;
                audioSource.PlayOneShot(meleeClip);
                if(coll != null && coll.CompareTag("Enemy")){
                    enemyHealth = coll.GetComponent<EnemyHealth>();
                    enemyHealth.ReceiveDamage();
                    if(enemyHealth.isDead){
                        playerHealth.AddBlood();
                        audioSource.PlayOneShot(souberBlood);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        coll = other;
    }

    private void OnTriggerExit(Collider other){
        coll = null;
    }
}
