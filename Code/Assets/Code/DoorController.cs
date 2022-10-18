using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float m_DoorDistance = 2.0f;
    public Animation m_Animation;
    public AnimationClip m_DoorOpen;
    public AnimationClip m_DoorClose;
    public AnimationClip m_DoorClosing;
    public AnimationClip m_DoorOpening;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        if (DetectionPlayer())
            SetOpenDoorAnamation();
        else
            SetCloseDoorAnimation();
        //Debug.Log(Vector3.Distance(l_PlayerPosition, transform.position));

    }

    bool DetectionPlayer()
	{
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        return Vector3.Distance(l_PlayerPosition,transform.position) <= m_DoorDistance;
	}

    void SetOpenDoorAnamation()
	{
        m_Animation.CrossFade(m_DoorOpening.name, 0.1f);
        m_Animation.CrossFadeQueued(m_DoorClosing.name, 0.1f);
        //StartCoroutine(StopDoor());
    }

    void SetCloseDoorAnimation()
	{
        //m_Animation.CrossFade(m_DoorClosing.name, 0.1f);
        m_Animation.CrossFadeQueued(m_DoorClose.name, 0.1f);
	}

    void SetIdelDoorAnimation()
	{
        m_Animation.CrossFade(m_DoorClose.name, 0.1f);
    }

    //IEnumerator StopDoor()
    //{
    //    yield return new WaitForSeconds(m_DoorOpen.length);
    //    m_Animation.Stop(m_DoorOpening.name);
    //}
}
