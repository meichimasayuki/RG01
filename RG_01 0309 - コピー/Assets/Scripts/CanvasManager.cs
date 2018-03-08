using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
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


    /*
     * フェード関連
     */
    [SerializeField]
    private Image fedePanel;

    // フェードイン
    public IEnumerator FedeIn()
    {
        Image panel = Instantiate(fedePanel);
        panel.transform.SetParent(gameObject.transform, false);
        FeadManager fede = panel.GetComponent<FeadManager>();
        yield return fede.FedeIn();
        Destroy(panel.gameObject);      // フェードイン時はオブジェクト削除

        yield return new WaitForFixedUpdate();
    }

    // フェードアウト
    public IEnumerator FedeOut()
    {
        Image panel = Instantiate(fedePanel);
        panel.transform.SetParent(gameObject.transform, false);
        FeadManager fede = panel.GetComponent<FeadManager>();
        yield return fede.FedeOut();
    }
}
