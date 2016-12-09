using UnityEngine;
using System.Collections;

namespace HexagonGame {
	public class HexagonGameBlock : MonoBehaviour {
		public enum HexagonGameBlockStatus {
			None,
			Borning,
			Waiting,
			Moving,
			Growing,
			Dying,
		}

		private HexagonGameBlockStatus _status = HexagonGameBlockStatus.None;

		private int _times;

		public int Times {
			get {
				return this._times;
			}
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void blockBorn(float duration) {
			
		}

		public void blockMove(Vector2 position, float duration) {
			
		}

		public void blockGrow(float duration) {
			
		}

		public void blockDie(Vector2 position, float duration) {
			
		}
	}
}
