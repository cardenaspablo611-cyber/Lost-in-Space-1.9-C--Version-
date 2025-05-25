using UnityEngine;
using F = Functions;

public class TakeDamage : MonoBehaviour {
    
    private float alpha = 0.8f;
    private SpriteRenderer spriteRenderer; 

    private float cooldownTime = 0.07f;
    private OmegaCooldown cooldown;

    private void Awake() {
    
        spriteRenderer = GetComponent<SpriteRenderer>();

        cooldown = new OmegaCooldown(cooldownTime); 
    }

    private void FixedUpdate() {
        

        if (alpha <= 0) {

            Destroy(gameObject);
        }

        if (cooldown.coolDownOver()) {
            alpha -= 0.035f;
            UpdateAlpha(alpha);
        }

    }

    private void UpdateAlpha(float alphaValue) {

        Color currentColor = spriteRenderer.color;
        currentColor.a = alphaValue;
        spriteRenderer.color = currentColor;
    }
}
