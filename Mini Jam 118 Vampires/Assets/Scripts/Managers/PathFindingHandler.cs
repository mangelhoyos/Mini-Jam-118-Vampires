using UnityEngine;

public class PathFindingHandler : MonoBehaviour
{
    [SerializeField]
    private PathfindingNode[] referencePoints;
    [SerializeField]
    private Transform playerTransform;

    public static PathFindingHandler Instance {private set; get;}

    private void Awake() 
    {
        Instance = this;
    }

    public PathfindingNode GetClosestReferencePoint(PathfindingNode actualNode)
    {
        if(referencePoints.Length < 1)
        {
            Debug.LogWarning("No hay elementos como referencias");
            return null;
        }
        
        int tryIndex = 0;
        PathfindingNode closestPoint = null;
        closestPoint = referencePoints[tryIndex];
        while(referencePoints[tryIndex].isOccupied)
        {
            closestPoint = referencePoints[tryIndex];
            tryIndex++;
        }
        closestPoint.isOccupied = true;
        
        float closestPointDistance = Vector3.Distance(closestPoint.transform.position, playerTransform.position);
        foreach(PathfindingNode point in referencePoints)
        {
            float actualPointDistance = Vector3.Distance(point.transform.position, playerTransform.position);
            
            if((closestPointDistance > actualPointDistance && !point.isOccupied) || (closestPointDistance > actualPointDistance && point == actualNode))
            {
                closestPoint.isOccupied = false;
                point.isOccupied = true;
                closestPoint = point;
                closestPointDistance = actualPointDistance;
            }
        }

        return closestPoint;
    }
}
