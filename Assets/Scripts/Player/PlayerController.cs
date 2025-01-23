using System;
using Cinemachine;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerController : NetworkBehaviour
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
        
        [Header("Shooting")]
        [SerializeField] private int health = 100;
        [SerializeField] private int NumberOfBullets = 30;
        [SerializeField] private float timeBetweenShots = 0.1f;
        
        [Header("Audio")] 
        [SerializeField] private AudioSource audioSource;
        private bool isLanding;
    
        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI debugText;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider playerCollider; // Reference to the player's collider
        [SerializeField] private MeshRenderer playerMeshRenderer; // Reference to the player's mesh renderer
        [SerializeField] private Animator playerAnimator;
        
        
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
        private float lastShotTime;
        private bool isDead;
        private NetworkObject networkObject;
        private int debugCounter;
        private bool canShoot = true;
        
        #endregion
    

        #region Methods
        
        #region Unity Methods
        
        void Start()
        {
            debugCounter = 0; // TODO remove
            
            virtualCamera.Priority = 10; // set camera priority so it's on top client side
            
            Cursor.lockState = CursorLockMode.Locked; // TODO move to GameManager
            Cursor.visible = false;
        
            rb = GetComponent<Rigidbody>();
            playerCollider = GetComponent<Collider>();
            playerMeshRenderer = GetComponent<MeshRenderer>();
            playerAnimator = GetComponent<Animator>();
            
            health = 100;
            canShoot = true;
            
            if (IsLocalPlayer)
            {
                GameManager.Instance.ReferenceCamera(virtualCamera.gameObject);
            }
        }

        void Update()
        {
            if (isDead) return;
            
            HandleMovementUpdate();
            HandleShooting();
            HandleAnimation();
            
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