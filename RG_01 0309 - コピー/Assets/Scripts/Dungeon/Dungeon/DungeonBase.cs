using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBase : MonoBehaviour
{
    protected const int AREA_SIZE = 11;     // 区画サイズ　11 * 11


    protected const int POS_X = 0;
    protected const int POS_Z = 1;

    protected const int CENTER = -1;
    protected const int TOP = 0;
    protected const int LEFT = 1;
    protected const int BOTTOM = 2;
    protected const int RIGHT = 3;
    protected const int TOP_LEFT = 4;
    protected const int TOP_RIGHT = 5;
    protected const int BOTTOM_LEFT = 6;
    protected const int BOTTOM_RIGHT = 7;
    protected const int LEFT_TOP = 8;
    protected const int LEFT_BOTTOM = 9;
    protected const int RIGHT_TOP = 10;
    protected const int RIGHT_BOTTOM = 11;
    protected const int TOP_BOTTOM = 12;
    protected const int LEFT_RIGHT = 13;
    protected const int DEADEND = 14;

    protected int _top;     // 
    protected int _left;    // 
    protected int _bottom;  // 
    protected int _right;   // 

    public int Top
    {
        get { return _top; }
    }
    public int Left
    {
        get { return _left; }
    }
    public int Bottom
    {
        get { return _bottom; }
    }
    public int Right
    {
        get { return _right; }
    }


    public enum TROUT_TYPE
    {
        FLOOR,      // 壁無し
        WALL,       // 壁
        COUNERWALL, // 角壁
        DOOR,       // 扉
        I,          // I字路
        L,          // L字路
        U,          // U字路（行き止まり）
    }
}