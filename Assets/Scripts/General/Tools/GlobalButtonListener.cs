using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalButtonListener : MonoBehaviour {

    private AudioManager audioManager;

    void Start() {
        audioManager = GetComponent<AudioManager>();
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button btn in buttons) {
            btn.onClick.AddListener(() => OnAnyButtonClicked(btn));
        }
    }

    void OnAnyButtonClicked(Button btn) {
        if (audioManager != null) {
            audioManager.PlayEffect("Button Clicked"); 
        }
    }
}