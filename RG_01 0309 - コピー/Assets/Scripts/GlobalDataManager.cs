using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalDataManager : MonoBehaviour
{
    private CharacterDataBase dataBase;
    public CharacterDataBase GetDataBase() { return dataBase; }

    private EnemyTableManager enemyTable;
    public EnemyTableManager GetEnemyTable() { return enemyTable; }

    private static GlobalDataManager globalData = null;
    public static GlobalDataManager GetGlobalData() { return globalData; }

    [SerializeField] private GameObject debugLogPrefab;
    private GameObject debugLogObject;

    // ダンジョンの難易度
    private string difficultyData = null;
    public string GetDifficulty() { return difficultyData; }
    public void SetDifficulty(string diff) { difficultyData = diff; }


    // ステージ(マス)のデータ
    private int troutCounter = 0;
    public int GetCounter() { return troutCounter; }
    public void AddCounter() { troutCounter++; }
    List<TroutData> troutDataList = new List<TroutData>();
    public void SaveTroutData(TroutData data)
    {
        troutDataList.Add(data);
    }
    public List<TroutData> LoadTroutData() { return troutDataList; }
    public void UpdateTroutData(int id)
    {
        troutDataList[id].IsPass = true;
    }

    // プレイヤーのデータ
    private PlayerStatesData playerStatesData = null;
    public void SavePlayerStatesData(BattlePlayer player)
    {
        PlayerStatesData data = new PlayerStatesData
        {
            LV = player.STATES.LV,
            HP = player.STATES.HP,
            MP = player.STATES.MP,
            EXP = player.STATES.EXP,
        };
        playerStatesData = data;
    }
    public PlayerStatesData LoadPlayerStatesData() { return playerStatesData; }
    private PlayerData playerData = null;
    public void SavePlayerData(Player player)
    {
        PlayerData data = new PlayerData
        {
            Position = player.gameObject.transform.position,
            Rotation = player.gameObject.transform.rotation,
        };
        playerData = data;
    }
    public PlayerData LoadPlayerData() { return playerData; }

    // 敵の情報
    private List<EnemyData> enemyDataList = new List<EnemyData>();
    public void SaveEnemyDataList(List<EnemyField> enemylist)
    {
        enemyDataList.Clear();
        for (int i = 0; i < enemylist.Count; i++)
        {
            EnemyData data = new EnemyData
            {
                Position = enemylist[i].gameObject.transform.position,
            };
            enemyDataList.Add(data);
        }
    }
    public List<EnemyData> LoadEnemyDataList() { return enemyDataList; }


    // リロードするためのフラグ
    private bool _reload = false;
    public bool IsReload() { return _reload; }
    public void OnReload() { _reload = true; }

    // データの削除
    public void DataClear()
    {
        _reload = false;
        troutDataList.Clear();
        playerData = null;
        enemyDataList.Clear();
        troutCounter = 0;
    }

    void Awake()
    {
        if (globalData == null)
        {
            globalData = this;
            dataBase = new CharacterDataBase();
            dataBase.LoadCharacterDataBase();
            enemyTable = new EnemyTableManager();
            enemyTable.LoadEnemyTableDataBase();
            DontDestroyOnLoad(gameObject);
            debugLogObject = Instantiate(debugLogPrefab);
            DontDestroyOnLoad(debugLogObject);
        }
        troutCounter = 0;
    }
}
