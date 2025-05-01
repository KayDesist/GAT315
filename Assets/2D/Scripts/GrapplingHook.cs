using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DistanceJoint2D))]
[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    [Header("Grapple Settings")]
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float swingForce = 25f;
    [SerializeField] private float reelSpeed = 3f;
    [SerializeField] private float minRopeLength = 1f;

    [Header("Rope Settings")]
    [SerializeField] private LineRenderer ropeRenderer;
    [SerializeField] private int ropeSegments = 15;
    [SerializeField] private float ropeWidth = 0.1f;
    [SerializeField] private float ropeSag = 0.5f;

    private bool isGrappling = false;
    private Vector2 grapplePoint;
    private DistanceJoint2D ropeJoint;
    private Rigidbody2D rb;
    private Camera mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ropeJoint = GetComponent<DistanceJoint2D>();
        mainCam = Camera.main;

        InitializeRope();
        DisableGrapple(); // Now this will work
    }

    void InitializeRope()
    {
        ropeRenderer.positionCount = ropeSegments;
        ropeRenderer.startWidth = ropeWidth;
        ropeRenderer.endWidth = ropeWidth;
    }

    // ADDED THIS MISSING METHOD
    void DisableGrapple()
    {
        isGrappling = false;
        ropeJoint.enabled = false;
        ropeRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            if (!isGrappling) StartGrapple();
            else DisableGrapple(); // Now using the proper method
        }

        if (isGrappling)
        {
            DrawRope();
            HandleSwingInput();
            AdjustRopeLength();
        }
    }

    void StartGrapple()
    {
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            maxDistance,
            grappleLayer
        );

        if (hit.collider != null)
        {
            isGrappling = true;
            grapplePoint = hit.point;

            ropeJoint.connectedAnchor = grapplePoint;
            ropeJoint.distance = Vector2.Distance(transform.position, grapplePoint);
            ropeJoint.enabled = true;

            ropeRenderer.enabled = true;
        }
    }

    void DrawRope()
    {
        for (int i = 0; i < ropeSegments; i++)
        {
            float t = i / (float)(ropeSegments - 1);
            Vector2 point = Vector2.Lerp(transform.position, grapplePoint, t);

            float sag = Mathf.Sin(t * Mathf.PI) * ropeSag;
            point += Vector2.down * sag;

            ropeRenderer.SetPosition(i, point);
        }
    }

    void HandleSwingInput()
    {
        Vector2 toGrapplePoint = grapplePoint - (Vector2)transform.position;
        Vector2 perpendicular = Vector2.Perpendicular(toGrapplePoint).normalized;

        if (transform.position.y < grapplePoint.y)
            perpendicular *= -1;

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.AddForce(perpendicular * horizontalInput * swingForce);
    }

    void AdjustRopeLength()
    {
        float scrollInput = Input.mouseScrollDelta.y;
        ropeJoint.distance = Mathf.Clamp(
            ropeJoint.distance - (scrollInput * reelSpeed),
            minRopeLength,
            maxDistance
        );
    }
}