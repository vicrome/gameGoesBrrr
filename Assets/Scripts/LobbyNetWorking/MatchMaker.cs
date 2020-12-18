using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MirrorBasics {

    [System.Serializable]
    public class Match
    {
        public string matchID;

        public SyncListGameObject players = new SyncListGameObject();

        public Match(string matchID, GameObject player) 
        {
            this.matchID = matchID;
            players.Add(player);
        }

        public Match() { }
    }

    [System.Serializable]
    public class SyncListGameObject : SyncList<GameObject> { }

    [System.Serializable]
    public class SyncListMatch : SyncList<Match> { }

    public class MatchMaker : NetworkBehaviour
    {
        public static MatchMaker instance;
        public SyncListMatch matches = new SyncListMatch();
        public SyncList<string> matchIDs = new SyncList<string>();

        [SerializeField] GameObject turnManagerPrefab;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }


        public bool HostGame(string matchID, GameObject playerGameObject, out int playerIndex) 
        {
            playerIndex = -1;
            if (!matchIDs.Contains(matchID))
            {
                matchIDs.Add(matchID);
                matches.Add(new Match(matchID, playerGameObject));
                playerIndex = 1;
                Debug.Log("Match created succesfully");
                return true;
            }
            else
            {
                Debug.Log("Match ID already exists");
                return false;
            }
        }

        public bool JoinGame(string matchID, GameObject playerGameObject, out int playerIndex)
        {
            playerIndex = -1;
            if (matchIDs.Contains(matchID))
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].matchID == matchID)
                    {
                        matches[i].players.Add(playerGameObject);
                        playerIndex = matches[i].players.Count;
                        break;
                    }
                }
                Debug.Log("Match joined succesfully");
                return true;
            }
            else
            {
                Debug.Log("Match ID does not exist");
                return false;
            }
        }

        public void BeginGame(string matchID)
        {
            GameObject newTurnManager = Instantiate(turnManagerPrefab);
            NetworkServer.Spawn(newTurnManager);
            newTurnManager.GetComponent<NetworkMatchChecker>().matchId = matchID.ToGuid();
            TurnManager turnManager = newTurnManager.GetComponent<TurnManager>();
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == matchID)
                {
                    foreach (var player in matches[i].players)
                    {
                        Player currentPlayer = player.GetComponent<Player>();
                        turnManager.AddPlayer(currentPlayer);
                        currentPlayer.StartGame();
                    }
                    break;
                }
            }
        }

        public static string GetRandomMatchID() 
        {
            string id = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);
                if (random<26)
                {
                    id += (char)(random + 65);
                }
                else
                {
                    id += (random - 26).ToString();
                }
            }

            Debug.Log("Random Match ID: " + id);
            return id;
        }
    }

    public static class MatchExtensions
    {
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hasBytes = provider.ComputeHash(inputBytes);

            return new Guid(hasBytes);
        }
    }
}
