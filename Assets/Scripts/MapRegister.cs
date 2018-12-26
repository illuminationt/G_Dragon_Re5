using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapRegister : SingletonMonoBehaviour<MapRegister> {
    //[0] :land,[1] : sky
    public List<List<string>> MapLand = new List<List<string>>();
    public List<List<string>> MapSky = new List<List<string>>();

    public bool IsMapRegisterd;
    //[0] : land , [1] : sky
    [SerializeField] private TextAsset[] m_mapData;

    
    protected override void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        base.Awake();
    }
    

    private void Start()
    {

        RegisterMap();
    }

    //public変数MapLand及びMapSkyにエクセルファイルを代入
    //string
    private void RegisterMap()
    {
        StringReader stringReader = new StringReader(m_mapData[0].text);


        while (stringReader.Peek() > -1)
        {
            string col = stringReader.ReadLine();
            //","で区切る
            string[] data = col.Split(',');

            List<string> colList = new List<string>();
            foreach (string datum in data)
            {
                colList.Add(datum);
            }
            MapLand.Add(colList);
        }

        stringReader = new StringReader(m_mapData[1].text);

        while (stringReader.Peek() > -1)
        {
            string col = stringReader.ReadLine();
            //","で区切る
            string[] data = col.Split(',');

            List<string> colList = new List<string>();
            foreach (string datum in data)
            {
                colList.Add(datum);
            }
            MapSky.Add(colList);
        }
    }

    //mapIDのデータの座標 z, x をafterStringで書き換える関数
    public void Rewrite(int z,int x,string afterString)
    {
        

        switch (WorldManager.WorldID)
        {
            case 0:
                MapLand[x][z] = afterString;
                break;
            case 1:
                MapSky[x][z] = afterString;
                break;
        }

    }
}
