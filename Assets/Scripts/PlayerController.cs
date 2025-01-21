using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveVector;
    private Vector3 moveDir, slopeMoveDir;

    private bool isGrounded;
    
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float speed = 10f;

    [Header("Jump Settings")]
    [SerializeField] private Transform feet;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private float groundDrag, airDrag;
    [SerializeField] private float maxAddedGravity, speedAddedGravity;
    private float currentAddedGravity;
    private RaycastHit slopeHit;
    [SerializeField] private float coyoteTime = 0.2f;
    private float currentFallTime;

    [Header("Camera effects")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float defaultFOV = 100;
    [SerializeField] private float dynamicFOVThreshold = 12;
    [SerializeField] private float dynamicFOVMultiplier = 3;
    [Space(10)]
    [SerializeField] private float shakeTime = 0.3f;
    [SerializeField] private float shakeAmplitude;
    private float currentShakeTime;
    private float shakeMultiplier;
    private float targetFOV;

    [Header("Audio")] 
    [SerializeField] private AudioSource audioSource;
    private bool isLanding;
    
    [Space(10)]
    [SerializeField] private TextMeshProUGUI debugText;
    
    private void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //ground check
        isGrounded = Physics.Raycast(feet.position, Vector3.down, 0.2f);
        
        slopeMoveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        
        moveDir = forward.normalized * moveVector.y + right * moveVector.x;
        
        ControlDrag();

        CameraDynamicFOV();

        

        if (isGrounded &&  Mathf.Abs(rb.linearVelocity.y) > 16)
        {
            StartCoroutine(Land());
        }

        if (isGrounded && rb.linearVelocity.y < 0)
        {
            currentFallTime = 0;
        }

        if (currentShakeTime > 0)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            currentShakeTime -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = (shakeAmplitude / shakeTime) * currentShakeTime;
            
            if (currentShakeTime <= 0f)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
        
    }

    private void FixedUpdate()
    {
        Move();
        
        if (!isGrounded)
        {
            currentAddedGravity = Mathf.SmoothStep(currentAddedGravity, maxAddedGravity, speedAddedGravity * Time.deltaTime);

            if (currentFallTime < coyoteTime + 0.1f)
            {
                currentFallTime += Time.deltaTime;
            }
        }
        else if (currentAddedGravity != 0)
        {
            currentAddedGravity = 0;
        }
        
        
    }

    void Move()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * currentAddedGravity);
        
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDir.normalized * speed, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDir.normalized * speed, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDir.normalized * speed * airMultiplier, ForceMode.Acceleration);
        }
        
    }

    void ControlDrag()
    {
        if (OnSlope() && moveDir.magnitude <= 0.1f)
        {
            rb.linearDamping = 30;
        }
        else if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }
    
    void CameraDynamicFOV()
    {
        if (rb.linearVelocity.magnitude < dynamicFOVThreshold)
        {
            targetFOV = defaultFOV;
        }
        else
        {
            targetFOV = defaultFOV + ((rb.linearVelocity.magnitude - dynamicFOVThreshold) * dynamicFOVMultiplier);
        }

        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFOV, 0.9f * Time.deltaTime);
    }

    public void CameraShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineNoise =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        //cinemachineNoise.m_AmplitudeGain = shakeAmplitude;
        currentShakeTime = shakeTime;
    }

    void Jump()
    {
        if (currentFallTime < coyoteTime)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            //AudioContainer.Instance.PlaySound(audioSource, AudioContainer.Instance.jump);

            currentFallTime = coyoteTime + 1;
        }
    }

    private IEnumerator Land()
    {
        if (!isLanding)
        {
            isLanding = true;
            //AudioContainer.Instance.PlaySound(audioSource, AudioContainer.Instance.land);
            yield return new WaitForSeconds(0.3f);
            isLanding = false;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(feet.position, Vector3.down, out slopeHit, 0.2f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            Jump();
        }
    }
}
