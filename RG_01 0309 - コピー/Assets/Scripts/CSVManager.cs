using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CharacterStatesCategory;
using EnemyTableCategory;

public class CSVManager
{
    /*
     * キャラクターステータスのデータベースをCSVから読み込む
     */
    public List<List<CharacterStates>> LoadMasterDataCSV ()
    {
        Debug.Log("キャラクターステータスのデータベースの読み込みを開始します。");
        TextAsset csv = Resources.Load("masterdata") as TextAsset;
        StringReader reader = new StringReader(csv.text);
        List<List<CharacterStates>> masterdatalist = new List<List<CharacterStates>>();
        List<CharacterStates> datalist = new List<CharacterStates>();
        int aaa = 0;
        while (reader.Peek() > -1)
        {
            aaa++;
            string line = reader.ReadLine();
            if (line.Contains("LV")) continue;
            string[] values = line.Split(',');

            CharacterStates data = new CharacterStates();
            for (int i = 0; i < values.Length; i++)
            {
                switch ((States.CATEGORY)i)
                {
                    case States.CATEGORY.LV:
                        data.LV = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.HP:
                        data.HP = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.MP:
                        data.MP = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.SP:
                        data.SP = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.ATK:
                        data.ATK = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.DEF:
                        data.DEF = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.INT:
                        data.INT = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.MND:
                        data.MND = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.EXP:
                        data.EXP = int.Parse(values[i]);
                        break;
                    case States.CATEGORY.NONE1:
                    case States.CATEGORY.NONE2:
                    case States.CATEGORY.NONE3:
                    case States.CATEGORY.NONE4:
                    default:
                        break;
                }
            }
            datalist.Add(data);
            if (values[0] == "10")
            {
                masterdatalist.Add(datalist);
                datalist = new List<CharacterStates>();
            }
        }
        Debug.Log("キャラクターステータスのデータベースの読み込みを終了しました。");
        for(int i = 0;i < masterdatalist.Count; i++)
        {
            Debug.Log(i + "種類目のキャラクターデータを読み込みました。");
            for (int n = 0; n < masterdatalist[i].Count; n++)
            {
                CharacterStates data = masterdatalist[i][n];
                Debug.Log("LV : " + data.LV + ", HP : " + data.HP + ", MP : " + data.MP + ", SP : " + data.SP + ", ATK : "
                    + data.ATK + ", DEF : " + data.DEF + ", INT : " + data.INT + ", MND : " + data.MND + ", EXP : " + data.EXP);
            }
        }
        return masterdatalist;
    }

    /*
     * 敵テーブルのデータベースをCSVから読み込む
     */
    public List<EnemyTable> LoadEnemyTableDataCSV()
    {
        Debug.Log("敵テーブルのデータベースの読み込みを開始します。");
        TextAsset csv = Resources.Load("enemytable") as TextAsset;
        StringReader reader = new StringReader(csv.text);
        List<EnemyTable> datalist = new List<EnemyTable>();
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            if (line.Contains("NUM")) continue;
            string[] values = line.Split(',');
            
            EnemyTable data = new EnemyTable();
            for (int i = 0; i < values.Length; i++)
            {
                switch ((Table.CATEGORY)i)
                {
                    case Table.CATEGORY.NUM:
                        data.NUM = int.Parse(values[i]);
                        break;
                    case Table.CATEGORY.MIN_LV:
                        data.MIN_LV = int.Parse(values[i]);
                        break;
                    case Table.CATEGORY.MAX_LV:
                        data.MAX_LV = int.Parse(values[i]);
                        break;
                    case Table.CATEGORY.TYPE:
                        data.TYPE = int.Parse(values[i]);
                        break;
                    default:
                        break;
                }
            }
            Debug.Log("出現数 : " + data.NUM + ", 最低レベル : " + data.MIN_LV + ", 最大レベル : " + data.MAX_LV + ", 種類 : " + data.TYPE);
            datalist.Add(data);
        }
        Debug.Log("敵テーブルのデータベースの読み込みを終了しました。");
        return datalist;
    }
}
