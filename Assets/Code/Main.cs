using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    GameObject go;
    private Deck deck = null;
    // Use this for initialization
	void Start () {
        print("hello world");

        deck = new Deck();
        
        print(deck == null);
        //print(deck.test);
        deck.createDeck();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
