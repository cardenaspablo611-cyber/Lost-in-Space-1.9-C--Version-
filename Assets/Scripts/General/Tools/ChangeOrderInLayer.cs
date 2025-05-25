using UnityEngine;
using UnityEngine.UI;

public class ChangeOrderInLayer : MonoBehaviour
{
    [SerializeField] public string sortingLayerName = "Default";
    [SerializeField] public int orderInLayer;

    private void Start() {

        SetSortingLayerAndOrder();
    }

    private void SetSortingLayerAndOrder() {
        
        TextMesh textMesh = GetComponent<TextMesh>();

        if (textMesh != null) {
            
            textMesh.GetComponent<Renderer>().sortingLayerName = sortingLayerName;
            textMesh.GetComponent<Renderer>().sortingOrder = orderInLayer;
        } else {
            Debug.LogError("ERROR - you wrote these | TextMesh component not found on the GameObject.");
        }
    }
}
