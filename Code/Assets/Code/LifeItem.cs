using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LifeItem : Item
{
	public float m_Life;
	public GameObject Particulas;
	public override void Pick(FFPlayerController Player)
	{
		if (Player.GetLife() < 150.0f)
		{
			Player.AddLife(m_Life);
            //GameController.GetGameController().AddRespawnItemsElement(this);
            gameObject.SetActive(false);
			Particulas.SetActive(false);
		}
	}

    
}
