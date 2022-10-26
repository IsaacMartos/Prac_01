using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LifeItem : Item
{
	public float m_Life;
	public GameObject Particulas;
	private AudioManager AudioManager;
	
	private void Start()
	{
		AudioManager = FindObjectOfType<AudioManager>();
	}

	public override void Pick(FFPlayerController Player)
	{
		if (Player.GetLife() < 100.0f)
		{
			AudioManager.SeleccionAudio(6, 1.5f);
			Player.AddLife(m_Life);
            //GameController.GetGameController().AddRespawnItemsElement(this);
            gameObject.SetActive(false);
			Particulas.SetActive(false);
		}
	}

    
}
