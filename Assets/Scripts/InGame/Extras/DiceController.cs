using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
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

    public Vector3 shownPos;
    public Vector3 hiddenPos;

    public int currentNum = 0;

    public static DiceController dice;

    public bool hidden = true;
    public bool rolled = false;
    public bool rolling = false;

    private bool tapped = false;

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
        dice = this;
        Hide();
    }

    private void Process()
    {
        if (!hidden && !rolled)
        {
            if (tapped)
            {
                tapped = !tapped;
                return;
            }

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

                rolling = true;

                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }

        if (rolling)
        {
            if (transform.position.y < 0)
            {
                Standby();
                return;
            }

            if (transform.GetComponent<Rigidbody>().velocity.magnitude < 0.001 && transform.position.y < 1)
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rolled = true;
                rolling = false;

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
            }
        }

        if (rolled && !hidden)
        {
            if (transform.localPosition == new Vector3(0, -0.3f, 1.0f))
            {
                if (BoardController.state == BoardController.BoardState.DiceRoll)
                {
                    BoardController.RolledNumber(currentNum);
                    print("Rolled: " + currentNum);
                }
            }
        }
    }

    private void Animate()
    {

        if (rolling) return;

        if (rolled)
        {
            if (Vector3.Distance(transform.eulerAngles, sideAngles[currentNum - 1]) < 0.01)
            {
                transform.eulerAngles = sideAngles[currentNum - 1];
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(sideAngles[currentNum - 1]), 4);
            }
        }
        else
        {
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 1.0f, 1.0f);
        }

        if (Vector3.Distance(transform.localPosition, targetPos) > 0.01)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.1f);
        }
        else
        {
            transform.localPosition = targetPos;
        }
    }

    public void Standby()
    {
        Show();
        rolled = false;
        tapped = true;
        rolling = false;
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        currentNum = 0;
        transform.rotation = Random.rotation;
    }

    public void Show()
    {
        targetPos = shownPos;
        hidden = false;
    }

    public void Hide()
    {
        targetPos = hiddenPos;
        hidden = true;
    }
}
