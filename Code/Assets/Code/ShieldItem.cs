using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShieldItem : Item
{
	public float m_Shield;
	public override void Pick(FFPlayerController Player)
	{
		if (Player.GetShield() < 150.0f)
		{
			Player.AddShield(m_Shield);
			gameObject.SetActive(false);
			//Debug.Log("mas escudo");
		}
	}
}
