using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class pieceScript : MonoBehaviour
{

    public Transform myPainting;
    public PaintingPlacement myPaintingPlacementScript;
    public PlayableDirector triggeredEvent;
    public Pickup puScript;
    
    
    void Start()
    {
        
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform == myPainting)
        {
            Debug.Log("ItemSuccessfullyPlaced");
            puScript.heldRb = null;
            puScript.heldObject = null;
            myPaintingPlacementScript.missingPiecePlaced();

            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider col)
    {
        
    }


    public void itemPlaced()
    {
        
    }
}
