﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class TurnManager : NetworkBehaviour
    {
        List<Player> players = new List<Player>();

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }
    }
}

