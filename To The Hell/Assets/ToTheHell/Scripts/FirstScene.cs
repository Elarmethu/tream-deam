using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class FirstScene : MonoBehaviour
{
    public Image fade;

    public CinemachineDollyCart cameraDolly;
    public CinemachineDollyCart orfeyDolly;

    public GameObject woolfObject;

    private bool animationChanged = false;
    private void Update()
    {
        if(!animationChanged && orfeyDolly.m_Position >= 46.0f)
        {
            animationChanged = true;
            woolfObject.SetActive(true);
        }
    }

    public void NeverSTART() // I very tired...
    {
        StartCoroutine(NeverFadeAway());
    }

    public IEnumerator NeverFadeAway() // P. T. Adamczyk & Olga Jankowska Never Fade Away(SAMURAI Cover) - so cool. Never fade away.... I so lucky lucky so lovely lovely I.
    {
        orfeyDolly.m_Speed = 5.4f;
        cameraDolly.m_Speed = 5.55f;

        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color color = fade.color;
            color.a = f;
            fade.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
