using UnityEngine;

public class SpaceParent : MonoBehaviour {

    [SerializeField] private Vector2 scale;
    private Vector2 position = new Vector2 (0,0);

    [SerializeField] private Vector2 start;
    [SerializeField] private Vector2 end;

    [SerializeField] private bool disableChildren;

    public Vector2 Start {get { return start;}}
    public Vector2 End {get { return end;}}

    private void Awake() {

        transform.localScale = scale;

        start = new Vector2 (position.x - scale.x / 2, position.y + scale.y / 2);
        end = new Vector2 (position.x + scale.x / 2, position.y - scale.y / 2);
    
        if (disableChildren) {
            DisableAllChildren();
        }
    }

    private void DisableAllChildren() {
    
        for (int i = 0; i < transform.childCount; i++) {
            
            GameObject child = transform.GetChild(i).gameObject;
            
            child.SetActive(false);
        }
    }
}
