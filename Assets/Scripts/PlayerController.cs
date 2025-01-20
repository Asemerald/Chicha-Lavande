using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    [Header("References")]
    public CharacterController characterController;

    private Vector3 velocity;
    private bool isGrounded;

    // Client-side prediction variables
    private Vector3 lastProcessedPosition;
    private Quaternion lastProcessedRotation;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            // Disable input processing for non-owners but keep the script for synchronization.
            enabled = false;
        }
    }

    void Update()
    {
        if (!IsOwner || !IsClient)
        {
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        // Check if the player is on the ground
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Send predicted position and rotation to the server
        UpdateServerWithPredictionServerRpc(transform.position, transform.rotation);
    }

    [ServerRpc]
    private void UpdateServerWithPredictionServerRpc(Vector3 position, Quaternion rotation, ServerRpcParams rpcParams = default)
    {
        // Validate and apply predicted position and rotation for the client
        if (IsServer)
        {
            lastProcessedPosition = position;
            lastProcessedRotation = rotation;

            UpdateClientsWithPositionClientRpc(position, rotation);
        }
    }

    [ClientRpc]
    private void UpdateClientsWithPositionClientRpc(Vector3 position, Quaternion rotation, ClientRpcParams rpcParams = default)
    {
        if (!IsOwner)
        {
            // Smoothly interpolate to the updated position
            transform.position = Vector3.Lerp(transform.position, position, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
        }
    }
}
