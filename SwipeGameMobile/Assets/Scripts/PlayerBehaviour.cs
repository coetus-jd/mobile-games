using UnityEngine;

/// <summary> 
/// Responsible for moving the player automatically and 
/// reciving input. 
/// </summary> 
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary> 
    /// A reference to the Rigidbody component 
    /// </summary> 
    private Rigidbody rb;

    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 1;

    [Tooltip("How fast the ball moves forwards automatically")]
    [Range(0, 10)]
    public float rollSpeed = 1;
    
    [Header("Swipe Properties")]
    public float swipeMove = 2f;

    public float minSwipeDistance = 2f;

    private Vector2 touchStart;

    // Start is called before the first frame update
    void Start()
    {
        // Get access to our Rigidbody component 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalSpeed = 0;

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

        horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

        if (Input.GetMouseButton((int)MouseButtons.Left))
            horizontalSpeed = CalculateMovement(Input.mousePosition);

#elif UNITTY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            // horizontalSpeed = CalculateMovement(myTouch.position);

            SwipeTeleport(touch);
        }
 
#endif

        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }

    /// <summary>
    /// FixedUpdate is called at a fixed framerate and is a prime place to put
    /// Anything based on time.
    /// </summary>
    void FixedUpdate()
    {
        // Check if we're moving to the side 
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }

    private float CalculateMovement(Vector3 pixelPos)
    {
        var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);

        float xMove = 0;

        xMove = worldPos.x < 0.5f ? -1 : 1;

        return xMove * dodgeSpeed;
    }

    private void SwipeTeleport(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
            touchStart = touch.position;
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = touch.position;

            float x = touchEnd.x - touchStart.x;

            if (Mathf.Abs(x) < minSwipeDistance)
            {
                return;
            }

            Vector3 moveDirection;

            if (x < 0)
            {
                moveDirection = Vector3.left;
            }
            else
            {
                moveDirection = Vector3.right;
            }

            RaycastHit hit;

            if (!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }
        }
    }
}