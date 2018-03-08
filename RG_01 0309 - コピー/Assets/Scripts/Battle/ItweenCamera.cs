using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItweenCamera : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    public float positionTime = 2.0f;
    public float rotationTime = 2.0f;

    void LateUpdate () {
        iTween.MoveUpdate(this.gameObject, iTween.Hash(
            "position", Target.position,
            "time", positionTime)
        );
        iTween.RotateUpdate(this.gameObject, iTween.Hash(
            "rotation", Target.rotation.eulerAngles,
            "time", rotationTime)
        );
    }
}