﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonGameManager : MonoBehaviour
{
    [SerializeField] private DungeonManager2 dungeonManager;      // ダンジョンマネージャー
    [SerializeField] private CameraController mainCamera;         // マップ用カメラ
    [SerializeField] private DungeonMapCamera mapCamera;         // マップ用カメラ
    [SerializeField] private CanvasBase canvasManager;         // マップ用カメラ
    [SerializeField] private Button resetButton;

    /*
     * 難易度別の部屋の数
     */
    private Dictionary<string, int> dungeonSize = new Dictionary<string, int>()
    {
        { "easy",2 },
        { "normal",3 },
        { "hard",4 },
        { "expert",5 },
    };

    private enum DUNGEON_STATE
    {
        DUNGEON = 0,
        BATTLE,
        CLEAR,
    }
    private DUNGEON_STATE dungeonState = DUNGEON_STATE.DUNGEON;

    void Start()
    {
        if (GlobalDataManager.GetGlobalData().IsReload())
        {
            DungeonReCreate();
        }
        else
        {
            string diff = GlobalDataManager.GetGlobalData().GetDifficulty();
            if (diff == null) diff = "easy";
            mapCamera.SetDungeonSize(dungeonSize[diff]);
            dungeonManager.Create(dungeonSize[diff]);
            resetButton.gameObject.SetActive(true);
        }
        StartCoroutine(GameStart());
    }

    // ゲームの準備完了
    private IEnumerator GameStart()
    {
        mainCamera.DungeonUpdate();
        yield return canvasManager.FedeIn();
        StartCoroutine(GameLoop());
    }

    // メインループ
    private IEnumerator GameLoop()
    {
        while (dungeonState == DUNGEON_STATE.DUNGEON)
        {
            yield return new WaitForFixedUpdate();

            dungeonManager.DungeonUpdate();
            mainCamera.DungeonUpdate();
        }
        StartCoroutine(GameFinish());
    }

    // ゲームの終了
    private IEnumerator GameFinish()
    {
        yield return canvasManager.FedeOut();
        if(dungeonState == DUNGEON_STATE.BATTLE) SceneManager.LoadScene("Battle");
        else if (dungeonState == DUNGEON_STATE.CLEAR) SceneManager.LoadScene("Test");
        else SceneManager.LoadScene("Main");
    }

    void Update()
    {

    }

    /*
     * 敵との遭遇
     * バトルシーンに遷移
     */
    public void Battle()
    {
        dungeonState = DUNGEON_STATE.BATTLE;
    }

    /*
     * アイテムの取得
     * メインシーン
     */
    public void Crear()
    {
        dungeonState = DUNGEON_STATE.CLEAR;
    }

    /*
     * ダンジョンの再生成
     * バトルシーンから戻ってくるとき
     */
    public void DungeonReCreate()
    {
        string difficulty = GlobalDataManager.GetGlobalData().GetDifficulty();
        mapCamera.SetDungeonSize(dungeonSize[difficulty]);
        dungeonManager.ReCreate();
        resetButton.gameObject.SetActive(true);
    }

    /*
     * ダンジョンのリセット
     * 現状はボタンから + 難易度選択にもどる
     */
    public void DungeonReset()
    {
        dungeonManager.Reset();
        SceneManager.LoadScene("Test");
    }
}
