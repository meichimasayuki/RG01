using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData {

    private Vector3 _position;

    // コンストラクタ
    public EnemyData() { }
    // デストラクタ
    ~EnemyData() { }

    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }
}
