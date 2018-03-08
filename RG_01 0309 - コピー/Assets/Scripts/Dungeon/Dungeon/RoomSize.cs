using System.Collections.Generic;
using UnityEngine;

public class RoomSize
{
    // 部屋サイズ　7 * 7 ～ 3 * 3
    private List<int[]> roomSizeList = new List<int[]>()
    {
        {new int[] { 3,3} },
        {new int[] { 3,5} },
        {new int[] { 5,3} },
        {new int[] { 3,7} },
        {new int[] { 7,3} },
        {new int[] { 5,5} },
        {new int[] { 5,7} },
        {new int[] { 7,5} },
        {new int[] { 7,7} },
    };

    protected int _width;   // 部屋の横幅
    protected int _height;  // 部屋の縦幅
    public int Widht
    {
        get { return _width; }
    }
    public int Height
    {
        get { return _height; }
    }

    // コンストラクタ
    public RoomSize(bool is_first_or_finish)
    {
        if (is_first_or_finish)
        {
            _width = _height = 5;
        }
        else
        {
            int rand = Random.Range(0, roomSizeList.Count);
            _width = roomSizeList[rand][0];
            _height = roomSizeList[rand][1];
        }
    }
    // デストラクタ
    ~RoomSize()
    {
    }
}