using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using Unity.Services.Vivox.AudioTaps;

namespace Network
{
    public class VivoxManager : MonoBehaviour
    {
        
        public static VivoxManager Instance { get; private set; }
        
        [SerializeField] private string vivoxServerUri = "https://unity.vivox.com/appconfig/20066-chich-19528-udash"; // Replace with your Vivox server URI
        [SerializeField] private string issuer = "20066-chich-19528-udash"; // Replace with your issuer
        [SerializeField] private string domain = "mtu1xp.vivox.com"; // Replace with your domain
        [SerializeField] private string secretKey = "UxcH0j4JwOP2kXwJyXqUOI4BzzXWWHkw"; // Replace with your Vivox secret key
        
        private string channelName = "default";
        
        private float _nextPosUpdate = 0;
        private bool _hasJoinedChannel = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            _hasJoinedChannel = false;
        }

        private void Update()
        {
            if (_hasJoinedChannel)
            {
                if (Time.time > _nextPosUpdate)
                {
                    UpdateListenerPosition(GameManager.Instance.playerGameObject);
                    _nextPosUpdate += 0.3f;
                }
            }
        }


        public async void LoginToVivoxAsync()
        {
            LoginOptions options = new LoginOptions();
            options.DisplayName = LobbyManager.Instance.playerName;
            options.EnableTTS = true;
            await VivoxService.Instance.LoginAsync(options);
        }
        
        public async void JoinChannelAsync(string channelName)
        {
            try
            {
                this.channelName = channelName;
                //Leave any existing channel
                await VivoxService.Instance.LeaveAllChannelsAsync();
                
                await VivoxService.Instance.JoinPositionalChannelAsync(channelName, ChatCapability.AudioOnly, new Channel3DProperties(
                    audibleDistance: 11,
                    conversationalDistance: 1,
                    audioFadeModel: AudioFadeModel.InverseByDistance,
                    audioFadeIntensityByDistanceaudio: 3
                    ));
                
                Debug.Log($"Joined channel: {channelName}");
                
                _hasJoinedChannel = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to join channel {channelName}: {e}");
                StartCoroutine(RetryConnection());
                Debug.LogWarning("Retrying connection in 1 second...");
            }
        }
        
        private IEnumerator RetryConnection()
        {
            yield return new WaitForSeconds(1);
            JoinChannelAsync(channelName);
        }
        
        public void UpdateListenerPosition(GameObject playerGameObject)
        {
            if (playerGameObject == null) return;
            VivoxService.Instance.Set3DPosition(Camera.main.gameObject, channelName);
        }


    }
}
