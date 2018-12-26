using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//バックグランドにアタッチ
public class ItemCountUI : MonoBehaviour {

    [SerializeField]private Text m_starCountText;
    [SerializeField] private GameObject m_starUI;

    private void Start()
    {

    }

    private void Update()
    {
        int count= Inventory.Instance.inventory.Count;


        m_starUI.SetActive(true);

        m_starCountText.text = "x " + count;
        

    }
}
