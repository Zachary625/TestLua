using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using HexagonGame;

namespace HexagonGame.Editor {
	[CustomEditor(typeof(HexagonGameBoard))]
	public class HexagonGameBoardEditor : UnityEditor.Editor {
		private bool _showBoardProperties = true;
		private bool _showCellProperties = true;
		private bool _clearToMake = false;

		public override void OnInspectorGUI() {
			serializedObject.Update ();

			HexagonGameBoard hexagonGameBoard = this.target as HexagonGameBoard;
			HexagonGameBoard.HexagonGameBoardProperties boardProperties = hexagonGameBoard.BoardProperties;
			HexagonGameBoard.HexagonGameCellProperties cellProperties = hexagonGameBoard.CellProperties;

			this.DrawDefaultInspector ();

			GUILayoutOption[] labelOptions = new GUILayoutOption[]{ GUILayout.MinWidth(100) };

			this._clearToMake = true;

			EditorGUILayout.BeginVertical ();
			this._showBoardProperties = EditorGUILayout.Foldout (this._showBoardProperties, "Board Properties");
			if (this._showBoardProperties) {
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Size: ", labelOptions);
				boardProperties.Size = EditorGUILayout.IntField (boardProperties.Size);
				EditorGUILayout.LabelField ("Cell Size: ", labelOptions);
				boardProperties.CellSize = EditorGUILayout.FloatField (boardProperties.CellSize);
				EditorGUILayout.LabelField ("Cell Gap: ", labelOptions);
				boardProperties.CellGap = EditorGUILayout.FloatField (boardProperties.CellGap);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Move Time: ", labelOptions);
				boardProperties.MoveTime = EditorGUILayout.FloatField (boardProperties.MoveTime);
				EditorGUILayout.LabelField ("Generate Time: ", labelOptions);
				boardProperties.GenerateTime = EditorGUILayout.FloatField (boardProperties.GenerateTime);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("News Per Move: ", labelOptions);
				boardProperties.NewsPerMove = EditorGUILayout.IntField (boardProperties.NewsPerMove);
				EditorGUILayout.LabelField ("Unit Num: ", labelOptions);
				boardProperties.UnitNum = EditorGUILayout.IntField (boardProperties.UnitNum);
				EditorGUILayout.LabelField ("Require Times: ", labelOptions);
				boardProperties.RequireTimes = EditorGUILayout.IntField (boardProperties.RequireTimes);
				EditorGUILayout.LabelField ("Goal: " + HexagonGameBoard.getBlockNum(boardProperties.UnitNum, boardProperties.RequireTimes));
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.EndVertical ();
			}

			if (boardProperties.CellSize <= 0) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("Cell Size <= 0!", MessageType.Error);
			}
			if (boardProperties.CellGap < 0) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("Cell Gap < 0!", MessageType.Error);
			}
			if (boardProperties.NewsPerMove <= 0) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("News Per Move <= 0!", MessageType.Error);
			}
			if (boardProperties.UnitNum <= 0) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("Unit Num <= 0!", MessageType.Error);
			}
			if (boardProperties.RequireTimes <= 0) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("Require Times <= 0!", MessageType.Error);
			}

			this._showCellProperties = EditorGUILayout.Foldout (this._showCellProperties, "Cell Properties");
			if (this._showCellProperties) {
				EditorGUILayout.BeginVertical ();

				EditorGUILayout.LabelField ("Hexagon Mask: ", labelOptions);
				cellProperties.MaskImage = EditorGUILayout.ObjectField (cellProperties.MaskImage, typeof(Texture), false) as Texture;

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Cell Bg Color: ", labelOptions);
				cellProperties.CellBgColor = (Color)EditorGUILayout.ColorField (cellProperties.CellBgColor);
				EditorGUILayout.LabelField ("Cell Bg Image: ", labelOptions);
				cellProperties.CellBgImage = EditorGUILayout.ObjectField (cellProperties.CellBgImage, typeof(Texture), false) as Texture;
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Block Bg Color: ", labelOptions);
				cellProperties.BlockBgColor = (Color)EditorGUILayout.ColorField (cellProperties.BlockBgColor);
				EditorGUILayout.LabelField ("Block Bg Image: ", labelOptions);
				cellProperties.BlockBgImage = EditorGUILayout.ObjectField (cellProperties.BlockBgImage, typeof(Texture), false) as Texture;
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Fg Font: ", labelOptions);
				cellProperties.FgFont = EditorGUILayout.ObjectField (cellProperties.FgFont, typeof(Font), false) as Font;
				EditorGUILayout.LabelField ("Fg Font Size: ", labelOptions);
				cellProperties.FgFontSize = EditorGUILayout.IntField (cellProperties.FgFontSize);
				EditorGUILayout.LabelField ("Fg Font Color: ", labelOptions);
				cellProperties.FgFontColor = (Color)EditorGUILayout.ColorField (cellProperties.FgFontColor);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.EndVertical ();
			}

			if (cellProperties.MaskImage == null) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("Mask Image is null!", MessageType.Error);
			}
			if (cellProperties.CellBgImage == null) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("Cell Bg Image is null!", MessageType.Error);
			}
			if (cellProperties.BlockBgImage == null) {
				this._clearToMake = false;
				EditorGUILayout.HelpBox ("Block Bg Image is null!", MessageType.Error);
			}

			if (GUILayout.Button ("Make Game Board")) {
				if (this._clearToMake) {
					hexagonGameBoard.GetComponent<HexagonGameBoard> ().makeGameBoard ();
				}
			}

			if (GUILayout.Button ("Make Game Blocks")) {
				
			}

			serializedObject.ApplyModifiedProperties ();
		}
	}
}
