using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

namespace Labyrinth
{
    public class GameManager : PunBehaviour
    {
        private const string GameVersion = "v1.0";
        private const string RoomName = "DefaultRoom";

        [SerializeField]  private Transform _spawnPoint;

        void Awake()
        {
            Debug.LogFormat("[GameManager]: Awake");

            PhotonNetwork.ConnectUsingSettings(GameVersion);
        }

        public override void OnConnectedToPhoton()
        {
            Debug.LogFormat("[GameManager]: OnConnectedToPhoton");
        }

        public override void OnLeftRoom()
        {
            Debug.LogFormat("[GameManager]: OnLeftRoom");
        }

        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            Debug.LogFormat("[GameManager]: OnMasterClientSwitched");
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Debug.LogFormat("[GameManager]: OnPhotonCreateRoomFailed");
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            Debug.LogFormat("[GameManager]: OnPhotonJoinRoomFailed");
        }

        public override void OnCreatedRoom()
        {
            Debug.LogFormat("[GameManager]: OnCreatedRoom");
        }

        public override void OnJoinedLobby()
        {
            Debug.LogFormat("[GameManager]: OnJoinedLobby");
        }

        public override void OnLeftLobby()
        {
            Debug.LogFormat("[GameManager]: OnLeftLobby");
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            Debug.LogFormat("[GameManager]: OnFailedToConnectToPhoton");
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogFormat("[GameManager]: OnDisconnectedFromPhoton");
        }

        public override void OnConnectionFail(DisconnectCause cause)
        {
            Debug.LogFormat("[GameManager]: OnConnectionFail");
        }

        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            Debug.LogFormat("[GameManager]: OnPhotonInstantiate");
        }

        public override void OnReceivedRoomListUpdate()
        {
            Debug.LogFormat("[GameManager]: OnReceivedRoomListUpdate");
        }

        public override void OnJoinedRoom()
        {
            Debug.LogFormat("[GameManager]: OnJoinedRoom");

            ResourceFactory.CreatePlayer(_spawnPoint);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Debug.LogFormat("[GameManager]: OnPhotonPlayerConnected: {0}", newPlayer.ID);
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            Debug.LogFormat("[GameManager]: OnPhotonPlayerDisconnected: {0}", otherPlayer.ID);
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.LogFormat("[GameManager]: OnPhotonRandomJoinFailed");
        }

        public override void OnConnectedToMaster()
        {
            Debug.LogFormat("[GameManager]: OnConnectedToMaster");
            
            RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
            PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
        }

        public override void OnPhotonMaxCccuReached()
        {
            Debug.LogFormat("[GameManager]: OnPhotonMaxCccuReached");
        }

        public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
        {
            Debug.LogFormat("[GameManager]: OnPhotonCustomRoomPropertiesChanged");
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            Debug.LogFormat("[GameManager]: OnPhotonPlayerPropertiesChanged");
        }

        public override void OnUpdatedFriendList()
        {
            Debug.LogFormat("[GameManager]: OnUpdatedFriendList");
        }

        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.LogFormat("[GameManager]: OnCustomAuthenticationFailed");
        }

        public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.LogFormat("[GameManager]: OnCustomAuthenticationResponse");
        }

        public override void OnWebRpcResponse(OperationResponse response)
        {
            Debug.LogFormat("[GameManager]: OnWebRpcResponse");
        }

        public override void OnOwnershipRequest(object[] viewAndPlayer)
        {
            Debug.LogFormat("[GameManager]: OnOwnershipRequest");
        }

        public override void OnLobbyStatisticsUpdate()
        {
            Debug.LogFormat("[GameManager]: OnLobbyStatisticsUpdate");
        }
    }
}