/***************************************************************
*file: FadeEffect.cs
*author: Xavier Felix & Marie Philavong
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/23/24
*
*purpose: Applies a FadeIn and FadeOut effect 
*         
****************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Image fadeImage; // image object
    public float fadeDuration = 1f; // duration of the effect

    private void Start()
    {
        // Start with a fadin
        StartCoroutine(FadeIn());
    }

    // function: FadeIn
    // purpose: Fade player's screen from black to visible
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeAmount = 1 - (elapsedTime / fadeDuration); // Fade from blaack to fully visible
            SetFade(fadeAmount);
            yield return null;
        }
    }

    // function: FadeOut
    // purpose: Fade player's screen from visible to black
    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeAmount = elapsedTime / fadeDuration; // Fade from fully visible to black
            SetFade(fadeAmount); 
            yield return null;
        }
    }

    // function: SetFade
    // purpose: Set the Fade amount in the Image object
    private void SetFade(float fadeAmount)
    {
        Color color = fadeImage.color; // current image color
        color.a = fadeAmount; // update color alpha with fadeAmount
        fadeImage.color = color; // set the color alpha back to the image obj
    }
}
