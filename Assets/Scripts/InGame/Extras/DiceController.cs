using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


    private List<Vector3> dirs = new List<Vector3>(new Vector3[]
    {
        Vector3.left,
        Vector3.back,
        Vector3.up,
        Vector3.down,
        Vector3.forward,
        Vector3.right
    });
    public List<Vector3> rotations = new List<Vector3>();
    private Vector3 upDir;

    private Vector3 targetPos;

    public Vector3 shownPos;
    public Vector3 hiddenPos;

    public int currentNum = 0;

    public static DiceController dice;

    public bool hidden = true;
    public bool rolled = false;
    public bool rolling = false;

    private bool tapped = false;
    private bool validRoll = false;

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
        if (!hidden && !rolled && !rolling)
        {
            if (tapped)
            {
                tapped = !tapped;
                return;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                validRoll = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        validRoll = true;
                        touchTimeStart = Time.time;
                        startPos = Input.GetTouch(0).position;
                    }
                }
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (validRoll)
                {
                    touchTimeFinish = Time.time;
                    timeInterval = touchTimeFinish - touchTimeStart;

                    if (timeInterval < 0.16f) timeInterval = 0.16f;

                    endPos = Input.GetTouch(0).position;

                    direction = startPos - endPos;

                    transform.GetComponent<Rigidbody>().isKinematic = false;
                    transform.GetComponent<Rigidbody>().AddForce(-direction.x * throwForceInX, -direction.y * throwForceInY, throwForceInZ / timeInterval);


                    rolling = true;

                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    transform.GetComponent<Rigidbody>().AddTorque(new Vector3(timeInterval * 10, 0, 0), ForceMode.Impulse);

                    transform.SetParent(null);
                }
            }
        }

        if (rolling)
        {
            if (transform.position.y < BoardController.board.transform.position.y)
            {
                Standby();
                return;
            }

            if (transform.GetComponent<Rigidbody>().velocity.magnitude < 0.001 && transform.position.y < 1)
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                rolled = true;
                rolling = false;
                transform.SetParent(Camera.main.transform);

                currentNum = GetUpSide() + 1;
            }
        }

        if (rolled && !hidden)
        {
            if (transform.localPosition == new Vector3(0, -0.3f, 1.0f))
            {
                if (BoardController.board.state == BoardController.BoardState.DiceRoll)
                {
                    BoardController.board.RolledNumber(currentNum);
                }
            }
        }
    }

    private void Animate()
    {

        if (rolling) return;

        if (rolled)
        {
            if (Vector3.Distance(transform.eulerAngles, rotations[currentNum - 1]) < 0.01)
            {
                transform.eulerAngles = rotations[currentNum - 1];
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotations[currentNum - 1]), 10);
            }
        }
        else
        {
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 1.0f, 1.0f);
        }

        if (Vector3.Distance(transform.localPosition, targetPos) > 0.01)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.2f);
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

    private int GetUpSide()
    {
        int closestDir = 0;
        float closestDot = -1;

        for (var i = 0; i < dirs.Count; i++)
        {
            float dot = Vector3.Dot(transform.TransformDirection(dirs[i]), Vector3.up);

            if (dot > closestDot)
            {
                closestDot = dot;
                closestDir = i;
            }
        }

        return closestDir;
    }
}
