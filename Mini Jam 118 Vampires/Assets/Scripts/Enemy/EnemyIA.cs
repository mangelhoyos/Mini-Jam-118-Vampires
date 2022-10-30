using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIA : MonoBehaviour
{
    [Header("Gun setup")]
    [SerializeField]
    private Transform gunTransform;

    [Header("Character behaviour setup")]
    [SerializeField]
    private float coroutineFrequency;
    [SerializeField]
    private float rotationCheckDistance;

    [Header("Enemy attack setup")]
    [SerializeField]
    private GameObject enemyBulletPrefab;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private float shootFireRate;
    private float actualTimer;

    private NavMeshAgent agent;

    private Transform targetTransform;
    private static string PLAYERGONAMETOFIND = "Player";

    private static float ANGULARSPEED = 120;
    private float playerDistance;
    private bool isInRange;

    private PathfindingNode selectedNode;
   
    private void OnEnable() 
    {
        targetTransform = GameObject.Find(PLAYERGONAMETOFIND).transform;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(nameof(MoveHandlerCoroutine));
    }

    void Update()
    {
        RotateGunTowardsPlayer();
        CheckRotateCharacterTowardsPlayer();
        ShootToPlayer();
    }

    void ShootToPlayer()
    {
        if(isInRange)
        {
            actualTimer += Time.deltaTime;
            if(actualTimer >= shootFireRate)
            {
                actualTimer = 0;
                GameObject bullet = Instantiate(enemyBulletPrefab, shootPosition.position, Quaternion.identity);
                bullet.transform.LookAt(targetTransform);
                Vector3 compareTargetPos = targetTransform.position;
                compareTargetPos.y = compareTargetPos.y - 0.6f;
                bullet.GetComponent<EnemyBullet>().SetObjective((compareTargetPos - transform.position).normalized);
                Destroy(bullet, 5f);
            }
        }
        else
        {
            actualTimer = 0;
        }
    }

    void RotateGunTowardsPlayer()
    {
        gunTransform.LookAt(targetTransform);   
    }

    void CheckRotateCharacterTowardsPlayer()
    {
        if(playerDistance < rotationCheckDistance)
        {
            agent.angularSpeed = 0;
            Vector3 newRotationVector = targetTransform.position;
            newRotationVector.y = transform.position.y;
            transform.LookAt(newRotationVector);
            isInRange = true;
        }
        else
        {
            agent.angularSpeed = ANGULARSPEED;
            isInRange = false;
        }
    }

    void CheckPlayerDistance()
    {
        playerDistance = Vector3.Distance(transform.position, targetTransform.position);
    }

    IEnumerator MoveHandlerCoroutine()
    {
        yield return new WaitForSeconds(coroutineFrequency);

        CheckPlayerDistance();
        MoveTowardsPlayer();

        StartCoroutine(nameof(MoveHandlerCoroutine));
    }

    void MoveTowardsPlayer()
    {
        PathfindingNode objective = PathFindingHandler.Instance.GetClosestReferencePoint(selectedNode);
        if(selectedNode != null && selectedNode != objective)
            selectedNode.isOccupied = false;

        objective.isOccupied = true;
        selectedNode = objective;

        agent.SetDestination(selectedNode.transform.position);
        
    }
}
