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
	public class TileMod : GlobalTile
	{
        public override bool CanPlace(int i, int j, int type)
        {
            return nservermod1dot4.IsInSinglePlayer || nservermod1dot4.LocalPlayerHasPermissionToBuild() || 
            type == 13 || (j >= Main.worldSurface - 20 && (type != TileID.Containers && type != TileID.Containers2) || ((type == TileID.Torches || type == TileID.Platforms || type == TileID.Rope || type == TileID.SilkRope || type == TileID.VineRope || type == TileID.WebRope || type == 29 || type == TileID.Campfire) && !TileID.Sets.HousingWalls[Main.tile[i, j].WallType]));
        }

        public override bool CanExplode(int i, int j, int type)
        {
            return nservermod1dot4.IsInSinglePlayer || nservermod1dot4.LocalPlayerHasPermissionToBuild() || (j >= Main.worldSurface - 20 && !TileID.Sets.HousingWalls[type]);
        }

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (nservermod1dot4.IsInSinglePlayer || nservermod1dot4.LocalPlayerHasPermissionToBuild())
                return true;
            switch (type)
            {
                default:
                    if (j >= Main.worldSurface - 20)
                    {
                        switch (type)
                        {
                            default:
                                return true;
                            case TileID.BlueDungeonBrick:
                            case TileID.GreenDungeonBrick:
                            case TileID.PinkDungeonBrick:
                            case TileID.LihzahrdBrick:
                            case TileID.LihzahrdAltar:
                            case TileID.Traps:
                            case TileID.GeyserTrap:
                            case TileID.Containers:
                            case TileID.Containers2:
                                break;
                        }
                    }
                    switch(type)
                    {
                        case TileID.Grass:
                        case TileID.JungleGrass:
                        case TileID.CorruptGrass:
                        case TileID.CrimsonGrass:
                        case TileID.HallowedGrass:
                        case TileID.MushroomGrass:
                            blockDamaged = true;
                            break;
                        default:
                            blockDamaged = false;
                            break;
                    }
                    return false;
                case TileID.Torches:
                case TileID.Platforms:
                case TileID.Rope:
                case TileID.SilkRope:
                case TileID.VineRope:
                case TileID.WebRope:
                case TileID.PiggyBank:
                    return !Main.wallHouse[Main.tile[i, j].WallType];
                case TileID.Tombstones:
                case TileID.Campfire:
                case TileID.Heart:
                case TileID.Vines:
                case TileID.CrimsonVines:
                case TileID.HallowedVines:
                case TileID.JungleVines:
                case 3: //Tall Grasses
                case TileID.Trees:
                case TileID.MushroomTrees:
                case TileID.PalmTree:
                case TileID.PineTree:
                case TileID.Pots:
                case TileID.CorruptThorns:
                case TileID.CrimsonThorns:
                case TileID.JungleThorns:
                case TileID.Cobweb:
                case 61: //Shortened Grass
                case 71: //Mushrooms
                case 73: //Tall Plants
                case TileID.Cactus:
                case 82: //Alchemy Plants
                case 83:
                case 84:
                case TileID.DyePlants:
                case 185: //Decorative stones
                case 186:
                case 187:
                    return true;
            }
        }

        public override void KillTile(int x, int y, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if(nservermod1dot4.IsInSinglePlayer)
                return;
            switch (type)
            {
                case 5:
                    if (!fail && Main.netMode != 1)
                    {
                        Tile tile = Framing.GetTileSafely(x, y + 1);
                        if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType]) //Has ground
                        {
                            tile = Framing.GetTileSafely(x, y);
                            bool RootTile = true;
                            if (tile.TileFrameX == 66 && tile.TileFrameY <= 45)
                            {
                                RootTile = false;
                            }
                            if (tile.TileFrameX == 88 && tile.TileFrameY >= 66 && tile.TileFrameY <= 110)
                            {
                                RootTile = false;
                            }
                            if (tile.TileFrameX == 22 && tile.TileFrameY >= 132 && tile.TileFrameY <= 176)
                            {
                                RootTile = false;
                            }
                            if (tile.TileFrameX == 44 && tile.TileFrameY >= 132 && tile.TileFrameY <= 176)
                            {
                                RootTile = false;
                            }
                            if (tile.TileFrameX == 44 && tile.TileFrameY >= 198)
                            {
                                RootTile = false;
                            }
                            if (tile.TileFrameX == 66 && tile.TileFrameY >= 198)
                            {
                                RootTile = false;
                            }
                            if (RootTile)
                                WorldMod.AddTreePlantingPosition(x, y);
                        }
                    }
                    break;
            }
        }
    }
}