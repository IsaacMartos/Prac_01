using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController m_GameController = null;
    private FFPlayerController m_Player;
    public float m_PlayerLife = 100f;
    float m_PlayerShield = 100f;

    int m_PlayerPoints = 0;
    int m_EasyDianaPoints = 10;
    int m_DianaPoints = 20;
    int m_DifficultDiana = 50; 

    int m_CurrentAmmo = 30;

    private void Start()
    {        
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        Debug.Log(m_PlayerLife + "Player life");
        //Debug.Log(m_PlayerPoints + "Player points");
        //Debug.Log(m_CurrentAmmo + "Player current ammo");
        //Debug.Log(m_PlayerShield + "Player shield");
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

    public FFPlayerController GetPlayer()
	{
        return m_Player;
	}

    public void SetPlayer(FFPlayerController Player)
	{
        m_Player = Player;
	}

    public void SetPlayerShield(float PlayerShield)
    {
        m_PlayerShield = PlayerShield;
    }
    public float GetPlayerShield()
    {
        return m_PlayerShield;
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

    public int HitEasyDiana()
    {
        m_PlayerPoints += m_EasyDianaPoints;
        return m_PlayerPoints;
    }
    public int HitNormalDiana()
    {
        m_PlayerPoints += m_DianaPoints;
        return m_PlayerPoints;
    }
    public int HitHardDiana()
    {
        m_PlayerPoints += m_DifficultDiana;
        return m_PlayerPoints;
    }




}
