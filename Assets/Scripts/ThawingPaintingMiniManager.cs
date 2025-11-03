using UnityEngine;

public class ThawingPaintingMiniManager : MonoBehaviour
{
    private int timesCalled = 0;
    public  GameObject breadTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void otherPaintingsCorrect()
    {
        timesCalled++;
        Debug.Log("Correctly called: " + timesCalled);
        if (timesCalled == 2)
        {
            breadTrigger.SetActive(true);
        }
    }
}
