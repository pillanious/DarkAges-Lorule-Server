﻿namespace Darkages.Network.ClientFormats
{
    public class ClientFormat06 : NetworkFormat
    {
        public ClientFormat06()
        {
            Secured = true;
            Command = 0x06;
        }

        public byte Direction { get; set; }
        public byte StepCount { get; set; }

        public override void Serialize(NetworkPacketReader reader)
        {
            Direction = reader.ReadByte();
            StepCount = reader.ReadByte();
        }

        public override void Serialize(NetworkPacketWriter writer)
        {
        }
    }
}