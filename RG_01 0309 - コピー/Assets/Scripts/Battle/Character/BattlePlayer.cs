using UnityEngine;
using UnityEngine.EventSystems;

public class BattlePlayer : BattleCharacter
{
    enum PLAYER_BATTLE_STATE
    {
        STAY,
        MOVE,
        DIFENSE,
        ATTACK,
        DAMAGE,
        DEAD,
    }

    private int _targetID = 0;
    public int TargetID
    {
        get { return _targetID; }
        set { _targetID = value; }
    }

    private BattleEnemy _enemy;
    public BattleEnemy Enemy
    {
        get { return _enemy; }
        set { _enemy = value; }
    }

    // プレイヤー関連
    private int pCombo = 0;
    private float movePointX = 0;
    private const float SPEED = 0.0065f;
    private Quaternion moveRotation;
    public float rotationTime = 0.3f;
    private float hangTime = 0.0f;          // ダメージモーションができるまで
    private const float HANG_TIME = 1.5f;   // ダメージモーションができるまで


    private float attackTime = 0.0f;
    private float runTime = 0.0f;


    // ボタンの関連
    private bool isPush = false;
    private float pushTime = 0.0f;


    /*
     * 更新
     * コルーチン管理をしたいので通常のアップデートは使わない
     * （とりあえずバトルシーンのみ）
     */
    public void BattleUpdate()
    {
        if (IsDead()) return;
        if (_enemy == null) return;
        if (isPush)
        {
            pushTime += Time.deltaTime;
            if (pushTime >= 0.2f) { Defense(); }
        }
        else
        {
            if (State == 2) State = 0;
            pushTime = 0.0f;
        }
        if (State == 1) { Attack(); }
        else if (State == 3) { Move(); }
        else if (State == 4) { KnockBack(); }

        DebugInfomation();

        iTween.RotateUpdate(this.gameObject, iTween.Hash(
            "rotation", moveRotation.eulerAngles,
            "time", rotationTime)
        );
    }

    void Update()
    {
        // マウスの位置からRayを発射
        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_ANDROID
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
#else
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
#endif
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.name.Contains("Stage"))
                {
                    pCombo = 0;
                    movePointX = hit.point.x;
                    if (State != 3) runTime = 0.0f;
                    State = 3;
                }
            }
        }
    }

    // デバッグ用のインフォメーション
    private void DebugInfomation()
    {
        string info = "Animation:";
        switch (State)
        {
            case 0:
                info += "Stay";
                break;
            case 1:
                info += pCombo == 0 ? "AttackMove" : "Attack" + pCombo.ToString();
                break;
            case 2:
                info += "Defense";
                break;
            case 3:
                info += "Move";
                break;
            case 4:
                info += "Damage";
                break;
            case 5:
                info += "Dead";
                break;
        }
        Manager.DebugInfomation(info);
    }

    // 攻撃中
    bool attack_flag = false; //　後々削除
    private void Attack()
    {
        if (IsApproach())
        {
            if (attackTime <= 0.0f)
            {
                int dir = _enemy.gameObject.transform.position.x > transform.position.x ? 1 : -1;
                Vector3 move = Vector3.right * dir * SPEED;
                moveRotation = Quaternion.LookRotation(move);
                PlayAnimation("Attack");
            }
            attackTime += Time.deltaTime;

            if (IsPlayingAnimation("AttackEnd") && attack_flag)
            {
                pCombo = 0;
                State = 0;
                PlayAnimation("Stay");
            }
            else
            {
                // ここはアニメ―ション側で呼びたい・・・
                // 現状アニメ―ションをいじれないので力業(Read Only)
                if (!attack_flag)
                {
                    if (attackTime >= 0.33f)
                    {
                        PlayerAttackEnemy();
                        attack_flag = true;
                    }
                }
            }
        }
    }

    // アニメ―ション側から呼んでもらう
    public void PlayerAttackEnemy()
    {
        Vector3 tpos = _enemy.gameObject.transform.position;
        float dis = ((_enemy.gameObject.transform.localScale.x / 2) + (transform.localScale.x / 2)) * 1.1f;
        if (Vector3.Distance(transform.position, tpos) < dis)
        {
            Manager.PlayerAttackEnemy(STATES.ATK, _enemy);
        }
    }

    // 攻撃のために敵に近づく
    private bool IsApproach()
    {
        if (attackTime > 0.0f) return true;
        Vector3 tpos = _enemy.gameObject.transform.position;
        float dis = ((_enemy.gameObject.transform.localScale.x / 2) + (transform.localScale.x / 2)) * 1.0f;
        if (Vector3.Distance(transform.position, tpos) < dis)
        {
            if (pCombo == 0) pCombo++;
            return true;
        }
        else
        {
            if (!IsPlayingAnimation("AttackEnd"))
            {
                if(runTime <= 0.0f) PlayAnimation("Run");
                runTime += Time.deltaTime;
                int dir = tpos.x > transform.position.x ? 1 : -1;
                Vector3 move = Vector3.right * dir * SPEED;
                transform.position += move;
                moveRotation = Quaternion.LookRotation(move);
                pCombo = 0;
            }
            return false;
        }
    }

    // 防御中
    private void Defense()
    {   
        State = 2;
        PlayAnimation("Stay");
    }

    // 移動中
    private void Move()
    {
        if(runTime <= 0.0f) PlayAnimation("Run");
        runTime += Time.deltaTime;
        int dir = movePointX > transform.position.x ? 1 : -1;
        Vector3 move = Vector3.right * dir * SPEED;
        transform.position += move;
        moveRotation = Quaternion.LookRotation(move);
        if (Mathf.Abs(movePointX - transform.position.x) <= SPEED)
        {
            State = 0;
            runTime = 0.0f;
            PlayAnimation("Stay");
        }
    }

    // ダメージによるノックバック
    private void KnockBack()
    {
        hangTime += Time.deltaTime;
        if (hangTime >= HANG_TIME)
        {
            PlayAnimation("Stay");
            State = 0;
        }
    }

    // ダメージ判定
    public void Damage(int atk)
    {
        STATES.HP -= (atk - STATES.DEF) >= 1 ? (atk - STATES.DEF) : 1;
        if (!IsDead())
        {
            PlayAnimation("Stay");
            hangTime = 0.0f;
            State = 4;
        }
        else
        {
            State = 5;
            PlayAnimation("Stay");
            DebugInfomation();
            Manager.DeadPlayer();
        }
    }

    // ボタンを押した
    public void PushDown()
    {
        isPush = true;
        pushTime = 0.0f;
    }

    // ボタンを離した
    public void PushUp()
    {
        if (_enemy == null) return;
        isPush = false;
        if (pushTime < 0.2f)
        {
            if (State == 1 || IsPlayingAnimation("Attack")) return;
            if (State != 3) runTime = 0.0f;
             State = 1;
            if (pCombo > 0) pCombo++;
            attackTime = 0.0f;
            attack_flag = false;
        }
        else
        {
            pCombo = 0;
            State = 0;
            PlayAnimation("Stay");
        }
    }
}
