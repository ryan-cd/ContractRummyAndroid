using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("input");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // the object identified by hit.transform was clicked
                // do whatever you want
                Debug.Log("hit");
            }
        }
	}
}
