using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeyItem : Item
{
    public int m_Keys;
    public GameObject Particulas;
    private AudioManager AudioManager;

    private void Start()
    {
        AudioManager = FindObjectOfType<AudioManager>();
    }
    public override void Pick(FFPlayerController Player)
    {
        if (Player.GetKeys() <= 0)
        {
            AudioManager.SeleccionAudio(2,1.5f);
            Player.AddKeys(m_Keys);
            GameController.GetGameController().AddRespawnItemsElement(this);
            gameObject.SetActive(false);
            Particulas.SetActive(false);
        }
    }
    
}
