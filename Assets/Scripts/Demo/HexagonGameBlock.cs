using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

		private int _unitNum;
		public int UnitNum {
			get {
				return this._unitNum;
			}
			set { 
				this._unitNum = value;
			}
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		private Vector2 _position;
		private float _duration;
		private float _progress;

		private IEnumerator _bornCoroutine() {
			RectTransform rectTransform = this.GetComponent<RectTransform> ();
			rectTransform.localScale = Vector3.zero;
			while (this._progress < this._duration) {
				rectTransform.localScale = Vector3.Lerp (Vector3.zero, Vector3.one, this._progress / this._duration);
				this._progress += Time.deltaTime;
				yield return 0;
			}
			rectTransform.localScale = Vector3.one;
		}

		private IEnumerator _moveCoroutine() {
			RectTransform rectTransform = this.GetComponent<RectTransform> ();
			Vector2 originalOffsetMin = rectTransform.offsetMin;
			Vector2 originalOffsetMax = rectTransform.offsetMax;
			Vector2 deltaPosition = this._position - (originalOffsetMin + originalOffsetMax) / 2;
			while (this._progress < this._duration) {
				rectTransform.offsetMin = originalOffsetMin + deltaPosition * this._progress / this._duration;
				rectTransform.offsetMax = originalOffsetMax + deltaPosition * this._progress / this._duration;
				this._progress += Time.deltaTime;
				yield return 0;
			}
			rectTransform.offsetMin = originalOffsetMin + deltaPosition;
			rectTransform.offsetMax = originalOffsetMax + deltaPosition;
		}

		private IEnumerator _growCoroutine() {
			RectTransform rectTransform = this.GetComponent<RectTransform> ();
			RectTransform textRectTransform = rectTransform.FindChild ("BlockText") as RectTransform;

			this._times++;
			textRectTransform.GetComponent<Text> ().text = "" + HexagonGameBoard.getBlockNum (this.UnitNum, this.Times);

			rectTransform.localScale = Vector3.one;
			Vector3 maxScale = new Vector3 (1.2f, 1.2f, 1.2f);
			while (this._progress < this._duration / 2) {
				textRectTransform.localScale = Vector3.Lerp (Vector3.one, maxScale, this._progress / this._duration / 2);
				this._progress += Time.deltaTime;
				yield return 0;
			}
			this._progress -= this._duration / 2;
			while (this._progress < this._duration) {
				textRectTransform.localScale = Vector3.Lerp (maxScale, Vector3.one, this._progress / this._duration / 2);
				this._progress += Time.deltaTime;
				yield return 0;
			}
			textRectTransform.localScale = Vector3.one;
		}

		private IEnumerator _dieCoroutine() {
			RectTransform rectTransform = this.GetComponent<RectTransform> ();
			Vector2 originalOffsetMin = rectTransform.offsetMin;
			Vector2 originalOffsetMax = rectTransform.offsetMax;
			Vector2 deltaPosition = this._position - (originalOffsetMin + originalOffsetMax) / 2;
			while (this._progress < this._duration) {
				rectTransform.offsetMin = originalOffsetMin + deltaPosition * this._progress / this._duration;
				rectTransform.offsetMax = originalOffsetMax + deltaPosition * this._progress / this._duration;
				this._progress += Time.deltaTime;
				yield return 0;
			}
			rectTransform.offsetMin = originalOffsetMin + deltaPosition;
			rectTransform.offsetMax = originalOffsetMax + deltaPosition;

			rectTransform.SetParent (null);
			Destroy (this.gameObject);
		}

		public void blockBorn(Vector2 position, float duration) {
			this._progress = 0;
			this._position = position;
			this._duration = duration;
			StartCoroutine ("_bornCoroutine");
		}

		public void blockMove(Vector2 position, float duration) {
			this._progress = 0;
			this._position = position;
			this._duration = duration;
			StartCoroutine ("_moveCoroutine");
		}

		public void blockGrow(Vector2 position, float duration) {
			this._progress = 0;
			this._position = position;
			this._duration = duration;
			StartCoroutine ("_growCoroutine");
			StartCoroutine ("_moveCoroutine");
		}

		public void blockDie(Vector2 position, float duration) {
			this._progress = 0;
			this._position = position;
			this._duration = duration;
			StartCoroutine ("_dieCoroutine");
		}
	}
}
