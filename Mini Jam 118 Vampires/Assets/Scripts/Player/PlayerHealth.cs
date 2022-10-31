using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private int life = 120;
    [SerializeField] private GameObject deathScreen;
    private Animator HUDAnimator;

    [Header("Player Blood")]
    [SerializeField]
    private GameObject[] bloodAmmo;

    [SerializeField]
    private Transform bloodCyilinder;

    [SerializeField]
    float depthOffsetMax;
    [SerializeField]
    float depthScaleMax;
    [SerializeField]
    float depthOffset;
    [SerializeField]
    float depthScale;

    int amountOfBlood;

    Vector3 initialScale;
    Vector3 initialLocalPosition;

    int actualAmmoIndex;

    void OnEnable()
    {
        initialScale = bloodCyilinder.transform.localScale;
        initialLocalPosition = bloodCyilinder.transform.localPosition;
        actualAmmoIndex = 0;
    }

    [ContextMenu("Add blood")]
    public void AddBlood()
    {
        Vector3 scale = bloodCyilinder.transform.localScale;
        scale.z += depthScale;
        scale.z = Mathf.Clamp(scale.z, 0.088108f, depthScaleMax);
        bloodCyilinder.transform.localScale = scale;

        Vector3 localPosition = bloodCyilinder.transform.localPosition;
        localPosition.z += depthOffset;
        localPosition.z = Mathf.Clamp(localPosition.z, -0.0002f, depthOffsetMax);
        bloodCyilinder.transform.localPosition = localPosition;

        amountOfBlood++;

    }

    public bool UseBlood()
    {
        if(amountOfBlood >= 4)
        {
            amountOfBlood = 0;
            bloodCyilinder.transform.localPosition = initialLocalPosition;
            bloodCyilinder.transform.localScale = initialScale;
            return true;
        }
        else{
            return false;
        }
    }

    [ContextMenu("Reload gun")]
    public void Reload()
    {
        StartCoroutine(nameof(ReloadAnimation));
    }

    [ContextMenu("Use ammo")]
    public void ReduceAmmo()
    {
        if(actualAmmoIndex >= bloodAmmo.Length)
            return;

        bloodAmmo[actualAmmoIndex].SetActive(false);
        actualAmmoIndex++;
    }

    IEnumerator ReloadAnimation()
    {
        for(int i = bloodAmmo.Length - 1; i >= 0; i--)
        {
            bloodAmmo[i].SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
        actualAmmoIndex = 0;
    }

    public void PlayerRecieveDamage(){
        HUDAnimator = deathScreen.GetComponentInChildren<Animator>();
        if(life > 0){
            life -= 20;
            if(life <= 0){
                HUDAnimator.SetTrigger("Death");
                StartCoroutine(nameof(ReloadScene));
            }
        }
        if(life == 40){
            HUDAnimator.SetBool("Danger", true);
        }
        Debug.Log(life);
    }

    public void PlayerHeal(){
        life = 120;
        HUDAnimator.SetBool("Danger", false);
    }

    private IEnumerator ReloadScene(){
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
