using UnityEngine;

public class FloatingTextRandomMover : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 1.2f;
    [SerializeField] private float moveDistance = 1.5f;
    [SerializeField] private float fadeOutTime = 0.7f;
    [SerializeField] private float randomAngleRange = 60f; // Degrees from vertical
    [SerializeField] private float rotationSpeed = 180f; // Degrees per second

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float elapsedTime;
    private TextMesh textMesh;
    private Color originalColor;
    private float initialScale;
    private float randomRotationDirection;

    void Start() {
        startPosition = transform.position;

        float angle = Random.Range(-randomAngleRange / 2f, randomAngleRange / 2f);
        Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;

        targetPosition = startPosition + direction * moveDistance;

        textMesh = GetComponent<TextMesh>();
        if (textMesh != null) {
            originalColor = textMesh.color;
        }

        initialScale = transform.localScale.x;
        randomRotationDirection = Random.value > 0.5f ? 1f : -1f;
    }

    void Update() {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);

        transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, t));

        // Fade out
        float fadeT = Mathf.Clamp01((moveDuration - elapsedTime) / fadeOutTime);
        if (textMesh != null) {
            Color c = originalColor;
            c.a = fadeT;
            textMesh.color = c;
        }

        // Scale down with fade
        float scale = Mathf.Lerp(initialScale, 0f, 1f - fadeT);
        transform.localScale = new Vector3(scale, scale, scale);

        // Rotate
        transform.Rotate(Vector3.forward, rotationSpeed * randomRotationDirection * Time.deltaTime);

        if (elapsedTime >= moveDuration) {
            Destroy(gameObject);
        }
    }
}