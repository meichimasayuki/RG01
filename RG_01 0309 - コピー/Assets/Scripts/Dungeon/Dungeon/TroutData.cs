using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroutData
{
    protected int _id;
    protected int _type;
    protected bool _ispass;
    protected string _category;
    protected Color _color;
    protected Color _mapcolor;
    protected Vector3 _positon;
    protected Quaternion _rotation;

    // コンストラクタ
    public TroutData() { }
    // デストラクタ
    ~TroutData() { }

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }
    public int Type
    {
        get { return _type; }
        set { _type = value; }
    }
    public bool IsPass
    {
        get { return _ispass; }
        set { _ispass = value; }
    }
    public string Category
    {
        get { return _category; }
        set { _category = value; }
    }
    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }
    public Color MapColor
    {
        get { return _mapcolor; }
        set { _mapcolor = value; }
    }
    public Vector3 Position
    {
        get { return _positon; }
        set { _positon = value; }
    }
    public Quaternion Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }
}
