using UnityEngine;
using F = Functions;

public class PauseController : MonoBehaviour {

    private bool itsPaused;
    public bool ItsPaused { get { return itsPaused;}}

    private UserInterfaceScript userInterface;
    private SpaceshipScript spaceship;

    private float originalTimeScale;
    public float OriginalTimeScale { get { return originalTimeScale;}}

    [SerializeField] private GameObject pauseMenu;

    private bool noMore = true;

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;

    private void Start() {

        Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;
        Transform child = parentTransform.GetChild(0);

        userInterface = F.GetScriptComponentName<UserInterfaceScript>("User Interface Object");
        spaceship = child.GetComponent<SpaceshipScript>();

        originalTimeScale = Time.timeScale;
    }

    private void Update() {

        if (noMore) {
            if (!spaceship.Deadth && (userInterface.P || userInterface.Escape)) {
                itsPaused = !itsPaused;

                if (itsPaused) {

                    Time.timeScale = 0.0f;
                    pauseMenu.SetActive(true);

                } else {

                    stopPause();
                }
            }
        }   
    }

    public void stopPause() {
        F.GetScriptComponentName<PanellAdminScript>("Panell Object").afterPausing(originalTimeScale);

        itsPaused = false;

        pauseMenu.SetActive(false);
    }
}
