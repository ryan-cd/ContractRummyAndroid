using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

    private static GameObject lastGameObjectClicked;
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
                //Debug.Log("hit "+hit.collider.gameObject.name+" "+hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name);
                lastGameObjectClicked = hit.collider.gameObject;
            }
        }
	}

    //getters
    public GameObject getLastGameObjectClicked()
    {
        return Inputs.lastGameObjectClicked;
    }
}
