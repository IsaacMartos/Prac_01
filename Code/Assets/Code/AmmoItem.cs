﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AmmoItem : Item
{
	public float m_Ammo;
	public override void Pick(FFPlayerController Player)
	{
		if (Player.GetAmmo() < 50)
		{
			Player.AddAmmo(m_Ammo);
			gameObject.SetActive(false);
			//Debug.Log("mas vida");
		}
	}
}