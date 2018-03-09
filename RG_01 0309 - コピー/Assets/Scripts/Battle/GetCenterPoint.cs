using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCenterPoint : MonoBehaviour
{
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TargetPointer targetPointerPrefab; // ターゲットをわかりやすく

    private TargetPointer targetPointer;                        // ターゲットをわかりやすく
    public float margin = 0.2f;                                 // 半径を少し余分にとるための値
    private const float MAIN_HEIGHT = 0.1f;                     // カメラの高さ
    private const float POINT_HEIGHT = 0.1f;                    // カメラポイントの高さ

    private Transform _player;
    private Transform _targetEnemy;
    public Transform Player
    {
        set { _player = value; }
    }
    public Transform TargetEnemy
    {
        set { _targetEnemy = value; }
    }

    void Start()
    {
        targetPointer = Instantiate(targetPointerPrefab);
    }

    void LateUpdate()
    {
        // ターゲットポインターの位置
        if (_targetEnemy.position == _player.position) targetPointer.gameObject.SetActive(false);
        else
        {
            targetPointer.gameObject.SetActive(true);
            targetPointer.Target = _targetEnemy;
        }


        // カメラ位置
        Vector3 center = (_player.position + _targetEnemy.position) / 2;
        transform.position = new Vector3(center.x, center.y + MAIN_HEIGHT, center.z);
        float radius = Vector3.Distance(center, _player.position);
        float distance = (radius + margin) / Mathf.Sin(mainCamera.fieldOfView * Mathf.Deg2Rad); // カメラの距離を算出
        cameraPoint.localPosition = new Vector3(0, POINT_HEIGHT, -distance);                    // カメラポイントをカメラの距離をもとに配置
        cameraPoint.LookAt(transform);
    }
}