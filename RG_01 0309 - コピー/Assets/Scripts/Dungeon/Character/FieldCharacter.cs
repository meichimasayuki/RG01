using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCharacter : CharacterBase
{
    /*
     * ダンジョン用マネージャー
     */
    private DungeonManager2 _manager;
    public DungeonManager2 Manager
    {
        get { return _manager; }
        set { _manager = value; }
    }



    /*
     * 回転関連
     */
    private const float ROTATION_SPEED = 0.05f;

    protected bool isRotation;

    private Quaternion fromRotation;
    private Quaternion toRotation;
    private float rotationTime = 0.0f;

    protected void StartRotation()
    {
        rotationTime = 0.0f;
        fromRotation = transform.rotation;
        isRotation = true;
    }

    protected void Rotation(Vector3 vector, float speed = ROTATION_SPEED)
    {
        if (!isRotation) return;
        toRotation = Quaternion.LookRotation(vector);
        rotationTime += Time.deltaTime;
        float ratio = Mathf.InverseLerp(0, 1, rotationTime / speed);
        transform.rotation = Quaternion.Slerp(fromRotation, toRotation, ratio);
        if (ratio == 1) { isRotation = false; }
    }

    protected void EndRotation()
    {
        isRotation = false;
    }
}
