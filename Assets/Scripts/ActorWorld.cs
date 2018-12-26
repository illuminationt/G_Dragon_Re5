using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract partial class ActorWorld : MonoBehaviour
{
    //マス目を移動する時間
    [SerializeField] public static float MovingDuration=0.8f;

    public bool IsMoving { get; protected set; }

    //補間アニメーションで値が狂うことが予想される。真の値を格納しておき、
    //アニメ後に真の値を代入する。
    protected int m_trueNextRotY;

    //配列上のポジション(yはダミー。コードが分かりやすくなるように。）
    public Vector3 GridPosition;
    
    public float RotY { get { return m_trueNextRotY; } set { m_trueNextRotY = (int)value; } }

    protected virtual void Start()
    {
        IsMoving = false;
        m_actorStateWorld = new StateWorldIdle();
    }

    public virtual void Execute(Vector3 mapSize)
    {


        ActorStateWorld next = m_actorStateWorld.Execute(this,mapSize);
        if (next!=m_actorStateWorld)
        {
            m_actorStateWorld.Exit();
            m_actorStateWorld = next;
            m_actorStateWorld.Enter();
        }




    }

    //ドラゴンと敵の移動はここのdx,dz及びrotYを操作
    public abstract void GetMove(out int dx, out int dz, out int rotY);

    //gridPosition更新
    //ドラゴンの位置更新
    private void restrictMove(Vector3 mapSize,ref int dx, ref int dz)
    {
        Vector3 nextPos = new Vector3(GridPosition.x + dx, 0, GridPosition.z + dz);
        //範囲外には出れない
        if (nextPos.x < 0 || nextPos.z < 0 || nextPos.x >= mapSize.x||nextPos.z>=mapSize.z)
        {
            dx = dz = 0;
            return;
        }

        int worldID = WorldManager.WorldID;

        int x = (int)GridPosition.x;
        int z = (int)GridPosition.z;


        //ここでMapRegisterを書き換えてる
        //移動時は"0"のときときだけ書き換える。
        if (dx != 0 || dz != 0)
        {
                MapRegister.Instance.Rewrite(z, x, "0");  
        }

        GridPosition += new Vector3(dx, 0, dz);
        x += dx;z += dz;

        if (dx != 0 || dz != 0)
        {
            switch (WorldManager.WorldID)
            {
                case 0:
                    if (MapRegister.Instance.MapLand[x][z] == "0")
                    {
                        MapRegister.Instance.Rewrite(z, x, "d");
                    }
                    break;
                case 1:
                    if (MapRegister.Instance.MapSky[x][z] == "0")
                    {
                        MapRegister.Instance.Rewrite(z, x, "d");
                    }
                    break;
            }

        }
      

    }


    //この関数に入るときはm_gridPositionには次に入るべき座標
    //transform.positionには移動前の座標が入っている
    public void smoothMove(int dx, int dz, int rotY)
    {
        if (dx != 0 || dz != 0 || rotY != 0)
        {
            StartCoroutine(stateLerp(dx, dz, rotY));
        }
    }

    IEnumerator stateLerp(int dx, int dz, int rotY)
    {
        IsMoving = true;
        float time = 0f;
        while (time < MovingDuration)
        {
            time += Time.deltaTime;
            float rate = Time.deltaTime / MovingDuration;
            transform.position += new Vector3(dx, 0, dz) * rate * MapGenerator.GridLength;
            transform.Rotate(new Vector3(0, rotY, 0) * rate);

            yield return null;
        }

        transform.position = GridPosition * MapGenerator.GridLength;
        transform.eulerAngles = new Vector3(0, m_trueNextRotY, 0);
        IsMoving = false;
    }

    public void SetPosition()
    {
        transform.position = GridPosition * MapGenerator.GridLength;
    }

    public abstract class ActorStateWorld
    {
        public virtual void Enter() { }
        public abstract ActorStateWorld Execute(ActorWorld actor,Vector3 mapSize);
        public virtual void Exit() { }
    }
    private ActorStateWorld m_actorStateWorld;
}