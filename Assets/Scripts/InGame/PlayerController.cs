using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName;
    public Color color;

    public Transform place;
    public int score = 0;

    private Vector3 targetLocation;
    private Vector3 cardViewTarget;
    private int rollNumber = -1;
    private Vector3 velocity = Vector3.zero;

    private bool waiting = false;

    private void Update()
    {
        Animate();
        Process();
    }

    private void Animate()
    {

        transform.position = Vector3.SmoothDamp(transform.position, targetLocation, ref velocity, 0.1f);
        if (Vector3.Distance(transform.position, targetLocation) < 0.01) transform.position = targetLocation;
    }

    private void Process()
    {
        if (waiting) return;
        if (transform.position != targetLocation) return;

        rollNumber--;
        List<Transform> exits = place.GetComponent<NodeController>().exitTransforms;

        if (exits.Count == 1)
        {
            place = exits[0];
        }
        else
        {
            GameManager.gm.SetGameState(GameManager.GameState.ChoosePath);
            waiting = true;
        }
    }

    public void Initiate(string playerName, Color color)
    {
        this.playerName = playerName;
        this.color = color;

        transform.GetComponent<MeshRenderer>().material.color = this.color;

        AssignPlace(transform.parent.parent.GetChild(0).GetChild(0));
    }

    public void SelectPath(Transform path)
    {
        waiting = false;
        place = path.GetComponent<PathAnimator>().destination;
    }

    public void AssignPlace(Transform place)
    {
        this.place = place;

        float angle = Random.value * Mathf.PI * 2;
        float dist = Random.value * 1.6f;

        targetLocation = place.position + new Vector3(Mathf.Cos(angle) * dist, 0.1f, Mathf.Sin(angle) * dist);
    }

}
