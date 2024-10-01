using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    [SerializeField] private float maxHeight = 1.0f;
    [SerializeField] private GameObject ball; 
    [SerializeField] private float launchForce = 10.0f; 
    private float initialYPosition;
    private bool isLaunching = false;
    private bool isDocked = false; 
    private InputActions inputActions; 

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Gameplay.Launch.performed += OnLaunchButton; 
        inputActions.Gameplay.Launch.canceled += OnLaunchButton; 
        inputActions.Enable(); 
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Launch.performed -= OnLaunchButton; 
        inputActions.Gameplay.Launch.canceled -= OnLaunchButton; 
        inputActions.Disable(); 
    }

    private void Start()
    {
        initialYPosition = transform.position.y;
    }
    
    public void OnLaunchButton(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            isLaunching = true;
        }

        if (context.canceled) 
        {
            if (isLaunching && isDocked) 
            {
                LaunchBall();
            }
            isLaunching = false;
        }
    }

    private void Update()
    {
        if (isLaunching)
        {
            float newY = Mathf.Lerp(transform.position.y, initialYPosition - maxHeight, Time.deltaTime * 5f); // Ajuste la vitesse ici
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            float newY = Mathf.Lerp(transform.position.y, initialYPosition, Time.deltaTime * 5f);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void LaunchBall()
    {
        if (ball != null)
        {
            Rigidbody2D ballRigidbody = ball.GetComponent<Rigidbody2D>();
            if (ballRigidbody != null)
            {
                ballRigidbody.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == ball) 
        {
            isDocked = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == ball) 
        {
            isDocked = false; 
        }
    }
}