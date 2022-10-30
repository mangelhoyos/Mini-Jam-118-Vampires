using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsAnim : MonoBehaviour
{
    [SerializeField]
    private float amplitude;

    void Update()
    {
        Vector3 newPos = transform.localPosition;
        newPos.y += Mathf.Sin(Time.time * 3f) * (amplitude * 0.5f);
        transform.localPosition = newPos;
    }
}
