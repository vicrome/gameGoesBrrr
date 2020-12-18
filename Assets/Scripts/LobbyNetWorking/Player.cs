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

        // Start is called before the first frame update
        void Start()
        {
            networkMatchChecker = GetComponent<NetworkMatchChecker>();

            if (isLocalPlayer)
            {
                localPlayer = this;
            }
            else
            {
                UILobby.instance.SpawnPlayerUIPrefab(this);
            }
        }

        public void HostGame(bool isPublic)
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID);
        }

        [Command]
        void CmdHostGame(string matchID)
        {
            myMatchID = matchID;
            if (MatchMaker.instance.HostGame(matchID, gameObject, out playerIndex))
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
    }
}
