﻿global using System.Collections.Generic;
global using Terraria.ModLoader;
global using static Terraria.Main;
global using static Terraria.ModLoader.ModContent;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;
using static Terraria.ID.Colors;

namespace TrueTooltips;

internal class TTGlobalItem : GlobalItem
{
    private Color RarityColor(Item item, TooltipLine line = null) => line switch
    {
        { OverrideColor: not null } => line.OverrideColor.Value,
        { Name: "ItemName", Mod: "Terraria" } or null => item switch
        {
            { rare: 0 } => Color.White,
            { expert: true } => new(DiscoR, DiscoG, DiscoB),
            { master: true } => new(255, masterColor * 200, 0),
            _ => Terraria.GameContent.UI.ItemRarity.GetColor(item.rare)
        },
        { IsModifier: true, IsModifierBad: false } => new(120, 190, 120),
        { IsModifierBad: true } => new(190, 120, 120),
        { Name: "JourneyResearch", Mod: "Terraria" } => new(255, 120, 187),
        _ => Color.White
    };

    private Color TextPulse(Color color) => new(color.R * mouseTextColor / 255, color.G * mouseTextColor / 255, color.B * mouseTextColor / 255, mouseTextColor);

    public override void ModifyTooltips(Item item, List<TooltipLine> lines)
    {
        Config config = GetInstance<Config>();
        Player player = LocalPlayer;
        Item itemAmmo = null;
        int value = 0;

        TooltipLine Find(string name) => lines.Find(l => l.Name == name);

        TooltipLine itemName = Find("ItemName"),
                    favorite = Find("Favorite"),
                    favoriteDesc = Find("FavoriteDesc"),
                    noTransfer = Find("NoTransfer"),
                    social = Find("Social"),
                    socialDesc = Find("SocialDesc"),
                    damage = Find("Damage"),
                    critChance = Find("CritChance"),
                    speed = Find("Speed"),
                    noSpeedScaling = Find("NoSpeedScaling"),
                    specialSpeedScaling = Find("SpecialSpeedScaling"),
                    knockback = Find("Knockback"),
                    fishingPower = Find("FishingPower"),
                    needsBait = Find("NeedsBait"),
                    baitPower = Find("BaitPower"),
                    equipable = Find("Equipable"),
                    wandConsumes = Find("WandConsumes"),
                    quest = Find("Quest"),
                    vanity = Find("Vanity"),
                    vanityLegal = Find("VanityLegal"),
                    defense = Find("Defense"),
                    pickPower = Find("PickPower"),
                    axePower = Find("AxePower"),
                    hammerPower = Find("HammerPower"),
                    tileBoost = Find("TileBoost"),
                    healLife = Find("HealLife"),
                    healMana = Find("HealMana"),
                    useMana = Find("UseMana"),
                    placeable = Find("Placeable"),
                    ammo = Find("Ammo"),
                    consumable = Find("Consumable"),
                    material = Find("Material"),
                    etherianManaWarning = Find("EtherianManaWarning"),
                    wellFedExpert = Find("WellFedExpert"),
                    buffTime = Find("BuffTime"),
                    oneDropLogo = Find("OneDropLogo"),
                    setBonus = Find("SetBonus"),
                    expert = Find("Expert"),
                    master = Find("Master"),
                    journeyResearch = Find("JourneyResearch"),
                    modifiedByMods = Find("ModifiedByMods"),
                    bestiaryNotes = Find("BestiaryNotes"),
                    specialPrice = Find("SpecialPrice"),
                    price = Find("Price");

        if(item.useAmmo > 0 && (config.ItemAmmo || config.WpnPlusAmmoDmg || config.WpnPlusAmmoKb && config.BetterKnockback)) itemAmmo = player.ChooseAmmo(item);

        if((keyState.PressingShift() || config.ItemModAlways) && item.ModItem != null && config.ItemMod && itemName != null) itemName.Text += " - " + item.ModItem.Mod.DisplayName;

        if(favorite != null)
        {
            if(!config.Favorite.On) lines.Remove(favorite);
            else if(config.Favorite.Color != Color.White) favorite.OverrideColor = config.Favorite.Color;
        }

        if(favoriteDesc != null)
        {
            if(!config.FavoriteDesc.On) lines.Remove(favoriteDesc);
            else if(config.FavoriteDesc.Color != Color.White) favoriteDesc.OverrideColor = config.FavoriteDesc.Color;
        }

        if(noTransfer != null)
        {
            if(!config.NoTransfer.On) lines.Remove(noTransfer);
            else if(config.NoTransfer.Color != Color.White) noTransfer.OverrideColor = config.NoTransfer.Color;
        }

        if(social != null)
        {
            if(!config.Social.On) lines.Remove(social);
            else if(config.Social.Color != Color.White) social.OverrideColor = config.Social.Color;
        }

        if(socialDesc != null)
        {
            if(!config.SocialDesc.On) lines.Remove(socialDesc);
            else if(config.SocialDesc.Color != Color.White) socialDesc.OverrideColor = config.SocialDesc.Color;
        }

        if(damage != null)
        {
            if(!config.Damage.On) lines.Remove(damage);
            else
            {
                if(config.Damage.Color != Color.White) damage.OverrideColor = config.Damage.Color;
                if(itemAmmo != null && config.WpnPlusAmmoDmg) damage.Text = Regex.Replace(damage.Text, @"^\d+", (player.GetWeaponDamage(item, true) + player.GetWeaponDamage(itemAmmo, true)).ToString());
            }
        }

        if(critChance != null)
        {
            if(!config.CritChance.On) lines.Remove(critChance);
            else if(config.CritChance.Color != Color.White) critChance.OverrideColor = config.CritChance.Color;
        }

        if(speed != null)
        {
            if(!config.Speed.On) lines.Remove(speed);
            else
            {
                if(config.Speed.Color != Color.White) speed.OverrideColor = config.Speed.Color;
                if(config.BetterSpeed) speed.Text = Math.Round(60f / item.useAnimation, 2) + Language.GetTextValue("Mods.TrueTooltips.TTGlobalItem.Speed");
            }
        }

        if(noSpeedScaling != null)
        {
            if(!config.NoSpeedScaling.On) lines.Remove(noSpeedScaling);
            else if(config.NoSpeedScaling.Color != Color.White) noSpeedScaling.OverrideColor = config.NoSpeedScaling.Color;
        }

        if(specialSpeedScaling != null)
        {
            if(!config.SpecialSpeedScaling.On) lines.Remove(specialSpeedScaling);
            else if(config.SpecialSpeedScaling.Color != Color.White) specialSpeedScaling.OverrideColor = config.SpecialSpeedScaling.Color;
        }

        if(knockback != null)
        {
            if(!config.Knockback.On) lines.Remove(knockback);
            else
            {
                if(config.Knockback.Color != Color.White) knockback.OverrideColor = config.Knockback.Color;
                if(config.BetterKnockback) knockback.Text = Math.Round(player.GetWeaponKnockback(item, item.knockBack) + (itemAmmo != null && config.WpnPlusAmmoDmg ? player.GetWeaponKnockback(itemAmmo, itemAmmo.knockBack) : 0), 2) + Language.GetTextValue("Mods.TrueTooltips.TTGlobalItem.Knockback");
            }
        }

        if(fishingPower != null)
        {
            if(!config.FishingPower.On) lines.Remove(fishingPower);
            else if(config.FishingPower.Color != Color.White) fishingPower.OverrideColor = config.FishingPower.Color;
        }

        if(needsBait != null)
        {
            if(!config.NeedsBait.On) lines.Remove(needsBait);
            else if(config.NeedsBait.Color != Color.White) needsBait.OverrideColor = config.NeedsBait.Color;
        }

        if(baitPower != null)
        {
            if(!config.BaitPower.On) lines.Remove(baitPower);
            else if(config.BaitPower.Color != Color.White) baitPower.OverrideColor = config.BaitPower.Color;
        }

        if(equipable != null)
        {
            if(!config.Equipable.On) lines.Remove(equipable);
            else if(config.Equipable.Color != Color.White) equipable.OverrideColor = config.Equipable.Color;
        }

        if(wandConsumes != null)
        {
            if(!config.WandConsumes.On) lines.Remove(wandConsumes);
            else if(config.WandConsumes.Color != Color.White) wandConsumes.OverrideColor = config.WandConsumes.Color;
        }

        if(quest != null)
        {
            if(!config.Quest.On) lines.Remove(quest);
            else if(config.Quest.Color != Color.White) quest.OverrideColor = config.Quest.Color;
        }

        if(vanity != null)
        {
            if(!config.Vanity.On) lines.Remove(vanity);
            else if(config.Vanity.Color != Color.White) vanity.OverrideColor = config.Vanity.Color;
        }

        if(vanityLegal != null)
        {
            if(!config.VanityLegal.On) lines.Remove(vanityLegal);
            else if(config.VanityLegal.Color != Color.White) vanityLegal.OverrideColor = config.VanityLegal.Color;
        }

        if(defense != null)
        {
            if(!config.Defense.On) lines.Remove(defense);
            else if(config.Defense.Color != Color.White) defense.OverrideColor = config.Defense.Color;
        }

        if(pickPower != null)
        {
            if(!config.PickPower.On) lines.Remove(pickPower);
            else if(config.PickPower.Color != Color.White) pickPower.OverrideColor = config.PickPower.Color;
        }

        if(axePower != null)
        {
            if(!config.AxePower.On) lines.Remove(axePower);
            else if(config.AxePower.Color != Color.White) axePower.OverrideColor = config.AxePower.Color;
        }

        if(hammerPower != null)
        {
            if(!config.HammerPower.On) lines.Remove(hammerPower);
            else if(config.HammerPower.Color != Color.White) hammerPower.OverrideColor = config.HammerPower.Color;
        }

        if(tileBoost != null)
        {
            if(!config.TileBoost.On) lines.Remove(tileBoost);
            else if(config.TileBoost.Color != Color.White) tileBoost.OverrideColor = config.TileBoost.Color;
        }

        if(healLife != null)
        {
            if(!config.HealLife.On) lines.Remove(healLife);
            else if(config.HealLife.Color != Color.White) healLife.OverrideColor = config.HealLife.Color;
        }

        if(healMana != null)
        {
            if(!config.HealMana.On) lines.Remove(healMana);
            else if(config.HealMana.Color != Color.White) healMana.OverrideColor = config.HealMana.Color;
        }

        if(useMana != null)
        {
            if(!config.UseMana.On) lines.Remove(useMana);
            else if(config.UseMana.Color != Color.White) useMana.OverrideColor = config.UseMana.Color;
        }

        if(placeable != null)
        {
            if(!config.Placeable.On) lines.Remove(placeable);
            else if(config.Placeable.Color != Color.White) placeable.OverrideColor = config.Placeable.Color;
        }

        if(ammo != null)
        {
            if(!config.Ammo.On) lines.Remove(ammo);
            else if(config.Ammo.Color != Color.White) ammo.OverrideColor = config.Ammo.Color;
        }

        if(consumable != null)
        {
            if(!config.Consumable.On) lines.Remove(consumable);
            else if(config.Consumable.Color != Color.White) consumable.OverrideColor = config.Consumable.Color;
        }

        if(material != null)
        {
            if(!config.Material.On) lines.Remove(material);
            else if(config.Material.Color != Color.White) material.OverrideColor = config.Material.Color;
        }

        if(etherianManaWarning != null)
        {
            if(!config.EtherianManaWarning.On) lines.Remove(etherianManaWarning);
            else if(config.EtherianManaWarning.Color != Color.White) etherianManaWarning.OverrideColor = config.EtherianManaWarning.Color;
        }

        if(wellFedExpert != null)
        {
            if(!config.WellFedExpert.On) lines.Remove(wellFedExpert);
            else if(config.WellFedExpert.Color != Color.White) wellFedExpert.OverrideColor = config.WellFedExpert.Color;
        }

        if(buffTime != null)
        {
            if(!config.BuffTime.On) lines.Remove(buffTime);
            else if(config.BuffTime.Color != Color.White) buffTime.OverrideColor = config.BuffTime.Color;
        }

        if(oneDropLogo != null)
        {
            if(!config.OneDropLogo.On) lines.Remove(oneDropLogo);
            else if(config.OneDropLogo.Color != Color.White) oneDropLogo.OverrideColor = config.OneDropLogo.Color;
        }

        if(item.prefix > 0)
        {
            if(!config.GoodModifier.On) lines.RemoveAll(l => l.IsModifier && !l.IsModifierBad);
            if(!config.BadModifier.On) lines.RemoveAll(l => l.IsModifierBad);

            foreach(TooltipLine line in lines)
            {
                if(line.IsModifier && !line.IsModifierBad && config.GoodModifier.Color != new Color(120, 190, 120)) line.OverrideColor = config.GoodModifier.Color;
                if(line.IsModifierBad && config.BadModifier.Color != new Color(190, 120, 120)) line.OverrideColor = config.BadModifier.Color;
            }
        }

        if(setBonus != null)
        {
            if(!config.SetBonus.On) lines.Remove(setBonus);
            else if(config.SetBonus.Color != Color.White) setBonus.OverrideColor = config.SetBonus.Color;
        }

        if(expert != null)
        {
            if(!config.Expert.On) lines.Remove(expert);
            else if(config.Expert.Color != Color.White) expert.OverrideColor = config.Expert.Color;
        }

        if(master != null)
        {
            if(!config.Master.On) lines.Remove(master);
            else if(config.Master.Color != Color.White) master.OverrideColor = config.Master.Color;
        }

        if(journeyResearch != null)
        {
            if(!config.JourneyResearch.On) lines.Remove(journeyResearch);
            else if(config.JourneyResearch.Color != new Color(255, 120, 187)) journeyResearch.OverrideColor = config.JourneyResearch.Color;
        }

        if(modifiedByMods != null)
        {
            if(!config.ModifiedByMods.On) lines.Remove(modifiedByMods);
            else if(config.ModifiedByMods.Color != Color.White) modifiedByMods.OverrideColor = config.ModifiedByMods.Color;
        }

        if(bestiaryNotes != null)
        {
            if(!config.BestiaryNotes.On) lines.Remove(bestiaryNotes);
            else if(config.BestiaryNotes.Color != Color.White) bestiaryNotes.OverrideColor = config.BestiaryNotes.Color;
        }

        if(itemAmmo != null && config.ItemAmmo) lines.Add(new TooltipLine(Mod, "ItemAmmo", itemAmmo.HoverName + ((keyState.PressingShift() || config.ItemModAlways) && itemAmmo.ModItem != null && config.ItemMod ? " - " + itemAmmo.ModItem.Mod.DisplayName : string.Empty)) { OverrideColor = RarityColor(itemAmmo) });

        if(item.shopSpecialCurrency == -1 && (item.type < 71 || item.type > 74) && config.BetterPrice)
        {
            player.GetItemExpectedPrice(item, out int calcForSelling, out int calcForBuying);

            if((value = item.isAShopItem || item.buyOnce ? calcForBuying : calcForSelling) > 0)
            {
                int value2 = value * item.stack;

                if(!item.buy)
                {
                    int amount = shopSellbackHelper.GetAmount(item);

                    if((value2 = value / 5) < 1) value2 = 1;

                    int temp = value2;

                    value2 *= item.stack;

                    if(amount > 0) value2 += (calcForBuying - temp) * Math.Min(amount, item.stack);
                }

                int platinum = value2 / 1_000_000,
                    gold = value2 / 10000 % 100,
                    silver = value2 / 100 % 100,
                    copper = value2 % 100;

                lines.Add(new TooltipLine(Mod, "BetterPrice", ((platinum > 0 ? $"[c/{TextPulse(CoinPlatinum).Hex3()}:{platinum} {Lang.inter[15].Value} ]" : string.Empty) + (gold > 0 ? $"[c/{TextPulse(CoinGold).Hex3()}:{gold} {Lang.inter[16].Value} ]" : string.Empty) + (silver > 0 ? $"[c/{TextPulse(CoinSilver).Hex3()}:{silver} {Lang.inter[17].Value} ]" : string.Empty) + (copper > 0 ? $"[c/{TextPulse(CoinCopper).Hex3()}:{copper} {Lang.inter[18].Value}]" : string.Empty)).TrimEnd()));
            }
        }

        if(specialPrice != null && !config.SpecialPrice) lines.Remove(specialPrice);
        if(price != null && (value > 0 || !config.Price)) lines.Remove(price);
    }

    public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int _x, ref int _y)
    {
        Config config = GetInstance<Config>();
        Microsoft.Xna.Framework.Graphics.Texture2D sprite = TextureAssets.Item[item.type].Value;
        Rectangle spriteRect = itemAnimations[item.type]?.GetFrame(sprite) ?? sprite.Frame();

        int x = mouseX + config.XOffset + 20,
            y = mouseY + config.YOffset + 20,
            width = 0,
            height = 0,
            bgRight = 0,
            bgLeft = 0,
            bgBottom = 0,
            bgTop = 0,
            offsetFromSprite = 0,
            max = new[] { spriteRect.Width, spriteRect.Height, 64 }.Max();

        static Vector2 StringSize(string text) => ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One);

        _x += config.XOffset;
        _y += config.YOffset;

        if((keyState.PressingShift() || config.SpriteAlways) && config.Sprite)
        {
            offsetFromSprite = Math.Max(spriteRect.Width, spriteRect.Height) + 14;

            _x += offsetFromSprite;
        }

        if(config.Background.A > 0)
        {
            x += 8;
            y += 2;

            _x += 8;
            _y += 2;

            bgRight = 14 + (config.BGXOffset + config.BGPaddingRight >= -13 ? config.BGXOffset + config.BGPaddingRight : 0);
            bgLeft = 14 + (config.BGXOffset <= 13 ? config.BGXOffset : 0);
            bgBottom = 4 + (config.BGYOffset + config.BGPaddingBottom >= -3 ? config.BGYOffset + config.BGPaddingBottom : 0);
            bgTop = 9 + (config.BGYOffset <= 8 ? config.BGYOffset : 0);
        }

        if(ThickMouse)
        {
            x += 6;
            y += 6;
        }

        if(!mouseItem.IsAir) x += 34;

        foreach(TooltipLine line in lines)
        {
            Vector2 stringSize = StringSize(line.Text);

            if(stringSize.X + offsetFromSprite > width) width = (int)stringSize.X + offsetFromSprite;

            height += (int)stringSize.Y + config.Spacing;
        }

        if(x + width + bgRight > screenWidth) x = _x = screenWidth - width - bgRight;
        if(x - bgLeft < 0) x = _x = bgLeft;
        if(y + height + bgBottom > screenHeight) y = _y = screenHeight - height - bgBottom;
        if(y - bgTop < 0) y = _y = bgTop;

        if(config.Background.A > 0) Utils.DrawInvBG(spriteBatch, new Rectangle(x + config.BGXOffset - 14, y + config.BGYOffset - 9, width + config.BGPaddingRight + 28, height + config.BGPaddingBottom + 13), config.Background);
        if(offsetFromSprite > 0) spriteBatch.Draw(sprite, new Vector2(x + (max - spriteRect.Width) / 2, y + (max - spriteRect.Height) / 2), spriteRect, Color.White);

        height = 0;

        foreach(TooltipLine line in lines)
        {
            if(line.Name == "OneDropLogo" && line.Mod == "Terraria")
            {
                spriteBatch.Draw(TextureAssets.OneDropLogo.Value, new Vector2(x + offsetFromSprite, y + height), TextPulse(RarityColor(item, line)));

                height += 24;
            }
            else ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(x + offsetFromSprite, y + height), TextPulse(RarityColor(item, line)), 0, Vector2.Zero, Vector2.One, -1, 2);

            height += (int)StringSize(line.Text).Y + config.Spacing;
        }

        return SettingsEnabled_OpaqueBoxBehindTooltips = false;
    }
}