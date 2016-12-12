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

		private Color _minBgColor;
		private Color _maxBgColor;

		public Color MinBgColor {
			get {
				return this._minBgColor;
			}
			set { 
				this._minBgColor = value;
			}
		}

		public Color MaxBgColor {
			get {
				return this._maxBgColor;
			}
			set { 
				this._maxBgColor = value;
			}
		}

		private int _maxTimes;
		public int MaxTimes {
			get {
				return this._maxTimes;
			}
			set {
				this._maxTimes = value;
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

			RectTransform bgRectTransform = rectTransform.FindChild ("BlockBackground") as RectTransform;
			RawImage bg = bgRectTransform.GetComponent<RawImage> ();
			bg.color = this.MinBgColor;

			RectTransform textRectTransform = rectTransform.FindChild ("BlockText") as RectTransform;
			Text text = textRectTransform.GetComponent<Text> ();
			text.text = "" + HexagonGameBoard.getBlockNum (this.UnitNum, this.Times);


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
			RectTransform bgRectTransform = rectTransform.FindChild ("BlockBackground") as RectTransform;

			Text text = textRectTransform.GetComponent<Text> ();
			RawImage bg = bgRectTransform.GetComponent<RawImage> ();

			this._times++;
			text.text = "" + HexagonGameBoard.getBlockNum (this.UnitNum, this.Times);

			rectTransform.localScale = Vector3.one;

			float scaleFactor = 2.0f;
			Vector3 maxScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
			while (this._progress < this._duration / 2) {
				textRectTransform.localScale = Vector3.Lerp (Vector3.one, maxScale, this._progress / this._duration / 2);

				float colorFactor = Mathf.Lerp(this._times - 1, this._times, this._progress / this._duration);
				bg.color = Color.Lerp (this.MinBgColor, this.MaxBgColor, colorFactor / this.MaxTimes);

				this._progress += Time.deltaTime;
				yield return 0;
			}
			while (this._progress < this._duration) {
				textRectTransform.localScale = Vector3.Lerp (maxScale, Vector3.one, (this._progress - this._duration / 2) / this._duration / 2);

				float colorFactor = Mathf.Lerp(this._times - 1, this._times, this._progress / this._duration);
				bg.color = Color.Lerp (this.MinBgColor, this.MaxBgColor, colorFactor / this.MaxTimes);

				this._progress += Time.deltaTime;
				yield return 0;
			}
			textRectTransform.localScale = Vector3.one;
		}

		private IEnumerator _dieCoroutine() {
			RectTransform rectTransform = this.GetComponent<RectTransform> ();
			RectTransform textRectTransform = rectTransform.FindChild ("BlockText") as RectTransform;
			RectTransform bgRectTransform = rectTransform.FindChild ("BlockBackground") as RectTransform;
			Text text = textRectTransform.GetComponent<Text> ();
			RawImage bg = bgRectTransform.GetComponent<RawImage> ();
			Color textColor = text.color;
			Color bgColor = bg.color;
			while (this._progress < this._duration) {
				text.color = Color.Lerp (textColor, Color.clear, this._progress / this._duration);
				bg.color = Color.Lerp (bgColor, Color.clear, this._progress / this._duration);

				this._progress += Time.deltaTime;
				yield return 0;
			}

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
			StartCoroutine ("_moveCoroutine");
			StartCoroutine ("_dieCoroutine");
		}
	}
}
