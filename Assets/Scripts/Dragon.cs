using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Actor
{
    public AudioSource AudioSource;

    protected override void Start()
    {
        base.Start();
        AudioSource = GetComponent<AudioSource>();
    }


    public override void DecideHand()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
          //  Action = Actions.Gu;
            GameObject.Find("Reel_L").GetComponent<ReelController>().Reel_Stop();
            GOD.Instance.isZpressed = true;

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
          //  Action = Actions.Choki;
            GameObject.Find("Reel_C").GetComponent<ReelController>().Reel_Stop();
            GOD.Instance.isXpressed = true;

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
          //  Action = Actions.Par;
            GameObject.Find("Reel_R").GetComponent<ReelController>().Reel_Stop();
            GOD.Instance.isCpressed = true;
        }
        else
        {

        }
        if (GOD.Instance.isZpressed && GOD.Instance.isXpressed && GOD.Instance.isCpressed&&GOD.Instance.isCheckedHandSlot)
        {
            if (GOD.Instance.isWin)
            {
                GOD.Instance.isWin = false;
                switch (GOD.Instance.winHand)
                {
                    case 0:Action = Actions.Gu; break;
                    case 1: Action = Actions.Choki; break;
                    case 2: Action = Actions.Par; break;
                }
            }
            else
            {
                switch (GOD.Instance.winHand)
                {
                    case 0: Action = Actions.Par; break;
                    case 1: Action = Actions.Gu; break;
                    case 2: Action = Actions.Choki; break;
                }
            }

            State = HandState.FINISH_DECIDE;
            GOD.Instance.isZpressed = false;
            GOD.Instance.isXpressed = false;
            GOD.Instance.isCpressed = false;
            GOD.Instance.isCheckedHandSlot = false;
        }
    }


}
