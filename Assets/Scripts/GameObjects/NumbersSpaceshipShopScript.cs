using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersSpaceshipShopScript : MonoBehaviour {

    float duration = 1f;

    public void disappear() {

        StartCoroutine(disappearNumberAnimation());
    }

    public void appear() {

        StartCoroutine(appearNumberAnimation());
    }

    IEnumerator disappearNumberAnimation()  {

        TextMesh textMesh = GetComponent<TextMesh>();
        Color startColor = textMesh.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0.35f); // Target alpha 90 / 255 = 0.35 

        float elapsedTime = 0f;
        while (elapsedTime < duration && textMesh.color.a > 90 / 255) {

            textMesh.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
            
        }

        textMesh.color = targetColor; 
    }

    IEnumerator appearNumberAnimation() {

        TextMesh textMesh = GetComponent<TextMesh>();
        Color startColor = textMesh.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Target alpha 255 / 255 = 1

        float elapsedTime = 0f;
        while (elapsedTime < duration) {

            textMesh.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textMesh.color = targetColor;
    }
    

}
