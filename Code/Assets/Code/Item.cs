using System;
using UnityEngine;
public abstract class Item : MonoBehaviour
{
	public abstract void Pick(FFPlayerController Player);

    public void Respawn()
    {
        gameObject.SetActive(true);
    }
}
