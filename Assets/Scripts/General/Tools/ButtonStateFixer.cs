using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonStateFixer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [Header("Button Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    
    private Image buttonImage;
    
    private void Start() {
        buttonImage = GetComponent<Image>();
        
        // Set initial sprite
        if (buttonImage != null && normalSprite != null) {
            buttonImage.sprite = normalSprite;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData) {
        if (buttonImage != null && highlightedSprite != null) {
            buttonImage.sprite = highlightedSprite;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        if (buttonImage != null && normalSprite != null) {
            buttonImage.sprite = normalSprite;
        }
    }
} 