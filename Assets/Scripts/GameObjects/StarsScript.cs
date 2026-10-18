using UnityEngine;
using F = Functions;

public class StarsScript : MonoBehaviour { 

    private Vector2 position;

    private float speed;

    private float starsFrame = 10;

    private float max = 7;
    private float min = 1;

    private SpaceParent space;
    private SpriteRenderer spriteRenderer;

    [Header("Planet Star Colors")]
    [SerializeField] private Color[] planetStarColors = new Color[] {
        new Color(1f, 1f, 1f, 1f),           // Planet 1: Destroyed - White
        new Color(1f, 0.85f, 0.45f, 1f),     // Planet 2: Desert - Orange
        new Color(0.75f, 0.55f, 1f, 1f),     // Planet 3: Techno - Purple
        new Color(0.55f, 0.85f, 1f, 1f),     // Planet 4: Electric - Blue
        new Color(1f, 0.45f, 0.45f, 1f)      // Planet 5: Octopus - Red
    };

    private static Color[] staticPlanetStarColors;
    private static int currentPlanetIndex = 0;
    private static Color targetColor = Color.white;
    private Color currentColor = Color.white;
    private float colorLerpTime = 0f;
    private const float colorTransitionDuration = 2f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetStatics() {
        currentPlanetIndex = 0;
        targetColor = Color.white;
        staticPlanetStarColors = null;
    }

    private void Awake() {
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (staticPlanetStarColors == null) {
            staticPlanetStarColors = planetStarColors;
        }
        
        if (spriteRenderer != null) {
            currentColor = GetPlanetColor(currentPlanetIndex);
            targetColor = currentColor;
            spriteRenderer.color = currentColor;
        }
    }

    private void Start() {

        float radius = F.randomFloat(min,max);
        transform.localScale = new Vector2 (radius,radius); 

        speed = F.randomFloat(min,max);

        position.x = F.randomFloat(space.Start.x - starsFrame,space.End.x + starsFrame);
        position.y = F.randomFloat(space.Start.y + starsFrame,space.End.y - starsFrame);
        
        transform.position = position;
    }

    private void FixedUpdate() {

        //if (F.checkGameOver()) { Destroy(gameObject); }

        starsMovement();

        checkOfLimit();

        UpdateStarColor();
    }

    private void starsMovement() {

        position = F.direction(position, speed);
        transform.position = position;
    }

    private void checkOfLimit() {

        if (transform.position.y < space.End.y - starsFrame * 2) {
            linearReset(); 
        } 
        
        if (transform.position.x < space.Start.x - starsFrame * 2) {
            lateralReset();
        } else if (transform.position.x > space.End.x + starsFrame * 2) {
            lateralReset();
        }
    }

    private void linearReset() {   

        float radius = F.randomFloat(min,max);
        transform.localScale = new Vector2 (radius,radius); 

        speed = F.randomFloat(min,max);

        position.x = F.randomFloat(space.Start.x - starsFrame,space.End.x + starsFrame);
        position.y = space.Start.y + starsFrame;
        
        transform.position = position;
    }

    private void lateralReset() {

        position.x *= -1;
        transform.position = position;
    }

    private void UpdateStarColor() {
        if (spriteRenderer == null) return;

        if (currentColor != targetColor) {
            colorLerpTime += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(colorLerpTime / colorTransitionDuration);
            currentColor = Color.Lerp(currentColor, targetColor, t);
            spriteRenderer.color = currentColor;
        }
    }

    private Color GetPlanetColor(int planetIndex) {
        Color[] colors = staticPlanetStarColors != null ? staticPlanetStarColors : planetStarColors;
        if (planetIndex >= 0 && planetIndex < colors.Length) {
            return colors[planetIndex];
        }
        return Color.white;
    }

    public static void SetPlanetIndex(int planetIndex) {
        currentPlanetIndex = planetIndex;
        
        if (staticPlanetStarColors != null && planetIndex >= 0 && planetIndex < staticPlanetStarColors.Length) {
            targetColor = staticPlanetStarColors[planetIndex];
        } else {
            targetColor = Color.white;
        }
        
        StarsScript[] allStars = FindObjectsOfType<StarsScript>();
        foreach (var star in allStars) {
            star.colorLerpTime = 0f;
        }
    }
}