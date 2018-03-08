    using UnityEngine;

public class PlayerData
{
    private Vector3 _position;
    private Quaternion _rotation;

    // コンストラクタ
    public PlayerData(){}
    // デストラクタ
    ~PlayerData(){}

    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }
    public Quaternion Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }
}
