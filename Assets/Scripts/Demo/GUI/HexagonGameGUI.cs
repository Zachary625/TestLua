using UnityEngine;
using System.Collections;

namespace HexagonGame {
	public class HexagonGameGUI : MonoBehaviour {
		public GameObject MainMenuPrefab;
		public GameObject ResultMenuPrefab;
		public GameObject GameMenuPrefab;

		public HexagonGameBoard GameBoard;

		// Use this for initialization
		void Start () {
			this._gotoMainMenu ();
		}

		// Update is called once per frame
		void Update () {

		}

		private void _clearGUI() {
			RectTransform rectTransform = this.GetComponent<RectTransform> ();
			while (rectTransform.childCount > 0) {
				GameObject childGUI = rectTransform.GetChild (0).gameObject;
				childGUI.GetComponent<RectTransform> ().SetParent (null);
				Destroy (childGUI);
			}

		}

		private void _setupGUI(RectTransform rt) {
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;

			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		} 

		private void _gotoMainMenu() {
			this._clearGUI ();

			if (!!MainMenuPrefab) {
				GameObject mainMenu = Instantiate (MainMenuPrefab);
				mainMenu.GetComponent<RectTransform> ().SetParent (this.GetComponent<RectTransform>());
				this._setupGUI (mainMenu.GetComponent<RectTransform>());
			}
		}

		public void OnGameStart() {
			this._clearGUI ();

			if (!!GameMenuPrefab) {
				GameObject gameMenu = Instantiate (GameMenuPrefab);
				gameMenu.GetComponent<RectTransform> ().SetParent (this.GetComponent<RectTransform>());
				this._setupGUI (gameMenu.GetComponent<RectTransform>());
			}

			this.GameBoard.SendMessage ("GameBoardStart");
		}

		public void OnGameFinish(bool? result) {
			if (result.HasValue) {
				if (!!ResultMenuPrefab) {
					GameObject resultMenu = Instantiate (ResultMenuPrefab);
					resultMenu.GetComponent<RectTransform> ().SetParent (this.GetComponent<RectTransform>());
					resultMenu.SendMessage ("GameResult", result.Value);
					this._setupGUI (resultMenu.GetComponent<RectTransform>());
				}
			} else {
				this._gotoMainMenu ();
			}

			this.GameBoard.SendMessage ("GameBoardFinish");
		}

		public void OnGotoMainMenu() {
			this._gotoMainMenu ();
		}
	}
}
