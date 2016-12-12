using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HexagonGameInputHandler : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private Vector2 _dragBeginPosition;
	private Vector2 _dragEndPosition;

	public void OnBeginDrag(PointerEventData data) {
		this._dragBeginPosition = data.position;
		// Debug.Log (" @ HexagonGameInputHandler.OnBeginDrag(): " + data.position.ToString());
	}

	public void OnDrag(PointerEventData data) {
	}

	public void OnEndDrag(PointerEventData data) {
		this._dragEndPosition = data.position;
		// Debug.Log (" @ HexagonGameInputHandler.OnEndDrag(): " + data.position.ToString());
		this.SendMessage ("HandleDrag", this._dragEndPosition - this._dragBeginPosition, SendMessageOptions.DontRequireReceiver);
	}
}
