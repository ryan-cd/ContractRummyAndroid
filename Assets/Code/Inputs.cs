using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

    private static GameObject lastGameObjectHit;
    private static ButtonWrapper lastButtonHit;
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
                lastGameObjectHit = hit.collider.gameObject;
            }
        }
	}

    //setters
    public static void setLastButtonHit(ButtonWrapper buttonWrapper)
    {
        lastButtonHit = buttonWrapper;
    }


    //getters
    public GameObject getLastGameObjectHit()
    {
        return Inputs.lastGameObjectHit;
    }

    public ButtonWrapper getLastButtonHit()
    {
        return Inputs.lastButtonHit;
    }
}
