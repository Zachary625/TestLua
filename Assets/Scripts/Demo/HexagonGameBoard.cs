using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace HexagonGame {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class HexagonGameBoard : MonoBehaviour {
		public class HexagonGameBoardProperties {
			public float CellSize = 10;
			public float CellGap = 2;
			private Vector2[] _sideVertexPositions = new Vector2[] {
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 30), Mathf.Sin(Mathf.Deg2Rad * 30)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90), Mathf.Sin(Mathf.Deg2Rad * 90)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 150), Mathf.Sin(Mathf.Deg2Rad * 150)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 210), Mathf.Sin(Mathf.Deg2Rad * 210)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270), Mathf.Sin(Mathf.Deg2Rad * 270)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 330), Mathf.Sin(Mathf.Deg2Rad * 330)),			
			};

			private Vector2[] _sideDirections = new Vector2[] {
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 150), Mathf.Sin(Mathf.Deg2Rad * 150)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 210), Mathf.Sin(Mathf.Deg2Rad * 210)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270), Mathf.Sin(Mathf.Deg2Rad * 270)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 330), Mathf.Sin(Mathf.Deg2Rad * 330)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 30), Mathf.Sin(Mathf.Deg2Rad * 30)),			
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90), Mathf.Sin(Mathf.Deg2Rad * 90)),			
			};

			public int Size = 5;
			public int Cells {
				get { 
					return getCellsForSize (this.Size);
				}
			}

			public static int getCellsForSize(int size) {
				// 1: 1
				// 2: 1 + 6
				// 3: 1 + 6 + 12
				return 1 + 3 * (size - 1) * size;
			}

			public Vector2 getPositionForCell(int index) {
				if (index == 0) {
					return Vector2.zero;
				} 

				// which ring is it on
				int ring = (int)Mathf.Floor (0.5f + Mathf.Sqrt(3 * (4 * index - 1)) / 6);

				// index within the ring
				int ringIndex = index - getCellsForSize(ring);
				int side = (int)Mathf.Floor (ringIndex / (ring * 1.0f));
				int sideIndex = ringIndex - side * ring;

				Debug.Log (" @ HexagonGameBoardProperties.getPositionForCell(" + index + "): ring: " + ring);
				Debug.Log (" @ HexagonGameBoardProperties.getPositionForCell(" + index + "): ringIndex: " + ringIndex);
				Debug.Log (" @ HexagonGameBoardProperties.getPositionForCell(" + index + "): side: " + side);
				Debug.Log (" @ HexagonGameBoardProperties.getPositionForCell(" + index + "): sideIndex: " + sideIndex);


				Vector2 basePosition = this._sideVertexPositions[side];
				Vector2 biasDirection = this._sideDirections[side];
				return (basePosition * ring + sideIndex * biasDirection) * (this.CellSize * Mathf.Sin(Mathf.Deg2Rad * 60) * 2 + this.CellGap);
			}

			public float MoveTime = 0.3f;
			public float GenerateTime = 0.3f;

			public int UnitNum = 3;
			public int RequireTimes = 10;
		}

		public HexagonGameBoardProperties BoardProperties = new HexagonGameBoardProperties();
		public class HexagonGameCellProperties
		{
			public Texture MaskImage;

			public Color EmptyBackgroundColor = Color.gray;
			public Texture EmptyBackgroundImage;
			public Texture BackgroundImage;
			public Font ForegroundFont;
			public int ForegroundFontSize = 14;
			public Color ForegroundFontColor = Color.red;
		}

		public HexagonGameCellProperties CellProperties = new HexagonGameCellProperties();

		public enum HexagonGameBoardPhase {
			None,
			Waiting,
			Moving,
			Generating,
			Win,
			Lose,
		}

		private HexagonGameBoardPhase _phase = HexagonGameBoardPhase.None;

		public HexagonGameBoardPhase Phase {
			get {
				return _phase;
			}
		}

		private GameObject _cellsAnchor;
		private GameObject _blocksAnchor;

		private Vector2 _cellSize;

		void Awake() {
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void makeGameBoard() {
			this._initCells ();
		}

		private void _gotoPhase(HexagonGameBoardPhase phase) {
			this._exitPhase (this._phase);
			this._phase = phase;
			this._enterPhase (this._phase);
		}

		private void _exitPhase(HexagonGameBoardPhase phase) {

		}

		private void _enterPhase(HexagonGameBoardPhase phase) {

		}

		private void _destroyCells() {
			this._cellsAnchor = this.transform.Find ("CellsAnchor").gameObject;
			while (this._cellsAnchor.transform.childCount > 0) {
				GameObject cellGameObject = this._cellsAnchor.transform.GetChild(0).gameObject;
				cellGameObject.transform.SetParent (null);
				if (Application.isEditor) {
					DestroyImmediate (cellGameObject);
				} else {
					Destroy (cellGameObject);
				}
			}
		}

		private void _initCells() {
			this._destroyCells ();

			this._cellsAnchor = this.transform.Find ("CellsAnchor").gameObject;
			this._cellSize = new Vector2 (2 * this.BoardProperties.CellSize, 2 * this.BoardProperties.CellSize * Mathf.Sin (Mathf.PI / 3));
			for (int index = 0; index < this.BoardProperties.Cells; index++) {
				this._initCell (index);
			}
		}

		private void _initCell(int index) {
			Vector2 position = this.BoardProperties.getPositionForCell (index);

			GameObject cellGameObject = new GameObject ();

			RectTransform cellRectTransform = cellGameObject.AddComponent<RectTransform> ();
			cellRectTransform.SetParent (this._cellsAnchor.transform);

			RawImage cellMaskImage = cellGameObject.AddComponent<RawImage> ();
			cellMaskImage.texture = this.CellProperties.MaskImage;

			Mask cellMask = cellGameObject.AddComponent<Mask> ();
			cellMask.showMaskGraphic = false;
			cellRectTransform.anchorMin = Vector2.one / 2;
			cellRectTransform.anchorMax = Vector2.one / 2;
			cellRectTransform.offsetMin = position - this._cellSize / 2;
			cellRectTransform.offsetMax = position + this._cellSize / 2;


			GameObject cellBackgroundGameObject = new GameObject ();
			RectTransform cellBgRectTransform = cellBackgroundGameObject.AddComponent<RectTransform> ();
			cellBgRectTransform.SetParent (cellRectTransform);
			cellBgRectTransform.anchorMin = Vector2.zero;
			cellBgRectTransform.anchorMax = Vector2.one;
			cellBgRectTransform.offsetMin = Vector2.zero;
			cellBgRectTransform.offsetMax = Vector2.zero;

			RawImage cellBgImage = cellBackgroundGameObject.AddComponent<RawImage> ();
			cellBgImage.texture = this.CellProperties.EmptyBackgroundImage;
			cellBgImage.color = this.CellProperties.EmptyBackgroundColor;
		}

	}
}
