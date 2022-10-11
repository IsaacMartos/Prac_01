using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController m_GameController = null;   
    float m_PlayerLife = 3f;
    int m_PlayerPoints = 0;
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
    public void SetPlayerLifes(float PlayerLife)
    {
        m_PlayerLife = PlayerLife;
    }
    public float GetPlayerLifes()
    {
        return m_PlayerLife;
    }

    public void SetCurrentAmmo(int CurrentAmmo)
    {
        m_CurrentAmmo = CurrentAmmo;
    }
    public int GetCurrentAmmo()
    {
        return m_CurrentAmmo;
    }
    public void SetPoints(int PlayerPoints)
    {
        m_PlayerPoints = PlayerPoints;
    }
    public int GetPoints()
    {
        return m_PlayerPoints;
    }




}
