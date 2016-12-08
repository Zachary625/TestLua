using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace HexagonGame {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class HexagonGameBoard : MonoBehaviour {
		public class HexagonGameBoardProperties {
			public float CellSize;
			public float CellGap;

			private Vector2[] _sideVertexPositions = new Vector2[] {
				new Vector2(Mathf.Cos(0), Mathf.Sin(0)),			
				new Vector2(Mathf.Cos(60), Mathf.Sin(60)),			
				new Vector2(Mathf.Cos(120), Mathf.Sin(120)),			
				new Vector2(Mathf.Cos(180), Mathf.Sin(180)),			
				new Vector2(Mathf.Cos(240), Mathf.Sin(240)),			
				new Vector2(Mathf.Cos(300), Mathf.Sin(300)),			
			};

			private Vector2[] _sideDirections = new Vector2[] {
				new Vector2(Mathf.Cos(120), Mathf.Sin(120)),			
				new Vector2(Mathf.Cos(180), Mathf.Sin(180)),			
				new Vector2(Mathf.Cos(240), Mathf.Sin(240)),			
				new Vector2(Mathf.Cos(300), Mathf.Sin(300)),			
				new Vector2(Mathf.Cos(0), Mathf.Sin(0)),			
				new Vector2(Mathf.Cos(60), Mathf.Sin(60)),			
			};

			public int Size;
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
				int ringIndex = (index - getCellsForSize(ring - 1));
				int side = (int)Mathf.Floor (ringIndex / (ring * 1.0f));
				int sideIndex = ringIndex - side * 6;
				Vector2 basePosition = this._sideVertexPositions[side];
				Vector2 biasDirection = this._sideDirections[side];
				return (basePosition + sideIndex * biasDirection) * (this.CellSize + this.CellGap / 2.0f);
			}

			public float MoveTime;
			public float GenerateTime;

			public int UnitNum;
			public int RequireTimes;
		}



		public HexagonGameBoardProperties BoardProperties = new HexagonGameBoardProperties();
		public class HexagonGameCellProperties
		{
			public RawImage MaskImage;
			public RawImage BackgroundImage;
			public Font ForegroundFont;
			public int ForegroundFontSize;
			public Color ForegroundFontColor;
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


		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

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
			// detach all children
			// destroy/buffer children
		}

		private void _initCells() {

		}

	}
}
