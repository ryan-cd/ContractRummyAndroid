using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("hit "+hit.collider.gameObject.name);
            }
        }
	}
}
