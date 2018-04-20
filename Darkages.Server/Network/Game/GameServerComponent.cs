﻿using Darkages.Network.Object;
using Newtonsoft.Json;
using System;

namespace Darkages.Network.Game
{
    public abstract class GameServerComponent : ObjectManager
    {
        public GameServerComponent(GameServer server)
        {
            Server = server;
        }

        [JsonIgnore] public GameServer Server { get; }

        public abstract void Update(TimeSpan elapsedTime);
    }
}