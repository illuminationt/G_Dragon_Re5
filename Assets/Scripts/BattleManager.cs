using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManager : MonoBehaviour
{
    private Dragon m_dragon;
     private Enemy m_enemy;
    [SerializeField] private BattleMenu m_menu;


    [SerializeField] private GameObject m_enemyGu;
    [SerializeField] private GameObject m_enemyChoki;
    [SerializeField] private GameObject m_enemyPar;
    [SerializeField] private GameObject m_boss0;
    [SerializeField] private GameObject m_boss1;

    [SerializeField] private GameObject m_dragonKid;
    [SerializeField] private GameObject m_dragonMature;

    [SerializeField] private Vector3 m_enemyGeneratePos = new Vector3(0, 0, 8);
    [SerializeField] private Vector3 m_dragonGeneratePos = Vector3.zero;

    public SceneSequencer Sequencer;

    private void Awake()
    {
        m_battleState = new StateBeforeBattle();
        Sequencer = GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>();

        //敵のカテゴリをGODから受け取りそれにあったENemyを生成
        GameObject obj = null;
        switch (GOD.Instance.MeetEnemyCategory)
        {
            case 0: obj = m_enemyGu; break;
            case 1: obj = m_enemyChoki; break;
            case 2: obj = m_enemyPar; break;
            case 10: obj = m_boss0; break;
            case 11: obj = m_boss1; break;
            default:
                Debug.LogError("EnemyWorldのCategoryに変な値が代入されている");
                break;
        }

        GameObject enemy = Instantiate(obj, m_enemyGeneratePos, Quaternion.Euler(new Vector3(0f,180f,0f)));
        m_enemy = enemy.GetComponent<Enemy>();

        //ドラゴンの生成（LandかSkyで作るドラゴンが変わる)
        GameObject dragon = null;
        switch (WorldManager.WorldID)
        {
            case 0: dragon = m_dragonKid; break;
            case 1: dragon = m_dragonMature; break;
            default:
                Debug.LogError("error");
                break;
        }

        GameObject dra = Instantiate(dragon, m_dragonGeneratePos, Quaternion.identity);
        m_dragon = dra.GetComponent<Dragon>();
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        BattleState next = m_battleState.Execute(m_dragon, m_enemy,m_menu);
        if (next != m_battleState)
        {
            m_battleState.Exit(m_dragon, m_enemy,m_menu);
            m_battleState = next;
            m_battleState.Enter(m_dragon, m_enemy,m_menu);
        }
    }






    public abstract class BattleState
    {
        //BattleManagerが保持してる、現在戦っているDragonとEnemyを渡す
        public virtual void Enter(Dragon dragon, Enemy enemy, BattleMenu menu) { }
        public abstract BattleState Execute(Dragon dragon, Enemy enemy,BattleMenu menu);
        public virtual void Exit(Dragon dragon, Enemy enemy, BattleMenu menu) { }


        protected Actor getWinner(Dragon dragon, Enemy enemy)
        {
            if (dragon.IsWinner)
            {
                return dragon;
            }
            else if (enemy.IsWinner)
            {
                return enemy;
            }
            else
            {
                return null;
            }
        }

    }
    private BattleState m_battleState;
}
