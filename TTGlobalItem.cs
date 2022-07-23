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
    private static Color priceColor;
    private static readonly Texture2D[] corners = new Texture2D[4];

    internal static void LoadCorners()
    {
        List<Color[]> cornerData = new() { new Color[100], new Color[100], new Color[100], new Color[100] };
        Texture2D background = TextureAssets.InventoryBack13.Value;
        var bgColor = new Color[1];
        int[] cornerX = { 0, background.Width - 10, 0, background.Width - 10 };
        int[] cornerY = { 0, 0, background.Height - 10, background.Height - 10 };

        background.GetData(0, new Rectangle(10, 10, 1, 1), bgColor, 0, 1);
        for(int i = 0; i < corners.Length; i++)
            corners[i] = new Texture2D(spriteBatch.GraphicsDevice, 10, 10);
        for(int i = 0; i < cornerData.Count; i++)
        {
            background.GetData(0, new Rectangle(cornerX[i], cornerY[i], 10, 10), cornerData[i], 0, 100);
            for(int j = 0; j < cornerData[i].Length; j++)
                cornerData[i][j] = cornerData[i][j].A > 0 ? Color.Transparent : bgColor[0];
            corners[i]?.SetData(cornerData[i]);
        }
    }

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
        Item itemAmmo = null;
        Player player = LocalPlayer;
        int value = 0;

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

        static string GetTextValue(string key) => Language.GetTextValue("Mods.TrueTooltips.TTGlobalItem." + key);

        if(item.useAmmo > 0 && (config.ItemAmmo || config.WpnPlusAmmoDmg || config.WpnPlusAmmoKb && config.BetterKnockback))
            itemAmmo = player.ChooseAmmo(item);
        if((keyState.PressingShift() && config.ItemMod == Mode.Shift || config.ItemMod == Mode.Always) && item.ModItem != null && itemName != null)
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
                    damage.Text = Regex.Replace(damage.Text, @"^\d+", keyState.PressingShift() && config.AmmoDmgKbSeparate == Mode.Shift || config.AmmoDmgKbSeparate == Mode.Always ? player.GetWeaponDamage(item, true) + " + " + player.GetWeaponDamage(itemAmmo, true) : (player.GetWeaponDamage(item, true) + player.GetWeaponDamage(itemAmmo, true)).ToString());
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
                    speed.Text = Math.Round(60d / item.useAnimation, 2) + GetTextValue("Speed");
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
                    knockback.Text = ((keyState.PressingShift() && config.AmmoDmgKbSeparate == Mode.Shift || config.AmmoDmgKbSeparate == Mode.Always) && itemAmmo != null && config.WpnPlusAmmoKb ? Math.Round(player.GetWeaponKnockback(item, item.knockBack), 2) + " + " + Math.Round(player.GetWeaponKnockback(itemAmmo, itemAmmo.knockBack), 2) : Math.Round(player.GetWeaponKnockback(item, item.knockBack) + (itemAmmo != null && config.WpnPlusAmmoKb ? player.GetWeaponKnockback(itemAmmo, itemAmmo.knockBack) : 0f), 2)) + GetTextValue("Knockback");
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
            lines.Add(new TooltipLine(Mod, "ItemAmmo", itemAmmo != null ? itemAmmo.HoverName + ((keyState.PressingShift() && config.ItemMod == Mode.Shift || config.ItemMod == Mode.Always) && itemAmmo.ModItem != null ? " - " + itemAmmo.ModItem.Mod.DisplayName : string.Empty) : GetTextValue("No") + item.useAmmo switch { 40 => GetTextValue("Arrow"), 71 => GetTextValue("Coin"), 97 => GetTextValue("Bullet"), 283 => GetTextValue("Dart"), 771 => GetTextValue("Rocket"), 780 => GetTextValue("Solution"), _ => Lang.GetItemNameValue(item.useAmmo) }) { OverrideColor = itemAmmo != null ? RarityColor(itemAmmo) : new Color(120, 120, 120) });
        if(item.shopSpecialCurrency == -1 && item.type is < 71 or > 74 && config.Price)
        {
            player.GetItemExpectedPrice(item, out int calcForSelling, out int calcForBuying);
            value = item.isAShopItem || item.buyOnce ? calcForBuying : calcForSelling;
            if(value > 0)
            {
                int value2 = value * item.stack;

                if(!item.buy)
                {
                    int amount = shopSellbackHelper.GetAmount(item);

                    value2 = value / 5;
                    if(value2 < 1)
                        value2 = 1;

                    int temp = value2;

                    value2 *= item.stack;
                    if(amount > 0)
                        value2 += (calcForBuying - temp) * Math.Min(amount, item.stack);
                }
                if(config.BetterPrice)
                {
                    int platinum = value2 / 1_000_000;
                    int gold = value2 / 10000 % 100;
                    int silver = value2 / 100 % 100;
                    int copper = value2 % 100;

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

    public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int effectX, ref int effectY)
    {
        Config config = GetInstance<Config>();
        Texture2D sprite = TextureAssets.Item[item.type].Value;
        Rectangle spriteFrame = itemAnimations[item.type]?.GetFrame(sprite) ?? sprite.Frame();
        bool showSprite = config.Sprite == Mode.Always || keyState.PressingShift() && config.Sprite == Mode.Shift;
        int x = mouseX + config.XOffset + 20;
        int y = mouseY + config.YOffset + 20;
        int tooltipWidth = 0;
        int tooltipHeight = config.TextYOffset - config.Spacing;
        int bgLeft = 0;
        int bgRight = 0;
        int bgTop = 0;
        int bgBottom = 0;
        int spriteMax = new[] { spriteFrame.Width + 28, spriteFrame.Height + 28, 64 }.Max();
        int xOffsetFromSprite = showSprite ? spriteMax + 14 : 0;
        int textLeft = Math.Max(-config.TextXOffset - xOffsetFromSprite, 0);
        int textTop = Math.Max(-config.TextYOffset, 0);

        static Vector2 GetStringSize(string text) => ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One);

        effectX += config.XOffset + config.TextXOffset;
        effectY += config.YOffset + config.TextYOffset;
        if(ThickMouse)
        {
            x += 6;
            y += 6;
        }
        if(!mouseItem.IsAir)
            x += 34;
        foreach(TooltipLine line in lines)
        {
            Vector2 stringSize = GetStringSize(line.Text);

            if(stringSize.X > tooltipWidth)
                tooltipWidth = (int)stringSize.X;
            if(line.Name == "OneDropLogo" && line.Mod == "Terraria")
                tooltipHeight += 24 + config.Spacing;
            else
                tooltipHeight += (int)stringSize.Y + config.Spacing;
        }
        tooltipWidth += Math.Max(config.TextXOffset, -tooltipWidth - 14);
        if(showSprite)
            tooltipHeight = Math.Max(tooltipHeight, spriteMax);
        if(config.Background.A > 0)
        {
            x += 4;
            y += 4;
            effectX += 4;
            effectY += 4;
            bgLeft = config.BGPaddingLeft + 14;
            bgRight = config.BGPaddingRight + 14;
            bgTop = config.BGPaddingTop + 14;
            bgBottom = config.BGPaddingBottom + 14;
        }
        if(x < textLeft + bgLeft)
            x = effectX = textLeft + bgLeft;
        if(x > screenWidth - bgRight - tooltipWidth - xOffsetFromSprite)
            x = effectX = screenWidth - bgRight - tooltipWidth - xOffsetFromSprite;
        if(y < textTop + bgTop)
            y = effectY = textTop + bgTop;
        if(y > screenHeight - bgBottom - tooltipHeight)
            y = effectY = screenHeight - bgBottom - tooltipHeight;
        if(config.Background.A > 0)
        {
            int bgX = x - textLeft - config.BGPaddingLeft - 4;
            int bgY = y - textTop - config.BGPaddingTop - 4;
            int bgWidth = config.BGPaddingLeft + textLeft + xOffsetFromSprite + tooltipWidth + config.BGPaddingRight + 8;
            int bgHeight = config.BGPaddingTop + textTop + tooltipHeight + config.BGPaddingBottom + 8;

            if(config.SpriteBG.A > 0 && showSprite)
            {
                Texture2D background = TextureAssets.InventoryBack13.Value;

                spriteBatch.Draw(background, new Vector2(bgX - 10, bgY - 10), new Rectangle(0, 0, 10, 10), config.Background);
                spriteBatch.Draw(background, new Vector2(bgX + bgWidth, bgY - 10), new Rectangle(background.Width - 10, 0, 10, 10), config.Background);
                spriteBatch.Draw(background, new Vector2(bgX - 10, bgY + bgHeight), new Rectangle(0, background.Height - 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Vector2(bgX + bgWidth, bgY + bgHeight), new Rectangle(background.Width - 10, background.Height - 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY - 10, bgWidth, 10), new Rectangle(10, 0, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX - 10, bgY, 10, bgHeight), new Rectangle(0, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX + bgWidth, bgY, 10, bgHeight), new Rectangle(background.Width - 10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY + bgHeight, bgWidth, 10), new Rectangle(10, background.Height - 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY, bgWidth, config.BGPaddingTop + textTop + 4), new Rectangle(10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, y, config.BGPaddingLeft + textLeft + 4, spriteMax), new Rectangle(10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(x + spriteMax, y, tooltipWidth + config.BGPaddingRight + 18, spriteMax), new Rectangle(10, 10, 10, 10), config.Background);
                spriteBatch.Draw(background, new Rectangle(bgX, y + spriteMax, bgWidth, tooltipHeight + config.BGPaddingBottom - spriteMax + 4), new Rectangle(10, 10, 10, 10), config.Background);
                if(corners[0] != null)
                    spriteBatch.Draw(corners[0], new Vector2(x, y), config.Background);
                if(corners[1] != null)
                    spriteBatch.Draw(corners[1], new Vector2(x + spriteMax - 10, y), config.Background);
                if(corners[2] != null)
                    spriteBatch.Draw(corners[2], new Vector2(x, y + spriteMax - 10), config.Background);
                if(corners[3] != null)
                    spriteBatch.Draw(corners[3], new Vector2(x + spriteMax - 10, y + spriteMax - 10), config.Background);
            }
            else
                Utils.DrawInvBG(spriteBatch, bgX - 10, bgY - 10, bgWidth + 20, bgHeight + 20, config.Background);
        }
        if(config.SpriteBG.A > 0 && showSprite)
            Utils.DrawInvBG(spriteBatch, x, y, spriteMax, spriteMax, config.SpriteBG);
        if(showSprite)
        {
            spriteBatch.Draw(sprite, new Vector2(x, y) + new Vector2(spriteMax, spriteMax) / 2f, spriteFrame, Color.White, 0f, spriteFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);
            effectX += xOffsetFromSprite;
        }
        tooltipHeight = config.TextYOffset;
        foreach(TooltipLine line in lines)
        {
            if(line.Name == "OneDropLogo" && line.Mod == "Terraria")
            {
                spriteBatch.Draw(TextureAssets.OneDropLogo.Value, new Vector2(x + xOffsetFromSprite + config.TextXOffset, y + tooltipHeight), TextPulse(RarityColor(item, line)));
                tooltipHeight += config.Spacing + 24;
            }
            else
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(x + xOffsetFromSprite + config.TextXOffset, y + tooltipHeight), TextPulse(RarityColor(item, line)), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                tooltipHeight += (int)GetStringSize(line.Text).Y + config.Spacing;
            }
        }
        return false;
    }
}
