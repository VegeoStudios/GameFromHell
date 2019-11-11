using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    private enum DiceState
    {
        Hidden,
        Standby,
        Rolling,
        Rolled
    }

    private DiceState state;

    private Vector2 startPos, endPos, direction;
    private float touchTimeStart, touchTimeFinish, timeInterval;

    [SerializeField]
    float throwForceInX = 1f;

    [SerializeField]
    float throwForceInY = 1f;

    [SerializeField]
    float throwForceInZ = 50f;

    public Vector3[] sideAngles;

    private Vector3 targetPos;

    public int currentNum = 0;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Process();
        Animate();
    }

    private void Init()
    {
        state = DiceState.Hidden;
    }

    private void Process()
    {
        switch (state)
        {
            case DiceState.Hidden:

                break;
            case DiceState.Standby:
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -0.3f, 1.0f), 0.1f);
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 1.0f, 1.0f);

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    touchTimeStart = Time.time;
                    startPos = Input.GetTouch(0).position;
                }
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    touchTimeFinish = Time.time;
                    timeInterval = touchTimeFinish - touchTimeStart;

                    if (timeInterval < 0.16f) timeInterval = 0.16f;

                    endPos = Input.GetTouch(0).position;

                    direction = startPos - endPos;

                    transform.GetComponent<Rigidbody>().isKinematic = false;
                    transform.GetComponent<Rigidbody>().AddForce(-direction.x * throwForceInX, -direction.y * throwForceInY, throwForceInZ / timeInterval);
                    transform.GetComponent<Rigidbody>().AddTorque(-direction.x * throwForceInX, -direction.y * throwForceInY, throwForceInZ / timeInterval);

                    state = DiceState.Rolling;

                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                break;
            case DiceState.Rolling:
                if (transform.GetComponent<Rigidbody>().velocity.magnitude < 0.001 && transform.position.y < 4)
                {
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    state = DiceState.Rolled;

                    int closestnum = 0;
                    float dist = 10000;

                    for (int i = 1; i <= 6; i++)
                    {
                        if (Vector3.Distance(transform.eulerAngles, sideAngles[i - 1]) < dist)
                        {
                            dist = Vector3.Distance(transform.eulerAngles, sideAngles[i - 1]);
                            closestnum = i;
                        }
                    }
                    currentNum = closestnum;
                    print("Number Rolled: " + closestnum);
                }
                break;
            case DiceState.Rolled:
                if (transform.localPosition == new Vector3(0, -0.3f, 1.0f))
                {

                }
                break;
        }
    }

    private void Animate()
    {
        if (state != DiceState.Standby && state != DiceState.Rolled)
        {
            return;
        }
        if (state == DiceState.Standby) transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 1.0f, 1.0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -0.3f, 1.0f), 0.1f);

        switch (state)
        {
            case DiceState.Hidden:
                if (Vector3.Distance(transform.localPosition, new Vector3(0, -1, 1.0f)) < 0.01)
                    transform.localPosition = new Vector3(0, -1, 1.0f);
                else
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -1, 1.0f), 0.1f);
                break;
            case DiceState.Standby:
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 1.0f, 1.0f);
                if (Vector3.Distance(transform.localPosition, new Vector3(0, -0.3f, 1.0f)) < 0.01)
                    transform.localPosition = new Vector3(0, -0.3f, 1.0f);
                else
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -0.3f, 1.0f), 0.1f);
                break;
            case DiceState.Rolling:
                break;
            case DiceState.Rolled:
                if (Vector3.Distance(transform.localPosition, new Vector3(0, -0.3f, 1.0f)) < 0.01)
                    transform.localPosition = new Vector3(0, -0.3f, 1.0f);
                else
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -0.3f, 1.0f), 0.1f);

                if (currentNum == 0) break;
                if (Vector3.Distance(transform.eulerAngles, sideAngles[currentNum - 1]) < 0.01)
                    transform.eulerAngles = sideAngles[currentNum - 1];
                else
                    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, sideAngles[currentNum - 1], 0.1f);

                break;
        }
    }

    public void Standby()
    {
        state = DiceState.Standby;
        transform.rotation = Random.rotation;
    }
}
