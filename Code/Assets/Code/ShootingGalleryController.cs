using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGalleryController : MonoBehaviour
{
    public List<GameObject> m_AnimatedObjects = new List<GameObject>();
    public Text m_Text;
    public GameObject m_PointsText;
    public KeyCode m_StartShootingGalleryKey = KeyCode.Return;
    public KeyCode m_RestartShootingGalleryKey = KeyCode.E;
    public GameObject m_RestartGallery;
    private bool m_StartShootingGallery = false;

    private void Start()
    {
        m_StartShootingGallery = false;
        foreach (GameObject obj in m_AnimatedObjects)
        {
            obj.GetComponent<Animation>().enabled = false;
        }
        m_Text.enabled = false;
        m_PointsText.SetActive(false);
        
    }

    private void Update()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        float actualPoints = GameController.GetGameController().GetPoints();

        if (CheckDistance() && m_StartShootingGallery == false)
        {
            m_Text.enabled = true;
            if (Input.GetKeyDown(m_StartShootingGalleryKey))
            {
                StartCoroutine(StartingShottingGallery());
                m_StartShootingGallery = true;
                m_Text.enabled = false;
                m_PointsText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(m_RestartShootingGalleryKey) && actualPoints >= 200)
        {
            GameController.GetGameController().GetPlayer().RespawnGalleryElements();
            GameController.GetGameController().RestartGamePoints();            
            m_RestartGallery.SetActive(false);
            StartCoroutine(StartingShottingGallery());
            m_StartShootingGallery = true;
            m_Text.enabled = false;
            m_PointsText.SetActive(true);               
            
        }

        
    }

    IEnumerator StartingShottingGallery()
    {
        yield return new WaitForSeconds(0.5f);
        
        foreach (GameObject obj in m_AnimatedObjects)
        {
            obj.GetComponent<Animation>().enabled = true;
        }
        //m_Text.enabled = false;
    }

       

    bool CheckDistance()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        return Vector3.Distance(l_PlayerPosition, transform.position) < 15f;
    }
}
