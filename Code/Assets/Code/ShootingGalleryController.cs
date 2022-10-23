using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGalleryController : MonoBehaviour
{
    public List<GameObject> m_AnimatedObjects = new List<GameObject>();
    public Text m_Text;
    public KeyCode m_StartShootingGalleryKey = KeyCode.Return;

    private void Start()
    {
        foreach (GameObject obj in m_AnimatedObjects)
        {
            obj.GetComponent<Animation>().enabled = false;
        }
        m_Text.enabled = false;

    }

    private void Update()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;

        if(Vector3.Distance(l_PlayerPosition,transform.position) < 15f)
        {            
            m_Text.enabled = true;            

            if (Input.GetKeyDown(m_StartShootingGalleryKey))
            {
                StartCoroutine(StartingShottingGallery());
                Destroy(m_Text);
            }
        }
       
    }

    IEnumerator StartingShottingGallery()
    {
        yield return new WaitForSeconds(0.5f);
        
        foreach (GameObject obj in m_AnimatedObjects)
        {
            obj.GetComponent<Animation>().enabled = true;
        }
        m_Text.enabled = false;

    }
}
