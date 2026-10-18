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
    [SerializeField] private Color[] planetColors = new Color[] {
        new Color(1f, 1f, 1f, 1f),        // Planet 1: Destroyed - White
        new Color(1f, 0.98f, 0.8f, 1f),   // Planet 2: Desert - Yellowish
        new Color(0.9f, 0.85f, 1f, 1f),   // Planet 3: Techno - Purple tint
        new Color(0.8f, 0.95f, 1f, 1f),   // Planet 4: Electric - Light blue
        new Color(1f, 0.8f, 0.8f, 1f)     // Planet 5: Octopus - Red tint
    };

    private static int currentPlanetIndex = 0;
    private static Color targetColor = Color.white;
    private Color currentColor = Color.white;
    private float colorLerpTime = 0f;
    private const float colorTransitionDuration = 2f;

    private void Awake() {
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null) {
            currentColor = GetPlanetColor(currentPlanetIndex);
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
        if (planetIndex >= 0 && planetIndex < planetColors.Length) {
            return planetColors[planetIndex];
        }
        return Color.white;
    }

    public static void SetPlanetIndex(int planetIndex) {
        currentPlanetIndex = planetIndex;
        targetColor = Color.white;
        
        StarsScript[] allStars = FindObjectsOfType<StarsScript>();
        if (allStars.Length > 0) {
            targetColor = allStars[0].GetPlanetColor(planetIndex);
            foreach (var star in allStars) {
                star.colorLerpTime = 0f;
            }
        }
    }
}