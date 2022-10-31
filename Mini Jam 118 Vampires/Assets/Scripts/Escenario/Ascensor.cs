using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ascensor : MonoBehaviour
{
    [SerializeField]
    private Transform objectiveTransform;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private UnityEvent OnPointReached;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectiveTransform.position, movementSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, objectiveTransform.position) < 0.5f)
        {
            OnPointReached?.Invoke();
            Destroy(this);
        }
    }
}
