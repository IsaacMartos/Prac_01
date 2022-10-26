using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShieldItem : Item
{
	public float m_Shield;
	public GameObject Particulas;
	private AudioManager AudioManager;
	private void Start()
	{
		AudioManager = FindObjectOfType<AudioManager>();
	}

	public override void Pick(FFPlayerController Player)
	{
		if (Player.GetShield() < 100.0f)
		{
			AudioManager.SeleccionAudio(7, 1.5f);
			Player.AddShield(m_Shield);
            //GameController.GetGameController().AddRespawnItemsElement(this);
            gameObject.SetActive(false);
			Particulas.SetActive(false);
		}
	}
    
}
