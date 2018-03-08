using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTable {

    private int _num;       // 敵の出現数
    private int _minlv;     // 最低レベル
    private int _maxlv;     // 最大レベル
    private int _type;      // 敵の種類（現状はスケルトンしかいないからそのまま）

    public int NUM
    {
        get { return _num; }
        set { _num = value; }
    }
    public int MIN_LV
    {
        get { return _minlv; }
        set { _minlv = value; }
    }
    public int MAX_LV
    {
        get { return _maxlv; }
        set { _maxlv = value; }
    }
    public int LEVEL
    {
        get { return Random.Range(_minlv,(_maxlv + 1)); }
    }
    public int TYPE
    {
        get { return _type; }
        set { _type = value; }
    }
}
namespace EnemyTableCategory
{
    public static class Table
    {
        public enum CATEGORY
        {
            NUM = 0,
            MIN_LV,
            MAX_LV,
            TYPE,
        }
        public enum LEVEL
        {
            EASY = 0,
            NORMAL,
            HARD,
            EXPERT,
            MASTER,
        }
    }
}
