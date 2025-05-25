using UnityEngine;
using F = Functions;

public class PauseMenu : MonoBehaviour {
    
    [SerializeField] private Sprite pauseMenuSprite;
    private SpriteRenderer spriteRenderer;

    private PauseController pauseController;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        spriteRenderer.sprite = pauseMenuSprite;

        pauseController = F.GetScriptComponentName<PauseController>("User Interface Object");


        gameObject.SetActive(false);
    }
}
