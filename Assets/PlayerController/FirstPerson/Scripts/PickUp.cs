using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    public InputActionReference clickAction;
    public InputActionReference throwAction;
    public Camera cam;
    public LayerMask pickupLayers = ~0;
    public float maxDistance = 3f;
    public Transform holdPoint;
    public float defaultHoldDistance = 2f;
    public float throwForce = 6f;
    public bool setKinematicOnHold = true;

    Rigidbody heldRb;
    Transform originalParent;
    bool originalKinematic, originalUseGravity;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (!holdPoint && cam)
        {
            var t = new GameObject("HoldPoint_Auto").transform;
            t.SetParent(cam.transform, false);
            t.localPosition = new Vector3(0, 0, defaultHoldDistance);
            holdPoint = t;
        }
    }

    void OnEnable()
    {
        if (clickAction) { clickAction.action.Enable(); clickAction.action.performed += OnClick; }
        if (throwAction) { throwAction.action.Enable(); throwAction.action.performed += OnThrow; }
    }

    void OnDisable()
    {
        if (clickAction) { clickAction.action.performed -= OnClick; clickAction.action.Disable(); }
        if (throwAction) { throwAction.action.performed -= OnThrow; throwAction.action.Disable(); }
    }

    void OnClick(InputAction.CallbackContext _)
    {
        if (!heldRb) TryPickup();
        else Drop();
    }

    void OnThrow(InputAction.CallbackContext _)
    {
        if (heldRb) Throw();
    }

    void TryPickup()
    {
        if (!cam) return;
        float radius = 0.5f; 
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.SphereCast(ray, radius, out var hit, maxDistance, pickupLayers, QueryTriggerInteraction.Collide))
        {
            var rb = hit.rigidbody;
            if (!rb) return;

            heldRb = rb;
            originalParent = rb.transform.parent;
            originalKinematic = rb.isKinematic;
            originalUseGravity = rb.useGravity;

            rb.transform.SetParent(holdPoint, true);
            rb.transform.localPosition = Vector3.zero;
            rb.transform.localRotation = Quaternion.identity;

            if (setKinematicOnHold)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

    void Drop()
    {
        if (!heldRb) return;
        heldRb.transform.SetParent(originalParent, true);
        heldRb.isKinematic = originalKinematic;
        heldRb.useGravity = originalUseGravity;
        heldRb = null;
    }

    void Throw()
    {
        if (!heldRb || !cam) return;
        heldRb.transform.SetParent(originalParent, true);
        heldRb.isKinematic = false;
        heldRb.useGravity = true;
        heldRb.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
        heldRb = null;
    }
}
