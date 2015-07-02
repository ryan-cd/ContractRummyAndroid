using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

    public enum InputTypes { GAME_OBJECT, BUTTON };
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

    public void resetInputs()
    {
        lastButtonHit = null;
        lastGameObjectHit = null;
    }


    //getters
    public bool inputExists()
    {
        return !(lastGameObjectHit == null && lastButtonHit == null);
    }

    public GameObject getLastGameObjectHit()
    {
        return Inputs.lastGameObjectHit;
    }

    public ButtonWrapper getLastButtonHit()
    {
        return Inputs.lastButtonHit;
    }
}
