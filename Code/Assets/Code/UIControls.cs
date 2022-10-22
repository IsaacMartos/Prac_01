using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{
    public List<Image> M_FadeImage = new List<Image>();

    public float m_Speed = 1.0f;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float alpha = M_FadeImage[0].color.a;

        while(alpha < 1)
        {
            alpha += Time.deltaTime * m_Speed;
            for (int i = 0; i < M_FadeImage.Count; i++)
            {
                M_FadeImage[i].color = new Color(M_FadeImage[i].color.r, M_FadeImage[i].color.g, M_FadeImage[i].color.g, alpha);
            }
            yield return null;
        }
        
        yield return null;
    }

    IEnumerator FadeOut()
    {
        float alpha = M_FadeImage[0].color.a;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * m_Speed;
            for (int i = 0; i < M_FadeImage.Count; i++)
            {
                M_FadeImage[i].color = new Color(M_FadeImage[i].color.r, M_FadeImage[i].color.g, M_FadeImage[i].color.g, alpha);
            }
            yield return null;
        }

        yield return null;
    }
}
