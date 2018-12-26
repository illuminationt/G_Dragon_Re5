using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManager : MonoBehaviour
{


    //バトル始まる前？一応用意した。使わないかも。
    public class StateBeforeBattle : BattleState
    {
        public override void Enter(Dragon dragon, Enemy enemy, BattleMenu menu)
        {

        }
        public override BattleState Execute(Dragon dragon, Enemy enemy,BattleMenu menu)
        {
            BattleState next = this;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //仮
                next = new StateDecideHand();
            }

            return next;
        }
    }

    public class StateAttackOrItem : BattleState
    {
        public override void Enter(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            menu.m_firstMenu.SetActive(true);
        }
        public override BattleState Execute(Dragon dragon, Enemy enemy,BattleMenu menu)
        {
            BattleState next = this;
            bool end=menu.CtrlFirstMenu();

            if (end)
            {
                switch (menu.IconFirstPos)
                {
                    case 0:
                        next = new StateDecideHand();
                        break;
                    case 1:
                        next = new StateOpenInventory();
                        break;
                    default:
                        Debug.LogError("error");
                        break;
                }
            }

            return next;
        }
    }

    //プレイヤーがジャンケンの手を考えている状態。
    public class StateDecideHand : BattleState
    {
        public override void Enter(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            Debug.Log("手を決めてください...G or C or P");
            enemy.DecideHand();
            print(GOD.Instance.winHand);
            GameObject.Find("GameController").GetComponent<GameController>().Play();
        }

        public override BattleState Execute(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            BattleState next = this;

            //両者の手を決める
            dragon.DecideHand();


            //両者ともに手を出し終わった。アニメーション（アクション）へGo
            if (dragon.State == Actor.HandState.FINISH_DECIDE &&
                enemy.State == Actor.HandState.FINISH_DECIDE)
            {
                dragon.State = enemy.State = Actor.HandState.DONT_DECIDE;
                next = new StateAction();
            }



            return next;
        }

        //両者の手が出し終わった。まず勝者の判別
        //死んでるかとかのチェックはアニメーションが終わってから。
        public override void Exit(Dragon dragon, Enemy enemy,BattleMenu menu)
        {
           
        }



    }

    public class StateOpenInventory : BattleState
    {
        public override void Enter(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            Inventory.Instance.Open();
        }

        public override BattleState Execute(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            BattleState next = this;

            Inventory.Instance.ChooseItem();




            return next;
        }
    }

    public class StateAction : BattleState
    {
        //Enterから1フレーム後にExecute
        bool isFirstFrame = true;
        public override void Enter(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            CalculateDamage(ref dragon, ref enemy);
            Debug.Log("Dragon : " + dragon.Action + " Enemy : " + enemy.Action);

            //アニメーション再生！
            Debug.Log("Enter Animation");
            Actor winner = getWinner(dragon, enemy);
            if (winner != null)
            {
                winner.Anim.SetTrigger("Attack");
            }

            //ドラゴンが勝つ（＝攻撃する）ときライオンの咆哮
            if (winner == dragon)
            {
                dragon.AudioSource.Play();
            }
        }

        public override BattleState Execute(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            BattleState next = this;
            if (isFirstFrame)
            {
                isFirstFrame = false;
                return next;
            }

            //ジャンケンの勝者が決まっている (＝あいこではない)
            Actor winner = getWinner(dragon, enemy);
            if (winner != null)
            {
                var nowAnimState = winner.Anim.GetCurrentAnimatorStateInfo(0);

                if (/*nowAnimState.fullPathHash != winner.hashAttack*/
                     !nowAnimState.IsName("Attack"))
                {//攻撃アニメーションが終わったら
                    Debug.Log("Dragon HP = " + dragon.HP + " , Enemy HP = " + enemy.HP);

                    //どちらかが死んでたらリザルト（ここらへんは他のメンバーと要相談
                    if (dragon.HP <= 0 || enemy.HP <= 0||Input.GetKey(KeyCode.P))
                    {
                        return new StateResult();
                    }

                    return new StateDecideHand();
                }
            }
            else
            {
                //あいこだった。即座に次の手を決めよう
                return new StateDecideHand();
            }
            return next;
        }

        //アニメーションが終わったら体力を見て死んでたらリザルトへGO
        public override void Exit(Dragon dragon, Enemy enemy, BattleMenu menu)
        {

        }

        //この中でダメージを計算してしまう
        private void CalculateDamage(ref Dragon dragon, ref Enemy enemy)
        {
            dragon.IsWinner = enemy.IsWinner = false;
            //ジャンケンの勝者判別
            switch (dragon.Action)
            {
                case Actor.Actions.Gu:
                    switch (enemy.Action)
                    {
                        case Actor.Actions.Gu:; break;
                        case Actor.Actions.Choki: dragon.IsWinner = true; enemy.HP -= GOD.Instance.dragonAttackGu; break;
                        case Actor.Actions.Par: enemy.IsWinner = true; dragon.HP -= enemy.AttackPar; break;
                        default: Debug.LogError("やばい"); break;
                    }
                    break;
                case Actor.Actions.Choki:
                    switch (enemy.Action)
                    {
                        case Actor.Actions.Gu: enemy.IsWinner = true; dragon.HP -= enemy.AttackGu; break;
                        case Actor.Actions.Choki: break;
                        case Actor.Actions.Par: dragon.IsWinner = true; enemy.HP -= GOD.Instance.dragonAttackChoki; break;
                        default: Debug.LogError("やばい"); break;
                    }
                    break;
                case Actor.Actions.Par:
                    switch (enemy.Action)
                    {
                        case Actor.Actions.Gu: dragon.IsWinner = true; enemy.HP -= GOD.Instance.dragonAttackPar; break;
                        case Actor.Actions.Choki: enemy.IsWinner = true; dragon.HP -= enemy.AttackChoki; break;
                        case Actor.Actions.Par: break;
                        default: Debug.LogError("やばい"); break;
                    }
                    break;
                default:
                    Debug.LogError("変な手が代入されているよ");
                    break;
            }


            //IsWinnerが両方ともfalse→アイコ
            //RuleBreakerを持ってるならそれでもプレイヤーが勝ったこととする
            if (dragon.IsWinner == false && enemy.IsWinner == false)
            {
                if (Inventory.Instance.inventory.Contains("RuleBreaker"))
                {
                    dragon.IsWinner = true;
                    switch (dragon.Action)
                    {
                        case Actor.Actions.Gu: enemy.HP -= GOD.Instance.dragonAttackGu; break;
                        case Actor.Actions.Choki: enemy.HP -= GOD.Instance.dragonAttackChoki; break;
                        case Actor.Actions.Par: enemy.HP -= GOD.Instance.dragonAttackPar; break;
                        default:
                            Debug.LogError("errore");
                            break;
                    }
                    Inventory.Instance.inventory.Remove("RuleBreaker");
                }
            }
        }

    }

    public class StateResult : BattleState
    {
        public override void Enter(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            Actor winner = getWinner(dragon,enemy);
            int x = GOD.Instance.X, z = GOD.Instance.Z;

            if (Input.GetKey(KeyCode.P))
            {
                winner = dragon;
            }

            //敵が勝ったら現在の座標からxを-1したところに戻る
            if (winner == enemy)
            {
                switch (WorldManager.WorldID)
                {
                    case 0:
                        MapRegister.Instance.MapLand[x][z-1] = "d";
                        GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>().ChangeScene("WorldLand");
                        break;
                    case 1:
                        MapRegister.Instance.MapSky[x][z-1] = "d";
                        GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>().ChangeScene("WorldSky");
                        break;
                }
                return;
            }
            
                switch (enemy.m_category)
                {
                    case 0:
                        GOD.Instance.dragonAttackPar += enemy.m_statusupPar;
                        break;
                    case 1:
                        GOD.Instance.dragonAttackGu += enemy.m_statusupGu;
                        break;
                    case 2:
                        GOD.Instance.dragonAttackChoki += enemy.m_statusupChoki;
                        break;
                }
            
            Debug.Log(winner.name + " win");

            //ザコ敵ならWorldシーンに戻る
            //ボス0ならSkyへ、ボス１ならエンディングムービー
            switch (GOD.Instance.MeetEnemyCategory)
            {
                case 0:
                case 1:
                case 2:

                    switch (WorldManager.WorldID)
                    {
                        case 0:
                            MapRegister.Instance.MapLand[x][z] = "d";

                            GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>().ChangeScene("WorldLand");
                            break;
                        case 1:
                            MapRegister.Instance.MapSky[x][z] = "d";
                            GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>().ChangeScene("WorldSky");
                            break;

                    }

                    break;
                case 10:
                    WorldManager.WorldID = 1;
                    GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>().ChangeScene("EvolveMovie");

                    break;
                case 11:
                    //エンディングムービー
                    Debug.Log("Ending !!");
                    WorldManager.WorldID = 0;
                    GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>().ChangeScene("EndingMovie");
                    break;
            }
        }

        public override BattleState Execute(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            BattleState next = this;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //シーン遷移
                GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>().ChangeScene("World");
            }
            return next;
        }

        public override void Exit(Dragon dragon, Enemy enemy, BattleMenu menu)
        {
            
        }
    }


}
