using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [SerializeField] private Transform hpBar;

    private BattleEnemy _target;
    public BattleEnemy Target
    {
        set { _target = value; }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float bar = (float)_target.HP / (float)_target.STATES.HP;
        iTween.ScaleUpdate(hpBar.gameObject, iTween.Hash(
           "y", bar,
           "time", 0.5f)
       );
        Hashtable parameters = new Hashtable();
        parameters.Add("y", (1.0f - bar) * -1);
        parameters.Add("islocal", true);
        parameters.Add("time", 0.5f);
        iTween.MoveUpdate(hpBar.gameObject, parameters);

        Vector3 target = new Vector3(_target.gameObject.transform.position.x, _target.gameObject.transform.localScale.y, _target.gameObject.transform.position.z);
        iTween.MoveUpdate(this.gameObject, iTween.Hash(
           "position", target,
           "time", 0.5f)
       );
    }
}
