using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace nservermod1dot4
{
	public class ItemMod : GlobalItem
	{
        public override bool CanUseItem(Item item, Player player)
        {
            if (nservermod1dot4.IsInSinglePlayer) return true;
            if (!nservermod1dot4.PlayerHasPermissionToBuildAndDestroy(player))
            {
                if (item.createWall > 0)
                    return player.position.Y >= (Main.worldSurface - 20) * 16;
                if (item.createTile == TileID.Containers || item.createTile == TileID.Containers2)
                    return false;
                if (item.type == ItemID.WaterBucket || item.type == ItemID.LavaBucket || item.type == ItemID.HoneyBucket || item.type == ItemID.BottomlessBucket || item.type == ItemID.BottomlessLavaBucket || item.type == ItemID.EmptyBucket)
                    return player.position.Y >= (Main.worldSurface - 20) * 16;
            }
            return true;
        }
    }
}