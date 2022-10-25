using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LifeItem : Item
{
	public float m_Life;
	public override void Pick(FFPlayerController Player)
	{
		if (Player.GetLife() < 150.0f)
		{
			Player.AddLife(m_Life);
			gameObject.SetActive(false);
			//Debug.Log("mas vida");
		}
	}
}
