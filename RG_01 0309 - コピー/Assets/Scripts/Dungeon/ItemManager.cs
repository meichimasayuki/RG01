using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private DungeonManager2 _manager;
    public DungeonManager2 Manager
    {
        get { return _manager; }
        set { _manager = value; }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Contains("Player"))
        {
            GlobalDataManager.GetGlobalData().DataClear();
            Manager.Crear();
        }
    }
}
