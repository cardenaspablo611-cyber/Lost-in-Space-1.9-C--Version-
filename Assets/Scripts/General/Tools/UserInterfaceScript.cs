using UnityEngine;

public class UserInterfaceScript : MonoBehaviour { 

    [SerializeField] private bool up;
    [SerializeField] private bool down;
    [SerializeField] private bool right;
    [SerializeField] private bool left;

    [SerializeField] private bool p;
    [SerializeField] private bool space;
    [SerializeField] private bool escape;
    [SerializeField] private bool f3;

    [SerializeField] private bool j;

    public bool Up { get { return up;}}
    public bool Down { get { return down;}}
    public bool Right { get { return right;}}
    public bool Left { get { return left;}}

    public bool P { get { return p;}}
    public bool Space { get { return space;}}
    public bool Escape { get { return escape;}}
    public bool F3 { get { return f3;}}
    public bool J { get { return j;}}

    [HideInInspector] public SpaceshipScript spaceshipScriptInstance;
    [SerializeField] private bool spaceship;

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;

    private bool printProblem = false;


    private void Start() {
        

        if (spaceship) {

            Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;
            Transform child = parentTransform.GetChild(0);

            spaceshipScriptInstance = child.GetComponent<SpaceshipScript>(); 
        }
    }

    private void Update() {

        up = false;
        down = false;
        right = false;
        left = false;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            if (spaceship) { 
                if (!spaceshipScriptInstance.UpSideDown) {
                    if (!spaceshipScriptInstance.OffLimitPositivX) {
                        right = true;
                    }
                } else {
                    if (!spaceshipScriptInstance.OffLimitNegativX) {
                        left = true;
                    }
                }
            } else {
                right = true;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            if (spaceship) {
                if (!spaceshipScriptInstance.UpSideDown) {
                    if (!spaceshipScriptInstance.OffLimitNegativX) {
                        left = true;
                    }
                } else {
                    if (!spaceshipScriptInstance.OffLimitPositivX) {
                        right = true;
                    }
                }
            } else {
                left = true;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            if (spaceship) {
                if (!spaceshipScriptInstance.UpSideDown) {
                    if (!spaceshipScriptInstance.OffLimitPositivY) {
                        up = true;
                    }
                } else {
                    if (!spaceshipScriptInstance.OffLimitNegativY) {
                        down = true;
                    }
                }
            } else {
                up = true;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {        
            if (spaceship) {
                if (!spaceshipScriptInstance.UpSideDown) {
                    if (!spaceshipScriptInstance.OffLimitNegativY) {
                        down = true;
                    }
                } else {
                    if (!spaceshipScriptInstance.OffLimitPositivY) {
                        up = true;
                    }
                }
            } else {
                down = true;
            }
        }

        if (up && down) {
            up = false;
            down = false;
        }

        if (left && right) {
            left = false;
            right = false;
        }

        p = false;
        space = false;
        escape = false;
        f3 = false;

        if (!PanellAdminScript.startAfterCount) {
            
            if (Input.GetKey(KeyCode.Space)) {
                space = true;
            } 


            if (Input.GetKeyUp(KeyCode.P)) {
                p = true;

            }
            
            if (Input.GetKeyUp(KeyCode.Escape)) {
                escape = true;
            }

            if (Input.GetKeyUp(KeyCode.F3)) {
                f3 = true;
            }

        }

        j = false;
        if (Input.GetKey(KeyCode.J)){
            j = true;
        }
    }
}
