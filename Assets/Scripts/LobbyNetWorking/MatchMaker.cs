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
        public bool isPublicMatch;
        public bool isMatchFull;
        public bool isInMatch;
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


        public bool HostGame(string matchID, GameObject playerGameObject, bool ispublicMatch, out int playerIndex) 
        {
            playerIndex = -1;
            if (!matchIDs.Contains(matchID))
            {
                matchIDs.Add(matchID);
                Match match = new Match(matchID, playerGameObject);
                matches.Add(match);
                Debug.Log("Match generated");
                match.isPublicMatch = ispublicMatch;
                playerGameObject.GetComponent<Player>().currentMatch = match;
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
                        playerGameObject.GetComponent<Player>().currentMatch = matches[i];
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

        public bool SearchGame(GameObject playerGameObject, out int playerIndex, out string matchID)
        {
            playerIndex = -1;
            matchID = string.Empty;
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].isPublicMatch && !matches[i].isMatchFull && !matches[i].isInMatch)
                {
                    matchID = matches[i].matchID;
                    if (JoinGame(matchID, playerGameObject, out playerIndex))
                    {
                        return true;
                    }                    
                }
            }
            return false;
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

        public void PlayerDisconnects (Player player, string matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == matchID)
                {
                    int playerIndex = matches[i].players.IndexOf(player.gameObject);
                    matches[i].players.RemoveAt(playerIndex);
                    Debug.Log("Player disconnected from match :" + matchID);
                    Debug.Log(matches[i].players.Count + "players remaining.");
                    if (matches[i].players.Count == 0)
                    {
                        Debug.Log("No more players in the Match. Ending " + matchID);
                        matches.RemoveAt(i);
                        matchIDs.Remove(matchID);
                    }
                    break;
                }
            }
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
