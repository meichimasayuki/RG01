using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapCamera : DungeomCamera
{
    [SerializeField]
    private Camera mapCamera = null;

    private const float REDUCING_WIDTH = 0.24f;     // 縮小時の横幅
    private const float EXPANSION_WIDTH = 1.0f;     // 拡大時の横幅
    private const float REDUCING_HEIGHT = 0.35f;    // 縮小時の縦幅
    private const float EXPANSION_HEIGHT = 1.0f;    // 拡大時の縦幅
    private const float REDUCING_X = 0.76f;         // 縮小時のX軸位置
    private const float EXPANSION_X = 0.0f;         // 拡大時のX軸位置
    private const float REDUCING_Y = 0.65f;         // 縮小時のY軸位置
    private const float EXPANSION_Y = 0.0f;         // 拡大時のY軸位置
    private const float REDUCING_SIZE = 4;          // 縮小時の描画サイズ
    private float EXPANSION_SIZE = 12;              // 拡大時の描画サイズ(ダンジョンの大きさによって変えるためコンスト×)
    private const float EXPANSION_RATIO = 1.5f;     // 拡大比率
    private const float SPEED = 0.2f;               // 拡大縮小速度
    
    enum STATE
    {
        NONE,
        REDUCING,
        EXPANSION,
    }
    private STATE expansionState = STATE.NONE;      // 拡大状態
    private bool expansionMap = false;              // 拡大フラグ
    private float expansionTime = 0.0f;             // 拡大時間

    private float _width = REDUCING_WIDTH;
    private float _height = REDUCING_HEIGHT;
    private float _x = REDUCING_X;
    private float _y = REDUCING_Y;
    private float _size = REDUCING_SIZE;

    void LateUpdate()
    {
        if (expansionState != STATE.NONE) {
            expansionTime += Time.deltaTime;
            float ratio = Mathf.Lerp(0, 1, expansionTime / SPEED);
            if (expansionState == STATE.REDUCING)
            {
                _x = Mathf.Lerp(EXPANSION_X, REDUCING_X, ratio);
                _y = Mathf.Lerp(EXPANSION_Y, REDUCING_Y, ratio);
                _width = Mathf.Lerp(EXPANSION_WIDTH, REDUCING_WIDTH, ratio);
                _height = Mathf.Lerp(EXPANSION_HEIGHT, REDUCING_HEIGHT, ratio);
                _size = Mathf.Lerp(EXPANSION_SIZE, REDUCING_SIZE, ratio);
            }
            if (expansionState == STATE.EXPANSION)
            {
                _x = Mathf.Lerp(REDUCING_X, EXPANSION_X, ratio);
                _y = Mathf.Lerp(REDUCING_Y, EXPANSION_Y, ratio);
                _width = Mathf.Lerp(REDUCING_WIDTH, EXPANSION_WIDTH, ratio);
                _height = Mathf.Lerp(REDUCING_HEIGHT, EXPANSION_HEIGHT, ratio);
                _size = Mathf.Lerp(REDUCING_SIZE, EXPANSION_SIZE, ratio);
            }
            mapCamera.rect = new Rect(_x, _y, _width, _height);
            mapCamera.orthographicSize = _size;
            if (ratio == 1) { expansionState = STATE.NONE; }
        }
    }

    public void Test()
    {
        if (expansionState != STATE.NONE) return;
        if (expansionMap) {
            expansionState = STATE.REDUCING;
        }
        else {
            expansionState = STATE.EXPANSION;
            //mapCamera.cullingMask |= (1 << LayerMask.NameToLayer("Field"));// Fieldレイヤーを表示する
        }
        expansionMap = !expansionMap;
        expansionTime = 0.0f;
        Target.gameObject.GetComponent<MapCameraPosition>().Expension();
    }

    // ダンジョン生成時に呼ばれる
    public void SetDungeonSize(int size)
    {
        EXPANSION_SIZE = size * REDUCING_SIZE * EXPANSION_RATIO;
        Target.gameObject.GetComponent<MapCameraPosition>().SetDungeonSize(size);
    }
}
