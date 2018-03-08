using System.Collections.Generic;
using UnityEngine;

public class EnemyTableManager
{
    private readonly int[,] TABLE = new int[5,2] { { 0, 3 }, { 3, 6 }, { 6, 9 }, { 9, 12 }, { 12, 15 } };
    private List<EnemyTable> _masterdata = new List<EnemyTable>();
    public EnemyTable GetEnemyTable(int diff_lv)
    {
        int rand = Random.Range(TABLE[diff_lv, 0], TABLE[diff_lv, 1]);
        return _masterdata[rand];
    }

    public void LoadEnemyTableDataBase()
    {
        CSVManager csvManager = new CSVManager();
        _masterdata = csvManager.LoadEnemyTableDataCSV();
    }
}
