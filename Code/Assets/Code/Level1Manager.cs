using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public bool m_GoingLevel2 = false;
    public UIControls m_UIControls;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameController.GetGameController().GetPlayer().ChangeLevel();
            SceneManager.LoadScene("Level2Scene");
            m_GoingLevel2 = true;
            StartCoroutine(m_UIControls.FadeIn());
        }
        //else
        //{
        //    m_GoingLevel2 = false;
        //}
    }
            

}
