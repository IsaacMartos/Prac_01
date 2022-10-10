using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    public GameObject m_Player;
    float m_PlayerLife = 3f;
    //float m_PlayerPoints;
    int m_CurrentAmmo = 30;

    private void Start()
    {        
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameController GetGameController()
    {
        if (m_GameController == null)
        {
            m_GameController = new GameObject("GameController").AddComponent<GameController>();
        }
        return m_GameController;
    }

    public static void DestroySingleton()
    {
        if (m_GameController != null)
            GameObject.Destroy(m_GameController.gameObject);
        m_GameController = null;
    }
    public void SetPlayerLife(float PlayerLife)
    {
        m_PlayerLife = PlayerLife;
    }
    public float SetPlayerLife()
    {
        return m_PlayerLife;
    }

    public void SetCurrentAmmo(int CurrentAmmo)
    {
        m_CurrentAmmo = CurrentAmmo;
    }
    public int SetCurrentAmmo()
    {
        return m_CurrentAmmo;
    }


}
