using UnityEngine;
public class CheckPoint : MonoBehaviour
{
    private void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameController.GetGameController().GetPlayer().SetRespawnCheckPoint(transform.position);
        }
    }
}
