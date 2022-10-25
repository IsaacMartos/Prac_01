using UnityEngine;

[CreateAssetMenu(menuName = "Resources/GameControllerData")]
public class GameControllerData : ScriptableObject
{
	public float m_Lifes;
	public float m_PlayerShield;
	public float m_CurrentAmmo;
	public float m_CurrentMaxAmmo;
	public float m_AmmoCapacity;
	public int m_Keys;
	
}
