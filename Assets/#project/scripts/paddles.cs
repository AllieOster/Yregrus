using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperController : MonoBehaviour
{
    public float angle = 45f; 
    public float rotationSpeed = 300f; 
    public float bounceForce = 10f; 
    private bool isLeftFlipper; 
    private InputActions inputActions; 
    private float targetAngle = 0f;

    private void Awake()
    {
        isLeftFlipper = (gameObject.name == "controlLeftPaddle");
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        if (isLeftFlipper)
        {
            inputActions.Gameplay.LeftPaddle.performed += _ => ActivateFlipper();
            inputActions.Gameplay.LeftPaddle.canceled += _ => DeactivateFlipper();
            inputActions.Gameplay.LeftPaddle.Enable(); 
        }
        else
        {
            inputActions.Gameplay.RightPaddle.performed += _ => ActivateFlipper();
            inputActions.Gameplay.RightPaddle.canceled += _ => DeactivateFlipper();
            inputActions.Gameplay.RightPaddle.Enable(); 
        }
    }

    private void OnDisable()
    {
        if (isLeftFlipper)
        {
            inputActions.Gameplay.LeftPaddle.performed -= _ => ActivateFlipper();
            inputActions.Gameplay.LeftPaddle.canceled -= _ => DeactivateFlipper();
            inputActions.Gameplay.LeftPaddle.Disable(); 
        }
        else
        {
            inputActions.Gameplay.RightPaddle.performed -= _ => ActivateFlipper();
            inputActions.Gameplay.RightPaddle.canceled -= _ => DeactivateFlipper();
            inputActions.Gameplay.RightPaddle.Disable(); 
        }
    }

    private void ActivateFlipper()
    {
        if (isLeftFlipper)
        {
            targetAngle = angle; 
        }
        else
        {
            targetAngle = -angle; 
        }

        Debug.Log("Flipper activated. Target angle: " + targetAngle);
    }

    private void DeactivateFlipper()
    {
        targetAngle = 0f; 
        Debug.Log("Flipper deactivated. Target angle: " + targetAngle);
    }

    private void Update()
    {
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                Vector2 forceDirection = isLeftFlipper ? new Vector2(1, 1) : new Vector2(-1, 1);        
                ballRb.AddForce(forceDirection.normalized * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}