using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
	//public GameObject Player;

	private void OnTriggerEnter(Collider other)
	{
		/*if (other.gameObject == Player)
			Player.transform.parent = transform;*/
		if (other.tag == "Player")
			other.transform.SetParent(transform);
	}

	private void OnTriggerExit(Collider other)
	{
		/*if (other.gameObject == Player)
			Player.transform.parent = null;*/
		if (other.tag == "Player")
			other.transform.SetParent(null);
	}


}
