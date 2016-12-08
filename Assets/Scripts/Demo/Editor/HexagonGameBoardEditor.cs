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

		public override void OnInspectorGUI() {
			serializedObject.Update ();

			HexagonGameBoard hexagonGameBoard = this.target as HexagonGameBoard;

			this.DrawDefaultInspector ();

			GUILayoutOption[] labelOptions = new GUILayoutOption[]{ GUILayout.MinWidth(100) };

			EditorGUILayout.BeginVertical ();
			HexagonGameBoard.HexagonGameBoardProperties boardProperties = hexagonGameBoard.BoardProperties;

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
				EditorGUILayout.LabelField ("Unit Num: ", labelOptions);
				boardProperties.UnitNum = EditorGUILayout.IntField (boardProperties.UnitNum);
				EditorGUILayout.LabelField ("Require Times: ", labelOptions);
				boardProperties.RequireTimes = EditorGUILayout.IntField (boardProperties.RequireTimes);
				EditorGUILayout.LabelField ("Goal: " + boardProperties.UnitNum * Mathf.Pow(2, boardProperties.RequireTimes));
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.EndVertical ();
			}

			this._showCellProperties = EditorGUILayout.Foldout (this._showCellProperties, "Cell Properties");
			if (this._showCellProperties) {
				EditorGUILayout.BeginVertical ();
				HexagonGameBoard.HexagonGameCellProperties cellProperties = hexagonGameBoard.CellProperties;

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Empty Color: ", labelOptions);
				cellProperties.EmptyBackgroundColor = (Color)EditorGUILayout.ColorField (cellProperties.EmptyBackgroundColor);
				EditorGUILayout.LabelField ("Empty Image: ", labelOptions);
				cellProperties.EmptyBackgroundImage = EditorGUILayout.ObjectField (cellProperties.EmptyBackgroundImage, typeof(Texture), false) as Texture;
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Hexagon Mask: ", labelOptions);
				cellProperties.MaskImage = EditorGUILayout.ObjectField (cellProperties.MaskImage, typeof(Texture), false) as Texture;
				EditorGUILayout.LabelField ("Background Image: ", labelOptions);
				cellProperties.BackgroundImage = EditorGUILayout.ObjectField (cellProperties.BackgroundImage, typeof(Texture), false) as Texture;
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Text Font: ", labelOptions);
				cellProperties.ForegroundFont = EditorGUILayout.ObjectField (cellProperties.ForegroundFont, typeof(Font), false) as Font;
				EditorGUILayout.LabelField ("Text Size: ", labelOptions);
				cellProperties.ForegroundFontSize = EditorGUILayout.IntField (cellProperties.ForegroundFontSize);
				EditorGUILayout.LabelField ("Text Color: ", labelOptions);
				cellProperties.ForegroundFontColor = (Color)EditorGUILayout.ColorField (cellProperties.ForegroundFontColor);
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.EndVertical ();
			}
			if (GUILayout.Button ("Make Game Board")) {
				hexagonGameBoard.GetComponent<HexagonGameBoard> ().makeGameBoard ();
			}

			serializedObject.ApplyModifiedProperties ();
		}
	}
}
