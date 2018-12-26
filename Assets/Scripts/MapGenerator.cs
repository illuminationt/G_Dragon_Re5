using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour {

    [SerializeField] private TextAsset[] m_mapData;

    [SerializeField] private GameObject m_dragonWorldKid;
    [SerializeField] private GameObject m_dragonWorldMature;
    [SerializeField] private GameObject[] m_enemiesWorld;
    [SerializeField] private GameObject[] m_bossesWorld;
    [SerializeField] private GameObject[] m_item;

    //ドラゴンの初期位置はここで調整してください
    //後でCSVファイルの中に入れるかも
    [SerializeField] private Vector2[] m_dragonInitPos;

    [NonSerialized]public DragonWorld Dragon = null;
    [NonSerialized] public List<EnemyWorld> EnemyList = new List<EnemyWorld>();
    [NonSerialized] public List<Item> DropItemList = new List<Item>();
    [NonSerialized] public Vector3 MapSize = Vector3.zero;

    [SerializeField] public static float GridLength = 4f;

    //MapRegisterに登録されているマップを生成する
    //MapRegisterの情報はドラゴンが歩いたり敵を倒したりするたびに変わる
    public void MakeMap(int worldID)
    {
        int x = 0, z = 0, zMax = 0;

        List<List<string>> map;
        switch (WorldManager.WorldID)
        {
            case 0:map = MapRegister.Instance.MapLand;break;
            case 1:map = MapRegister.Instance.MapSky;break;
            default:map = null;break;
        }

        foreach(List<string> list in map)
        {
            foreach(string str in list)
            {
                GameObject obj = GetObjectFromData(str);
                if (obj == null)
                {
                    z++;
                    continue;
                }
                //まずActor(ドラゴンと敵)
                DragonWorld dragon = obj.GetComponent<DragonWorld>();
                if (dragon != null)
                {
                    dragon.GridPosition = new Vector3(x, 0, z);
                    dragon.RotY = GOD.Instance.RotY;
                    dragon.transform.eulerAngles = new Vector3(0, GOD.Instance.RotY, 0);

                    dragon.transform.position = dragon.GridPosition * GridLength;
                    Dragon= dragon;
                }

                EnemyWorld enemy = obj.GetComponent<EnemyWorld>();
                if (enemy!= null)
                {
                    enemy.GridPosition = new Vector3(x, 0, z);
                    enemy.transform.position = enemy.GridPosition * GridLength;
                    EnemyList.Add(enemy);
                }

                //次にアイテム
                Item item = obj.GetComponent<Item>();
                if (item != null)
                {
                    item.GridPosition = new Vector3(x, 0, z);
                    item.transform.position = item.GridPosition * GridLength;
                    DropItemList.Add(item);
                }

                z++;
            }
            zMax = z;
            z = 0;
            x++;
        }

        MapSize = new Vector3(x, 0, zMax);
    }
    
    //エクセルに入れた文字によって生成するオブジェクト（敵の種類、ボスの種類)を返します
    private GameObject GetObjectFromData(string datum)
    {
        GameObject obj = null;
        switch (datum)
        {
            case "0":break;
            case "d":
                if (WorldManager.WorldID == 0) { obj = m_dragonWorldKid; }
                else if (WorldManager.WorldID == 1) { obj = m_dragonWorldMature; }
                break;
            case "e0":obj = m_enemiesWorld[0];break;
            case "e1":obj = m_enemiesWorld[1];break;
            case "e2":obj = m_enemiesWorld[2];break;
            case "e3": obj = m_enemiesWorld[3]; break;
            case "e4": obj = m_enemiesWorld[4]; break;
            case "e5": obj = m_enemiesWorld[5]; break;
            case "b0":obj = m_bossesWorld[0];break;
            case "b1":obj = m_bossesWorld[1];break;
            case "i0":obj = m_item[0];break;
            default:
                Debug.LogError("Excelファイルに変な値が書き込まれています");
                break;
        }
        if (obj != null)
        {
            return Instantiate(obj);
        }
        else
        {
            return null;
        }
    }

}
