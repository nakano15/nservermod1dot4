using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace nservermod1dot4
{
	public class WallMod : GlobalWall
	{
        public override bool CanExplode(int i, int j, int type)
        {
            return nservermod1dot4.IsInSinglePlayer || nservermod1dot4.LocalPlayerHasPermissionToBuild();
        }

        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            fail = (!nservermod1dot4.IsInSinglePlayer && !nservermod1dot4.LocalPlayerHasPermissionToBuild()) && j < Main.worldSurface - 20;
        }
    }
}