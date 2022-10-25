using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController m_GameController = null;
    private FFPlayerController m_Player;
    public float m_PlayerLife = 100f;
    public float m_PlayerShield = 100f;
    int m_PlayerPoints = 0;
    int m_EasyDianaPoints = 10;
    int m_DianaPoints = 20;
    int m_DifficultDiana = 50;
    public int m_PlayerKeys = 0;
    public float m_CurrentAmmo = 30f;
    public float m_CurrentMaxAmmo = 60f;
    public float m_AmmoCapacity = 30f;

    public List<DroneEnemy> m_RespawnObjects = new List<DroneEnemy>();

    private void Start()
    {        
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        Debug.Log(m_PlayerKeys);
    }

    public static GameController GetGameController()
    {
        if (m_GameController == null)
        {
            m_GameController = new GameObject("GameController").AddComponent<GameController>();
            GameControllerData l_GameControllerData = Resources.Load<GameControllerData>("GameControllerData");
            m_GameController.m_PlayerLife = l_GameControllerData.m_Lifes;
            m_GameController.m_PlayerShield = l_GameControllerData.m_PlayerShield;
            m_GameController.m_CurrentAmmo = l_GameControllerData.m_CurrentAmmo;
            m_GameController.m_CurrentMaxAmmo = l_GameControllerData.m_CurrentMaxAmmo;
            m_GameController.m_PlayerKeys = l_GameControllerData.m_Keys;
        }
        return m_GameController;
    }

    public static void DestroySingleton()
    {
        if (m_GameController != null)
            GameObject.Destroy(m_GameController.gameObject);
        m_GameController = null;
    }
    public void AddRespawnElement(DroneEnemy RespawnObject)
    {
        m_RespawnObjects.Add(RespawnObject);
    }

    public void RespawnElements()
    {
        foreach (DroneEnemy l_RespawnObject in m_RespawnObjects)
        {
            l_RespawnObject.Respawn();
        }

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
    public void RestartGame()
    {
        m_Player.RestartGame();
    }

    public void SetPlayerShield(float PlayerShield)
    {
        m_PlayerShield = PlayerShield;
    }
    public float GetPlayerShield()
    {
        return m_PlayerShield;
    }
    public void SetCurrentAmmo(float CurrentAmmo)
    {
        m_CurrentAmmo = CurrentAmmo;
    }
    public float GetCurrentAmmo()
    {
        return m_CurrentAmmo;
    }
    public void SetCurrentMaxAmmo(float CurrentMaxAmmo)
    {
        m_CurrentMaxAmmo = CurrentMaxAmmo;
    }
    public float GetCurrentMaxAmmo()
    {
        return m_CurrentMaxAmmo;
    }

    public void SetAmmoCapacity(float AmmoCapacity)
    {
        m_AmmoCapacity = AmmoCapacity;
    }
    public float GetAmmoCapacity()
    {
        return m_AmmoCapacity;
    }

    public void SetPlayerKeys(int PlayerKeys)
    {
        m_PlayerKeys = PlayerKeys;
    }
    public int GetPlayerKeys()
    {
        return m_PlayerKeys;
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
