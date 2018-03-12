using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : CharacterBase
{
    /*
     * バトル用マネージャー
     */
    private BattleManager _manager;
    public BattleManager Manager
    {
        get { return _manager; }
        set { _manager = value; }
    }

    /*
     * ステータス
     */
    CharacterStates _states = new CharacterStates();
    public CharacterStates STATES
    {
        get { return _states; }
        set { _states = value; }
    }
    private int _hp;
    private int _mp;
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int MP
    {
        get { return _mp; }
        set { _mp = value; }
    }


    // 倒されているかどうか
    public bool IsDead() { return _hp <= 0; }
}
