using System.Collections.Generic;
using UnityEngine;

public class TCOObjectPool
{
    List<GameObject> m_ObjectsPool;
    int m_CurrentElementID = 0;

    public TCOObjectPool(int ElementsCount, GameObject Element)
    {
        m_ObjectsPool = new List<GameObject>();
        for (int i = 0; i < ElementsCount; i++)
        {
            GameObject l_Element = GameObject.Instantiate(Element);
            l_Element.SetActive(false);
            m_ObjectsPool.Add(l_Element);
        }
        m_CurrentElementID = 0;
    }

    public GameObject GetNextElement()
    {
        GameObject l_Element = m_ObjectsPool[m_CurrentElementID];
        ++m_CurrentElementID;
        if (m_CurrentElementID >= m_ObjectsPool.Count)
            m_CurrentElementID = 0;
        return l_Element;
    }
}
