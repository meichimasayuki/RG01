using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road2 : DungeonBase2
{
    private List<TroutData> troutDataList = new List<TroutData>();

    private Door inDoor;
    private Door outDoor;
    private int[] nowPoint; // x , z

    // コンストラクタ
    public Road2(Door in_door, Door out_door)
    {
        inDoor = in_door;
        outDoor = out_door;
        nowPoint = new int[] { outDoor.X, outDoor.Z };
        _top = inDoor.Z < outDoor.Z ? inDoor.Z : outDoor.Z;
        _left = inDoor.X > outDoor.X ? inDoor.X : outDoor.X;
        _bottom = inDoor.Z < outDoor.Z ? outDoor.Z : inDoor.Z;
        _right = inDoor.X > outDoor.X ? outDoor.X : inDoor.X;
        Create();
    }
    // デストラクタ
    ~Road2()
    {

    }

    // 道の生成
    public void Create()
    {
        int loop = 0;
        int type = (int)TROUT_TYPE.FLOOR;
        int dir_type = CENTER;
        string category = "ROAD";
        Color color = new Color(0.1f, 0.3f, 0.1f);
        Vector3 position = new Vector3(nowPoint[POS_X], 0, nowPoint[POS_Z]);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);

        TroutData strout = new TroutData()
        {
            ID = 0,            // ここでは未だIDを振らない
            Type = type,
            IsPass = false,
            Category = category,
            Color = color,
            Position = position,
            Rotation = rotation,
        };
        troutDataList.Add(strout);

        // 道のルート作成
        while (true)
        {
            List<int> road_dir = NextRoadDir();
            if (road_dir.Count == 0) break;
            int rand = Random.Range(0, road_dir.Count);
            int dir = road_dir[rand];
            switch (dir)
            {
                case TOP:
                    nowPoint[POS_Z] -= 1;
                    _bottom -= 1;
                    break;
                case LEFT:
                    nowPoint[POS_X] += 1;
                    _right += 1;
                    break;
                case BOTTOM:
                    nowPoint[POS_Z] += 1;
                    _top += 1;
                    break;
                case RIGHT:
                    nowPoint[POS_X] -= 1;
                    _left -= 1;
                    break;
                default:
                    break;
            }
            TroutData trout = new TroutData()
            {
                ID = 0,            // ここでは未だIDを振らない
                Type = type,
                IsPass = false,
                Category = category,
                Color = color,
                Position = new Vector3(nowPoint[POS_X], 0, nowPoint[POS_Z]),
                Rotation = rotation,
            };
            troutDataList.Add(trout);
            loop++;
            if (loop > 1000) break;
        }
        if (troutDataList.Count < 3) return; // 短い場合は生成やめます
        // 道の向きとグラフィックタイプの指定
        for (int i = 0; i < troutDataList.Count; i++)
        {
            float now_top = troutDataList[i].Position.z;
            float now_left = troutDataList[i].Position.x;
            if (i == 0)
            {// 最初の道
                float after_top = troutDataList[i + 1].Position.z;
                float after_left = troutDataList[i + 1].Position.x;
                if (outDoor.Dir == TOP)
                {
                    if (now_top == after_top)
                    {
                        if (now_left > after_left) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_BOTTOM; }
                        else { type = (int)TROUT_TYPE.L; dir_type = LEFT_BOTTOM; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = TOP_BOTTOM; }

                }
                else if (outDoor.Dir == LEFT)
                {
                    if (now_left == after_left)
                    {
                        if (now_top > after_top) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_TOP; }
                        else { type = (int)TROUT_TYPE.L; dir_type = RIGHT_BOTTOM; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = LEFT_RIGHT; }
                }
                else if (outDoor.Dir == BOTTOM)
                {
                    if (now_top == after_top)
                    {
                        if (now_left > after_left) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_TOP; }
                        else { type = (int)TROUT_TYPE.L; dir_type = LEFT_TOP; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = TOP_BOTTOM; }

                }
                else if (outDoor.Dir == RIGHT)
                {
                    if (now_left == after_left)
                    {
                        if (now_top > after_top) { type = (int)TROUT_TYPE.L; dir_type = LEFT_TOP; }
                        else { type = (int)TROUT_TYPE.L; dir_type = LEFT_BOTTOM; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = LEFT_RIGHT; }
                }
            }
            else if (i == troutDataList.Count - 1)
            {// 最後の道
                float before_top = troutDataList[i - 1].Position.z;
                float before_left = troutDataList[i - 1].Position.x;
                if (inDoor.Dir == TOP)
                {
                    if (now_top == before_top)
                    {
                        if (now_left > before_left) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_BOTTOM; }
                        else { type = (int)TROUT_TYPE.L; dir_type = LEFT_BOTTOM; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = TOP_BOTTOM; }

                }
                else if (inDoor.Dir == LEFT)
                {
                    if (now_left == before_left)
                    {
                        if (now_top > before_top) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_TOP; }
                        else { type = (int)TROUT_TYPE.L; dir_type = RIGHT_BOTTOM; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = LEFT_RIGHT; }
                }
                else if (inDoor.Dir == BOTTOM)
                {
                    if (now_top == before_top)
                    {
                        if (now_left > before_left) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_TOP; }
                        else { type = (int)TROUT_TYPE.L; dir_type = LEFT_TOP; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = TOP_BOTTOM; }

                }
                else if (inDoor.Dir == RIGHT)
                {
                    if (now_left == before_left)
                    {
                        if (now_top > before_top) { type = (int)TROUT_TYPE.L; dir_type = LEFT_TOP; }
                        else { type = (int)TROUT_TYPE.L; dir_type = LEFT_BOTTOM; }
                    }
                    else { type = (int)TROUT_TYPE.I; dir_type = LEFT_RIGHT; }
                }
                else if (inDoor.Dir == DEADEND)
                {
                    if (now_left == before_left)
                    {
                        if (now_top > before_top) { type = (int)TROUT_TYPE.U; dir_type = LEFT_BOTTOM; }
                        else { type = (int)TROUT_TYPE.U; dir_type = RIGHT_TOP; }
                    }
                    else
                    {
                        if (now_left > before_left) { type = (int)TROUT_TYPE.U; dir_type = LEFT_TOP; }
                        else { type = (int)TROUT_TYPE.U; dir_type = RIGHT_BOTTOM; }
                    }
                }
            }
            else
            {// 最初と最後ではない道
                float after_top = troutDataList[i + 1].Position.z;
                float after_left = troutDataList[i + 1].Position.x;
                float before_top = troutDataList[i - 1].Position.z;
                float before_left = troutDataList[i - 1].Position.x;
                if (now_top == before_top && now_top == after_top) { type = (int)TROUT_TYPE.I; dir_type = LEFT_RIGHT; }
                else if (now_left == before_left && now_left == after_left) { type = (int)TROUT_TYPE.I; dir_type = TOP_BOTTOM; }
                else
                {
                    if (now_top == before_top)
                    {
                        if (now_left > before_left)
                        {
                            if (now_top > after_top) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_TOP; }
                            else { type = (int)TROUT_TYPE.L; dir_type = RIGHT_BOTTOM; }
                        }
                        else
                        {
                            if (now_top > after_top) { type = (int)TROUT_TYPE.L; dir_type = LEFT_TOP; }
                            else { type = (int)TROUT_TYPE.L; dir_type = LEFT_BOTTOM; }
                        }
                    }
                    else
                    {
                        if (now_top > before_top)
                        {
                            if (now_left > after_left) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_TOP; }
                            else { type = (int)TROUT_TYPE.L; dir_type = LEFT_TOP; }
                        }
                        else
                        {
                            if (now_left > after_left) { type = (int)TROUT_TYPE.L; dir_type = RIGHT_BOTTOM; }
                            else { type = (int)TROUT_TYPE.L; dir_type = LEFT_BOTTOM; }
                        }
                    }
                }
            }
            troutDataList[i].Type = type;
            troutDataList[i].Rotation = GetDirectionRotation(dir_type);
        }
    }


    /*
     * マスの向き
     * （グラフィック側で向きを合わせればもっと簡略化できそう）
     */
    private Quaternion GetDirectionRotation(int dir_type)
    {
        switch (dir_type)
        {
            case LEFT_TOP:
                return Quaternion.Euler(0, 180, 0);
            case LEFT_BOTTOM:
                return Quaternion.Euler(0, 90, 0);
            case RIGHT_TOP:
                return Quaternion.Euler(0, 270, 0);
            case RIGHT_BOTTOM:
                return Quaternion.Euler(0, 0, 0);
            case TOP_BOTTOM:
                return Quaternion.Euler(0, 0, 0);
            case LEFT_RIGHT:
                return Quaternion.Euler(0, 270, 0);
        }
        return Quaternion.Euler(0, 0, 0);
    }

    List<int> NextRoadDir()
    {
        List<int> road_dir = new List<int>();
        if (nowPoint[POS_Z] - 1 >= _top) road_dir.Add(TOP);
        if (nowPoint[POS_X] + 1 <= _left) road_dir.Add(LEFT);
        if (nowPoint[POS_Z] + 1 <= _bottom) road_dir.Add(BOTTOM);
        if (nowPoint[POS_X] - 1 >= _right) road_dir.Add(RIGHT);
        return road_dir;
    }

    public int GetRoadLength()
    {
        return troutDataList.Count;
    }

    // マネージャにマスデータを返す
    public List<TroutData> GetTroutDataList() { return troutDataList; }
}
