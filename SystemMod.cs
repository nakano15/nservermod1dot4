using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace nservermod1dot4
{
	public class SystemMod : ModSystem
	{
        public override void OnWorldLoad()
        {
			Initialize();
        }

        public override void PreWorldGen()
        {
			Initialize();
        }

		public void Initialize()
		{
			WorldMod.Initialize();
		}

        public override void PreUpdateWorld()
        {
			WorldMod.PreUpdate();
        }

        public override void PreUpdateEntities()
        {
			nservermod1dot4.OnlinePlayers = 0;
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
					nservermod1dot4.OnlinePlayers++;
			}
			DateTime LastTime = nservermod1dot4.CurrentTime;
			nservermod1dot4.CurrentTime = DateTime.Now;
			if (nservermod1dot4.CurrentTime.Hour != LastTime.Hour)
				nservermod1dot4.GetHourValue = (byte)nservermod1dot4.CurrentTime.Hour;
			else
				nservermod1dot4.GetHourValue = 255;
			if (nservermod1dot4.CurrentTime.Minute != LastTime.Minute)
				nservermod1dot4.GetMinuteValue = (byte)nservermod1dot4.CurrentTime.Minute;
			else
				nservermod1dot4.GetMinuteValue = 255;
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
			if(Main.netMode == 1 && nservermod1dot4.Apocalypse && Main.LocalPlayer.ZoneOverworldHeight)
			{
				tileColor.R = (byte)(tileColor.R * (204f / 256));
				tileColor.G = (byte)(tileColor.G * (73f / 256));
				tileColor.B = (byte)(tileColor.B * (41f / 256));
				backgroundColor.R = (byte)(backgroundColor.R * (204f / 256));
				backgroundColor.G = (byte)(backgroundColor.G * (73f / 256));
				backgroundColor.B = (byte)(backgroundColor.B * (41f / 256));
			}
        }

        public override void ModifyLightingBrightness(ref float scale)
        {
			if(Main.netMode == 1 && nservermod1dot4.Apocalypse && Main.LocalPlayer.ZoneOverworldHeight)
			{
				scale *= 0.9f;
			}
        }

        public override void PostUpdateTime()
        {
			if (!nservermod1dot4.IsInSinglePlayer && nservermod1dot4.OnlinePlayers == 0)
				WorldMod.UpdateHourlyRespawn();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			layers.Insert(20, nservermod1dot4.worldsettingsui);
        }

        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
			if (WorldMod.EnteredHardmodeOnce)
			{
				for (int i = 0; i < list.Count; i++)
				{
					switch (list[i].Name)
					{
						case "Hardmode Good Remix":
						case "Hardmode Good":
						case "Hardmode Evil":
						case "Hardmode Walls":
							list.RemoveAt(i);
							i--;
							break;
					}
				}
			}
			else
			{
				list.Add(new Terraria.GameContent.Generation.PassLegacy("Flip Entered Hardmode Flag", delegate(GenerationProgress progress, Terraria.IO.GameConfiguration configuration)
				{
					WorldMod.EnteredHardmodeOnce = true;
				}));
			}
        }
    }
}