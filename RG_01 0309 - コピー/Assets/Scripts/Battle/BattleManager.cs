using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EnemyTableCategory;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;           // キャンバスマネージャー
    [SerializeField] private GetCenterPoint centerPoint;            // カメラの位置
    [SerializeField] private BattlePlayer playerPrefab;             // プレイヤーのプレハブ
    [SerializeField] private BattleEnemy enemyPrefab;               // 敵のプレハブ
    [SerializeField] private Text animText;                         // デバッグ用
    [SerializeField] private TargetPointer targetPointerPrefab;     // ターゲットをわかりやすく

    private BattlePlayer player;                                    // プレイヤー
    private List<BattleEnemy> enemyList = new List<BattleEnemy>();  // 敵のリスト
    private List<int> targetList = new List<int>();                 // ターゲットリスト
    private TargetPointer targetPointer;                        // ターゲットをわかりやすく

    private float PLAYER_POSITION = -0.5f;                          // プレイヤーの初期位置
    private enum BATTLE_STATE
    {
        BATTLE = 0,
        WIN,
        LOSE,
    }
    private BATTLE_STATE battleState = BATTLE_STATE.BATTLE;         // バトルの状態
    private int totalEXP = 0;


    /*
     * 初期化
     * プレイヤーと敵を生成
     * カメラの設定
     * 後々テーブルで敵の数などを決めれるようにする
     */
    void Start ()
    {
        // プレイヤーの生成
        player = Instantiate(playerPrefab, new Vector3(PLAYER_POSITION, 0.0f, 0.0f), Quaternion.Euler(0, 90, 0)) as BattlePlayer;
        PlayerStatesData stateData = GlobalDataManager.GetGlobalData().LoadPlayerStatesData();
        if (stateData == null)
        {
            player.STATES = GlobalDataManager.GetGlobalData().GetDataBase().GetPlayerStates(1);
            player.HP = player.STATES.HP;
        }
        else
        {
            player.STATES = GlobalDataManager.GetGlobalData().GetDataBase().GetPlayerStates(stateData.LV);
            player.STATES.HP = stateData.HP;
            player.HP = stateData.HP;
            player.STATES.MP = stateData.MP;
            player.STATES.EXP = stateData.EXP;
        }
        canvasManager.CreateStatesUI(player , true);

        // エネミーの生成
        EnemyTable table = GlobalDataManager.GetGlobalData().GetEnemyTable().GetEnemyTable((int)Table.LEVEL.EASY);
        int enemy_num = table.NUM;
        for (int i = 0; i < enemy_num; i++)
        {
            BattleEnemy enemy = Instantiate(enemyPrefab, new Vector3((i * 0.3f) + 0.3f, 0.0f, 0.0f), Quaternion.Euler(0, 270, 0)) as BattleEnemy;
            enemy.STATES = GlobalDataManager.GetGlobalData().GetDataBase().GetSkeletonStates(table.LEVEL);
            enemy.HP = enemy.STATES.HP;
            enemy.Manager = this;
            enemy.Player = player;
            enemy.ID = i;
            enemyList.Add(enemy);
            //canvasManager.CreateStatesUI(enemy);
            targetList.Add(i);
        }

        player.Manager = this;
        player.Enemy = enemyList[player.TargetID];

        // カメラ位置の設定
        centerPoint.Player = player.transform;
        centerPoint.TargetEnemy = enemyList[player.TargetID].transform;
        StartCoroutine(GameStart());

        // ターゲットポインター
        targetPointer = Instantiate(targetPointerPrefab);
        targetPointer.Target = player.Enemy;
    }

    // ゲームの準備完了
    private IEnumerator GameStart()
    {
        yield return canvasManager.FedeIn();
        StartCoroutine(GameLoop());
    }

    // メインループ
    private IEnumerator GameLoop()
    {
        while (battleState == BATTLE_STATE.BATTLE)
        {
            yield return new WaitForFixedUpdate();
            player.BattleUpdate();
            for(int i = 0; i < enemyList.Count; i++) enemyList[i].BattleUpdate();
        }
        StartCoroutine(GameFinish());
    }

    // ゲームの終了
    private IEnumerator GameFinish()
    {
        yield return canvasManager.FedeOut();
        if (battleState == BATTLE_STATE.WIN) {
            player.STATES.EXP += totalEXP;
            while (true)
            {
                int exp = GlobalDataManager.GetGlobalData().GetDataBase().GetPlayerStates(player.STATES.LV).EXP;
                if (exp <= player.STATES.EXP)
                {
                    player.STATES.LV++;
                    player.STATES.HP = GlobalDataManager.GetGlobalData().GetDataBase().GetPlayerStates(player.STATES.LV).HP;
                    player.STATES.MP = GlobalDataManager.GetGlobalData().GetDataBase().GetPlayerStates(player.STATES.LV).MP;
                    Debug.Log("レベルが" + player.STATES.LV + "に上がりました。");
                }
                else break;
            }
            GlobalDataManager.GetGlobalData().SavePlayerStatesData(player);
        }
        else
        {
            GlobalDataManager.GetGlobalData().DataClear();
        }
        SceneManager.LoadScene("Main");
    }

    /*
     * 撃破したor撃破された
     * 倒れるモーションが終わり次第呼ばれる
     */
    public void DeadPlayer()
    {
        battleState = BATTLE_STATE.LOSE;
    }
    public void DeadEnemy()
    {
        if (targetList.Count == 0) { battleState = BATTLE_STATE.WIN; }
    }

    /*
     * ターゲットしている敵に攻撃
     */
    public void PlayerAttackEnemy(int atk, BattleEnemy enemy)
    {
        enemy.Damage(atk);// とりあえず１
        if (enemy.IsDead())
        {
            totalEXP += enemy.STATES.EXP;
            for (int i = 0; i < targetList.Count; i++)
            {
                if (enemy.ID == targetList[i]) targetList.RemoveAt(i);
            }
            ChangeTargetEnemy();
        }
    }

    /*
     * ターゲットの変更
     */
    public void ChangeTargetEnemy()
    {
        Debug.Log("敵の撃破");
        int target = player.TargetID + 1;
        if(targetList.Count == 0)
        {
            centerPoint.TargetEnemy = player.transform;
            player.Enemy = null;
            targetPointer.gameObject.SetActive(false);
            return;
        }
        bool match = false;
        for (int i = 0;i < targetList.Count; i++)
        {
            if (target == targetList[i])
            {
                player.TargetID = target;
                centerPoint.TargetEnemy = enemyList[target].transform;
                player.Enemy = enemyList[target];
                targetPointer.Target = enemyList[target];
                match = true;
            }
        }
        if (!match)
        {
            player.TargetID = targetList[0];
            centerPoint.TargetEnemy = enemyList[targetList[0]].transform;
            player.Enemy = enemyList[targetList[0]];
        }
    }

    // ボタンを押した
    public void PushDown() { player.PushDown(); }

    // ボタンを離した
    public void PushUp() { player.PushUp(); }

    // デバッグ用
    public void DebugInfomation(string text) { animText.text = text; }
}
