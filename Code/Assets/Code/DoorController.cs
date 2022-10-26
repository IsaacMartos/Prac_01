using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorController : MonoBehaviour
{
    public float m_DoorDistance = 2.0f;
    public Animation m_Animation;
    public AnimationClip m_DoorOpen;
    public AnimationClip m_DoorClose;
    public AnimationClip m_DoorClosing;
    public AnimationClip m_DoorOpening;
    bool m_IsClosed = false;

    private AudioManager AudioManager;
    
    void Start()
    {
        SetIdelDoorAnimation();
        AudioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        if (DetectionPlayer())
        {
            
            SetOpenDoorAnamation();
            
        }

        else if (!m_IsClosed)
		{
            SetCloseDoorAnimation();
            //AudioManager.SeleccionAudio(4, 0.5f);
        }
          
        
    }

    bool DetectionPlayer()
	{
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        return Vector3.Distance(l_PlayerPosition,transform.position) <= m_DoorDistance;
	}

    void SetOpenDoorAnamation()
	{
        m_IsClosed = false;
        m_Animation.CrossFade(m_DoorOpening.name, 0.1f);
        m_Animation.CrossFadeQueued(m_DoorOpen.name, 0.0f);
        
        //m_Animation.CrossFadeQueued(m_DoorClosing.name, 0.1f);
        //StartCoroutine(StopDoor());
    }

    void SetCloseDoorAnimation()
	{
        m_IsClosed = true;
        m_Animation.CrossFade(m_DoorClosing.name, 0.1f);
        m_Animation.CrossFadeQueued(m_DoorClose.name, 0.0f);
        //m_Animation.CrossFadeQueued(m_DoorClose.name, 0.1f);
    }

    void SetIdelDoorAnimation()
	{
        m_IsClosed = true;
        m_Animation.CrossFade(m_DoorClose.name, 0.1f);
    }

   
}
