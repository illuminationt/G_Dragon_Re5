using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum State
{
    Start,
    Playing,
    Stay,
}

public class GameController : MonoBehaviour
{
    public Button[] stopbt;
    public Button playbt;

    public GameObject[] reels;
    ReelController[] rc = new ReelController[3];

    int[] lineL, lineC, lineR;
    int stopline_len = 3;
    State state = State.Start;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            rc[i] = reels[i].GetComponent<ReelController>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (stopline_len == 3 && state == State.Playing)
        {
            state = State.Stay;
            Chack();
            playbt.interactable = true;
        }
    }

    public void Play()
    { //Playボタンを押した時の動作
        playbt.interactable = false;
        stopline_len = 0;
        state = State.Playing;
        for (int i = 0; i < 3; i++)
        {
            rc[i].Reel_Move();
            stopbt[i].interactable = true;
        }

    }

    public void Stopbt_f(int id)
    {
        stopbt[id].interactable = false;
    }


    public void SetLineL(int[] line)
    {
        lineL = new int[3];
        lineL = line;
        stopline_len++;
    }
    public void SetLineC(int[] line)
    {
        lineC = new int[3];
        lineC = line;
        stopline_len++;
    }
    public void SetLineR(int[] line)
    {
        lineR = new int[3];
        lineR = line;
        stopline_len++;
    }



    public void Chack()
    {
        GOD.Instance.isCheckedHandSlot = true;
       // GOD.Instance.isWin = false;
        for (int i = 1; i < 2; i++)
        {
            if (lineL[i] == lineC[i] && lineC[i] == lineR[i] && lineC[i] == GOD.Instance.winHand)
            {
                switch (i)
                {
                    case 0:
                        Debug.Log("一番下のラインが揃ったよ。");
                        break;
                    case 1:
                        Debug.Log("真ん中のラインが揃ったよ。");
                        GOD.Instance.isWin = true;

                        break;
                    case 2:
                        Debug.Log("一番上のラインが揃ったよ。");
                        break;
                    default:
                        Debug.Log("設定ミスでは???");
                        break;
                }
            }
        }

        /*if (lineL[0] == lineC[1] && lineC[1] == lineR[2])
        {
            Debug.Log("ラインの左下がりが揃ったよ。");
        }
        if (lineL[2] == lineC[1] && lineC[1] == lineR[0])
        {
            Debug.Log("ラインの右上がりが揃ったよ。");
        }*/
    }
}