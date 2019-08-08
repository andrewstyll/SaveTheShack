using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* dictates the camera behaviour based on user interaction. Handles pinch zooming,
 * swipe movement and snap zooming on a target
 */
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
    private const float DAMP_TIME = 0.4f; // time to perform SnapZoom transitons
    private bool SNAP_FLAG = false; // flag to signal if regular controls or SnapZoom is happening
    private float zoomSpeed = 0.0f; // SnapZoom zoom speed
    private Vector3 snapPosition; // position of the current day transform
    private Vector3 moveVelocity; // SnapZoom scroll speed

    // PinchZoom variables
    private bool ACTIVE_PINCH_ZOOM; // is there currently a pinch zoom being performed?
    private Vector2[] lastTouches; // store the last touch positions of pinch zoom to get delta with new ones
    private float speedMousePinchZoom = 0.5f; // zoom speed when using a scroll button (faster, so faster speed)
    private float speedTouchPinchZoom = 0.0005f; // zoom speed when using a phone

    // DragMove variables
    private int touchID; // id used to identify same finger performing touch
    private Vector3 dragStartPosition; // starting position of the touch/mouseclick before the drag
    private float dragSpeed = 5.0f; // speed that the screen will follow the dragged finger/mouse

    // time delay before a user is allowed to interact with the calendar/before SnapZoom occurs
    private float moveTimeBuffer = 1.0f; 

    // block user input
    private bool blockInput = false;

    private void Awake() {
        this.mainCamera = GetComponentInChildren<Camera>();

        DayUI.NotifyCalendarSelectDay += SetSnapFlag;
        MonthUI.NotifyCurrentDay += SetSnapPosition;
        CalendarUI.BlockInput += BlockInput;
    }

    // Start is called before the first frame update
    void Start() {
        // set screen boundaries
        this.maxBounds = this.mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        this.minBounds = this.mainCamera.ScreenToWorldPoint(new Vector3(0, 0));
    }

    // Update is called once per frame, fixed update for undelayed update time since timing is important
    // for smooth camera zoom
    private void FixedUpdate() {
        if(moveTimeBuffer > 0) {
            moveTimeBuffer -= Time.deltaTime;
        } else {

            if (SNAP_FLAG) {
                SnapZoom();
                MaintainBounds();
                // once we are close enough to the zoom target, stop the snap zoom
                if (mainCamera.transform.position == snapPosition && 
                    System.Math.Abs(mainCamera.orthographicSize - zoomMinSize) < EPSILON ) {
                    SNAP_FLAG = false;
                }
            } else if(!blockInput) {
                // require 2 fingers for pinch zoom to occur
                if(Input.touchCount != 2) {
                    ACTIVE_PINCH_ZOOM = false;
                }
                PinchZoom();
                DragMove();
                MaintainBounds();
            }
        }
    }

    // remove event listeners to avoid listeners attached to null objects
    private void OnDestroy() {
        DayUI.NotifyCalendarSelectDay -= SetSnapFlag;
        MonthUI.NotifyCurrentDay -= SetSnapPosition;
        CalendarUI.BlockInput -= BlockInput;
    }

    private void DragMove() {
        if (Application.platform == RuntimePlatform.Android) {
            // use TouchPhase class to determine the state of the current touch (touchDrag, touch has just started etc...)
            // use that to determine a start and finish point for the camera.
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
            // on mouse drag, move the camera so that it follows the velocity of the mouse
            if (Input.GetMouseButtonDown(0)) {
                dragStartPosition = Input.mousePosition;
            } else if (Input.GetMouseButton(0)) {
                DragMoveLogic(Input.mousePosition);
            }
        }
    }

    // determine the differenct between the start position of the drag and the new position. 
    // use the offset*drag speed per frame to determine how much to move by (this method is called on update/fixedupdate)
    private void DragMoveLogic(Vector3 inputPosition) {
        Vector3 offset = mainCamera.ScreenToViewportPoint(dragStartPosition - inputPosition);
        Vector3 move = new Vector3(offset.x * dragSpeed, offset.y * dragSpeed);

        mainCamera.transform.Translate(move, Space.World);

        // update dragstartposition as the input position to move the starting point (useful for drags that change direction)
        dragStartPosition = inputPosition;
    }

    // zooms camera on center of screen
    private void PinchZoom() {
        // use scroll wheel or two touch detection to modify orthographic size
        if (Application.platform == RuntimePlatform.Android) {
            // on two touch, store touch position. if two fingers are still down, get the difference of those positions and 
            // calculate the distance to zoom based on the pinch zoom speed and the distance the fingers have moved. 
            // zoom is in units relative to orthographic camerasize
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
            // scrolling with mousewheel uses an input property so no need for calculation
            float zoomDelta = Input.mouseScrollDelta.y;
            PinchZoomLogic(zoomDelta * speedMousePinchZoom);
        }
    }

    // zoom within the min and max allowable camera sizes
    private void PinchZoomLogic(float zoomDelta) {
        if (mainCamera.orthographicSize + zoomDelta > zoomMaxSize) {
            mainCamera.orthographicSize = zoomMaxSize;
        } else if (mainCamera.orthographicSize + zoomDelta < zoomMinSize) {
            mainCamera.orthographicSize = zoomMinSize;
        } else {
            mainCamera.orthographicSize += zoomDelta;
        }
    }

    // ensure that the camera doesn't exit the boundaries set by the background
    private void MaintainBounds() {
        float height = 2f * this.mainCamera.orthographicSize; // this math is constant for orthoSize use
        float width = height * this.mainCamera.aspect;

        Vector3 pos = mainCamera.transform.position;
        pos.x = Mathf.Clamp(mainCamera.transform.position.x, minBounds.x + (width / 2), maxBounds.x - (width / 2));
        pos.y = Mathf.Clamp(mainCamera.transform.position.y, minBounds.y + (height / 2), maxBounds.y - (height / 2));
        mainCamera.transform.position = pos;
    }
   
    // snap to the current day, scaling the camera position and orthographic size
    private void SnapZoom() {
        // smoothdamp allows for a smooth transition from the first argument to the second
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoomMinSize, ref zoomSpeed, DAMP_TIME);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, snapPosition, ref moveVelocity, DAMP_TIME);
    }

    private void SetMinMaxZoomSize(Transform currentDay) {
        // y position may have negative values and zoomMinSize can only be positive
        this.zoomMinSize = System.Math.Abs(currentDay.position.y / 2.0f); 
        this.zoomMaxSize = this.maxBounds.y;
    }

    /**** Events ****/

    // set the snap position based on the current day transform to provide a 
    // snap zoom location and to also calculate the zoom min size 
    private void SetSnapPosition(Transform currentDay) {
        this.snapPosition = new Vector3(currentDay.position.x,
                                        currentDay.position.y,
                                        this.mainCamera.transform.position.z);
        //SetMinMaxZoomSize(currentDay);
        SNAP_FLAG = true;
    }

    private void SetSnapFlag() {
        SNAP_FLAG = true;
    }

    // blocks user input
    private void BlockInput(bool blockInput) {
        this.blockInput = blockInput;
    }
}
