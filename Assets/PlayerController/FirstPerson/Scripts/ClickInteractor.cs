using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


public class ClickInteractor : MonoBehaviour
{
    [Header("Raycast")]
    public Camera cam;
    public float maxDistance = 4f;
    public LayerMask interactableMask = ~0;

    [Header("Debounce")]
    public float clickCooldown = 0.15f;
    private float _lastClickTime = -999f;

    private void Awake()
    {
        if (cam == null)
        {
            var go = GameObject.FindGameObjectWithTag("MainCamera");
            if (go) cam = go.GetComponent<Camera>();
        }
    }

    private bool GetLeftClickDownOnce()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    private void Update()
    {
        if (cam == null) return;
        if (Time.time - _lastClickTime < clickCooldown) return;
        if (!GetLeftClickDownOnce()) return;
        _lastClickTime = Time.time;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableMask, QueryTriggerInteraction.Ignore))
        {
            var ctrl = hit.collider.GetComponentInParent<ToyPicture>();
            if (ctrl != null) ctrl.Toggle();
        }
    }
}