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

    // Star tint per planet (Destroyed, Desert, Techno, Electric, Octopus).
    // Each planet has a palette; every star picks one color from it at random,
    // so Techno stars come out red, blue or purple like its enemies.
    private static readonly Color[][] planetStarPalettes = new Color[][] {
        // 1 Destroyed - browns/café (75%) + aquamarine (10%) + white (15%)
        // 15 browns + 2 aquamarines + 3 whites = 20 total
        new Color[] {
            new Color(0.36f, 0.29f, 0.17f, 1f), // Brown dark #5c4b2b
            new Color(0.66f, 0.56f, 0.36f, 1f), // Brown light #a98f5d
            new Color(0.62f, 0.52f, 0.33f, 1f), // Brown medium #9e8554
            new Color(0.55f, 0.45f, 0.28f, 1f), // Brown warm #8c7347
            new Color(0.36f, 0.29f, 0.17f, 1f), // Brown dark
            new Color(0.66f, 0.56f, 0.36f, 1f), // Brown light
            new Color(0.62f, 0.52f, 0.33f, 1f), // Brown medium
            new Color(0.55f, 0.45f, 0.28f, 1f), // Brown warm
            new Color(0.36f, 0.29f, 0.17f, 1f), // Brown dark
            new Color(0.66f, 0.56f, 0.36f, 1f), // Brown light
            new Color(0.62f, 0.52f, 0.33f, 1f), // Brown medium
            new Color(0.55f, 0.45f, 0.28f, 1f), // Brown warm
            new Color(0.36f, 0.29f, 0.17f, 1f), // Brown dark
            new Color(0.62f, 0.52f, 0.33f, 1f), // Brown medium
            new Color(0.55f, 0.45f, 0.28f, 1f), // Brown warm = 15 browns (75%)
            new Color(0.00f, 0.35f, 0.44f, 1f), // Aquamarine #005a70
            new Color(0.27f, 0.53f, 0.60f, 1f), // Aqua light #448899 = 2 aqua (10%)
            new Color(1f, 1f, 1f, 1f),           // White
            new Color(1f, 1f, 1f, 1f),           // White
            new Color(1f, 1f, 1f, 1f)            // White = 3 white (15%)
        },
        new Color[] { new Color(1f, 0.78f, 0.3f, 1f) },                             // 2 Desert - yellow/orange
        new Color[] { new Color(0.45f, 0.62f, 1f, 1f),                              // 3 Techno - mostly blue,
                      new Color(0.45f, 0.62f, 1f, 1f),                              //            then purple,
                      new Color(0.45f, 0.62f, 1f, 1f),                              //            a little red
                      new Color(0.75f, 0.42f, 1f, 1f),                              //            (repeats = weight)
                      new Color(0.75f, 0.42f, 1f, 1f),
                      new Color(1f, 0.38f, 0.38f, 1f) },
        new Color[] { new Color(0.45f, 0.8f, 1f, 1f) },                             // 4 Electric - light blue
        new Color[] { new Color(1f, 0.35f, 0.35f, 1f),                              // 5 Octopus - red mixed with grey,
                      new Color(0.85f, 0.55f, 0.55f, 1f),                           //            like the planet
                      new Color(0.65f, 0.65f, 0.65f, 1f),
                      new Color(0.65f, 0.65f, 0.65f, 1f) }
    };

    private static int currentPlanetIndex = 0;
    private Color startColor = Color.white;
    private Color targetColor = Color.white;
    private Color currentColor = Color.white;
    private float colorLerpTime = 0f;
    private const float colorTransitionDuration = 2f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetStatics() {
        currentPlanetIndex = 0;
        // Every new run (scene load) starts again with white stars
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
        currentPlanetIndex = 0;
    }

    private void Awake() {
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null) {
            currentColor = PickPlanetColor(currentPlanetIndex);
            startColor = currentColor;
            targetColor = currentColor;
            colorLerpTime = colorTransitionDuration;
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

        if (colorLerpTime < colorTransitionDuration) {
            colorLerpTime += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(colorLerpTime / colorTransitionDuration);
            currentColor = Color.Lerp(startColor, targetColor, t);
            spriteRenderer.color = currentColor;
        }
    }

    private static Color PickPlanetColor(int planetIndex) {
        if (planetIndex < 0 || planetIndex >= planetStarPalettes.Length) return Color.white;
        Color[] palette = planetStarPalettes[planetIndex];
        return palette[Random.Range(0, palette.Length)];
    }

    private void BeginColorTransition(Color newTarget) {
        startColor = currentColor;
        targetColor = newTarget;
        colorLerpTime = 0f;
    }

    public static void SetPlanetIndex(int planetIndex) {
        currentPlanetIndex = planetIndex;

        StarsScript[] allStars = FindObjectsOfType<StarsScript>();
        foreach (var star in allStars) {
            star.BeginColorTransition(PickPlanetColor(planetIndex));
        }
    }
}
