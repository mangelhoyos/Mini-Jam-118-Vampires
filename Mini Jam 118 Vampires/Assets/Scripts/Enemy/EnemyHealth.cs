using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Hearth setup")]
    [SerializeField]
    private Animator hearthAnimator;
    [SerializeField]
    private GameObject hearthObject;

    [SerializeField]
    private NavMeshAgent agentDisable;
    [SerializeField]
    private EnemyIA enemyDisable;
    [SerializeField]
    private Rigidbody rbDisable;

    private static int maxHealth = 3;
    private int actualHealth;

    public bool isDead = false;

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
        if(enemyDisable != null)
            enemyDisable.enabled = false;

        if(agentDisable != null)    
            agentDisable.enabled = false;
        rbDisable.isKinematic = false;
        Vector3 impulseDirection =  (transform.position - GameObject.Find("Player").transform.position).normalized;
        impulseDirection.y += 1f;
        rbDisable.AddForce(impulseDirection * impulseForce, ForceMode.Impulse);

        if(PointsManager.Instance != null)
            PointsManager.Instance.AddPoints(150);

        if(agentDisable != null)
        {
            Destroy(gameObject, 4f);
            EnemySpawnHandler.Instance.actualNormalEnemies--;
        }
        else
        {
            Destroy(gameObject.transform.parent.gameObject, 4f);
            EnemySpawnHandler.Instance.actualStaticEnemies--;
        }
    }

    public void HideHearth()
    {
        hearthObject.SetActive(false);
    }
}
