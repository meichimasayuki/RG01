using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public void Finish()
    {
        Application.Quit();
    }

    private int SS_index = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // スクリーンショット保存
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Resources/screenshot_" + SS_index++ + ".jpg");
            Debug.Log("スクリーンショット");
        }
    }
}
