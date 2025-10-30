using UnityEngine;
using UnityEngine.Playables;

public class PaintingPlacement : MonoBehaviour
{

    [SerializeField] public Transform newPlacement;
    [SerializeField] public Transform correctPlacement;
    [SerializeField] public PlayableDirector triggeredEvent;
    private GameObject placementIcon;

    [SerializeField] private PuzzleManager myManager;
    public bool correctlyPlaced = false; //determines if it is both correctly placed
    public bool hasMissingPiece = false; //does this painting have a missing piece in it?
    public bool placedMissingPiece = false; //if the painting DOES have a missing piece, has it been placed?
    public bool completed = false;


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PaintingSpot")
        {
            //Debug.Log("I'm colliding with a valid place");
            newPlacement = col.gameObject.transform;
            placementIcon = newPlacement.transform.GetChild(1).gameObject;
            placementIcon.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log("I'm no longer colliding with a valid place");
        newPlacement = null;
        if (placementIcon != null)
        {
            placementIcon.SetActive(false);
            placementIcon = null;
        }
        
    }


    public void placed()
    {
        if (placementIcon != null)
        {
            placementIcon.SetActive(false);
            placementIcon = null;
        }

        
        if (newPlacement == correctPlacement)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            correctlyPlaced = true;
            Debug.Log("I am Correctly Placed");

            if (!hasMissingPiece) //if there is no piece missing from the painting, then it's complete!
            {
                completed = true;
                myManager.checkPlacement();
            }else if (hasMissingPiece && placedMissingPiece) //if there was a missing piece but it's in the painting now, it's complete! 
            {
                completed = true;
                myManager.checkPlacement(); 
            }
        }
    }


    public void missingPiecePlaced()
    {
        placedMissingPiece = true;
        completed = true;

        //change the painting based on the added missing piece 
        if (triggeredEvent != null)
        {
            triggeredEvent.Play();
        }

        if (correctlyPlaced)
        {
            completed = true;
        }
        myManager.checkPlacement();
    }

}


