using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

public class PuzzleManager : MonoBehaviour
{
    public PlayableDirector triggeredEvent;
    [SerializeField] private List<PaintingPlacement> puzzlePaintings = new List<PaintingPlacement>();
    private int numOfCorrectPaintings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkPlacement()
    {
        numOfCorrectPaintings = 0;
        //Debug.Log("Check Placement Called");

        for (int i = 0; i < puzzlePaintings.Count; i++)
        {
            if (puzzlePaintings[i].completed)
            {
                numOfCorrectPaintings++;
            }
        }

        
        Debug.Log("Completed Paintings: " + numOfCorrectPaintings + " out of " + puzzlePaintings.Count);

        if (numOfCorrectPaintings == puzzlePaintings.Count)
        {
            triggeredEvent.Play();
        }

    }

}
