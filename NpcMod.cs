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
	public class NpcMod : GlobalNPC
	{
        public override void SetDefaults(NPC npc)
        {
            if (nservermod1dot4.IsInSinglePlayer)
                return;
            bool HarderDungeon = WorldMod.IsHarderDungeonAndSkeleEnabled.Value;
            switch (npc.type)
            {
                /*case NPCID.SkeletronHead:
                case NPCID.SkeletronHand:
                    if (HarderDungeon)
                    {
                        npc.lifeMax *= 3;
                        npc.damage += 20;
                        npc.defense += 10;
                    }
                    break;
                case NPCID.AngryBones:
                case NPCID.AngryBonesBig:
                case NPCID.AngryBonesBigHelmet:
                case NPCID.AngryBonesBigMuscle:
                case NPCID.DarkCaster:
                case NPCID.CursedSkull:
                    if (HarderDungeon)
                    {
                        npc.lifeMax *= 3;
                        npc.damage += 10;
                        npc.defense += 5;
                    }
                    break;
                case NPCID.BlazingWheel:
                case NPCID.SpikeBall:
                    if (HarderDungeon)
                    {
                        npc.damage += 30;
                    }
                    break;*/
                case NPCID.Werewolf:
                    if (Main.rand.Next(100) == 0)
                        npc.GivenName = "Furry";
                    if (!Main.hardMode)
                    {
                        npc.lifeMax = 500;
                        npc.defense = 8;
                    }
                    break;
            }
        }

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            if (npc.type == NPCID.Golfer)
            {
                for(int i = 0; i < items.Length; i++)
                {
                    if(items[i].type == ItemID.LawnMower)
                    {
                        items[i].SetDefaults(0);
                    }
                }
            }
            if (npc.type == NPCID.Merchant)
            {
                foreach (Item i in items)
                {
                    if (i.type == 0)
                    {
                        i.SetDefaults(ItemID.Bottle);
                        break;
                    }
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            if (Main.netMode <= 1)
                return;
            switch (npc.type)
            {
                case NPCID.SkeletronHead:
                    if (npc.ai[2] == 0)
                    {
                        if (npc.ai[1] == 0) //20, -10
                        {
                            for (int x = -1; x < 2; x += 2)
                            {
                                NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X + x * 20, (int)npc.Center.Y - 10, NPCID.CursedSkull);
                            }
                        }
                        else if (npc.ai[1] == 1)
                        {
                            NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y + 10, NPCID.DarkCaster);
                        }
                    }
                    else if (npc.ai[1] == 0 && (npc.ai[2] == 200 || npc.ai[2] == 400 || npc.ai[2] == 600))
                    {
                        NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y + 10, NPCID.AngryBones);
                    }
                    break;
                case NPCID.SkeletronHand:
                    if(npc.ai[3] == 100 || npc.ai[3] == 200)
                    {
                        NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y + 10, NPCID.WaterSphere);
                    }
                    break;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (!WorldMod.IsCustomMobSpawnsEnabled.Value) return;
            if (!Main.hardMode)
            {
                if (!spawnInfo.PlayerInTown)
                {
                    float PlayerOnlineSpawnChance = 1f + nservermod1dot4.OnlinePlayers * 0.02f;
                    if (Main.moonPhase == 0 && !Main.dayTime && !NPC.AnyNPCs(NPCID.Werewolf))
                    {
                        pool.Add(NPCID.Werewolf, (1f / 30) * PlayerOnlineSpawnChance);
                    }
                    if (Main.raining && spawnInfo.Player.ZoneSnow)
                    {
                        pool.Add(NPCID.IceGolem, (1f / 300) * PlayerOnlineSpawnChance);
                    }
                    if (spawnInfo.Player.ZoneSandstorm)
                    {
                        pool.Add(NPCID.SandElemental, (1f / 300) * PlayerOnlineSpawnChance);
                    }
                    if (spawnInfo.Player.ZoneSkyHeight && !NPC.AnyNPCs(NPCID.WyvernHead))
                    {
                        Tile tile = Main.tile[(int)(spawnInfo.Player.Center.X * (1f / 16)), (int)(spawnInfo.Player.Center.Y * (1f / 16))];
                        if (tile.WallType == 0)
                            pool.Add(NPCID.WyvernHead, (1f / 1000) * PlayerOnlineSpawnChance);
                    }
                    if (spawnInfo.SpawnTileType == TileID.Sand)
                    {
                        pool.Add(NPCID.Mummy, (1f / 50) * PlayerOnlineSpawnChance);
                    }
                    if (spawnInfo.SpawnTileType == TileID.Ebonsand || spawnInfo.SpawnTileType == TileID.Crimsand)
                    {
                        pool.Add(NPCID.DarkMummy, (1f / 50) * PlayerOnlineSpawnChance);
                    }
                    if (spawnInfo.SpawnTileType == TileID.MushroomGrass)
                    {
                        pool.Add(NPCID.TruffleWorm, (1f / 300) * PlayerOnlineSpawnChance);
                    }
                    if (spawnInfo.Player.ZoneJungle)
                    {
                        pool.Add(NPCID.Lihzahrd, (1f / 250) * PlayerOnlineSpawnChance);
                    }
                    if (spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneOverworldHeight)
                    {
                        pool.Add(NPCID.Wolf, (1f / 250) * PlayerOnlineSpawnChance);
                    }
                    if(spawnInfo.Player.ZoneGraveyard)
                    {
                        pool.Add(NPCID.Reaper, PlayerOnlineSpawnChance * (1f / 100));
                    }
                    if(spawnInfo.Player.ZoneCorrupt)
                    {
                        pool.Add(NPCID.Corruptor, PlayerOnlineSpawnChance * (1f / 200));
                    }
                    if(spawnInfo.Player.ZoneCrimson)
                    {
                        pool.Add(NPCID.Herpling, PlayerOnlineSpawnChance  * (1f / 200));
                    }
                    if(spawnInfo.Player.ZoneJungle)
                    {
                        pool.Add(NPCID.GiantTortoise, PlayerOnlineSpawnChance  * (1f / 200));
                    }
                    if(spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneDirtLayerHeight)
                    {
                        pool.Add(NPCID.IceTortoise, PlayerOnlineSpawnChance  * (1f / 200));
                    }
                    if (spawnInfo.Player.ZoneUnderworldHeight && !NPC.AnyNPCs(NPCID.RedDevil))
                    {
                        pool.Add(NPCID.RedDevil, PlayerOnlineSpawnChance * (1f / 200));
                    }
                    if (spawnInfo.Player.ZoneDungeon)
                    {
                        pool.Add(NPCID.Paladin, PlayerOnlineSpawnChance * (1f / 2500));
                        pool.Add(NPCID.BoneLee, PlayerOnlineSpawnChance * (1f / 1000));
                        pool.Add(NPCID.SkeletonSniper, PlayerOnlineSpawnChance * (1f / 1000));
                    }
                    if(spawnInfo.Player.ZoneCorrupt)
                    {
                        pool.Add(NPCID.BigMimicCorruption, PlayerOnlineSpawnChance * (1f / 1000));
                    }
                    if(spawnInfo.Player.ZoneCrimson)
                    {
                        pool.Add(NPCID.BigMimicCrimson, PlayerOnlineSpawnChance * (1f / 1000));
                    }
                    if(spawnInfo.Player.ZoneJungle)
                    {
                        pool.Add(NPCID.BigMimicJungle, PlayerOnlineSpawnChance * (1f / 1000));
                    }
                    if(spawnInfo.Player.ZoneHallow)
                    {
                        pool.Add(NPCID.BigMimicHallow, PlayerOnlineSpawnChance * (1f / 1000));
                    }
                    if(spawnInfo.Player.ZoneSnow)
                    {
                        pool.Add(NPCID.IceMimic, PlayerOnlineSpawnChance * (1f / 250));
                        if(spawnInfo.Player.ZoneCorrupt)
                        {
                            pool.Add(NPCID.PigronCorruption, PlayerOnlineSpawnChance * (1f / 250));
                        }
                        if(spawnInfo.Player.ZoneCrimson)
                        {
                            pool.Add(NPCID.PigronCrimson, PlayerOnlineSpawnChance * (1f / 250));
                        }
                    }
                    else
                    {
                        pool.Add(NPCID.Mimic, PlayerOnlineSpawnChance * (1f / 250));
                    }
                    if(Main.netMode == 2 && nservermod1dot4.Apocalypse && spawnInfo.Player.statLifeMax >= 200)
                    {
                        if(spawnInfo.Player.ZoneOverworldHeight)
                        {
                            pool.Add(NPCID.Demon, PlayerOnlineSpawnChance * (1f / 20));
                            pool.Add(NPCID.VoodooDemon, PlayerOnlineSpawnChance * (1f / 80));
                            pool.Add(NPCID.FireImp, PlayerOnlineSpawnChance * (1f / 40));
                            pool.Add(NPCID.BurningSphere, PlayerOnlineSpawnChance * (1f / 15));
                        }
                        if(spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight)
                        {
                            pool.Add(NPCID.BlazingWheel, PlayerOnlineSpawnChance * (1f / 50));
                            pool.Add(NPCID.RuneWizard, PlayerOnlineSpawnChance * (1f / 150));
                        }
                        if(spawnInfo.Player.ZoneCorrupt)
                        {
                            pool.Add(NPCID.VileSpit, PlayerOnlineSpawnChance * (1f / 50));
                            pool.Add(NPCID.PigronCorruption, PlayerOnlineSpawnChance * (1f / 150));
                        }
                        if(spawnInfo.Player.ZoneCrimson)
                        {
                            pool.Add(NPCID.FloatyGross, PlayerOnlineSpawnChance * (1f / 100));
                            pool.Add(NPCID.PigronCrimson, PlayerOnlineSpawnChance * (1f / 150));
                        }
                        if(spawnInfo.Player.ZoneJungle)
                        {
                            pool.Add(NPCID.LihzahrdCrawler, PlayerOnlineSpawnChance * (1f / 100));
                            pool.Add(NPCID.PigronCrimson, PlayerOnlineSpawnChance * (1f / 150));
                        }
                    }
                }
                if (spawnInfo.Player.ZoneBeach && spawnInfo.Water)
                {
                    pool.Add(NPCID.CreatureFromTheDeep, 1f / 300);
                }
                if (!Main.dayTime)
                {
                    pool.Add(NPCID.Psycho, 1f / 1000);
                }
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.SkeletronHead:
                    WorldMod.RefreshDungeonTime();
                    return;
            }
        }

        public bool IsEngagingNpc(int NpcID, Player player)
        {
            if (!NPC.AnyNPCs(NpcID)) return false;
            NPC npc = Main.npc[NPC.FindFirstNPC(NpcID)];
            return (Math.Abs(player.Center.X - npc.Center.X) < 1000 &&
                Math.Abs(player.Center.Y - npc.Center.Y) < 800);
        }
    }
}