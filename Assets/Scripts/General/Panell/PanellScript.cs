using UnityEngine;
using F = Functions;

public class PanellScript : MonoBehaviour {

    [SerializeField] private Vector2 frame;

    [SerializeField] private bool up;
    [SerializeField] private bool down;
    [SerializeField] private bool right;
    [SerializeField] private bool left;
    [SerializeField] private bool center;

    private SpaceParent space;

    private void Start() {
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");

        if (center) {
            transform.position = new Vector2(0, 0);
        }

        if (up && left) {
            transform.position = new Vector2 (space.Start.x + frame.x, space.Start.y - frame.y);
        } else if (up && right) {
            transform.position = new Vector2 (space.Start.x - frame.x, space.End.y - frame.y);
        } else if (down && left) {
            transform.position = new Vector2 (space.Start.x + frame.x, space.End.y + frame.y);
        } else if (down && right) {
            transform.position = new Vector2 (space.End.x - frame.x, space.End.y + frame.y);
        }
    }

    private void Update() {
        if (Functions.checkGameOver()) { Destroy(gameObject); }
    }
}
