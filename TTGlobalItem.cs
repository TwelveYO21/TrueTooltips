﻿global using Microsoft.Xna.Framework.Graphics;
global using System.Collections.Generic;
global using Terraria;
global using Terraria.GameContent;
global using Terraria.ModLoader;
global using static Terraria.Main;
global using static Terraria.ModLoader.ModContent;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria.Localization;
using Terraria.UI.Chat;
using static Terraria.ID.Colors;

namespace TrueTooltips;

internal class TTGlobalItem : GlobalItem
{
    internal static Texture2D[] Corners = new Texture2D[4];
    private static Color priceColor;

    private static Color RarityColor(Item item, TooltipLine line = null) => line switch
    {
        { OverrideColor: not null } => line.OverrideColor.Value,
        { Name: "ItemName", Mod: "Terraria" } or null => item switch
        {
            { rare: 0 } => Color.White,
            { expert: true } or { rare: -12 } => new(DiscoR, DiscoG, DiscoB),
            { master: true } or { rare: -13 } => new(255, (int)(masterColor * 200f), 0),
            _ => Terraria.GameContent.UI.ItemRarity.GetColor(item.rare)
        },
        { IsModifier: true, IsModifierBad: false } => new(120, 190, 120),
        { IsModifierBad: true } => new(190, 120, 120),
        { Name: "JourneyResearch", Mod: "Terraria" } => new(255, 120, 187),
        { Name: "Price", Mod: "Terraria" } => priceColor,
        _ => Color.White
    };

    private static Color TextPulse(Color color) => new(color.R * mouseTextColor / 255, color.G * mouseTextColor / 255, color.B * mouseTextColor / 255, mouseTextColor);

    public override void ModifyTooltips(Item item, List<TooltipLine> lines)
    {
        Config config = GetInstance<Config>();
        int value = 0;
        Item itemAmmo = null;
        Player player = LocalPlayer;

        TooltipLine Find(string name) => lines.Find(l => l.Name == name);

        TooltipLine itemName = Find("ItemName");
        TooltipLine favorite = Find("Favorite");
        TooltipLine favoriteDesc = Find("FavoriteDesc");
        TooltipLine noTransfer = Find("NoTransfer");
        TooltipLine social = Find("Social");
        TooltipLine socialDesc = Find("SocialDesc");
        TooltipLine damage = Find("Damage");
        TooltipLine critChance = Find("CritChance");
        TooltipLine speed = Find("Speed");
        TooltipLine noSpeedScaling = Find("NoSpeedScaling");
        TooltipLine specialSpeedScaling = Find("SpecialSpeedScaling");
        TooltipLine knockback = Find("Knockback");
        TooltipLine fishingPower = Find("FishingPower");
        TooltipLine needsBait = Find("NeedsBait");
        TooltipLine baitPower = Find("BaitPower");
        TooltipLine equipable = Find("Equipable");
        TooltipLine wandConsumes = Find("WandConsumes");
        TooltipLine quest = Find("Quest");
        TooltipLine vanity = Find("Vanity");
        TooltipLine vanityLegal = Find("VanityLegal");
        TooltipLine defense = Find("Defense");
        TooltipLine pickPower = Find("PickPower");
        TooltipLine axePower = Find("AxePower");
        TooltipLine hammerPower = Find("HammerPower");
        TooltipLine tileBoost = Find("TileBoost");
        TooltipLine healLife = Find("HealLife");
        TooltipLine healMana = Find("HealMana");
        TooltipLine useMana = Find("UseMana");
        TooltipLine placeable = Find("Placeable");
        TooltipLine ammo = Find("Ammo");
        TooltipLine consumable = Find("Consumable");
        TooltipLine material = Find("Material");
        TooltipLine etherianManaWarning = Find("EtherianManaWarning");
        TooltipLine wellFedExpert = Find("WellFedExpert");
        TooltipLine buffTime = Find("BuffTime");
        TooltipLine oneDropLogo = Find("OneDropLogo");
        TooltipLine setBonus = Find("SetBonus");
        TooltipLine expert = Find("Expert");
        TooltipLine master = Find("Master");
        TooltipLine journeyResearch = Find("JourneyResearch");
        TooltipLine modifiedByMods = Find("ModifiedByMods");
        TooltipLine bestiaryNotes = Find("BestiaryNotes");
        TooltipLine specialPrice = Find("SpecialPrice");
        TooltipLine price = Find("Price");

        static string Translation(string key) => Language.GetTextValue("Mods.TrueTooltips.TTGlobalItem." + key);

        if(item.useAmmo > 0 && (config.ItemAmmo || config.WpnPlusAmmoDmg || config.WpnPlusAmmoKb && config.BetterKnockback))
            itemAmmo = player.ChooseAmmo(item);

        if((keyState.PressingShift() && config.ItemMod == Config.State.Shift || config.ItemMod == Config.State.Always) && item.ModItem != null && itemName != null)
            itemName.Text += " - " + item.ModItem.Mod.DisplayName;

        if(favorite != null)
        {
            if(!config.Favorite.On)
                lines.Remove(favorite);
            else if(config.Favorite.Color != Color.White)
                favorite.OverrideColor = config.Favorite.Color;
        }

        if(favoriteDesc != null)
        {
            if(!config.FavoriteDesc.On)
                lines.Remove(favoriteDesc);
            else if(config.FavoriteDesc.Color != Color.White)
                favoriteDesc.OverrideColor = config.FavoriteDesc.Color;
        }

        if(noTransfer != null)
        {
            if(!config.NoTransfer.On)
                lines.Remove(noTransfer);
            else if(config.NoTransfer.Color != Color.White)
                noTransfer.OverrideColor = config.NoTransfer.Color;
        }

        if(social != null)
        {
            if(!config.Social.On)
                lines.Remove(social);
            else if(config.Social.Color != Color.White)
                social.OverrideColor = config.Social.Color;
        }

        if(socialDesc != null)
        {
            if(!config.SocialDesc.On)
                lines.Remove(socialDesc);
            else if(config.SocialDesc.Color != Color.White)
                socialDesc.OverrideColor = config.SocialDesc.Color;
        }

        if(damage != null)
        {
            if(!config.Damage.On)
                lines.Remove(damage);
            else
            {
                if(config.Damage.Color != Color.White)
                    damage.OverrideColor = config.Damage.Color;
                if(itemAmmo != null && config.WpnPlusAmmoDmg)
                    damage.Text = Regex.Replace(damage.Text, @"^\d+", keyState.PressingShift() && config.AmmoDmgKbSeparate == Config.State.Shift || config.AmmoDmgKbSeparate == Config.State.Always ? player.GetWeaponDamage(item, true) + " + " + player.GetWeaponDamage(itemAmmo, true) : (player.GetWeaponDamage(item, true) + player.GetWeaponDamage(itemAmmo, true)).ToString());
            }
        }

        if(critChance != null)
        {
            if(!config.CritChance.On)
                lines.Remove(critChance);
            else if(config.CritChance.Color != Color.White)
                critChance.OverrideColor = config.CritChance.Color;
        }

        if(speed != null)
        {
            if(!config.Speed.On)
                lines.Remove(speed);
            else
            {
                if(config.Speed.Color != Color.White)
                    speed.OverrideColor = config.Speed.Color;
                if(config.BetterSpeed)
                    speed.Text = Math.Round(60d / item.useAnimation, 2) + Translation("Speed");
            }
        }

        if(noSpeedScaling != null)
        {
            if(!config.NoSpeedScaling.On)
                lines.Remove(noSpeedScaling);
            else if(config.NoSpeedScaling.Color != Color.White)
                noSpeedScaling.OverrideColor = config.NoSpeedScaling.Color;
        }

        if(specialSpeedScaling != null)
        {
            if(!config.SpecialSpeedScaling.On)
                lines.Remove(specialSpeedScaling);
            else if(config.SpecialSpeedScaling.Color != Color.White)
                specialSpeedScaling.OverrideColor = config.SpecialSpeedScaling.Color;
        }

        if(knockback != null)
        {
            if(!config.Knockback.On)
                lines.Remove(knockback);
            else
            {
                if(config.Knockback.Color != Color.White)
                    knockback.OverrideColor = config.Knockback.Color;
                if(config.BetterKnockback)
                    knockback.Text = ((keyState.PressingShift() && config.AmmoDmgKbSeparate == Config.State.Shift || config.AmmoDmgKbSeparate == Config.State.Always) && itemAmmo != null && config.WpnPlusAmmoKb ? Math.Round(player.GetWeaponKnockback(item, item.knockBack), 2) + " + " + Math.Round(player.GetWeaponKnockback(itemAmmo, itemAmmo.knockBack), 2) : Math.Round(player.GetWeaponKnockback(item, item.knockBack) + (itemAmmo != null && config.WpnPlusAmmoKb ? player.GetWeaponKnockback(itemAmmo, itemAmmo.knockBack) : 0), 2)) + Translation("Knockback");
            }
        }

        if(fishingPower != null)
        {
            if(!config.FishingPower.On)
                lines.Remove(fishingPower);
            else if(config.FishingPower.Color != Color.White)
                fishingPower.OverrideColor = config.FishingPower.Color;
        }

        if(needsBait != null)
        {
            if(!config.NeedsBait.On)
                lines.Remove(needsBait);
            else if(config.NeedsBait.Color != Color.White)
                needsBait.OverrideColor = config.NeedsBait.Color;
        }

        if(baitPower != null)
        {
            if(!config.BaitPower.On)
                lines.Remove(baitPower);
            else if(config.BaitPower.Color != Color.White)
                baitPower.OverrideColor = config.BaitPower.Color;
        }

        if(equipable != null)
        {
            if(!config.Equipable.On)
                lines.Remove(equipable);
            else if(config.Equipable.Color != Color.White)
                equipable.OverrideColor = config.Equipable.Color;
        }

        if(wandConsumes != null)
        {
            if(!config.WandConsumes.On)
                lines.Remove(wandConsumes);
            else if(config.WandConsumes.Color != Color.White)
                wandConsumes.OverrideColor = config.WandConsumes.Color;
        }

        if(quest != null)
        {
            if(!config.Quest.On)
                lines.Remove(quest);
            else if(config.Quest.Color != Color.White)
                quest.OverrideColor = config.Quest.Color;
        }

        if(vanity != null)
        {
            if(!config.Vanity.On)
                lines.Remove(vanity);
            else if(config.Vanity.Color != Color.White)
                vanity.OverrideColor = config.Vanity.Color;
        }

        if(vanityLegal != null)
        {
            if(!config.VanityLegal.On)
                lines.Remove(vanityLegal);
            else if(config.VanityLegal.Color != Color.White)
                vanityLegal.OverrideColor = config.VanityLegal.Color;
        }

        if(defense != null)
        {
            if(!config.Defense.On)
                lines.Remove(defense);
            else if(config.Defense.Color != Color.White)
                defense.OverrideColor = config.Defense.Color;
        }

        if(pickPower != null)
        {
            if(!config.PickPower.On)
                lines.Remove(pickPower);
            else if(config.PickPower.Color != Color.White)
                pickPower.OverrideColor = config.PickPower.Color;
        }

        if(axePower != null)
        {
            if(!config.AxePower.On)
                lines.Remove(axePower);
            else if(config.AxePower.Color != Color.White)
                axePower.OverrideColor = config.AxePower.Color;
        }

        if(hammerPower != null)
        {
            if(!config.HammerPower.On)
                lines.Remove(hammerPower);
            else if(config.HammerPower.Color != Color.White)
                hammerPower.OverrideColor = config.HammerPower.Color;
        }

        if(tileBoost != null)
        {
            if(!config.TileBoost.On)
                lines.Remove(tileBoost);
            else if(config.TileBoost.Color != Color.White)
                tileBoost.OverrideColor = config.TileBoost.Color;
        }

        if(healLife != null)
        {
            if(!config.HealLife.On)
                lines.Remove(healLife);
            else if(config.HealLife.Color != Color.White)
                healLife.OverrideColor = config.HealLife.Color;
        }

        if(healMana != null)
        {
            if(!config.HealMana.On)
                lines.Remove(healMana);
            else if(config.HealMana.Color != Color.White)
                healMana.OverrideColor = config.HealMana.Color;
        }

        if(useMana != null)
        {
            if(!config.UseMana.On)
                lines.Remove(useMana);
            else if(config.UseMana.Color != Color.White)
                useMana.OverrideColor = config.UseMana.Color;
        }

        if(placeable != null)
        {
            if(!config.Placeable.On)
                lines.Remove(placeable);
            else if(config.Placeable.Color != Color.White)
                placeable.OverrideColor = config.Placeable.Color;
        }

        if(ammo != null)
        {
            if(!config.Ammo.On)
                lines.Remove(ammo);
            else if(config.Ammo.Color != Color.White)
                ammo.OverrideColor = config.Ammo.Color;
        }

        if(consumable != null)
        {
            if(!config.Consumable.On)
                lines.Remove(consumable);
            else if(config.Consumable.Color != Color.White)
                consumable.OverrideColor = config.Consumable.Color;
        }

        if(material != null)
        {
            if(!config.Material.On)
                lines.Remove(material);
            else if(config.Material.Color != Color.White)
                material.OverrideColor = config.Material.Color;
        }

        if(etherianManaWarning != null)
        {
            if(!config.EtherianManaWarning.On)
                lines.Remove(etherianManaWarning);
            else if(config.EtherianManaWarning.Color != Color.White)
                etherianManaWarning.OverrideColor = config.EtherianManaWarning.Color;
        }

        if(wellFedExpert != null)
        {
            if(!config.WellFedExpert.On)
                lines.Remove(wellFedExpert);
            else if(config.WellFedExpert.Color != Color.White)
                wellFedExpert.OverrideColor = config.WellFedExpert.Color;
        }

        if(buffTime != null)
        {
            if(!config.BuffTime.On)
                lines.Remove(buffTime);
            else if(config.BuffTime.Color != Color.White)
                buffTime.OverrideColor = config.BuffTime.Color;
        }

        if(oneDropLogo != null)
        {
            if(!config.OneDropLogo.On)
                lines.Remove(oneDropLogo);
            else if(config.OneDropLogo.Color != Color.White)
                oneDropLogo.OverrideColor = config.OneDropLogo.Color;
        }

        if(item.prefix > 0)
        {
            if(!config.GoodModifier.On)
                lines.RemoveAll(l => l.IsModifier && !l.IsModifierBad);
            if(!config.BadModifier.On)
                lines.RemoveAll(l => l.IsModifierBad);

            foreach(TooltipLine line in lines)
            {
                if(line.IsModifier && !line.IsModifierBad && config.GoodModifier.Color != new Color(120, 190, 120))
                    line.OverrideColor = config.GoodModifier.Color;
                if(line.IsModifierBad && config.BadModifier.Color != new Color(190, 120, 120))
                    line.OverrideColor = config.BadModifier.Color;
            }
        }

        if(setBonus != null)
        {
            if(!config.SetBonus.On)
                lines.Remove(setBonus);
            else if(config.SetBonus.Color != Color.White)
                setBonus.OverrideColor = config.SetBonus.Color;
        }

        if(expert != null)
        {
            if(!config.Expert.On)
                lines.Remove(expert);
            else if(config.Expert.Color != Color.White)
                expert.OverrideColor = config.Expert.Color;
        }

        if(master != null)
        {
            if(!config.Master.On)
                lines.Remove(master);
            else if(config.Master.Color != Color.White)
                master.OverrideColor = config.Master.Color;
        }

        if(journeyResearch != null)
        {
            if(!config.JourneyResearch.On)
                lines.Remove(journeyResearch);
            else if(config.JourneyResearch.Color != new Color(255, 120, 187))
                journeyResearch.OverrideColor = config.JourneyResearch.Color;
        }

        if(modifiedByMods != null)
        {
            if(!config.ModifiedByMods.On)
                lines.Remove(modifiedByMods);
            else if(config.ModifiedByMods.Color != Color.White)
                modifiedByMods.OverrideColor = config.ModifiedByMods.Color;
        }

        if(bestiaryNotes != null)
        {
            if(!config.BestiaryNotes.On)
                lines.Remove(bestiaryNotes);
            else if(config.BestiaryNotes.Color != Color.White)
                bestiaryNotes.OverrideColor = config.BestiaryNotes.Color;
        }

        if(item.useAmmo > 0 && config.ItemAmmo)
            lines.Add(new TooltipLine(Mod, "ItemAmmo", itemAmmo != null ? itemAmmo.HoverName + ((keyState.PressingShift() && config.ItemMod == Config.State.Shift || config.ItemMod == Config.State.Always) && itemAmmo.ModItem != null ? " - " + itemAmmo.ModItem.Mod.DisplayName : string.Empty) : Translation("No") + item.useAmmo switch { 40 => Translation("Arrow"), 71 => Translation("Coin"), 97 => Translation("Bullet"), 283 => Translation("Dart"), 771 => Translation("Rocket"), 780 => Translation("Solution"), _ => Lang.GetItemNameValue(item.useAmmo) }) { OverrideColor = itemAmmo != null ? RarityColor(itemAmmo) : new Color(120, 120, 120) });

        if(item.shopSpecialCurrency == -1 && (item.type < 71 || item.type > 74) && config.Price)
        {
            player.GetItemExpectedPrice(item, out int calcForSelling, out int calcForBuying);

            if((value = item.isAShopItem || item.buyOnce ? calcForBuying : calcForSelling) > 0)
            {
                int value2 = value * item.stack;

                if(!item.buy)
                {
                    int amount = shopSellbackHelper.GetAmount(item);

                    if((value2 = value / 5) < 1)
                        value2 = 1;

                    int temp = value2;

                    value2 *= item.stack;

                    if(amount > 0)
                        value2 += (calcForBuying - temp) * Math.Min(amount, item.stack);
                }

                if(config.BetterPrice)
                {
                    int platinum = value2 / 1_000_000,
                        gold = value2 / 10000 % 100,
                        silver = value2 / 100 % 100,
                        copper = value2 % 100;

                    lines.Add(new TooltipLine(Mod, "BetterPrice", ((platinum > 0 ? $"[c/{TextPulse(CoinPlatinum).Hex3()}:{platinum} {Lang.inter[15].Value} ]" : string.Empty) + (gold > 0 ? $"[c/{TextPulse(CoinGold).Hex3()}:{gold} {Lang.inter[16].Value} ]" : string.Empty) + (silver > 0 ? $"[c/{TextPulse(CoinSilver).Hex3()}:{silver} {Lang.inter[17].Value} ]" : string.Empty) + (copper > 0 ? $"[c/{TextPulse(CoinCopper).Hex3()}:{copper} {Lang.inter[18].Value}]" : string.Empty)).TrimEnd()));
                }

                priceColor = value2 switch
                {
                    >= 1_000_000 => CoinPlatinum,
                    >= 10000 => CoinGold,
                    >= 100 => CoinSilver,
                    _ => CoinCopper
                };

            }
            else if(item.type != 3817)
                priceColor = new Color(120, 120, 120);
        }

        if(specialPrice != null && !config.SpecialPrice)
            lines.Remove(specialPrice);
        if(price != null && (value > 0 && config.BetterPrice || !config.Price))
            lines.Remove(price);
    }

    public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int _x, ref int _y)
    {
        Config config = GetInstance<Config>();
        int bgPadBottom = 0;
        int bgPadLeft = 0;
        int bgPadRight = 0;
        int bgPadTop = 0;
        int offsetFromSprite = 0;
        int tooltipHeight = 0;
        int widestLineWidth = 0;
        int x = mouseX + config.XOffset + 20;
        int y = mouseY + config.YOffset + 20;
        Texture2D sprite = TextureAssets.Item[item.type].Value;
        Rectangle spriteSize = itemAnimations[item.type]?.GetFrame(sprite) ?? sprite.Frame();
        int spriteMax = new[] { spriteSize.Width + 28, spriteSize.Height + 28, 64 }.Max();

        static Vector2 StringSize(string text) => ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One);

        _x += config.XOffset;
        _y += config.YOffset;

        if(ThickMouse)
        {
            x += 6;
            y += 6;
        }

        if(!mouseItem.IsAir)
            x += 34;

        foreach(TooltipLine line in lines)
        {
            Vector2 stringSize = StringSize(line.Text);

            if(stringSize.X > widestLineWidth)
                widestLineWidth = (int)stringSize.X;

            if(line.Name == "OneDropLogo" && line.Mod == "Terraria")
                tooltipHeight += 24 + config.Spacing;
            else
                tooltipHeight += (int)stringSize.Y + config.Spacing;
        }

        if(config.Background.A > 0)
        {
            x += 4;
            y += 4;
            _x += 4;
            _y += 4;

            bgPadRight = config.BGPaddingRight + 14;
            bgPadLeft = -config.BGPaddingLeft - 14;
            bgPadBottom = config.BGPaddingBottom + 14;
            bgPadTop = -config.BGPaddingTop - 14;
        }

        if(config.Sprite == Config.State.Always || keyState.PressingShift() && config.Sprite == Config.State.Shift)
            offsetFromSprite = (spriteMax -= (spriteMax - spriteSize.Width) % 2) + 14;

        if(offsetFromSprite > 0)
            tooltipHeight = Math.Max(tooltipHeight, spriteMax);

        if(x + offsetFromSprite + widestLineWidth + bgPadRight > screenWidth)
            x = _x = screenWidth - bgPadRight - widestLineWidth - offsetFromSprite;
        if(x + bgPadLeft < 0)
            x = _x = -bgPadLeft;
        if(y + tooltipHeight + bgPadBottom > screenHeight)
            y = _y = screenHeight - bgPadBottom - tooltipHeight;
        if(y + bgPadTop < 0)
            y = _y = -bgPadTop;

        if(config.Background.A > 0)
        {

            if(config.SpriteBG.A > 0 && offsetFromSprite > 0)
            {
                int bgHeight = config.BGPaddingTop + tooltipHeight + config.BGPaddingBottom + 8;
                int bgWidth = config.BGPaddingLeft + offsetFromSprite + widestLineWidth + config.BGPaddingRight + 8;
                int bgX = x - config.BGPaddingLeft - 4;
                int bgY = y - config.BGPaddingTop - 4;
                Texture2D background = TextureAssets.InventoryBack13.Value;

                spriteBatch.Draw(background, new Vector2(bgX - 10, bgY - 10), new Rectangle(0, 0, 10, 10), config.Background);
                spriteBatch.Draw(background, new Vector2(bgX + bgWidth, bgY - 10), new Rectangle(background.Width - 10, 0, 10, 10), config.Background);
                spriteBatch.Draw(background, new Vector2(bgX - 10, bgY + bgHeight), new Rectangle(0, background.Height - 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Vector2(bgX + bgWidth, bgY + bgHeight), new Rectangle(background.Width - 10, background.Height - 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY - 10, bgWidth, 10), new Rectangle(10, 0, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX - 10, bgY, 10, bgHeight), new Rectangle(0, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX + bgWidth, bgY, 10, bgHeight), new Rectangle(background.Width - 10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY + bgHeight, bgWidth, 10), new Rectangle(10, background.Height - 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY, bgWidth, config.BGPaddingTop + 4), new Rectangle(10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, y, config.BGPaddingLeft + 4, spriteMax), new Rectangle(10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(x + spriteMax, y, widestLineWidth + config.BGPaddingRight + 18, spriteMax), new Rectangle(10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, y + spriteMax, bgWidth, tooltipHeight + config.BGPaddingBottom + 4 - spriteMax), new Rectangle(10, 10, 10, 10), config.Background);
                spriteBatch.Draw(Corners[0], new Vector2(x, y), config.Background);
                spriteBatch.Draw(Corners[1], new Vector2(x + spriteMax - 10, y), config.Background);
                spriteBatch.Draw(Corners[2], new Vector2(x, y + spriteMax - 10), config.Background);
                spriteBatch.Draw(Corners[3], new Vector2(x + spriteMax - 10, y + spriteMax - 10), config.Background);
            }
            else
                Utils.DrawInvBG(spriteBatch, x - config.BGPaddingLeft - 14, y - config.BGPaddingTop - 14, config.BGPaddingLeft + offsetFromSprite + widestLineWidth + config.BGPaddingRight + 28, config.BGPaddingTop + tooltipHeight + config.BGPaddingBottom + 28, config.Background);
        }

        if(config.SpriteBG.A > 0 && offsetFromSprite > 0)
            Utils.DrawInvBG(spriteBatch, x, y, spriteMax, spriteMax, config.SpriteBG);

        if(offsetFromSprite > 0)
        {
            float scale = 1f;
            Color color = Color.White;

            void Draw(Color color) => spriteBatch.Draw(sprite, new Vector2(x + (spriteMax - spriteSize.Width - (spriteMax - spriteSize.Width) % 2) / 2, y + (spriteMax - spriteSize.Height - (spriteMax - spriteSize.Height) % 2) / 2), spriteSize, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            _x += offsetFromSprite;

            Terraria.UI.ItemSlot.GetItemLight(ref color, ref scale, item);
            Draw(item.GetAlpha(color));
            if(item.color != Color.Transparent)
                Draw(item.GetColor(Color.White));
        }

        tooltipHeight = 0;

        foreach(TooltipLine line in lines)
        {
            if(line.Name == "OneDropLogo" && line.Mod == "Terraria")
            {
                spriteBatch.Draw(TextureAssets.OneDropLogo.Value, new Vector2(x + offsetFromSprite, y + tooltipHeight), TextPulse(RarityColor(item, line)));
                tooltipHeight += 24 + config.Spacing;
            }
            else
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(x + offsetFromSprite, y + tooltipHeight), TextPulse(RarityColor(item, line)), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                tooltipHeight += (int)StringSize(line.Text).Y + config.Spacing;
            }
        }

        return false;
    }
}
