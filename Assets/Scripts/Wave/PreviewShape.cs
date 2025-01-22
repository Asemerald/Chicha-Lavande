using System;
using Unity.Netcode;
using UnityEngine;

namespace Wave
{
    public class PreviewShape : MonoBehaviour
    {
        public static PreviewShape instance;
        public ulong instantierId = 99;
        
        
        
        private void Start()
        {
            //if instantierID is the same as playerid, then destroy instantly
            
            if (instantierId == GameManager.Instance.playerGameObject.transform.parent.GetComponent<NetworkObject>().NetworkObjectId)
            {
                Destroy(gameObject);
            }
            
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance.gameObject);
                instance = this;
            }
            
            
        }
    }
}