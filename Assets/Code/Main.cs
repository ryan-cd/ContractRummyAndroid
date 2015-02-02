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
        
        //this will create a deck, shuffle, and initialize all players with their hands
        deck.initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
