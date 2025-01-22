using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    #region Fields
    
    [SerializeField] public Transform[] spawnPoints;
    
    #endregion
    
    #region Variables
    public static GameManager Instance { get; private set; }
    
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
    
    #endregion
}
