using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Hearth setup")]
    [SerializeField]
    private Animator hearthAnimator;
    [SerializeField]
    private GameObject hearthObject;

    private static int maxHealth = 3;
    private int actualHealth;

    private bool isDead = false;

    private const string EXPLODEANIMATIONPARAMETER = "Explotar";

    private static float impulseForce = 10f;

    private void OnEnable() 
    {
        actualHealth = maxHealth;
        hearthObject.SetActive(false);
        //Reset animator
        hearthAnimator.Rebind();
        hearthAnimator.ResetTrigger(EXPLODEANIMATIONPARAMETER);
        hearthAnimator.Update(0f);
        isDead = false;
    }

    [ContextMenu("Damage enemy")]
    public void ReceiveDamage()
    {
        actualHealth--;
        if(actualHealth == 1)
        {
            hearthObject.SetActive(true);
        }
        else if(actualHealth <= 1 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        hearthAnimator.SetTrigger(EXPLODEANIMATIONPARAMETER);
        Invoke(nameof(HideHearth),hearthAnimator.runtimeAnimatorController.animationClips[1].length + 0.3f);
        GetComponent<EnemyIA>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Vector3 impulseDirection =  (transform.position - GameObject.Find("Player").transform.position).normalized;
        impulseDirection.y += 1f;
        rb.AddForce(impulseDirection * impulseForce, ForceMode.Impulse);

        if(PointsManager.Instance != null)
            PointsManager.Instance.AddPoints(150);

        Destroy(gameObject, 3f);
    }

    public void HideHearth()
    {
        hearthObject.SetActive(false);
    }
}
