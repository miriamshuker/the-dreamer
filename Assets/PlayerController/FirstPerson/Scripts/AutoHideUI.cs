using UnityEngine;

public class TriggerHideUI : MonoBehaviour
{
    [Tooltip("UI")]
    public GameObject uiToHide;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (uiToHide != null)
                uiToHide.SetActive(false);
        }
    }
}
