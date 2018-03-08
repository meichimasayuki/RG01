using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesData
{
    int _lv;
    int _hp;
    int _mp;
    int _exp;
    public int LV
    {
        get { return _lv; }
        set { _lv = value; }
    }
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int MP
    {
        get { return _mp; }
        set { _mp = value; }
    }
    public int EXP
    {
        get { return _exp; }
        set { _exp = value; }
    }
}
