using UnityEngine;

public class BattleEnemy : BattleCharacter
{
    enum ENEMT_BATTLE_STATE
    {
        STAY,
        ATTACK,
        DAMAGE,
        DEAD,
    }

    private int _enemyID;
    public int ID
    {
        get { return _enemyID; }
        set { _enemyID = value; }
    }
    private BattlePlayer _player;
    public BattlePlayer Player
    {
        set { _player = value; }
    }

    private bool isActive = true;
    public float rotationTime = 0.3f;
    private float stateTime = 0.0f;
    public float THINKING_TIME = 2.0f;
    private const float WALK_SPEED = 0.0015f;

    private float attackTime = 0.0f;
    private float walkTime = 0.0f;
    

    /*
     * 更新
     * コルーチン管理をしたいので通常のアップデート以外で
     * （とりあえずバトルシーンのみ）
     */
    public void BattleUpdate()
    {
        if (!isActive) return;
        switch (State)
        {
            case (int)ENEMT_BATTLE_STATE.STAY:
                Stay();
                break;
            case (int)ENEMT_BATTLE_STATE.ATTACK:
                Attack();
                break;
            case (int)ENEMT_BATTLE_STATE.DAMAGE:
                KnockBack();
                break;
            case (int)ENEMT_BATTLE_STATE.DEAD:
                Dead();
                break;
            default:
                break;
        }
        if (!IsDead())
        {
            Vector3 pos = _player.transform.position - new Vector3(0.0f, 0.0f, 0.01f); // 回転をカメラ方向にするために Z 軸ずらす
            Vector3 vec = Vector3.Normalize(pos - transform.position);
            iTween.RotateUpdate(this.gameObject, iTween.Hash(
                "rotation", Quaternion.LookRotation(vec).eulerAngles,
                "time", rotationTime)
            );
        }
    }

    private void StateReset()
    {
        stateTime = 0.0f;
        walkTime = 0.0f;
        PlayAnimation("Stay");
        State = (int)ENEMT_BATTLE_STATE.STAY;
    }

    private void Stay()
    {
        stateTime += Time.deltaTime;
        if (stateTime >= THINKING_TIME)
        {
            int rand = Random.Range(0, 10);
            if (rand < 5)
            {
                StateReset();
            }
            else
            {
                attackTime = 0.0f;
                attack_flag = false;
                State = (int)ENEMT_BATTLE_STATE.ATTACK;
            }
        }
    }

    // 攻撃中
    bool attack_flag = false; //　後々削除
    private void Attack()
    {
        if (IsApproach())
        {
            if (attackTime <= 0.0f)
            {
                PlayAnimation("Attack");
            }
            attackTime += Time.deltaTime;

            if (IsPlayingAnimation("AttackEnd") && attack_flag)
            {
                StateReset();
            }
            else
            {
                // ここはアニメ―ション側で呼びたい・・・
                // 現状アニメ―ションをいじれないので力業(Read Only)
                if (!attack_flag)
                {
                    if (attackTime >= 1.0f)
                    {
                        EnemyAttackPlayer();
                        attack_flag = true;
                    }
                }
            }
        }
    }

    // アニメ―ション側から呼んでもらう
    public void EnemyAttackPlayer()
    {
        Vector3 tpos = _player.gameObject.transform.position;
        float dis = ((_player.gameObject.transform.localScale.x / 2) + (transform.localScale.x / 2)) * 0.6f; // IsTrriger使ってもいいような・・・
        if (Vector3.Distance(transform.position, tpos) < dis)
        {
            _player.Damage(STATES.ATK);
        }
    }

    // 攻撃のために敵に近づく
    private bool IsApproach()
    {
        if(attackTime > 0.0f) return true;
        Vector3 tpos = _player.gameObject.transform.position;
        float dis = ((_player.gameObject.transform.localScale.x / 2) + (transform.localScale.x / 2)) * 0.6f; // IsTrriger使ってもいいような・・・
        if (Vector3.Distance(transform.position, tpos) < dis)
        {
            return true;
        }
        else
        {
            if (!IsPlayingAnimation("AttackEnd"))
            {
                if(walkTime == 0.0f) PlayAnimation("Walk");
                walkTime += Time.deltaTime;
                int dir = tpos.x > transform.position.x ? 1 : -1;
                Vector3 move = Vector3.right * dir * WALK_SPEED;
                transform.position += move;
            }
            return false;
        }
    }

    private void KnockBack()
    {
        if (IsEndAnimation())
        {
            StateReset();
        }
    }

    private void Dead()
    {
        if (IsPlayingAnimation("DeadEnd"))
        {
            Manager.DeadEnemy();
            isActive = false;
            gameObject.SetActive(isActive);
        }
    }

    // ダメージ判定
    public void Damage(int atk) {
        STATES.HP -= (atk - STATES.DEF) >= 1 ? (atk - STATES.DEF) : 1;
        if (!IsDead())
        {
            PlayAnimation("Damage");
            State = (int)ENEMT_BATTLE_STATE.DAMAGE;
        }
        else
        {
            PlayAnimation("Dead");
            State = (int)ENEMT_BATTLE_STATE.DEAD;
        }
    }
}
