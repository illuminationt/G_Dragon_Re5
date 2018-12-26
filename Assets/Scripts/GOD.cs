using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOD : SingletonMonoBehaviour<GOD>
{
    //主人公ドラゴンの攻撃力
    public int dragonAttackGu;
    public int dragonAttackChoki;
    public int dragonAttackPar;

    //ワールドで接触したEnemyのカテゴリ（int)
    public int MeetEnemyCategory;

    //現在戦っているマス目の座標(GridPosition)
    public int X, Z, RotY;


    //相手が出した手に勝てる手
    public int winHand;
    //自分がスロットを止めたかどうか
    public bool isZpressed = false;
    public bool isXpressed = false;
    public bool isCpressed = false;
    //スロットがそろう&&その手がEnemyに勝利してる
    public bool isWin = false;
    public bool isCheckedHandSlot = false;

    protected override void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        base.Awake();
    }
    

    // Use this for initialization
    void Start () {
		
	}

    //バグで進行不能になったときのためにEscキーでタイトルに戻れるようにした。
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }

}
