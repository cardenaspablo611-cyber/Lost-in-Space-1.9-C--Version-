using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAnimationScript : MonoBehaviour {

    private float duration = 1f;
    private float elapsedTime;

    public void startAppearAnimation() {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;

        if (color.a == 0f) {
            StartCoroutine(changeAlphaValue("appear"));
        }

    }

    public void startDisappearAnimation() {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;

        if (color.a == 1f) {
            StartCoroutine(changeAlphaValue("disappear"));
        }

    }

    IEnumerator changeAlphaValue(string animation) {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;

        if (animation == "appear") {
            color.a = 0; 
        } else if (animation == "disappear") {
            color.a = 1; 
        }

        spriteRenderer.color = color;

        elapsedTime = 0f;
        float alpha = 0f;

        while (elapsedTime < duration) {
            if (animation == "appear") {
                alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            } else if (animation == "disappear") {
                alpha = Mathf.Lerp(1, 0, elapsedTime / duration);

            }
            color.a = alpha;

            spriteRenderer.color = color;
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        if (animation == "appear") {
            color.a = 1; 
        } else if (animation == "disappear") {
            color.a = 0; 
        }
        spriteRenderer.color = color;
    }   
}
