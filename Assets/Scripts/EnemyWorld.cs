using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWorld :ActorWorld {

    //Enemy0,1,2 , Boss0,1
    //-1はエラーチェック
    public int Category = -1;

    protected override void Start()
    {
        base.Start();
        GetComponent<Animator>().speed = Random.Range(0.9f, 1f);
    }

    public override void GetMove(out int dx, out int dz, out int rotY)
    {
        dx = dz = rotY = 0;
    }
}
