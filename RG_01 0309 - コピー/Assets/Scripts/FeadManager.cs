using System.Collections;
using UnityEngine;

public class FeadManager : AnimationManager
{
    private bool isFede;  // フェード中

    // フェードイン
    public IEnumerator FedeIn()
    {
        isFede = true;
        PlayAnimation("FedeIn");
        while (isFede) { yield return new WaitForFixedUpdate(); }
        yield return new WaitForFixedUpdate();
    }

    // フェードアウト
    public IEnumerator FedeOut()
    {
        isFede = true;
        PlayAnimation("FedeOut");
        while (isFede) { yield return new WaitForFixedUpdate(); }
        yield return new WaitForFixedUpdate();
    }

    /*
     * アニメーション終了
     * アニメーション側から呼ばれる
     */
    public void FedeFinish() { isFede = false; }
}
