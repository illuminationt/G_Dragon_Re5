using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandUI : MonoBehaviour {

    [SerializeField] private GameObject m_GuUI;
    [SerializeField] private GameObject m_ChokiUI;
    [SerializeField] private GameObject m_ParUI;

    private void Update()
    {

        switch (GOD.Instance.winHand)
        {
            case 0:
                m_GuUI.SetActive(false);
                m_ChokiUI.SetActive(true);
                m_ParUI.SetActive(false);
                break;
            case 1:
                m_GuUI.SetActive(false);
                m_ChokiUI.SetActive(false);
                m_ParUI.SetActive(true);
                break;
            case 2:
                m_GuUI.SetActive(true);
                m_ChokiUI.SetActive(false);
                m_ParUI.SetActive(false);
                break;
        }
    }
}
