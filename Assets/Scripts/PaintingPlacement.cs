using UnityEngine;
using UnityEngine.Playables; 

public class PaintingPlacement : MonoBehaviour
{

    public Transform newPlacement;
    public Transform correctPlacement;
    public PlayableDirector triggeredEvent;
    
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
        }
    }

    void OnCollisionExit(Collision col)
    {
        Debug.Log("I'm no longer colliding with a valid place");
        newPlacement = null;
    }

    public void correctTrigger()
    {
        Debug.Log("I have been placed in the correct spot!");
        gameObject.layer = LayerMask.NameToLayer("Default");
        if (triggeredEvent != null)
        {
            triggeredEvent.Play();  
        }
    }

}
