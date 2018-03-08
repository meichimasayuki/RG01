using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area2 : DungeonBase2
{
    private Room2 room;                             // 部屋
    private int areaID;                             // 区画番号
    private const int OUTSKIRTS = 2;                // 部屋の外周部の余白
    private int DUNGEON_SIZE = 0;                   // ダンジョンの大きさ
    List<int> nextAreaIDList = new List<int>();     // 次に進む区画番号のリスト

    // コンストラクタ
    public Area2(int id, int size, Vector3 position)
    {
        areaID = id + 1;                            // 都合上 0 を使いたくないので +1 (余り計算)
        DUNGEON_SIZE = size;                        // ここ以外で値の変更はしない
        _top = (int)position.z;
        _left = (int)position.x;
        _bottom = (int)position.z + AREA_SIZE;
        _right = (int)position.x - AREA_SIZE;
        CreateRoom(areaID);
    }
    // デストラクタ
    ~Area2() {
        room = null;
        nextAreaIDList.Clear();
    }

    /*
     * 部屋の生成
     * 区画生成時と同時に生成
     */
    public void CreateRoom(int area_num)
    {
        // 部屋の大きさだけ先に生成
        RoomSize size = new RoomSize(area_num == 1 || area_num == (DUNGEON_SIZE * DUNGEON_SIZE));

        // 部屋が区画内に収まる範囲で部屋の位置決め（ランダム）
        int room_top = Random.Range(_top + OUTSKIRTS, _bottom - OUTSKIRTS - size.Height);
        int room_left = Random.Range(_right + OUTSKIRTS + size.Widht, _left - OUTSKIRTS);
        Vector3 position = new Vector3(room_left, 0, room_top);

        // 部屋の作成
        room = new Room2(size, position);
        room.Create(area_num);
    }


    public Vector3 GetRoomInRandomPosition()
    {
        return room.GetRoomInRandomPosition();
    }

    /*
     * 隣接する区画のリスト
     */
    public List<int> GetNextToAreaList()
    {
        List<int> next_to_area_list = new List<int>();
        int nextto_top = areaID - DUNGEON_SIZE;
        int nextto_left = areaID - 1;
        int nextto_bottom = areaID + DUNGEON_SIZE;
        int nextto_right = areaID + 1;
        if (nextto_top > 0) { next_to_area_list.Add(nextto_top); }                                      // 上
        if (nextto_left % DUNGEON_SIZE != 0) { next_to_area_list.Add(nextto_left); }                    // 左
        if (nextto_bottom <= (DUNGEON_SIZE * DUNGEON_SIZE)) { next_to_area_list.Add(nextto_bottom); }    // 下
        if (nextto_right % DUNGEON_SIZE != 1) { next_to_area_list.Add(nextto_right); }                  // 右
        return next_to_area_list;
    }

    /*
     * 部屋への入り口を作成
     * 次に進む区画番号の登録
     */
    public void CreateDoor(int id)
    {
        int dir = GetDirection(id);
        if (dir == -1) return;
        room.CreateDoor(dir);
        nextAreaIDList.Add(id);
    }

    /*
     * 渡された区画番号の方向
     */
    private int GetDirection(int id)
    {
        if (areaID - DUNGEON_SIZE == id) { return TOP; }         // 上
        else if (areaID - 1 == id) { return LEFT; }              // 左
        else if (areaID + DUNGEON_SIZE == id) { return BOTTOM; } // 下
        else if (areaID + 1 == id) { return RIGHT; }             // 右
        return -1;
    }

    /*
     * 次に進む区画番号のリストの取得
     */
    public List<int> GetNextAreaIDList()
    {
        return nextAreaIDList;
    }

    /*
     * 
     */
    public Door GetDoor(int index)
    {
        return room.GetDoor(index);
    }

    /*
     * 袋小路の作成
     */
    public Road2 CreateBlindAlley()
    {
        if (nextAreaIDList.Count != 2) return null;
        List<int> dir_list = new List<int>()
        {
            TOP,
            LEFT,
            BOTTOM,
            RIGHT,
        };
        for (int i = 0; i < nextAreaIDList.Count; i++)
        {
            int dir = GetDirection(nextAreaIDList[i]);
            for (int n = 0; n < dir_list.Count; n++)
            {
                if (dir == dir_list[n]) { dir_list.RemoveAt(n); break; }
            }
        }
        int rand = Random.Range(0, dir_list.Count);
        room.CreateDoor(dir_list[rand]);

        Door door_dummy = new Door()
        {
            Dir = DEADEND,
        };

        switch (dir_list[rand])
        {
            case TOP:
                door_dummy.X = Random.Range(Mathf.Abs(_left - 2), Mathf.Abs(_right + 2)) * -1;
                door_dummy.Z = _top + 1;
                break;
            case LEFT:
                door_dummy.X = _left - 1;
                door_dummy.Z = Random.Range(_top + 3, _bottom - 3);
                break;
            case BOTTOM:
                door_dummy.X = Random.Range(Mathf.Abs(_left - 2), Mathf.Abs(_right + 2)) * -1;
                door_dummy.Z = _bottom - 2;
                break;
            case RIGHT:
                door_dummy.X = _right + 2;
                door_dummy.Z = Random.Range(_top + 3, _bottom - 3);
                break;
        }

        Door out_door = room.GetDoor(2);
        Road2 road = new Road2(door_dummy, out_door);
        // あまりにも短い袋小路状態なら作成しない
        if (road.GetRoadLength() > 3/* MG 気を付けて */) return road;
        else
        {
            Debug.Log("短い袋小路が生成されました。");
            Debug.Log("袋小路を削除 ＆ ドアを削除");
            room.DestroyDoor();
            return null;
        }
    }


    public void RoomCenter()
    {
        room.RoomCenter();
    }

    // マネージャにマスデータを返す
    public List<TroutData> GetTroutDataList() { return room.GetTroutDataList(); }
}
