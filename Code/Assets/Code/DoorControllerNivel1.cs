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
}
