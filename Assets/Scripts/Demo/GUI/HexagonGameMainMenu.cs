using UnityEngine;
using System.Collections;

public class HexagonGameMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnStartButtonClicked() {
		this.SendMessageUpwards ("OnGameStart");
	} 
}
