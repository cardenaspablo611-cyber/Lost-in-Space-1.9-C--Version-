using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableEnableButton : MonoBehaviour {

    [SerializeField] private string currentState;

    [SerializeField] private Sprite enableSprite;
    [SerializeField] private Sprite disableSprite;
    [SerializeField] private Sprite mouseOnTopSprite;

    [SerializeField] private GameObject selected;

    private Button buttonSprites;
    private Image buttonImage;

    private AppearAnimationScript animationScript;

    private void Start () {

        buttonSprites = GetComponent<Button>();
        buttonImage = gameObject.GetComponent<Image>();

        animationScript = selected.GetComponent<AppearAnimationScript>();
    }

    private void Update() {

        if (currentState == "enable") {

            buttonImage.sprite = enableSprite;
            buttonSprites.spriteState = new SpriteState {highlightedSprite = mouseOnTopSprite};
            animationScript.startDisappearAnimation();

        } else if (currentState == "disable"){

            buttonImage.sprite = disableSprite;
            buttonSprites.spriteState = new SpriteState {highlightedSprite = null};
            animationScript.startDisappearAnimation();
        
        } else if (currentState == "selected") {

            buttonImage.sprite = disableSprite;
            buttonSprites.spriteState = new SpriteState {highlightedSprite = null};
            animationScript.startAppearAnimation();

        }

    }
    
    public void changeButtonState(string state) {

        currentState = state;
    }
}
