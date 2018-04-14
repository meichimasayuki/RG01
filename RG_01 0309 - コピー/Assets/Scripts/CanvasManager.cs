using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : CanvasBase
{
    [SerializeField] private HorizontalLayoutGroup statesLayout;    // HP表示など
    [SerializeField] private StatesUIManager statesPrefab;          // HP表示など

    public void CreateStatesUI(BattleCharacter chara, bool isplayer =false)
    {
        StatesUIManager states = Instantiate(statesPrefab);
        states.CHARACTER = chara;
        if (isplayer) states.PlayerColor();
        else states.EnemyColor();
        states.transform.SetParent(statesLayout.gameObject.transform, false);
    }
}
