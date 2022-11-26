using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Chat;

namespace nservermod1dot4
{
	public class NetMod
	{
        public enum MessageIDs : byte
        {
            BuildingAllowed,
            AskServerTime,
            SendServerTime,
            SendRollCommand
        }

        private static ModPacket StartNewMessage(MessageIDs mID)
        {
            ModPacket packet = nservermod1dot4.GetModPacket;
            packet.Write((byte)mID);
            return packet;
        }

        public static void SendBuildingAllowed(int From = -1, int To = -1)
        {
            if (Main.netMode == 0)
                return;
            ModPacket packet = StartNewMessage(MessageIDs.BuildingAllowed);
            packet.Write(nservermod1dot4.IsInSinglePlayer);
            packet.Send(To, From);
        }

        public static void SendAskServerTime(int From = -1)
        {
            if (Main.netMode == 0)
                return;
            ModPacket packet = StartNewMessage(MessageIDs.AskServerTime);
            packet.Send(-1, From);
        }

        public static void SendServerTimetoPlayer(int To = -1)
        {
            if (Main.netMode == 0)
                return;
            ModPacket packet = StartNewMessage(MessageIDs.SendServerTime);
            packet.Write(nservermod1dot4.CurrentTime.Ticks);
            packet.Send(To, -1);
        }

        public static void SendRollCommand(int From = -1)
        {
            if (Main.netMode == 0)
                return;
            ModPacket packet = StartNewMessage(MessageIDs.SendRollCommand);
            packet.Send(-1, From);
        }

        public static void ReceiveMessage(MessageIDs message, BinaryReader reader, int WhoAmI)
        {
            switch (message)
            {
                case MessageIDs.BuildingAllowed:
                    bool Allow = reader.ReadBoolean();
                    nservermod1dot4.IsInSinglePlayer = Allow;
                    Main.NewText("Building is allowed? " + Allow);
                    break;
                case MessageIDs.AskServerTime:
                    if(Main.netMode == 2)
                        SendServerTimetoPlayer(WhoAmI);
                    break;
                case MessageIDs.SendServerTime:
                    {
                        DateTime dt = new DateTime(reader.ReadInt64());
                        Main.NewText("Server Time: " + dt.ToShortTimeString());
                    }
                    break;
                case MessageIDs.SendRollCommand:
                    {
                        if(Main.netMode == 2)
                        {
                            ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral(Main.player[WhoAmI].name + " rolled a " + Main.rand.Next(0, 101) + " out of 100."), new Microsoft.Xna.Framework.Color(255, 128, 0));
                        }
                    }
                    break;
            }
        }
    }
}