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
	public class ProjMod : GlobalProjectile
	{
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == Terraria.ID.ProjectileID.Tombstone)
            {
                projectile.Kill();
                return false;
            }
            return base.PreAI(projectile);
        }
    }
}