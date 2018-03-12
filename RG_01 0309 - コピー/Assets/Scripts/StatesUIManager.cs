using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatesUIManager : MonoBehaviour
{
    [SerializeField] private Text lvpText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text mpText;

    private BattleCharacter _character = null;
    public BattleCharacter CHARACTER
    {
        set { _character = value; }
    }

    void Start ()
    {
	}

    void Update()
    {
        if (_character != null)
        {
            lvpText.text = "LV." + _character.STATES.LV.ToString();
            hpText.text = "HP : " + _character.HP.ToString();
            mpText.text = "MP : " + _character.STATES.MP.ToString();
        }
    }

    public void PlayerColor()
    {
        gameObject.GetComponent<Image>().color = Color.yellow;
    }
    public void EnemyColor()
    {
        gameObject.GetComponent<Image>().color = Color.grey;
    }
}
