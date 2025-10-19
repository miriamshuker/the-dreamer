using UnityEngine;
using UnityEngine.Playables;

public class PaintingPlacement : MonoBehaviour
{

    public Transform newPlacement;
    public Transform correctPlacement;
    public PlayableDirector triggeredEvent;
    private GameObject placementIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "PaintingSpot")
        {
            Debug.Log("I'm colliding with a valid place");
            newPlacement = col.gameObject.transform;
            placementIcon = newPlacement.transform.GetChild(1).gameObject;
            placementIcon.SetActive(true);
        }
    }

    void OnCollisionExit(Collision col)
    {
        Debug.Log("I'm no longer colliding with a valid place");
        newPlacement = null;
        if (placementIcon != null)
        {
            placementIcon.SetActive(false);
            placementIcon = null;
        }
        
    }


    public void placed()
    {
        placementIcon.SetActive(false);
        placementIcon = null;
        if (newPlacement ==correctPlacement)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        if (triggeredEvent != null)
        {
            triggeredEvent.Play();
        }
        }
    }

}
