﻿#region

using Darkages.Network.Game;
using System;

#endregion

namespace Darkages.Types
{
    public class DialogSession
    {
        public DialogSession(Aisling user, int serial)
        {
            Serial = serial;
            SessionPosition = user.Position;
            CurrentMapID = user.CurrentMapId;
            Sequence = 0;
        }

        public Action<GameServer, GameClient, ushort, string> Callback { get; set; }
        public int CurrentMapID { get; set; }
        public ushort Sequence { get; set; }
        public int Serial { get; set; }
        public Position SessionPosition { get; set; }
        public Dialog StateObject { get; set; }
    }
}