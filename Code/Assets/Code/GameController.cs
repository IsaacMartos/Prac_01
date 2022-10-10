using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    public GameObject m_Player;
    public Text m_CurrentAmmoText;
    float m_PlayerLife;
    float m_PlayerPoints;
    float m_CurrentAmmo;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //m_CurrentAmmo = m_Player.GetComponent<FFPlayerController>().current;
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

    public void ReloadCapacity(float Points)
    {

    }
}
