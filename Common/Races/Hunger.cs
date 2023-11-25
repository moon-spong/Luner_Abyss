using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using MrPlagueRaces.Content.Buffs;
using static Terraria.ModLoader.ModContent;

namespace Hunger
{
    public class Hunger : Player
    {

        public override void Load()
        {
            Description = "Nobody knows where this monster came from but it's hungry.";
            AbilitiesDescription = $"";

            HairColor = new Color(255, 255, 255);
            SkinColor = new Color(255, 255, 255);
            DetailColor = new Color(255, 255, 255);
            EyeColor = new Color(255, 255, 255);

            StarterShirt = false;
            StarterPants = false;
            CensorClothing = false;

        }

        public override void ResetEffects(Player player)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 8;
            player.noKnockback = true;
            player.maxRunSpeed += 1f;
            player.thorns += 10f;
            player.maxMinions += 1;
            player.wings = 30;
            player.wingsLogic = 2;
            player.wingTimeMax = 90 + (player.statManaMax2 / 2);
            player.noFallDmg = true;
            player.buffImmune[31] = true;
            player.endurance += 3f;
            player.canWallJump = true;
        }

        public override void ProcessTriggers(Player player, TriggersSet triggersSet)
        {
            var Hunger = player.GetModPlayer<Hunger>();
            var Hunger = player.GetModPlayer<Hunger>();
            if (ModContent.GetInstance<MrPlagueRacesConfig>().raceStats)
            {
                if (!player.dead)
                {
                    if (MrPlagueRaces.RaceAbilityKeybind1.Current)
                    {
                        if (player.ownedProjectileCounts[ProjectileType<SoulClaw>()] == 0)
                        {
                            Projectile.NewProjectile(Wiring.GetProjectileSource(0, 0), player.Center.X, player.Center.Y, 0, 0, ProjectileType<SoulClaw>(), player.statLifeMax2 / 4, 1, player.whoAmI);
                        }
                    }
                }
            }
        }

        public override void ProcessTriggers(Player player, TriggersSet triggersSet)
        {
            var Hunger = player.GetModPlayer<Hunger>();
            var Hunger = player.GetModPlayer<Hunger>();
            if (ModContent.GetInstance<MrPlagueRacesConfig>().raceStats)
            {
                if (!player.dead)
                {
                    if (MrPlagueRaces.RaceAbilityKeybind1.JustPressed)
                    {
                        if (player.mount.Type != MountType<StealthMoth>())
                        {
                            player.mount.SetMount(MountType<StealthMoth>(), player, false);
                            SoundEngine.PlaySound(SoundID.AbigailUpgrade, player.Center);
                            int num = Gore.NewGore(Wiring.GetProjectileSource(0, 0), new Vector2(player.position.X, player.position.Y - 10f), player.velocity, 99);
                            Main.gore[num].velocity *= 0.3f;
                            num = Gore.NewGore(Wiring.GetProjectileSource(0, 0), new Vector2(player.position.X, player.position.Y + (float)(player.height / 2) - 10f), player.velocity, 99);
                            Main.gore[num].velocity *= 0.3f;
                            num = Gore.NewGore(Wiring.GetProjectileSource(0, 0), new Vector2(player.position.X, player.position.Y + (float)player.height - 10f), player.velocity, 99);
                            Main.gore[num].velocity *= 0.3f;
                        }
                        else
                        {
                            player.mount.Dismount(player);
                            SoundEngine.PlaySound(SoundID.AbigailAttack, player.Center);
                            int num = Gore.NewGore(Wiring.GetProjectileSource(0, 0), new Vector2(player.position.X, player.position.Y - 10f), player.velocity, 99);
                            Main.gore[num].velocity *= 0.3f;
                            num = Gore.NewGore(Wiring.GetProjectileSource(0, 0), new Vector2(player.position.X, player.position.Y + (float)(player.height / 2) - 10f), player.velocity, 99);
                            Main.gore[num].velocity *= 0.3f;
                            num = Gore.NewGore(Wiring.GetProjectileSource(0, 0), new Vector2(player.position.X, player.position.Y + (float)player.height - 10f), player.velocity, 99);
                            Main.gore[num].velocity *= 0.3f;
                        }
                    }
                    if (MrPlagueRaces.RaceAbilityKeybind2.JustPressed && player.mount.Type == MountType<StealthMoth>())
                    {
                        Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 10f;
                        if (player.ownedProjectileCounts[ProjectileType<LeechMoth>()] == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item111, player.Center);
                            SoundEngine.PlaySound(SoundID.Item171, player.Center);
                            Hunger.LeechMoth = Projectile.NewProjectile(Wiring.GetProjectileSource(0, 0), player.Center.X, player.Center.Y, velocity.X, velocity.Y, ProjectileType<LeechMoth>(), 1, 0, player.whoAmI);
                        }
                        else
                        {
                            Main.projectile[Hunger.LeechMoth].ai[0] = 60;
                        }
                    }
                    if (player.mount.Type == MountType<StealthMoth>())
                    {
                        if (player.controlUseItem)
                        {
                            player.controlUseItem = false;
                        }
                    }
                }
            }
        }

        public class Wings : PlayerDrawLayer
        {
            private Asset<Texture2D>[] Wings_Texture = new Asset<Texture2D>[10];
            private string[] PlayerColors = { "ColorSkin", "ColorDetail", "Colorless", "ColorEyes", "ColorHair", "ColorSkin/Glowmask", "ColorDetail/Glowmask", "Colorless/Glowmask", "ColorEyes/Glowmask", "ColorHair/Glowmask" };

            public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Wings);

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                return (drawInfo.skinVar < 10 && drawPlayer.wings == 0);
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                Vector2 helmetOffset = drawInfo.helmetOffset;

                var luner = drawPlayer.GetModPlayer<luner>();
                var Hunger = drawPlayer.GetModPlayer<Hunger>();
                if (luner.race != null)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Wings_Texture[i] = luner.GetRaceTexture(drawPlayer, $"{PlayerColors[i]}/Wings");
                    }
                    Vector2 bodyPosition = new Vector2((float)(int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2)), (float)(int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f)) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    MakeColoredDrawDatas(ref drawInfo, Wings_Texture, null, new Vector2(bodyPosition.X - (9 * drawPlayer.direction), bodyPosition.Y - 9), new Rectangle(0, Wings_Texture[0].Height() / 4 * Hunger.wingFrame, Wings_Texture[0].Width(), Wings_Texture[0].Height() / 4), drawPlayer.bodyRotation, new Vector2((float)(Wings_Texture[0].Width() / 2), (float)(Wings_Texture[0].Height() / 14)), 1f, drawInfo.playerEffect, 0);
                }
            }

            private void MakeColoredDrawDatas(ref PlayerDrawSet drawInfo, Asset<Texture2D>[] texture, Asset<Texture2D>[,] textureHair, Vector2 position, Rectangle? sourceRect, float rotation, Vector2 origin, float scale, SpriteEffects effect, int inactiveLayerDepth)
            {
                DrawData drawData;
                Player drawPlayer = drawInfo.drawPlayer;
                int index;
                for (index = 0; index < 10; index++)
                {
                    if (textureHair != null && textureHair[index, drawPlayer.hair] != ModContent.Request<Texture2D>("MrPlagueRaces/Assets/Textures/Blank"))
                    {
                        drawData = new DrawData(textureHair[index, drawPlayer.hair].Value, position, sourceRect, PlayerColor(ref drawInfo, index), rotation, origin, scale, effect, 0);
                        drawData.shader = PlayerShader(ref drawInfo, index);
                        drawInfo.DrawDataCache.Add(drawData);
                    }
                    if (texture != null && texture[index] != ModContent.Request<Texture2D>("MrPlagueRaces/Assets/Textures/Blank"))
                    {
                        drawData = new DrawData(texture[index].Value, position, sourceRect, PlayerColor(ref drawInfo, index), rotation, origin, scale, effect, 0);
                        drawData.shader = PlayerShader(ref drawInfo, index);
                        drawInfo.DrawDataCache.Add(drawData);
                    }
                }
            }

            private Color PlayerColor(ref PlayerDrawSet drawInfo, int index)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                var luner = drawPlayer.GetModPlayer<luner>();
                Color color = (index == 0 ? drawInfo.colorHead : index == 1 ? luner.colorDetail : index == 2 ? drawInfo.colorEyeWhites : index == 3 ? drawInfo.colorEyes : index == 4 ? drawInfo.colorHair : index == 5 ? drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, 0f) : index == 6 ? drawPlayer.GetImmuneAlpha(luner.detailColor, 0f) : index == 7 ? drawPlayer.GetImmuneAlpha(Color.White, 0f) : index == 8 ? drawPlayer.GetImmuneAlpha(drawPlayer.eyeColor, 0f) : drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(useLighting: false), 0f));
                return color;
            }

            private int PlayerShader(ref PlayerDrawSet drawInfo, int index)
            {
                int shader = (index == 0 ? drawInfo.skinDyePacked : index == 1 ? drawInfo.skinDyePacked : index == 2 ? 0 : index == 3 ? 0 : index == 4 ? drawInfo.hairDyePacked : index == 5 ? drawInfo.skinDyePacked : index == 6 ? drawInfo.skinDyePacked : index == 7 ? 0 : index == 8 ? 0 : drawInfo.hairDyePacked);
                return shader;
            }
        }

        public override void ModifyDrawLayers(Player player, List<Player> layers)
        {
            var modPlayer = player.GetModPlayer<MrPlagueRaces.Hunger>();

            bool hideChestplate = modPlayer.hideChestplate;
            bool hideLeggings = modPlayer.hideLeggings;

            Main.playerTextures[0, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head");
            Main.playerTextures[0, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2");
            Main.playerTextures[0, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes");
            Main.playerTextures[0, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[0, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_1");
            }
            else
            {
                Main.playerTextures[0, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[0, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[0, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_1");
            }
            else
            {
                Main.playerTextures[0, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[0, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[0, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_1");
            }
            else
            {
                Main.playerTextures[0, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[0, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");
            Main.playerTextures[0, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[0, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_1");
                Main.playerTextures[0, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_1");
            }
            else
            {
                Main.playerTextures[0, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[0, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[0, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_1_2");
            }
            else
            {
                Main.playerTextures[0, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[0, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_1_2");
            }
            else
            {
                Main.playerTextures[0, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[1, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head");
            Main.playerTextures[1, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2");
            Main.playerTextures[1, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes");
            Main.playerTextures[1, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[1, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_2");
            }
            else
            {
                Main.playerTextures[1, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[1, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[1, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_2");
            }
            else
            {
                Main.playerTextures[1, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[1, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[1, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_2");
            }
            else
            {
                Main.playerTextures[1, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[1, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");
            Main.playerTextures[1, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[1, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_2");
                Main.playerTextures[1, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_2");
            }
            else
            {
                Main.playerTextures[1, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[1, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[1, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_2_2");
            }
            else
            {
                Main.playerTextures[1, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[1, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_2_2");
            }
            else
            {
                Main.playerTextures[1, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[2, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head");
            Main.playerTextures[2, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2");
            Main.playerTextures[2, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes");
            Main.playerTextures[2, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[2, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_3");
            }
            else
            {
                Main.playerTextures[2, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[2, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[2, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_3");
            }
            else
            {
                Main.playerTextures[2, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[2, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[2, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_3");
            }
            else
            {
                Main.playerTextures[2, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[2, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");
            Main.playerTextures[2, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[2, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_3");
                Main.playerTextures[2, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_3");
            }
            else
            {
                Main.playerTextures[2, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[2, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[2, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_3_2");
            }
            else
            {
                Main.playerTextures[2, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[2, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_3_2");
            }
            else
            {
                Main.playerTextures[2, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[3, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head");
            Main.playerTextures[3, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2");
            Main.playerTextures[3, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes");
            Main.playerTextures[3, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[3, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_4");
            }
            else
            {
                Main.playerTextures[3, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[3, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[3, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_4");
            }
            else
            {
                Main.playerTextures[3, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[3, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[3, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_4");
            }
            else
            {
                Main.playerTextures[3, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[3, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");
            Main.playerTextures[3, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[3, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_4");
                Main.playerTextures[3, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_4");
            }
            else
            {
                Main.playerTextures[3, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[3, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[3, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_4_2");
            }
            else
            {
                Main.playerTextures[3, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[3, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_4_2");
            }
            else
            {
                Main.playerTextures[3, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[8, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head");
            Main.playerTextures[8, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2");
            Main.playerTextures[8, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes");
            Main.playerTextures[8, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[8, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_9");
            }
            else
            {
                Main.playerTextures[8, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[8, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[8, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_9");
            }
            else
            {
                Main.playerTextures[8, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[8, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[8, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_9");
            }
            else
            {
                Main.playerTextures[8, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[8, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand");
            Main.playerTextures[8, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[8, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_9");
                Main.playerTextures[8, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_9");
            }
            else
            {
                Main.playerTextures[8, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[8, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[8, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_9_2");
            }
            else
            {
                Main.playerTextures[8, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[8, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_9_2");
            }
            else
            {
                Main.playerTextures[8, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[4, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head_Female");
            Main.playerTextures[4, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2_Female");
            Main.playerTextures[4, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_Female");
            Main.playerTextures[4, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[4, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_5");
            }
            else
            {
                Main.playerTextures[4, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[4, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[4, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_5");
            }
            else
            {
                Main.playerTextures[4, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[4, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[4, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_5");
            }
            else
            {
                Main.playerTextures[4, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[4, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");
            Main.playerTextures[4, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg_Female");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[4, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_5");
                Main.playerTextures[4, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_5");
            }
            else
            {
                Main.playerTextures[4, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[4, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[4, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_5_2");
            }
            else
            {
                Main.playerTextures[4, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[4, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_5_2");
            }
            else
            {
                Main.playerTextures[4, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[5, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head_Female");
            Main.playerTextures[5, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2_Female");
            Main.playerTextures[5, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_Female");
            Main.playerTextures[5, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[5, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_6");
            }
            else
            {
                Main.playerTextures[5, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[5, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[5, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_6");
            }
            else
            {
                Main.playerTextures[5, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[5, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[5, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_6");
            }
            else
            {
                Main.playerTextures[5, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[5, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");
            Main.playerTextures[5, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg_Female");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[5, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_6");
                Main.playerTextures[5, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_6");
            }
            else
            {
                Main.playerTextures[5, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[5, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[5, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_6_2");
            }
            else
            {
                Main.playerTextures[5, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[5, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_6_2");
            }
            else
            {
                Main.playerTextures[5, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[6, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head_Female");
            Main.playerTextures[6, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2_Female");
            Main.playerTextures[6, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_Female");
            Main.playerTextures[6, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[6, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_7");
            }
            else
            {
                Main.playerTextures[6, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[6, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[6, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_7");
            }
            else
            {
                Main.playerTextures[6, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[6, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[6, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_7");
            }
            else
            {
                Main.playerTextures[6, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[6, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");
            Main.playerTextures[6, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg_Female");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[6, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_7");
                Main.playerTextures[6, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_7");
            }
            else
            {
                Main.playerTextures[6, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[6, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[6, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_7_2");
            }
            else
            {
                Main.playerTextures[6, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[6, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_7_2");
            }
            else
            {
                Main.playerTextures[6, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[7, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head_Female");
            Main.playerTextures[7, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2_Female");
            Main.playerTextures[7, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_Female");
            Main.playerTextures[7, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[7, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_8");
            }
            else
            {
                Main.playerTextures[7, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[7, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[7, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_8");
            }
            else
            {
                Main.playerTextures[7, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[7, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[7, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_8");
            }
            else
            {
                Main.playerTextures[7, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[7, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");
            Main.playerTextures[7, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg_Female");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[7, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_8");
                Main.playerTextures[7, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_8");
            }
            else
            {
                Main.playerTextures[7, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[7, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[7, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_8_2");
            }
            else
            {
                Main.playerTextures[7, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[7, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_8_2");
            }
            else
            {
                Main.playerTextures[7, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[9, 0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Head_Female");
            Main.playerTextures[9, 1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_2_Female");
            Main.playerTextures[9, 2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Eyes_Female");
            Main.playerTextures[9, 3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Body_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[9, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeves_10");
            }
            else
            {
                Main.playerTextures[9, 4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[9, 5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[9, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shirt_10");
            }
            else
            {
                Main.playerTextures[9, 6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[9, 7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Arm_Female");

            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[9, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_10");
            }
            else
            {
                Main.playerTextures[9, 8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerTextures[9, 9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hand_Female");
            Main.playerTextures[9, 10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Leg_Female");

            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[9, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_10");
                Main.playerTextures[9, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Shoes_10");
            }
            else
            {
                Main.playerTextures[9, 11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
                Main.playerTextures[9, 12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[1].type == ItemID.FamiliarShirt || player.armor[11].type == ItemID.FamiliarShirt) && !hideChestplate)
            {
                Main.playerTextures[9, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Sleeve_10_2");
            }
            else
            {
                Main.playerTextures[9, 13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }
            if ((player.armor[2].type == ItemID.FamiliarPants || player.armor[12].type == ItemID.FamiliarPants) && !hideLeggings)
            {
                Main.playerTextures[9, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Pants_10_2");
            }
            else
            {
                Main.playerTextures[9, 14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            }

            Main.playerHairTexture[0] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_1");
            Main.playerHairTexture[1] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_2");
            Main.playerHairTexture[2] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_3");
            Main.playerHairTexture[3] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_4");
            Main.playerHairTexture[4] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_5");
            Main.playerHairTexture[5] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_6");
            Main.playerHairTexture[6] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_7");
            Main.playerHairTexture[7] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_8");
            Main.playerHairTexture[8] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_9");
            Main.playerHairTexture[9] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_10");
            Main.playerHairTexture[10] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_11");
            Main.playerHairTexture[11] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_12");
            Main.playerHairTexture[12] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_13");
            Main.playerHairTexture[13] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_14");
            Main.playerHairTexture[14] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_15");
            Main.playerHairTexture[15] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_16");
            Main.playerHairTexture[16] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_17");
            Main.playerHairTexture[17] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_18");
            Main.playerHairTexture[18] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_19");
            Main.playerHairTexture[19] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_20");
            Main.playerHairTexture[20] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_21");
            Main.playerHairTexture[21] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_22");
            Main.playerHairTexture[22] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_23");
            Main.playerHairTexture[23] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_24");
            Main.playerHairTexture[24] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_25");
            Main.playerHairTexture[25] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_26");
            Main.playerHairTexture[26] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_27");
            Main.playerHairTexture[27] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_28");
            Main.playerHairTexture[28] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_29");
            Main.playerHairTexture[29] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_30");
            Main.playerHairTexture[30] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_31");
            Main.playerHairTexture[31] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_32");
            Main.playerHairTexture[32] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_33");
            Main.playerHairTexture[33] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_34");
            Main.playerHairTexture[34] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_35");
            Main.playerHairTexture[35] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_36");
            Main.playerHairTexture[36] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_37");
            Main.playerHairTexture[37] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_38");
            Main.playerHairTexture[38] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_39");
            Main.playerHairTexture[39] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_40");
            Main.playerHairTexture[40] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_41");
            Main.playerHairTexture[41] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_42");
            Main.playerHairTexture[42] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_43");
            Main.playerHairTexture[43] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_44");
            Main.playerHairTexture[44] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_45");
            Main.playerHairTexture[45] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_46");
            Main.playerHairTexture[46] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_47");
            Main.playerHairTexture[47] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_48");
            Main.playerHairTexture[48] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_49");
            Main.playerHairTexture[49] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_50");
            Main.playerHairTexture[50] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_51");
            Main.playerHairTexture[51] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_51");
            Main.playerHairTexture[51] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_52");
            Main.playerHairTexture[52] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_53");
            Main.playerHairTexture[53] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_54");
            Main.playerHairTexture[54] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_55");
            Main.playerHairTexture[55] = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Hairstyles/Hair_56");
            Main.playerHairTexture[56] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[57] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[58] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[59] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[60] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[61] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[62] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[63] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[64] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[65] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[66] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[67] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[68] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[69] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[70] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[71] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[72] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[73] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[74] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[75] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[76] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[77] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[78] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[79] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[80] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[81] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[82] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[83] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[84] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[85] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[86] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[87] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[88] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[89] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[90] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[91] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[92] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[93] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[94] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[95] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[96] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[97] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[98] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[99] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[100] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[101] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[102] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[103] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[104] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[105] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[106] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[107] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[108] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[109] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[110] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[111] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[112] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[113] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[114] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[115] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[116] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[117] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[118] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[119] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[120] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[121] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[122] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[123] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[124] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[125] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[126] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[127] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[128] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[129] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[130] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[131] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[132] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairTexture[133] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");

            Main.playerHairAltTexture[0] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[1] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[2] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[3] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[4] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[5] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[6] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[7] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[8] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[9] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[10] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[11] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[12] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[13] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[14] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[15] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[16] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[17] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[18] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[19] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[20] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[21] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[22] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[23] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[24] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[25] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[26] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[27] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[28] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[29] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[30] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[31] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[32] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[33] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[34] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[35] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[36] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[37] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[38] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[39] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[40] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[41] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[42] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[43] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[44] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[45] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[46] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[47] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[48] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[49] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[50] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[51] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[52] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[53] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[54] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[55] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[56] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[57] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[58] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[59] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[60] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[61] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[62] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[63] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[64] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[65] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[66] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[67] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[68] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[69] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[70] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[71] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[72] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[73] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[74] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[75] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[76] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[77] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[78] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[79] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[80] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[81] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[82] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[83] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[84] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[85] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[86] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[87] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[88] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[89] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[90] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[91] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[92] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[93] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[94] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[95] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[96] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[97] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[98] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[99] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[100] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[101] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[102] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[103] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[104] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[105] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[106] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[107] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[108] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[109] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[110] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[111] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[112] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[113] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[114] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[115] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[116] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[117] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[118] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[119] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[120] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[121] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[122] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[123] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[124] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[125] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[126] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[127] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[128] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[129] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[130] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[131] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[132] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");
            Main.playerHairAltTexture[133] = ModContent.GetTexture("MrPlagueRaces/Content/RaceTextures/Blank");

            Main.ghostTexture = ModContent.GetTexture("luner/Content/RaceTextures/Hunger/Ghost");
        }
    }
}