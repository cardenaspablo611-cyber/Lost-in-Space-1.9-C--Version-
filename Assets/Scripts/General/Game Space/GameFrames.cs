using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFrames : MonoBehaviour {

    [SerializeField] private GameObject littleFramePrefab;
    [SerializeField] private GameObject bigFramePrefab;
    [SerializeField] private GameObject frameParent;

    [SerializeField] private float littleFrameScale;
    [SerializeField] private float bigFrameScale;

    private SpaceParent space;
    private Vector2 spaceScale;

    private void Start () {
        space = GameObject.Find("Space - Parent").GetComponent<SpaceParent>();

        spaceScale = Functions.getGameObjectSizeWithScale("Space - Parent");
    
        createBigFrame(new Vector2 (-bigFrameScale / 2, space.Start.y + bigFrameScale / 2), new Vector2 (spaceScale.x + bigFrameScale, bigFrameScale)); // UP
        createBigFrame(new Vector2 (bigFrameScale / 2, space.End.y - bigFrameScale / 2), new Vector2 (spaceScale.x + bigFrameScale, bigFrameScale)); // DOWN
        createBigFrame(new Vector2 (space.End.x + bigFrameScale / 2, bigFrameScale / 2), new Vector2 (bigFrameScale, spaceScale.y + bigFrameScale)); // RIGHT
        createBigFrame(new Vector2 (space.Start.x - bigFrameScale / 2, -bigFrameScale / 2), new Vector2 (bigFrameScale, spaceScale.y + bigFrameScale)); // LEFT

        createLittleFrame(new Vector2 (-littleFrameScale / 2, space.Start.y + littleFrameScale / 2), new Vector2 (spaceScale.x + littleFrameScale, littleFrameScale)); // UP
        createLittleFrame(new Vector2 (littleFrameScale / 2, space.End.y - littleFrameScale / 2), new Vector2 (spaceScale.x + littleFrameScale, littleFrameScale)); // DOWN
        createLittleFrame(new Vector2 (space.End.x + littleFrameScale / 2, littleFrameScale / 2), new Vector2 (littleFrameScale, spaceScale.y + littleFrameScale)); // RIGHT
        createLittleFrame(new Vector2 (space.Start.x - littleFrameScale / 2, -littleFrameScale / 2), new Vector2 (littleFrameScale, spaceScale.y + littleFrameScale)); // LEFT

    }

    private void createLittleFrame(Vector2 position, Vector2 scale) {

        GameObject littleFrame = Instantiate(littleFramePrefab) as GameObject;

        littleFrame.transform.localScale = scale;
        littleFrame.transform.position = position;

        littleFrame.transform.SetParent(frameParent.transform);
    }

    private void createBigFrame(Vector2 position, Vector2 scale) {

        GameObject bigFrame = Instantiate(bigFramePrefab) as GameObject;

        bigFrame.transform.localScale = scale;
        bigFrame.transform.position = position;

        bigFrame.transform.SetParent(frameParent.transform);
    }
}
