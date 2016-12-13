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
	public float _dragThreshold = 15;
	private bool _dragged = false;

	public void OnBeginDrag(PointerEventData data) {
		this._dragged = false;
		this._dragBeginPosition = data.position;
		// Debug.Log (" @ HexagonGameInputHandler.OnBeginDrag(): " + data.position.ToString());
	}

	public void OnDrag(PointerEventData data) {
		if (!this._dragged) {
			if ((data.position - this._dragBeginPosition).magnitude > this._dragThreshold) {
				this._drag (data.position - this._dragBeginPosition);
			}
		}
	}

	public void OnEndDrag(PointerEventData data) {
		this._dragEndPosition = data.position;
		if (!this._dragged) {
			this._drag (this._dragEndPosition - this._dragBeginPosition);
		}
	}

	private void _drag(Vector2 dragVector) {
		this._dragged = true;
		this.SendMessage ("HandleDrag", dragVector, SendMessageOptions.DontRequireReceiver);
	}
}
