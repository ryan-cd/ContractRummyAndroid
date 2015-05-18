using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {

    private static GameObject lastGameObjectClicked;
	// Use this for initialization
	void Start () {

	}
	
    /*public static Button CreateButton(Button buttonPrefab, Canvas canvas, Vector2 cornerTopRight, Vector2 cornerBottomLeft)
    {
        var button = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as Button;
        var rectTransform = button.GetComponent<RectTransform>();
        rectTransform.SetParent(canvas.transform);
        rectTransform.anchorMax = cornerTopRight;
        rectTransform.anchorMin = cornerBottomLeft;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        return button;
    }*/
	
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
