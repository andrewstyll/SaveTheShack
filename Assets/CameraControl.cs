using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // camera control
    private Camera mainCamera;
    private const float MAX_SIZE = 5.0f; // max size of orthographic view
    private float MIN_SIZE = 2.0f; // min size of orthographic view
    private const float DAMP_TIME = 0.2f; // time to perform zoom transitons
    private const float EPSILON = 0.1f; // floating point error

    private float zoomSpeed;
    private Vector3 moveVelocity;


    private float moveTimeBuffer = 1.0f;
    private Vector3 snapPosition;
    private bool SNAP_FLAG = false;

    private void Awake() {
        this.mainCamera = GetComponentInChildren<Camera>();

        DayUI.NotifyCalendarSelectDay += SetSnapFlag;
        MonthUI.NotifyCurrentDay += SetSnapPosition;
    }

    // Start is called before the first frame update
    void Start() {

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
            }
        }
    }

    private void OnDestroy() {
        DayUI.NotifyCalendarSelectDay -= SetSnapFlag;
        MonthUI.NotifyCurrentDay -= SetSnapPosition;
    }

    // zooms camera to focus on the gameobject argument
    private void PinchZoom() {
        // use scroll wheel or two touch detection to modify orthographic size
        float zoomDelta = (-1)*Input.mouseScrollDelta.y*0.5f;
        if(mainCamera.orthographicSize + zoomDelta > MAX_SIZE) {
            mainCamera.orthographicSize = MAX_SIZE;
        } else if(mainCamera.orthographicSize + zoomDelta < MIN_SIZE) {
            mainCamera.orthographicSize = MIN_SIZE;
        } else {
            mainCamera.orthographicSize += zoomDelta;
        }
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
