using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // camera control
    private Camera mainCamera;

    // Boundaries and sizing variables
    private Vector3 minBounds;
    private Vector3 maxBounds;

    // blah blah blah
    private const float MAX_SIZE = 5.0f; // max size of orthographic view
    private float MIN_SIZE = 2.0f; // min size of orthographic view
    private const float DAMP_TIME = 0.2f; // time to perform zoom transitons
    private const float EPSILON = 0.1f; // floating point error

    // SnapZoom variables
    private Vector3 snapPosition;
    private bool SNAP_FLAG = false;
    private float zoomSpeed; // snapZoom zoom speed
    private Vector3 moveVelocity; // snapZoom scroll speed

    // DragMove variables
    private Vector3 dragStartPosition;
    private Vector3 dragVelocity;
    private float dragSpeed = 10.0f;

    private float moveTimeBuffer = 1.0f;

    private void Awake() {
        this.mainCamera = GetComponentInChildren<Camera>();

        DayUI.NotifyCalendarSelectDay += SetSnapFlag;
        MonthUI.NotifyCurrentDay += SetSnapPosition;
    }

    // Start is called before the first frame update
    void Start() {
        this.maxBounds = this.mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        this.minBounds = this.mainCamera.ScreenToWorldPoint(new Vector3(0, 0));
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if(moveTimeBuffer > 0) {
            moveTimeBuffer -= Time.deltaTime;
        } else {

            if (SNAP_FLAG) {
                SnapZoom();
                if (mainCamera.transform.position == snapPosition && 
                    System.Math.Abs(mainCamera.orthographicSize - MIN_SIZE) < EPSILON) {
                    SNAP_FLAG = false;
                }
            } else {
                PinchZoom();
                DragMove();
                MaintainBounds();
            }
        }
    }

    private void OnDestroy() {
        DayUI.NotifyCalendarSelectDay -= SetSnapFlag;
        MonthUI.NotifyCurrentDay -= SetSnapPosition;
    }

    private void DragMove() {
        // on mouse drag, move the camera so that it follows the velocity of the mouse/finger

        Vector3 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0)) {
            dragStartPosition = mousePosition;
        } else if(Input.GetMouseButton(0)) {
            Vector3 offset = mainCamera.ScreenToViewportPoint(dragStartPosition - mousePosition);
            Vector3 move = new Vector3(offset.x * dragSpeed, offset.y * dragSpeed);

            mainCamera.transform.Translate(move, Space.World);

            dragStartPosition = mousePosition;
        }

    }

    // zooms camera to focus on the gameobject argument
    private void PinchZoom() {
        // use scroll wheel or two touch detection to modify orthographic size
        float zoomDelta = (-1) * Input.mouseScrollDelta.y * 0.5f;
        if (mainCamera.orthographicSize + zoomDelta > MAX_SIZE) {
            mainCamera.orthographicSize = MAX_SIZE;
        } else if (mainCamera.orthographicSize + zoomDelta < MIN_SIZE) {
            mainCamera.orthographicSize = MIN_SIZE;
        } else {
            mainCamera.orthographicSize += zoomDelta;
        }
    }

    private void MaintainBounds() {
        float height = 2f * this.mainCamera.orthographicSize;
        float width = height * this.mainCamera.aspect;

        Vector3 pos = mainCamera.transform.position;
        pos.x = Mathf.Clamp(mainCamera.transform.position.x, minBounds.x + (width / 2), maxBounds.x - (width / 2));
        pos.y = Mathf.Clamp(mainCamera.transform.position.y, minBounds.y + (height / 2), maxBounds.y - (height / 2));
        mainCamera.transform.position = pos;
    }

    private void GetRequiredSize() {

    }
   
    private void SnapZoom() {
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, MIN_SIZE, ref zoomSpeed, DAMP_TIME);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, snapPosition, ref moveVelocity, DAMP_TIME);
    }

    private void SetMinZoomSize(Transform currentDay) {
        //this.MIN_SIZE = 
    }

    /**** Events ****/

    // set the snap position based on the current day transform to provide a 
    // snap zoom location and to also calculate the zoom min size 
    private void SetSnapPosition(Transform currentDay) {
        this.snapPosition = new Vector3(currentDay.position.x,
                                        currentDay.position.y,
                                        this.mainCamera.transform.position.z);
        SetMinZoomSize(currentDay);
        SNAP_FLAG = true;
    }

    private void SetSnapFlag() {
        SNAP_FLAG = true;
    }
}
