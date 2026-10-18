using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonStateFixer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [Header("Button Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    
    private Image buttonImage;
    private bool isHovering = false;
    
    private void Start() {
        buttonImage = GetComponent<Image>();
        ResetToNormal();
    }

    private void OnEnable() {
        ResetToNormal();
        DeselectButton();
    }

    private void OnDisable() {
        isHovering = false;
    }
    
    public void OnPointerEnter(PointerEventData eventData) {
        isHovering = true;
        if (buttonImage != null && highlightedSprite != null) {
            buttonImage.sprite = highlightedSprite;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        isHovering = false;
        ResetToNormal();
    }

    public void OnClick() {
        DeselectButton();
    }

    private void ResetToNormal() {
        if (buttonImage != null && normalSprite != null) {
            buttonImage.sprite = normalSprite;
        }
    }

    private void DeselectButton() {
        if (EventSystem.current != null) {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
} 