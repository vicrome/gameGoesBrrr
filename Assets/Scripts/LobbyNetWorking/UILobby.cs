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

        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;
                SpawnPlayerUIPrefab(Player.localPlayer);
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
                SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = Player.localPlayer.myMatchID;

            }
            else
            {
                joinMatchInput.interactable = true;
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }

        public void SpawnPlayerUIPrefab(Player player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
            newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);
        }

        public void BeginGame() 
        {
            Player.localPlayer.BeginGame();
        }
    }
}
