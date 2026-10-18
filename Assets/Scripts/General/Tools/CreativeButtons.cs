using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreativeButtons : MonoBehaviour {

    private void Start() {

        Image image = this.GetComponent<Image>();
        if (image != null && image.sprite != null && image.sprite.texture != null) {
            if (image.sprite.texture.isReadable) {
                try {
                    image.alphaHitTestMinimumThreshold = 0.1f;
                } catch (System.InvalidOperationException e) {
                    Debug.LogWarning($"[CreativeButtons] Could not set alphaHitTestMinimumThreshold: {e.Message}");
                }
            } else {
                Debug.LogWarning($"[CreativeButtons] Texture '{image.sprite.texture.name}' is not readable. Enable Read/Write in import settings for pixel-perfect hit detection.");
            }
        }
    }

}
