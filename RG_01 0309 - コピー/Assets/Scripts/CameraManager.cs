using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float xAspect = 1334.0f;
    public float yAspect = 750.0f;

    void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = calcAspect(xAspect, yAspect);
        camera.rect = rect;
    }
    // アスペクト比計算
    private Rect calcAspect(float width, float height)
    {
        float target_aspect = width / height;                             // 設定したアスペクト比
        float window_aspect = (float)Screen.width / (float)Screen.height; // スクリーンのアスペクト比
        float scale_height = window_aspect / target_aspect;
        Rect rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

        if (1.0f > scale_height)
        {
            rect.x = 0;
            rect.y = (1.0f - scale_height) / 2.0f;
            rect.width = 1.0f;
            rect.height = scale_height;
        }
        else
        {
            float scale_width = 1.0f / scale_height;
            rect.x = (1.0f - scale_width) / 2.0f;
            rect.y = 0.0f;
            rect.width = scale_width;
            rect.height = 1.0f;
        }
        return rect;
    }
}
