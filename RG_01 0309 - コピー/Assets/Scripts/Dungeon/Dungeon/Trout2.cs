using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trout2 : DungeonBase
{
    [SerializeField]
    private List<GameObject> troutList = new List<GameObject>();

    private GameObject troutObj;
    private int beforeTroutType;
    private int dirType;

    private int troutID;            // マス管理のためのID
    private bool isPass = false;//false;    // プレイヤーが通ったかのフラグ
    private string troutCategory;   // 部屋・道などのカテゴリ
    private Color saveColor;        // オブジェクトの色

    public int GetTroutID() { return troutID; }
    public bool GetIsPass() { return isPass; }
    public string GetCategory() { return troutCategory; }
    public Color GetColor() { return saveColor; }
    public Vector3 GetPosition() { return transform.position; }
    public Quaternion GetRotation() { return transform.rotation; }

    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = isPass;
    }

    public void PlayerPass()
    {
        isPass = true;
        //gameObject.GetComponent<MeshRenderer>().enabled = true;
        GlobalDataManager.GetGlobalData().UpdateTroutData(troutID);
    }

    public void ReCreate(TroutData data)
    {
        troutID = data.ID;
        transform.position = data.Position;
        isPass = data.IsPass;
        troutCategory = data.Category;
        transform.rotation = data.Rotation;
        troutObj = Instantiate(troutList[data.Type], data.Position, data.Rotation) as GameObject;
        SetColor(data.Color);
        _top = (int)data.Position.z;
        _left = (int)data.Position.x;
    }

    public void SetCategory(string category)
    {
        troutCategory = category;
    }

    public void SetColor(Color color)
    {
        saveColor = color;
        Renderer[] objs = troutObj.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer obj in objs)
        {
            obj.material.color = color;
        }
        color = troutCategory.Contains("DOOR") ? color / 2 : color;
        Renderer[] mapobj = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer obj in mapobj)
        {
            obj.material.color = color;
        }
    }

    public void Reset()
    {
        Destroy(troutObj);
    }
}
