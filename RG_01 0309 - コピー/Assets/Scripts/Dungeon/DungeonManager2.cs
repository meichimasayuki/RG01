using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager2 : DungeonBase
{
    [SerializeField] private DungeonGameManager gameManager;
    [SerializeField] private Player playerOwner;
    [SerializeField] private EnemyField enemyObj;
    [SerializeField] private GameObject ItemObj;

    [SerializeField]
    private Trout2 troutObj;

    private List<GameObject> itemList = new List<GameObject>();
    private List<EnemyField> enemyList = new List<EnemyField>();
    private List<Area2> areaList = new List<Area2>();
    private List<Road2> roadList = new List<Road2>();
    private List<Trout2> troutList = new List<Trout2>();
    private List<TroutData> troutDataList = new List<TroutData>();
    private List<int> areaRouteList = new List<int>();       // 区画のルートリスト

    // ダンジョンサイズ
    private int DUNGEON_SIZE = 5;

    void Start()
    {
        playerOwner.Manager = this;
    }

    public void DungeonUpdate()
    {
        playerOwner.DungeonUpdate();
        for (int i = 0; i < enemyList.Count; i++) enemyList[i].DungeonUpdate();
    }

    // ステージのリセット
    public void Reset()
    {
        areaList.Clear();
        roadList.Clear();
        for (int i = 0; i < troutList.Count; i++)
        {
            troutList[i].Reset();
            Destroy(troutList[i].gameObject);
        }
        troutDataList.Clear();
        areaRouteList.Clear();
        troutList.Clear();
        for (int i = 0; i < enemyList.Count; i++)
        {
            Destroy(enemyList[i].gameObject);
        }
        enemyList.Clear();
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }
        itemList.Clear();

        GlobalDataManager.GetGlobalData().DataClear();

        playerOwner.Reset();
        playerOwner.IsVisiblePlayer(false);
    }

    // 復帰
    public void ReCreate()
    {
        List<TroutData> datalist = GlobalDataManager.GetGlobalData().LoadTroutData();
        for (int i = 0; i < datalist.Count; i++)
        {
            Trout2 trout = Instantiate(troutObj, Vector3.zero, Quaternion.Euler(0, 0, 0)) as Trout2;
            trout.ReCreate(datalist[i]);
            trout.name = "MapTrout";
            troutList.Add(trout);
        }
        List<EnemyData> enemylist = GlobalDataManager.GetGlobalData().LoadEnemyDataList();
        Debug.Log("敵を" + enemylist.Count + "体再出現させます。");
        for (int i = 0; i < enemylist.Count; i++)
        {
            EnemyField enemy = Instantiate(enemyObj, enemylist[i].Position, Quaternion.Euler(0, 0, 0)) as EnemyField;
            enemy.Manager = this;
            enemy.Target = playerOwner.gameObject;
            enemy.FirstAreaCheck();
            enemyList.Add(enemy);
        }
        playerOwner.Respawn();
    }

    // ステージの作成
    public void Create(int size)
    {
        Debug.Log("ダンジョンの生成を開始します。");
        GlobalDataManager.GetGlobalData().OnReload();
        // ここ以外で値の変更はしない
        DUNGEON_SIZE = size;

        // 区画の生成
        CreateArea();
        // ルートの生成
        CreateRoute();
        // 道の生成
        CreateRoad();
        // 袋小路の生成
        CreateBlindAlley();

        // ダンジョンデータ生成後マスの生成
        for (int i = 0; i < areaList.Count; i++)
        {
            List<TroutData> trout_list = areaList[i].GetTroutDataList();
            for (int n = 0; n < trout_list.Count; n++)
            {
                troutDataList.Add(trout_list[n]);
            }
        }
        for (int i = 0; i < roadList.Count; i++)
        {
            List<TroutData> trout_list = roadList[i].GetTroutDataList();
            for (int n = 0; n < trout_list.Count; n++)
            {
                troutDataList.Add(trout_list[n]);
            }
        }
        for (int i = 0; i < troutDataList.Count; i++)
        {
            troutDataList[i].ID = i;
            Trout2 trout = Instantiate(troutObj, troutDataList[i].Position, troutDataList[i].Rotation) as Trout2;
            trout.ReCreate(troutDataList[i]);
            trout.name = "MapTrout";
            GlobalDataManager.GetGlobalData().SaveTroutData(troutDataList[i]);
            troutList.Add(trout);
        }

        for(int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].FirstAreaCheck();
        }

        playerOwner.IsVisiblePlayer(true);
        playerOwner.ResetSetUpPlayerCamera();
        playerOwner.FirstAreaCheck();
        Debug.Log("ダンジョンの生成を終了しました。");
    }

    /*
     * 区画の生成
     * 区画の生成時に部屋も生成する
     */
    private void CreateArea()
    {
        Debug.Log("区画の生成を開始します。");
        // スタートする区画のID
        int start_area_id = Random.Range(0, DUNGEON_SIZE * DUNGEON_SIZE) + 1;
        areaRouteList.Add(start_area_id);

        for (int z = 0, id = 0; z < DUNGEON_SIZE; z++)
        {
            for (int x = 0; x < DUNGEON_SIZE; x++, id++)
            {
                // 区画のサイズ分ずれてもらう
                Vector3 slide_position = transform.position + new Vector3(-x * AREA_SIZE, 0, z * AREA_SIZE);
                // 区画の生成
                Area2 area = new Area2(id, DUNGEON_SIZE, slide_position);
                areaList.Add(area);
            }
        }
        Debug.Log("区画の生成を終了しました。");
    }

    // 経路の作成
    private void CreateRoute()
    {
        Debug.Log("ルートの生成を開始します。");
        int loop = 0; //　無限ループ回避用　とりあえず 1000
        int color_index = 0;
        while (areaRouteList.Count < DUNGEON_SIZE * DUNGEON_SIZE)
        {
            int area_index = areaRouteList[areaRouteList.Count - 1] - 1;

            // 次のアリア方向の取得
            List<int> dir_list = SqueezeDir(areaList[area_index].GetNextToAreaList());
            int next_index = 0;

            // 行き止まり処理
            if (dir_list.Count == 0)
            {
                areaList[area_index].RoomCenter();
                for (int i = areaRouteList.Count - 1; i >= 0; i--)
                {
                    int r_area_index = areaRouteList[i] - 1;
                    List<int> r_dir_list = SqueezeDir(areaList[r_area_index].GetNextToAreaList());
                    if (r_dir_list.Count != 0)
                    {
                        int rand = Random.Range(0, r_dir_list.Count);
                        next_index = r_dir_list[rand];
                        areaList[r_area_index].CreateDoor(next_index);
                        areaList[next_index - 1].CreateDoor(r_area_index + 1);
                        areaRouteList.Add(next_index);
                        color_index++;
                        break;
                    }
                }
            }
            else
            {
                int rand = Random.Range(0, dir_list.Count);
                next_index = dir_list[rand];
                areaList[area_index].CreateDoor(next_index);
                areaList[next_index - 1].CreateDoor(area_index + 1);

                areaRouteList.Add(next_index);
                color_index++;
                if (areaRouteList.Count >= DUNGEON_SIZE * DUNGEON_SIZE)
                {
                    EnemyField enemy = Instantiate(enemyObj, areaList[area_index].GetRoomInRandomPosition(), Quaternion.Euler(0, 0, 0)) as EnemyField;
                    enemy.Manager = this;
                    enemy.Target = playerOwner.gameObject;
                    enemyList.Add(enemy);
                    areaList[next_index - 1].RoomCenter();
                }
                else if (areaRouteList[0] - 1 == area_index)
                {
                    playerOwner.transform.position = areaList[areaRouteList[0] - 1].GetRoomInRandomPosition();
                }
                else
                {
                    EnemyField enemy = Instantiate(enemyObj, areaList[area_index].GetRoomInRandomPosition(), Quaternion.Euler(0, 0, 0)) as EnemyField;
                    enemy.Manager = this;
                    enemy.Target = playerOwner.gameObject;
                    enemyList.Add(enemy);
                }
            }
            if (loop++ >= 1000) break;
        }
        Debug.Log("ルートの生成を終了しました。");
    }

    // 次に進むエリアを絞る（一度通った道はなし）
    private List<int> SqueezeDir(List<int> dir_list)
    {
        for (int i = (dir_list.Count - 1); i >= 0; i--)
        {
            for (int n = 0; n < areaRouteList.Count; n++)
            {
                if (areaRouteList[n] == dir_list[i]) { dir_list.RemoveAt(i); break; }
            }
        }
        return dir_list;
    }

    // 経路に沿って部屋をつなぐ道を作成
    private void CreateRoad()
    {
        Debug.Log("道の生成を開始します。");
        for (int i = 0; i < areaRouteList.Count; i++)
        {
            List<int> now_index_list = areaList[areaRouteList[i] - 1].GetNextAreaIDList();
            if (i == 0)
            {
                Door out_door = areaList[areaRouteList[i] - 1].GetDoor(0);
                Door in_door = areaList[areaRouteList[i + 1] - 1].GetDoor(0);
                Road2 road = new Road2(in_door, out_door);
                roadList.Add(road);
            }
            else if (now_index_list.Count >= 2)
            {
                Door out_door = areaList[areaRouteList[i] - 1].GetDoor(1);
                Door in_door = areaList[areaRouteList[i + 1] - 1].GetDoor(0);
                Road2 road = new Road2(in_door, out_door);
                roadList.Add(road);
                if (now_index_list.Count == 3)
                {
                    out_door = areaList[areaRouteList[i] - 1].GetDoor(2);
                    in_door = areaList[now_index_list[2] - 1].GetDoor(0);
                    road = new Road2(in_door, out_door);
                    roadList.Add(road);
                }
            }
        }
        Debug.Log("道の生成を終了しました。");
    }

    // 袋小路を作成
    private void CreateBlindAlley()
    {
        Debug.Log("袋小路の生成を開始します。");
        for (int i = 0; i < areaList.Count; i++)
        {
            Road2 road = areaList[i].CreateBlindAlley();
            if (road != null)
            {
                roadList.Add(road);
            }
        }
        Debug.Log("袋小路の生成を終了しました。");
    }

    /*
     * 敵にあたってシーン遷移
     * プレイヤーと敵の情報を保存
     */
    public void Battle(GameObject enemy)
    {
        Debug.Log("戦闘画面に移行します。");
        for (int i = 0; i < enemyList.Count; i++)
        {
            EnemyField e = enemy.GetComponent<EnemyField>();
            if (enemyList[i] == e)
            {
                enemyList.RemoveAt(i);
                Debug.Log("残りのダンジョン内にいる敵は、 " + enemyList.Count + " 体です。");
            }
        }
        GlobalDataManager.GetGlobalData().SavePlayerData(playerOwner);
        GlobalDataManager.GetGlobalData().SaveEnemyDataList(enemyList);
        gameManager.Battle();
    }

    // プレイヤーが通った道をマップに表示
    public void PlayerPass(Trout2 trout)
    {
        if (trout.GetIsPass()) return;
        string category = trout.GetCategory();
        if (category.Contains("ROAD")) { trout.PlayerPass(); }
        if (category.Contains("ROOM"))
        {
            category = category.Replace("DOOR", "");
            category = category.Replace("CENTER", "");
            for (int i = 0; i < troutList.Count; i++)
            {
                if (troutList[i].GetCategory().Contains(category)) { troutList[i].PlayerPass(); }
            }
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].MapEnemyActive(trout);
            }
        }
    }

    // 敵が移動できる範囲内にあるマスリスト
    public List<Vector3> GetEnemyAreaInTroutList(Trout2 trout)
    {
        List<Vector3> poslist = new List<Vector3>();
        string category = trout.GetCategory();
        category = category.Replace("DOOR", "");
        category = category.Replace("CENTER", "");
        for (int i = 0; i < troutList.Count; i++)
        {
            if (troutList[i].GetCategory().Contains(category)) { poslist.Add(troutList[i].GetPosition()); }
        }
        return poslist;
    }

    // アイテムを出現させる
    public void AppendItem()
    {
        for (int i = 0; i < troutList.Count; i++)
        {
            if (troutList[i].GetCategory().Contains("CENTER"))
            {
                GameObject item = Instantiate(ItemObj, troutList[i].GetPosition(), Quaternion.Euler(0, 0, 0)) as GameObject;
                itemList.Add(item);
            }
        }
    }
}
