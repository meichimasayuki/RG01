using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2 : DungeonBase2
{
    private List<TroutData> troutDataList = new List<TroutData>();
    private List<Door> doorList = new List<Door>();
    private RoomSize roomSize;

    // コンストラクタ
    public Room2(RoomSize size, Vector3 position)
    {
        _top = (int)position.z;
        _left = (int)position.x;
        _bottom = (int)position.z + size.Height;
        _right = (int)position.x - size.Widht;
        roomSize = size;
    }
    // デストラクタ
    ~Room2()
    {
        troutDataList.Clear();
        doorList.Clear();
        roomSize = null;
    }

    // 部屋の作成
    public void Create(int room_num)
    {
        for (int z = 0; z < roomSize.Height; z++)
        {
            for (int x = 0; x < roomSize.Widht; x++)
            {
                int type = -1;
                int dir_type = -1;
                if (z == 0)
                {
                    if (x == 0) { type = (int)TROUT_TYPE.COUNERWALL; dir_type = TOP_LEFT; }
                    else if (x == roomSize.Widht - 1) { type = (int)TROUT_TYPE.COUNERWALL; dir_type = TOP_RIGHT; }
                    else { type = (int)TROUT_TYPE.WALL; dir_type = TOP; }
                }
                else if (z == roomSize.Height - 1)
                {
                    if (x == 0) { type = (int)TROUT_TYPE.COUNERWALL; dir_type = BOTTOM_LEFT; }
                    else if (x == roomSize.Widht - 1) { type = (int)TROUT_TYPE.COUNERWALL; dir_type = BOTTOM_RIGHT; }
                    else { type = (int)TROUT_TYPE.WALL; dir_type = BOTTOM; }
                }
                else
                {
                    if (x == 0) { type = (int)TROUT_TYPE.WALL; dir_type = LEFT; }
                    else if (x == roomSize.Widht - 1) { type = (int)TROUT_TYPE.WALL; dir_type = RIGHT; }
                    else { type = (int)TROUT_TYPE.FLOOR; dir_type = CENTER; }
                }
                string category = "ROOM" + room_num.ToString();
                Color color = new Color(0.1f, 0.1f, 0.1f);
                Vector3 position = new Vector3(_left - x, 0, _top + z);

                TroutData trout = new TroutData()
                {
                    ID = 0,            // ここでは未だIDを振らない
                    Type = type,
                    IsPass = false,
                    Category = category,
                    Color = color,
                    Position = position,
                    Rotation = GetDirectionRotation(dir_type),
                };
                troutDataList.Add(trout);
            }
        }
    }

    public void RoomCenter()
    {
        Debug.Log("部屋の中心を取得します。");
        int center_x = roomSize.Widht/2;
        int center_z = roomSize.Height/2;
        int index = center_z * roomSize.Widht + center_x;
        troutDataList[index].Category += "CENTER";
    }

    /*
     * マスの向き
     * （グラフィック側で向きを合わせればもっと簡略化できそう）
     */
    private Quaternion GetDirectionRotation(int dir_type)
    {
        switch (dir_type)
        {
            case TOP:
                return Quaternion.Euler(0, 90, 0);
            case LEFT:
                return Quaternion.Euler(0, 0, 0);
            case BOTTOM:
                return Quaternion.Euler(0, 270, 0);
            case RIGHT:
                return Quaternion.Euler(0, 180, 0);
            case TOP_LEFT:
                return Quaternion.Euler(0, 0, 0);
            case TOP_RIGHT:
                return Quaternion.Euler(0, 90, 0);
            case BOTTOM_LEFT:
                return Quaternion.Euler(0, 270, 0);
            case BOTTOM_RIGHT:
                return Quaternion.Euler(0, 180, 0);
            case CENTER:
                return Quaternion.Euler(0, 0, 0);
        }
        return Quaternion.Euler(0, 0, 0);
    }

    // 道との通路につながるドアを生成
    public void CreateDoor(int dir)
    {
        Door door = new Door()
        {
            Dir = dir,
        };
        int index = 0;
        switch (door.Dir)
        {
            case TOP:
                door.X = Random.Range(Mathf.Abs(_left - 1), Mathf.Abs(_right + 1)) * -1;
                door.Z = _top - 1;
                index = Mathf.Abs(door.X - _left);
                break;
            case LEFT:
                door.X = _left + 1;
                door.Z = Random.Range(_top + 1, _bottom - 1);
                index = (door.Z - _top) * roomSize.Widht;
                break;
            case BOTTOM:
                door.X = Random.Range(Mathf.Abs(_left - 1), Mathf.Abs(_right + 1)) * -1;
                door.Z = _bottom;
                index = (troutDataList.Count - roomSize.Widht) + Mathf.Abs(door.X - _left);
                break;
            case RIGHT:
                door.X = _right;
                door.Z = Random.Range(_top + 1, _bottom - 1);
                index = ((door.Z - _top + 1) * roomSize.Widht) - 1;
                break;
        }
        troutDataList[index].Type = (int)TROUT_TYPE.DOOR;
        troutDataList[index].Category = troutDataList[index].Category + "DOOR";
        door.Index = index;
        doorList.Add(door);
    }

    public void DestroyDoor()
    {
        troutDataList[doorList[doorList.Count - 1].Index].Type = (int)TROUT_TYPE.WALL;
        string category = troutDataList[doorList[doorList.Count - 1].Index].Category;
        troutDataList[doorList[doorList.Count - 1].Index].Category = category.Replace("DOOR", "");
    }

    public Door GetDoor(int index)
    {
        return doorList[index];
    }

    // 部屋の中のランダムな位置
    public Vector3 GetRoomInRandomPosition()
    {
        int rand = Random.Range(0, troutDataList.Count);
        return troutDataList[rand].Position;
    }

    // マネージャにマスデータを返す
    public List<TroutData> GetTroutDataList() { return troutDataList; }
}