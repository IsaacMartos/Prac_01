using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public UIControls m_UIControls;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameController.GetGameController().GetPlayer().ChangeLevel();
            StartCoroutine(GoingLevel2());
            StartCoroutine(m_UIControls.FadeIn());
        }        
    }

    IEnumerator GoingLevel2()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level2Scene");
    }
            

}
