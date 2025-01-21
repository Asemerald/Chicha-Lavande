using System;
using Cinemachine;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerController
    {
        
        #region Fields
        
        [Header("Movement Settings")]
        public float speed = 10f;

        [Header("Jump Settings")]
        [SerializeField] private Transform feet;
        [SerializeField] private float jumpForce = 10;
        [SerializeField] private float airMultiplier = 0.4f;
        [SerializeField] private float groundDrag, airDrag;
        [SerializeField] private float maxAddedGravity, speedAddedGravity;

        [SerializeField] private float coyoteTime = 0.2f;


        [Header("Camera effects")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float defaultFOV = 100;
        [SerializeField] private float dynamicFOVThreshold = 12;
        [SerializeField] private float dynamicFOVMultiplier = 3;
        [Space(10)]
        [SerializeField] private float shakeTime = 0.3f;
        [SerializeField] private float shakeAmplitude;
        
        [Header("Audio")] 
        [SerializeField] private AudioSource audioSource;
        private bool isLanding;
    
        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI debugText;
        
        #endregion
        
        #region Variables
        
        private float currentShakeTime;
        private float shakeMultiplier;
        private float targetFOV;
        private Vector2 moveVector;
        private Vector3 moveDir, slopeMoveDir;
        private float currentFallTime;
        private float currentAddedGravity;
        private RaycastHit slopeHit;
        private bool isGrounded;
        private Rigidbody rb;
        
        #endregion
    

        #region Methods
        
        #region Unity Methods
        
        void Start()
        {
            virtualCamera.Priority = 10; // set camera priority so it's on top client side
            
            Cursor.lockState = CursorLockMode.Locked; // TODO move to GameManager
            Cursor.visible = false;
        
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            HandleMovementUpdate();
            UpdateServerWithPredictionServerRpc(lastProcessedPosition, lastProcessedRotation);
        }

        void FixedUpdate()
        {
            HandleMovementFixedUpdate();
        }

        #endregion
        

        
        #endregion
    }
}