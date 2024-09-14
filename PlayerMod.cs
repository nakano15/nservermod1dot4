using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace nservermod1dot4
{
	public class PlayerMod : ModPlayer
	{
        /*public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if(Main.netMode == 2 && damageSource.SourceOtherIndex == 2 && Player.ZoneUnderworldHeight && !NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                NPC.SpawnOnPlayer(Player.whoAmI, NPCID.WallofFlesh);
            }
        }*/
    }
}