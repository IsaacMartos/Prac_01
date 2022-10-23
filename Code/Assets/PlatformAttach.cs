using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{	

	public void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(transform);
        }
    }
	/*if (other.gameObject == Player)
		Player.transform.parent = transform;*/
	/*if (other.transform.tag == "Player")
		other.transform.SetParent(transform);*/


	private void OnTriggerExit(Collider other)
	{
        /*if (other.gameObject == Player)
			Player.transform.parent = null;*/
        /*if (other.transform.tag == "Player")
			other.transform.SetParent(null);*/
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(null);
        }
    }


}
