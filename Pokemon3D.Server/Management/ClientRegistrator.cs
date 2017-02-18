﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.Server.Management
{
    class ClientRegistrator : IClientRegistrator
    {
        private readonly GameServerConfiguration _configuration;
        private readonly IMessageBroker _messageBroker;
        private readonly object _clientRegistrationLockObject = new object();
        private readonly Dictionary<Guid, Player> _players = new Dictionary<Guid, Player>();

        public ClientRegistrator(GameServerConfiguration configuration, IMessageBroker messageBroker)
        {
            _configuration = configuration;
            _messageBroker = messageBroker;
        }

        public bool RegisterClient(Player player)
        {
            lock (_clientRegistrationLockObject)
            {
                if (_players.Count < _configuration.MaxPlayerCount)
                {
                    _messageBroker.Notify($"Player '{player}' has entered the game");
                    _players.Add(player.UniqueIdentifier, player);
                    return true;
                }

                _messageBroker.Notify($"Player '{player}' could not enter the game because the server is full.");
                return false;
            }
        }

        public void UnregisterClient(Player player)
        {
            lock (_clientRegistrationLockObject)
            {
                _players.Remove(player.UniqueIdentifier);
                _messageBroker.Notify($"Player '{player}' quit the server");
            }
        }

        public Player GetPlayer(Guid uniqueId)
        {
            lock (_clientRegistrationLockObject)
            {
                Player player;
                if (_players.TryGetValue(uniqueId, out player)) return player;
            }

            return null;
        }
    }
}
