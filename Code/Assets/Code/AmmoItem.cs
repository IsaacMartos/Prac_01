using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AmmoItem : Item
{
	public float m_Ammo;
	public GameObject Particulas;
	private AudioManager AudioManager;
	private void Start()
	{
		AudioManager = FindObjectOfType<AudioManager>();
	}
	public override void Pick(FFPlayerController Player)
	{
		if (Player.GetAmmo() < 50.0f)
		{
			AudioManager.SeleccionAudio(5, 1.5f);
			Player.AddAmmo(m_Ammo);
            //GameController.GetGameController().AddRespawnItemsElement(this);
            gameObject.SetActive(false);
			Particulas.SetActive(false);
			//Debug.Log("mas vida");
		}
	}
}
