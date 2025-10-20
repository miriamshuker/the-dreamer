using UnityEngine;
using UnityEngine.Playables;

public class KeyScript : MonoBehaviour
{

    public Transform myLock;
    public PlayableDirector triggeredEvent;
    private GameObject placementIcon;
    public bool correctLock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Lock" && col.gameObject.transform == myLock)
        {
            Debug.Log("I'm colliding with the correct Lock");
            placementIcon = myLock.transform.GetChild(0).gameObject;
            placementIcon.SetActive(true);
            correctLock = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (placementIcon != null)
        {
            placementIcon.SetActive(false);
            placementIcon = null;
        }
        correctLock = false;
        
    }


    public void keyPlaced()
    {
        placementIcon.SetActive(false);
        placementIcon = null;
        gameObject.layer = LayerMask.NameToLayer("Default");
        if (triggeredEvent != null)
        {
            triggeredEvent.Play();
        }
    }

}
