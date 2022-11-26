using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace nservermod1dot4.Interfaces
{
	public class WorldSettingsInterface : LegacyGameInterfaceLayer
	{
        public WorldSettingsInterface() : 
            base ("N Server Mod: World Setting", Draw, InterfaceScaleType.UI)
        {
            
        }

        public static bool Active = false;

        public static bool Draw()
        {
            if(!Active)
            {
                if (Main.netMode == 0 && Main.playerInventory)
                {
                    Vector2 ButtonPosition = new Vector2(2, 2);
                    
                    Main.spriteBatch.Draw(nservermod1dot4.worldsettingsbuttontexture.Value, ButtonPosition, Color.White);
                    if(Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 28 &&
                        Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 28)
                    {
                        Main.LocalPlayer.mouseInterface = true;
                        if(Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            Active = true;
                            Main.playerInventory = false;
                        }
                    }
                }
                return true;
            }
            const float OptionTextWidth = 300;
            Vector2 StartPosition = new Vector2(Main.screenWidth * 0.5f - OptionTextWidth, Main.screenHeight * 0.5f - 30 * 5);
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)StartPosition.X - 2, (int)StartPosition.Y - 2, (int)OptionTextWidth * 2 + 4, 30 * 10 + 4), Color.Black);
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)StartPosition.X, (int)StartPosition.Y, (int)OptionTextWidth * 2, 30 * 10), Color.White);
            {
                Vector2 XPosition = new Vector2(StartPosition.X + OptionTextWidth * 2, StartPosition.Y);
                Color color = Color.Red;
                if(Main.mouseX >= XPosition.X - 10 && Main.mouseX < XPosition.X + 10 && 
                    Main.mouseY >= XPosition.Y - 10 && Main.mouseY < XPosition.Y + 10)
                {
                    color = Color.Yellow;
                    Main.LocalPlayer.mouseInterface = true;
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        Active = false;
                        return true;
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, "X", XPosition, color, 1, 0.5f, 0.5f);
            }
            if (Main.mouseX >= StartPosition.X && Main.mouseX < StartPosition.X + OptionTextWidth * 2 &&
                Main.mouseY >= StartPosition.Y && Main.mouseY < StartPosition.Y + 30 * 5)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            for (int y = 0; y < 10; y++)
            {
                for(int x = 0; x < 2; x++)
                {
                    byte Index = (byte)(x + y * 2);
                    Vector2 TextPosition = new Vector2(OptionTextWidth * x + StartPosition.X, StartPosition.Y + 30 * y);
                    switch (Index)
                    {
                        default:
                            continue;
                        case 0:
                            SettingButton("Chest Respawn", TextPosition,
                                WorldMod.IsChestRespawnEnabled);
                            break;
                        case 1:
                            SettingButton("Ores Respawn", TextPosition,
                                WorldMod.IsOreRespawnEnabled);
                            break;
                        case 2:
                            SettingButton("Life Crystal Respawn", TextPosition,
                                WorldMod.IsLCRespawnEnabled);
                            break;
                        case 3:
                            SettingButton("2x2 Hours Dungeon Reset", TextPosition,
                                WorldMod.IsDungeonResetEnabled);
                            break;
                        case 4:
                            SettingButton("Wall of Flesh Enabled", TextPosition,
                                WorldMod.IsWofEnabled);
                            break;
                        case 5:
                            SettingButton("Spider Web Respawn", TextPosition,
                                WorldMod.IsSpiderWebRespawnEnabled);
                            break;
                        case 6:
                            SettingButton("Shadow Orb Respawn", TextPosition,
                                WorldMod.IsShadowOrbRespawnEnabled);
                            break;
                        case 7:
                            SettingButton("Pots Respawn", TextPosition,
                                WorldMod.IsPotsRespawnEnabled);
                            break;
                        case 8:
                            SettingButton("Enchanted Sword Respawn", TextPosition,
                                WorldMod.IsEnchantedSwordRespawnEnabled);
                            break;
                        case 9:
                            SettingButton("Surface Grief Protection", TextPosition,
                                WorldMod.IsSurfaceProtectionEnabled);
                            break;
                        case 10:
                            SettingButton("Harder Dungeon and Skeletron", TextPosition,
                                WorldMod.IsHarderDungeonAndSkeleEnabled);
                            break;
                        case 11:
                            SettingButton("Custom Spawns", TextPosition,
                                WorldMod.IsCustomMobSpawnsEnabled);
                            break;
                    }
                }
            }
            return true;
        }
        
        private static void SettingButton(string Text, Vector2 Position, bool? State)
        {
            string T = "[" + (State.Value ? "ON" : "OFF") + "]" + Text;
            Vector2 Dim = Utils.DrawBorderString(Main.spriteBatch, T, Position, Color.White);
            if(Main.mouseX >= Position.X && Main.mouseX < Position.X + Dim.X &&
                Main.mouseY >= Position.Y && Main.mouseY < Position.Y + Dim.Y)
            {
                Utils.DrawBorderString(Main.spriteBatch, T, Position, Color.Yellow);
                if(Main.mouseLeft && Main.mouseLeftRelease)
                {
                    State = !State.Value;
                }
            }
        }
    }
}