using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // camera control
    private Camera mainCamera;

    // Boundaries and sizing variables
    private Vector3 minBounds; // min bounds of the background
    private Vector3 maxBounds; // max bounds of the backgrounds
    private float zoomMaxSize = 5.0f; // max size of orthographic view
    private float zoomMinSize = 2.0f; // min size of orthographic view

    private const float EPSILON = 0.1f; // floating point error

    // SnapZoom variables
    private const float DAMP_TIME = 0.2f; // time to perform SnapZoom transitons
    private bool SNAP_FLAG = false; // flag to signal if regular controls or snapzoom is happening
    private float zoomSpeed; // snapZoom zoom speed
    private Vector3 snapPosition; // position of the current day transform
    private Vector3 moveVelocity; // snapZoom scroll speed

    // PinchZoom variables
    private bool ACTIVE_PINCH_ZOOM; // is there currently a pinch zoom being performed?
    private Vector2[] lastTouches; // store the last touch positions of pinch zoom to get delta with new ones
    private float speedMousePinchZoom = 0.5f; // zoom speed when using a scroll button (faster, so faster speed)
    private float speedTouchPinchZoom = 0.0005f; // zoom speed when using a phone

    // DragMove variables
    private int touchID; // id used to identify same finger performing touch
    private Vector3 dragStartPosition; // starting position of the touch/mouseclick before the drag
    private float dragSpeed = 5.0f; // speed that the screen will follow the dragged finger/mouse

    private float moveTimeBuffer = 1.0f; // time delay before a user is allowed to interact with the calendar

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
                    System.Math.Abs(mainCamera.orthographicSize - zoomMinSize) < EPSILON) {
                    SNAP_FLAG = false;
                }
            } else {
                if(Input.touchCount != 2) {
                    ACTIVE_PINCH_ZOOM = false;
                }
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
        if (Application.platform == RuntimePlatform.Android) {
            if(Input.touchCount == 1) {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary) {
                    dragStartPosition = touch.position;
                } else if( touch.phase == TouchPhase.Moved ) {
                    DragMoveLogic(touch.position);
                } else {
                    Debug.Log("Unused touch phase");
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0)) {
                dragStartPosition = Input.mousePosition;
            } else if (Input.GetMouseButton(0)) {
                DragMoveLogic(Input.mousePosition);
            }
        }
    }

    private void DragMoveLogic(Vector3 inputPosition) {
        Vector3 offset = mainCamera.ScreenToViewportPoint(dragStartPosition - inputPosition);
        Vector3 move = new Vector3(offset.x * dragSpeed, offset.y * dragSpeed);

        mainCamera.transform.Translate(move, Space.World);

        dragStartPosition = inputPosition;
    }

    // zooms camera to focus on the gameobject argument
    private void PinchZoom() {
        // use scroll wheel or two touch detection to modify orthographic size
        if (Application.platform == RuntimePlatform.Android) {
            if(Input.touchCount == 2) {
                Vector2[] newTouches = { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if(!ACTIVE_PINCH_ZOOM) {
                    lastTouches = newTouches;
                    ACTIVE_PINCH_ZOOM = true;
                } else {
                    float newDistance = Vector2.Distance(newTouches[0], newTouches[1]);
                    float oldDistance = Vector2.Distance(lastTouches[0], lastTouches[1]);
                    float zoomDelta = newDistance - oldDistance;
                    PinchZoomLogic((-1) * zoomDelta * speedTouchPinchZoom);
                }
            }
        } else {
            float zoomDelta = Input.mouseScrollDelta.y;
            PinchZoomLogic(zoomDelta * speedMousePinchZoom);
        }
    }

    private void PinchZoomLogic(float zoomDelta) {
        if (mainCamera.orthographicSize + zoomDelta > zoomMaxSize) {
            mainCamera.orthographicSize = zoomMaxSize;
        } else if (mainCamera.orthographicSize + zoomDelta < zoomMinSize) {
            mainCamera.orthographicSize = zoomMinSize;
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
   
    private void SnapZoom() {
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoomMinSize, ref zoomSpeed, DAMP_TIME);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, snapPosition, ref moveVelocity, DAMP_TIME);
    }

    private void SetMinMaxZoomSize(Transform currentDay) {
        this.zoomMinSize = currentDay.position.y / 2f;
        this.zoomMaxSize = this.maxBounds.y;
    }

    /**** Events ****/

    // set the snap position based on the current day transform to provide a 
    // snap zoom location and to also calculate the zoom min size 
    private void SetSnapPosition(Transform currentDay) {
        this.snapPosition = new Vector3(currentDay.position.x,
                                        currentDay.position.y,
                                        this.mainCamera.transform.position.z);
        SetMinMaxZoomSize(currentDay);
        SNAP_FLAG = true;
    }

    private void SetSnapFlag() {
        SNAP_FLAG = true;
    }
}
