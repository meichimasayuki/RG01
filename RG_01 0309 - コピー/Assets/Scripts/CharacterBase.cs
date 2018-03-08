using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : AnimationManager
{
    /*
     * 共通データ関連
     */
    private int _state;
    public int State
    {
        get { return _state; }
        set { _state = value; }
    }
}
