using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HexagonGameResultMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GameResult(bool result) {
		Text resultText = this.GetComponent<RectTransform> ().FindChild ("ResultText").GetComponent<Text> ();
		resultText.text = result ?  "Win" : "Lose";
	}

	public void OnMainMenuButtonClicked () {
		this.SendMessageUpwards ("OnGotoMainMenu");
	}

	public void OnRestartButtonClicked() {
		this.SendMessageUpwards ("OnGameStart");
	}
}
