using UnityEngine;
using System.Collections;

public class ShowMenuOnHover : MonoBehaviour {

    [SerializeField] private  GameObject contextMenu;
    private float fadeDuration = 0.25f; 
    private SpriteRenderer spriteRenderer;
    private float currentFadeTime;

    private bool fadeIn = true;
    private bool fadeOut = false;

    private bool isMouseOverMenu;

    void Start() {
        spriteRenderer = contextMenu.GetComponent<SpriteRenderer>();
        SetAlpha(0f);
    }

    void Update() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        isMouseOverMenu = false;

        if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject == contextMenu)) {
            isMouseOverMenu = true;

            if (fadeIn) {
                StartCoroutine(FadeIn());
            }
            
        } else if (!isMouseOverMenu){

            if (!isMouseOverMenu && fadeOut) {
            StartCoroutine(FadeOut());
            }
        }
        
    }

    private IEnumerator FadeIn() {

        fadeIn = false;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {

            elapsedTime += Time.deltaTime;
            float progress = Mathf.Pow(elapsedTime / fadeDuration, 2);
            SetAlpha(Mathf.Clamp01(progress));
            yield return null;
        }
        SetAlpha(1f);
        fadeOut = true;
        changeChildren(true);
    }

    private IEnumerator FadeOut() {

        fadeOut = false;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {

            elapsedTime += Time.deltaTime;
            float progress = Mathf.Pow(elapsedTime / fadeDuration, 2);
            SetAlpha(Mathf.Clamp01(1f - progress));
            yield return null;
        }
        SetAlpha(0f);
        fadeIn = true;
        changeChildren(false);
    }

    private void SetAlpha(float alpha) {

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void changeChildren(bool state) {
        
        foreach (Transform child in transform) {
            child.gameObject.SetActive(state);
        }
    }
}

