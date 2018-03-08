using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeomCamera : MonoBehaviour
{
    [SerializeField]
    protected Transform Target;

    void LateUpdate()
    {
        transform.position = Target.position;
        transform.rotation = Target.rotation;
    }
}
