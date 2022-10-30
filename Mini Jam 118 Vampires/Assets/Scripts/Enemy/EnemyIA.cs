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

    private NavMeshAgent agent;

    private Transform targetTransform;
    private static string PLAYERGONAMETOFIND = "Player";

    private static float ANGULARSPEED = 120;
    private float playerDistance;

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
        }
        else
        {
            agent.angularSpeed = ANGULARSPEED;
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
