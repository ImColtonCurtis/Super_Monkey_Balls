using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    [SerializeField] Transform targetObj;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = targetObj.position;
    }
}
