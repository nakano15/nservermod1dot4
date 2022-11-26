using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
			nservermod1dot4.WofSpawnMessageTimes();
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
    }
}