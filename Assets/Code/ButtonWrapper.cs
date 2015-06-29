using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonWrapper : MonoBehaviour{

    GameObject button = null;

    public void construct(string prefabName, Vector3 position, Quaternion rotation, string text, UnityAction callback)
    {
        button = (GameObject)Instantiate(Resources.Load(prefabName), position, rotation);
        button.transform.SetParent(GameObject.Find("Canvas").transform, false);
        button.GetComponentInChildren<Text>().text = text;
        button.GetComponent<Button>().onClick.AddListener(callback);
    }

    //getters
    public string getText()
    {
        return button.GetComponentInChildren<Text>().text;
    }

    //setters
    public void setText(string newText)
    {
        button.GetComponentInChildren<Text>().text = newText;
    }
}
