using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateCam : MonoBehaviour
{

    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    private bool allowMove = true;

    private enum CamState
    {
        moving, zooming, none
    }

    private CamState state = CamState.none;

    public float movedistthresh = 2;
    private float movedist = 0;

    public float sens;

    void Update()
    {
        if (Input.touchCount != 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                allowMove = !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (movedist < movedistthresh && allowMove) CreationController.cc.Deselect();
                movedist = 0;
                allowMove = true;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                movedist += (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition).magnitude;
            }
        }

        if (!allowMove) return;

        switch (state)
        {
            case CamState.moving:
                if (Input.touchCount == 2)
                {
                    state = CamState.zooming;
                    break;
                }
                if (Input.touchCount == 0)
                {
                    state = CamState.none;
                    break;
                }

                if (movedist > movedistthresh)
                {
                    Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.transform.position.y - CreationController.cc.backgroundPlane.position.y));
                    Camera.main.transform.position += direction;
                }
                break;
            case CamState.zooming:
                if (Input.touchCount == 1)
                {
                    break;
                }
                if (Input.touchCount == 0)
                {
                    state = CamState.none;
                    break;
                }

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.02f);
                break;
            case CamState.none:
                if (Input.touchCount != 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        break;
                    }

                    touchStart = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.transform.position.y-CreationController.cc.backgroundPlane.position.y));
                    state = CamState.moving;
                }
                break;
        }
    }

    void zoom(float increment)
    {
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(Camera.main.transform.position.y - increment, zoomOutMin, zoomOutMax), Camera.main.transform.position.z);
    }

}
