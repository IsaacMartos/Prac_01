using UnityEngine;

public class DoorControllerNivel1 : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_DoorOpen;
    public AnimationClip m_DoorClose;
    public AnimationClip m_DoorClosing;
    public AnimationClip m_DoorOpening;
    public int m_DoorOpenPoints = 100;
    bool m_IsClosed = false;
    void Start()
    {
        SetIdelDoorAnimation();
    }

	void Update()
	{
        if (GameController.GetGameController().GetPoints() >= m_DoorOpenPoints)
        {
            SetOpenDoorAnamation();
        }

        else if (!m_IsClosed)
            SetCloseDoorAnimation();
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
