using UnityEngine;

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
            isDead = true;
            hearthAnimator.SetTrigger(EXPLODEANIMATIONPARAMETER);
            Invoke(nameof(HideHearth),hearthAnimator.runtimeAnimatorController.animationClips[1].length + 0.3f);
        }
    }

    public void HideHearth()
    {
        hearthObject.SetActive(false);
    }
}
