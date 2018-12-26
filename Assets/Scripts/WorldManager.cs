﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : MonoBehaviour
{
    [SerializeField] private MapGenerator m_mapGenerator;

    [SerializeField] private string m_BattleSceneName;
    [SerializeField] private string m_TitleSceneName;
    private SceneSequencer sceneSequencer;

    private bool IsSceneChanged = false;

    public static int WorldID = 0;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        sceneSequencer = GameObject.Find("SceneSequencer").GetComponent<SceneSequencer>();

        m_mapGenerator.MakeMap(WorldID);
        Initialize(m_mapGenerator);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sceneSequencer.ChangeScene(m_BattleSceneName);
        }
        */

        Execute();

        //ドラゴンと敵が同じグリッドに来たらバトルへGO
        if (DragonMeetsEnemy()&&!IsSceneChanged)
        {
            IsSceneChanged = true;
            GOD.Instance.X = (int)m_dragon.GridPosition.x;
            GOD.Instance.Z = (int)m_dragon.GridPosition.z;
            GOD.Instance.RotY = (int)m_dragon.RotY;

            if (GOD.Instance.MeetEnemyCategory == 11)
            {
                sceneSequencer.ChangeScene("BattleSkyBoss");
            }

            sceneSequencer.ChangeScene(m_BattleSceneName);

            // TransitionManager.Instance.transite("Battle", 1f, 1f);

        }

        CheckItem();
    }


    //ここでドラゴン、敵、ボスの座標管理（→シーン遷移）
    private List<EnemyWorld> m_enemies = new List<EnemyWorld>();
    private List<Item> m_dropItem = new List<Item>();
    private DragonWorld m_dragon = null;
    private Vector3 m_mapSize;


    public void Initialize(MapGenerator generator)
    {
        m_dragon = generator.Dragon;
        m_enemies = generator.EnemyList;
        m_mapSize = generator.MapSize;
        m_dropItem = generator.DropItemList;
    }

    private void Execute()
    {
        m_dragon.Execute(m_mapSize);
        foreach (EnemyWorld enemy in m_enemies)
        {
            enemy.Execute(m_mapSize);
        }
    }

    private bool DragonMeetsEnemy()
    {
        Vector3 dragonPos = m_dragon.GridPosition;
        foreach(EnemyWorld enemy in m_enemies)
        {
            if (enemy.GridPosition == dragonPos)
            {
                GOD.Instance.MeetEnemyCategory = enemy.Category;
                return true;
            }

            
        }
        return false;
    }

    private void CheckItem()
    {
        Vector3 dragonPos = m_dragon.GridPosition;
        foreach(Item item in m_dropItem)
        {
            if (item.GridPosition == dragonPos)
            {
                if (item != null)
                {
                    if (!item.IsGotten)
                    {
                        item.IsGotten = true;
                        Inventory.Instance.inventory.Add(item.Name);
                        Destroy(item.gameObject, ActorWorld.MovingDuration);
                    }
                }
             }
        }

    }
}