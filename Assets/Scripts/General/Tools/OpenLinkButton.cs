using UnityEngine;

public class OpenLinkButton : MonoBehaviour {

    [SerializeField] private string url;

    public void OpenLink() {

        Debug.Log("Link: " + url + "opened");

        Application.OpenURL(url);
    }
}

