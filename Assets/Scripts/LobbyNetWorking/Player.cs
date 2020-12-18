using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace MirrorBasics {

    public class Player : NetworkBehaviour
    {
        public static Player localPlayer;
        [SyncVar] public string myMatchID;
        [SyncVar] public int playerIndex;

        NetworkMatchChecker networkMatchChecker;

        [SyncVar] public Match currentMatch;
        GameObject playerLobbyUI;

        // Start is called before the first frame update
        void Awake()
        {
            networkMatchChecker = GetComponent<NetworkMatchChecker>();
        }

        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                localPlayer = this;
            }
            else
            {
                Debug.Log("Spawning other player UI");
                playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab(this);
            }
        }

        public override void OnStopClient()
        {
            Debug.Log("Client stopped");
            ClientDisconnects();
        }

        public override void OnStopServer()
        {
            Debug.Log("Client stopped on server");
            ServerDisconnects();
        }

        public void HostGame(bool isPublicMatch)
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID, isPublicMatch);
        }

        [Command]
        void CmdHostGame(string matchID, bool isPublicMatch)
        {
            myMatchID = matchID;
            if (MatchMaker.instance.HostGame(matchID, gameObject, isPublicMatch, out playerIndex))
            {
                Debug.Log("Game hosted succesfully");
                networkMatchChecker.matchId = matchID.ToGuid();
                TargetHostGame(true, matchID, playerIndex);
            }
            else
            {
                Debug.Log("Game hosted failed");
                TargetHostGame(false, matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            myMatchID = matchID;
            Debug.Log("myMatchID:" + myMatchID);
            UILobby.instance.HostSuccess(success, matchID);
        }

        public void JoinGame(string inputMatchID)
        {
            CmdJoinGame(inputMatchID);
        }

        [Command]
        void CmdJoinGame(string matchID)
        {
            myMatchID = matchID;
            if (MatchMaker.instance.JoinGame(matchID, gameObject, out playerIndex))
            {
                Debug.Log("Game joined succesfully");
                networkMatchChecker.matchId = matchID.ToGuid();
                TargetJoinGame(true, matchID, playerIndex);
            }
            else
            {
                Debug.Log("Game joined failed");
                TargetJoinGame(false, matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            myMatchID = matchID;
            Debug.Log("myMatchID:" + myMatchID);
            UILobby.instance.JoinSuccess(success, matchID);
        }

        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        void CmdBeginGame()
        {
            MatchMaker.instance.BeginGame(myMatchID);
            Debug.Log("Game beginning");
        }

        public void StartGame()
        {
            TargetBeginGame();
        }

        [TargetRpc]
        void TargetBeginGame()
        {
            Debug.Log("myMatchID:" + myMatchID + "is beginnning the scene 2");
            // Cargar la scena de nuestro Juego.
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }

        public void SearchGame()
        {
            CmdSearchGame();
        }

        [Command]
        public void CmdSearchGame()
        {
            if (MatchMaker.instance.SearchGame(gameObject, out playerIndex, out myMatchID))
            {
                Debug.Log("Game found succesfully");
                networkMatchChecker.matchId = myMatchID.ToGuid();
                TargetSearchGame(true, myMatchID, playerIndex);
            }
            else
            {
                Debug.Log("Game not found");
                TargetSearchGame(false, myMatchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetSearchGame(bool success, string matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            myMatchID = matchID;
            Debug.Log("myMatchID:" + myMatchID);
            UILobby.instance.SearchSuccess(success, matchID);
        }

        public void DisconnectGame()
        {
            CmdDisconnectGame();
        }

        [Command]
        public void CmdDisconnectGame()
        {
            ServerDisconnects();
        }

        void ServerDisconnects()
        {
            MatchMaker.instance.PlayerDisconnects(this, myMatchID);
            networkMatchChecker.matchId = string.Empty.ToGuid();
            RpcDisconnectGame(false, myMatchID, playerIndex);
        }

        [ClientRpc]
        void RpcDisconnectGame(bool success, string matchID, int _playerIndex)
        {
            ClientDisconnects();
        }

        void ClientDisconnects()
        {
            if (playerLobbyUI != null)
            {
                Destroy(playerLobbyUI);
            }
        }
    }
}
