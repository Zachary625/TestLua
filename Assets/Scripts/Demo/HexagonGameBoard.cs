using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace HexagonGame {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class HexagonGameBoard : MonoBehaviour {
		public enum Direction {
			OClock_2,
			OClock_12,
			OClock_10,
			OClock_8,
			OClock_6,
			OClock_4,
		}

		private static readonly Vector2[] _sideVertexPositions = new Vector2[] {
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 30), Mathf.Sin(Mathf.Deg2Rad * 30)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90), Mathf.Sin(Mathf.Deg2Rad * 90)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 150), Mathf.Sin(Mathf.Deg2Rad * 150)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 210), Mathf.Sin(Mathf.Deg2Rad * 210)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270), Mathf.Sin(Mathf.Deg2Rad * 270)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 330), Mathf.Sin(Mathf.Deg2Rad * 330)),			
		};

		private static readonly Vector2[] _sideDirections = new Vector2[] {
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 150), Mathf.Sin(Mathf.Deg2Rad * 150)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 210), Mathf.Sin(Mathf.Deg2Rad * 210)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270), Mathf.Sin(Mathf.Deg2Rad * 270)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 330), Mathf.Sin(Mathf.Deg2Rad * 330)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 30), Mathf.Sin(Mathf.Deg2Rad * 30)),			
			new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90), Mathf.Sin(Mathf.Deg2Rad * 90)),			
		};

		public class HexagonGameBoardProperties {
			public float CellSize = 40;
			public float CellGap = 5;

			public int Size = 3;
			public int Cells {
				get { 
					return getCellsForSize (this.Size);
				}
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

			public Color EmptyBackgroundColor = Color.white;
			public Texture EmptyBackgroundImage;
			public Texture BackgroundImage;
			public Font ForegroundFont;
			public int ForegroundFontSize = 14;
			public Color ForegroundFontColor = Color.red;
		}

		public HexagonGameCellProperties CellProperties = new HexagonGameCellProperties();

		private Vector2[] _cellPositions;
		private int[,] _cellAdjacency;
		private int[] _blockStatusFront;
		private int[] _blockStatusBack;

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


		private static int getCellsForSize(int size) {
			// 0: 0
			// 1: 1
			// 2: 1 + 6
			// 3: 1 + 6 + 12
			if(size <= 0) {
				return 0;
			}
			return 1 + 3 * (size - 1) * size;
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

			this._cellPositions = new Vector2[this.BoardProperties.Cells];
			this._cellAdjacency = new int[this.BoardProperties.Cells, 6];
			this._blockStatusFront = new int[this.BoardProperties.Cells];
			this._blockStatusBack = new int[this.BoardProperties.Cells];

			for (int index = 0; index < this.BoardProperties.Cells; index++) {
				for(int direction = 0; direction < 6; direction++) {
					this._cellAdjacency [index, direction] = -1;
				}
				this._initCell (index);
			}

			for (int cellIndex = 0; cellIndex < this.BoardProperties.Cells; cellIndex++) {
				for (int adjacentIndex = cellIndex + 1; adjacentIndex < this.BoardProperties.Cells; adjacentIndex++) {
					Vector2 bias = this._cellPositions [adjacentIndex] - this._cellPositions [cellIndex];
					float distance = Mathf.Abs (bias.magnitude - (2 * this.BoardProperties.CellSize * Mathf.Sin(Mathf.Deg2Rad * 60) + this.BoardProperties.CellGap));
//					Debug.Log (" @ HexagonGameBoard._initCells(): " + cellIndex + " to " + adjacentIndex + ": " + distance);

					if (distance < 0.01f) {
						Vector2 direction = bias.normalized;
						for(int d = 0; d < 6; d++) {
							float rotation = Mathf.Abs ((_sideVertexPositions [d] - direction).magnitude);
							if (rotation < 0.01f) {
								this._cellAdjacency [cellIndex, d] = adjacentIndex;
								this._cellAdjacency [adjacentIndex, (d + 3) % 6] = cellIndex;

								Debug.Log (" @ HexagonGameBoard._initCells(): " + cellIndex + " -> " + d + " = " + adjacentIndex);
							}
						}
					}
				}
			}


//			for (int ring = 0; ring < this.BoardProperties.Size; ring++) {
//				for(int side = 0; side < 6; side++) {
//					for (int sideIndex = 0; sideIndex < ring; sideIndex++) {
//						this._initCell (this._calculateCellIndex(ring, side, sideIndex));
//					}
//				}				
//			}
		}

		private void _initCell(int index) {
			Vector2 position = this.getPositionForCell (index);
			this._cellPositions [index] = position;

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

		private void _initBlock(int index, int times) {
		
		}

		private void _destroyBlock(int index) {
			
		}

		public Vector2 getPositionForCell(int index) {
			if (index == 0) {
				return Vector2.zero;
			} 

			// which ring is it on
			int ring = (int)Mathf.Floor (0.5f + Mathf.Sqrt(3 * (4 * index - 1)) / 6);

			// index within the ring
			int ringIndex = index - getCellsForSize(ring);

			// which side of the ring is it on
			int side = (int)Mathf.Floor (ringIndex / (ring * 1.0f));

			// index within the side
			int sideIndex = ringIndex - side * ring;

			Vector2 basePosition = _sideVertexPositions[side];
			Vector2 biasDirection = _sideDirections[side];
			return (basePosition * ring + sideIndex * biasDirection) * (this.BoardProperties.CellSize * Mathf.Sin(Mathf.Deg2Rad * 60) * 2 + this.BoardProperties.CellGap);
		}

		private int _calculateCellIndex(int ring, int side, int sideIndex) {
			return getCellsForSize (ring) + ring * side + sideIndex;
		}
	}
}
