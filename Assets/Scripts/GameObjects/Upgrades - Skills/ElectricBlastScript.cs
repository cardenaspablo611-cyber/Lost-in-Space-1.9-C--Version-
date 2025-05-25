using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBlastScript : MonoBehaviour {

    [SerializeField] private GameObject electricBlastPrefab;
    private GameObject electricBlast;

    private void Start() {

        StartCoroutine(excecuteBlast());
    }

    private void FixedUpdate() {
        if (electricBlast != null) {
            electricBlast.transform.position = transform.position;
        }
    }

    private IEnumerator excecuteBlast() {

        yield return new WaitForSeconds(3);

        while (true) {

            electricBlast = Instantiate(electricBlastPrefab, transform.position, Quaternion.identity) as GameObject;
            
            yield return new WaitForSeconds(20);
        }
    }    
}
