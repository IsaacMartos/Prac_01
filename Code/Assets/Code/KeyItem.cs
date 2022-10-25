using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeyItem : Item
{
    public int m_Keys;
    public override void Pick(FFPlayerController Player)
    {
        if (Player.GetKeys() <= 0)
        {
            Player.AddKeys(m_Keys);
            GameController.GetGameController().AddRespawnItemsElement(this);
            gameObject.SetActive(false);
        }
    }
    
}
