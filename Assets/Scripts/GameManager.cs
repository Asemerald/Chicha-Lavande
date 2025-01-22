using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    #region Fields
    
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public GameObject playerGameObject;
    
    #endregion
    
    #region Variables
    public static GameManager Instance { get; private set; }
    public event EventHandler OnPlayerSpawned; // TODO faire un truc avec jsp
    
    #endregion
    
    #region Methods
    
    #region Unity Methods

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    #endregion
    
    public void ReferenceCamera(GameObject Camera)
    {
        playerGameObject = Camera;
    }
    
        
    
    
    #endregion
}
