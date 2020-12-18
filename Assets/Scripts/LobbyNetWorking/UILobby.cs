using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics
{

    public class UILobby : MonoBehaviour
    {
        public static UILobby instance;

        [Header("Host Join")]
        [SerializeField] InputField joinMatchInput;
        [SerializeField] List<Selectable> lobbySelectables = new List<Selectable>();
        [SerializeField] Canvas lobbyCanvas;
        [SerializeField] Canvas searchCanvas;


        [Header("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText;
        [SerializeField] GameObject beginGameButton;

        GameObject playerLobbyUI;

        bool isSearching = false;

        private void Start()
        {
            instance = this;
        }

        public void HostPrivate()
        {
            joinMatchInput.interactable = false;
            lobbySelectables.ForEach(x => x.interactable = false);

            Player.localPlayer.HostGame(false);
        }

        public void HostPublic()
        {
            joinMatchInput.interactable = false;
            lobbySelectables.ForEach(x => x.interactable = false);

            Player.localPlayer.HostGame(true);
        }

        public void Join()
        {
            joinMatchInput.interactable = false;
            lobbySelectables.ForEach(x => x.interactable = false);

            Player.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }

        public void SearchGame()
        {
            Debug.Log("Searching for game");
            searchCanvas.enabled = true;
            StartCoroutine(SearchingForGame());
        }

        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;
                if (playerLobbyUI != null)
                {
                    Destroy(playerLobbyUI);
                }
                playerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = matchID;
                beginGameButton.SetActive(true);
            }
            else
            {
                joinMatchInput.interactable = true;
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }

        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;
                beginGameButton.SetActive(false);
                if (playerLobbyUI != null)
                {
                    Destroy(playerLobbyUI);
                }
                playerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = matchID;
            }
            else
            {
                joinMatchInput.interactable = true;
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }

        public void SearchSuccess(bool success, string matchID)
        {
            if (success)
            {
                searchCanvas.enabled = false;
                JoinSuccess(success, matchID);
                isSearching = false;
            }
        }

        public GameObject SpawnPlayerUIPrefab(Player player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
            newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);

            return newUIPlayer;
        }

        public void BeginGame() 
        {
            Player.localPlayer.BeginGame();
        }

        public void CancelSearch()
        {
            searchCanvas.enabled = false;
            isSearching = false;
            lobbySelectables.ForEach(x => x.interactable = true);
        }

        public void DisconnectLobby()
        {
            if (playerLobbyUI != null)
            {
                Destroy(playerLobbyUI);
            }
            Player.localPlayer.DisconnectGame();

            lobbyCanvas.enabled = false;
            lobbySelectables.ForEach(x => x.interactable = true);
            beginGameButton.SetActive(false);

        }

        IEnumerator SearchingForGame() {
            isSearching = true;
            float currentTime = 1;
            while (isSearching)
            {
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    currentTime = 1;
                    Player.localPlayer.SearchGame();
                }
                yield return null;
            }
        }
    }
}
