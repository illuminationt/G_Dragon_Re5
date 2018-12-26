using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{

    public int m_statusupGu;
    public int m_statusupChoki;
    public int m_statusupPar;
    public int m_category;

    public override void DecideHand()
    {
        if (State != HandState.DONT_DECIDE)
        {
            return;
        }

        int hand = Random.Range(0, 3);

        switch (hand)
        {
            case 0:
                Action = Actions.Gu;
                GOD.Instance.winHand = 2;
                break;
            case 1:
                Action = Actions.Choki;
                GOD.Instance.winHand = 0;
                break;
            case 2:
                Action = Actions.Par;
                GOD.Instance.winHand = 1;
                break;
        }
        //仮
        //Action = Actions.Gu;
        State = HandState.FINISH_DECIDE;
    }

    //対戦相手を引数に入れて必殺技をドーン
    public virtual void Special(ref Dragon dragon)
    {
        //仮
        dragon.HP -= 50;
    }

}
