using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace nservermod1dot4
{
	public class nservermod1dot4 : Mod
	{
        public const bool Apocalypse = false;
		private static bool _SinglePlayerMode = false;
		private static nservermod1dot4 instance;
		public static bool IsInSinglePlayer { get { return _SinglePlayerMode; } internal set { _SinglePlayerMode = value; } }
		public static ModPacket GetModPacket { get { return instance.GetPacket(); } }
		public static DateTime CurrentTime = new DateTime();
		private static byte NewHourVal = 255, NewMinuteVal = 255;
		public static byte GetHourValue { get { return NewHourVal; } internal set { NewHourVal = value; } }
		public static byte GetMinuteValue { get { return NewMinuteVal; } internal set { NewMinuteVal = value; }}
		public static byte WofSpawnMessages = 255;
		public static byte OnlinePlayers = 0;
		private static Mod HerosMod;
        private const string ModifyWorldPermissionString = "ModifyWorld";
        public static Asset<Texture2D> worldsettingsbuttontexture;
        internal static LegacyGameInterfaceLayer worldsettingsui;

		public override void Load()
		{
			instance = this;
			_SinglePlayerMode = Main.netMode == 0;
            if(!Main.dedServ)
            {
                worldsettingsbuttontexture = ModContent.Request<Texture2D>("nservermod1dot4/Interfaces/WorldSettingButton");
            }
            worldsettingsui = new Interfaces.WorldSettingsInterface();
		}

		public override void Unload()
		{
			instance = null;
			WorldMod.OnUnload();
            worldsettingsui = null;
		}

        public override void PostSetupContent()
        {
            if(ModLoader.HasMod("HEROsMod"))
            {
                HerosMod = ModLoader.GetMod("HEROsMod");
                HerosMod.Call("AddPermission", ModifyWorldPermissionString, "Allow Modifying World", new Action<bool>(delegate(bool b){ }));
            }
        }

		public static bool PlayerHasPermissionToBuildAndDestroy(Player p)
		{
			return PlayerHasPermissionToBuildAndDestroy(p.whoAmI);
		}

		public static bool PlayerHasPermissionToBuildAndDestroy(int i)
		{
			if (HerosMod == null) return false;
			return (bool)HerosMod.Call("HasPermission", i, ModifyWorldPermissionString);
		}

		public static bool LocalPlayerHasPermissionToBuild()
		{
			return Main.netMode == 1 && PlayerHasPermissionToBuildAndDestroy(Main.myPlayer);
		}

        public static void SendMessage(string Message, byte R = 255, byte G = 255, byte B = 255)
        {
            if (Main.netMode == 0)
            {
                Main.NewText(Message, R, G, B);
            }
            else if (Main.netMode == 2)
            {
                ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral(Message), new Microsoft.Xna.Framework.Color(R, G, B));
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetMod.ReceiveMessage((NetMod.MessageIDs)reader.ReadByte(), reader, whoAmI);
        }

        internal static void WofSpawnMessageTimes()
        {
            if (WofSpawnMessages < 255)
            {
                WofSpawnMessages++;
                if (WofSpawnMessages == 10)
                {
                    SendMessage("<Wall of Flesh> *Nhac!*", 255, 0, 0);
                }
                if (WofSpawnMessages == 70)
                {
                    SendMessage("<Wall of Flesh> *Crunch, Munch, Munch, Munch.*", 255, 0, 0);
                }
                if (WofSpawnMessages == 130)
                {
                    SendMessage("<Wall of Flesh> *Gulp.*", 255, 0, 0);
                }
                if (WofSpawnMessages == 190)
                {
                    SendMessage("<Wall of Flesh> *Buurp!*", 255, 0, 0);
                    for(byte i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && Main.player[i].ZoneUnderworldHeight)
                        {
                            Main.player[i].AddBuff(Terraria.ID.BuffID.Poisoned, 60 * 60, false);
                        }
                    }
                }
                if (WofSpawnMessages == 250)
                {
                    SendMessage("*Don't feed the wall of flesh right now.*", 255, 0, 0);
                }
            }
        }
	}
}