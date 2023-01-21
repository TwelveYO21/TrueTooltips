using System;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria.Localization;
using Terraria.UI.Chat;
using static Terraria.ID.Colors;

namespace TrueTooltips;

internal class MyGlobalItem : GlobalItem
{
    internal static List<int> blackList = new();

    private static Action loadCorners;

    private static Color priceColor;

    private static readonly MyModConfig config = GetInstance<MyModConfig>();

    private static readonly Texture2D[] corners = new Texture2D[4];

    internal static void LoadCorners()
    {
        loadCorners = () =>
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

            loadCorners = () => { };
        };
    }

    internal static void ToggleTooltip(int type)
    {
        if(blackList.Contains(type))
            blackList.Remove(type);
        else
            blackList.Add(type);
    }

    private static Color RarityColor(Item item, TooltipLine line = null) =>
        line switch
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
        if(blackList.Contains(item.type))
            return;

        Item itemAmmo = null;
        Player player = LocalPlayer;
        int value = 0;

        TooltipLine Find(string name) => lines.Find(line => line.Name == name);

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

        static string GetTextValue(string key) => Language.GetTextValue("Mods.TrueTooltips.MyGlobalItem." + key);

        if(item.useAmmo > 0 && (config.itemAmmo || config.wpnPlusAmmoDmg || config.wpnPlusAmmoKb && config.betterKnockback))
            itemAmmo = player.ChooseAmmo(item);

        if((keyState.PressingShift() && config.itemMod == Mode.Shift || config.itemMod == Mode.Always) && item.ModItem != null && itemName != null)
            itemName.Text += " - " + item.ModItem.Mod.DisplayName;

        if(favorite != null)
        {
            if(!config.favorite.on)
                lines.Remove(favorite);
            else if(config.favorite.color != Color.White)
                favorite.OverrideColor = config.favorite.color;
        }
        if(favoriteDesc != null)
        {
            if(!config.favoriteDesc.on)
                lines.Remove(favoriteDesc);
            else if(config.favoriteDesc.color != Color.White)
                favoriteDesc.OverrideColor = config.favoriteDesc.color;
        }
        if(noTransfer != null)
        {
            if(!config.noTransfer.on)
                lines.Remove(noTransfer);
            else if(config.noTransfer.color != Color.White)
                noTransfer.OverrideColor = config.noTransfer.color;
        }
        if(social != null)
        {
            if(!config.social.on)
                lines.Remove(social);
            else if(config.social.color != Color.White)
                social.OverrideColor = config.social.color;
        }
        if(socialDesc != null)
        {
            if(!config.socialDesc.on)
                lines.Remove(socialDesc);
            else if(config.socialDesc.color != Color.White)
                socialDesc.OverrideColor = config.socialDesc.color;
        }
        if(damage != null)
        {
            if(!config.damage.on)
                lines.Remove(damage);
            else
            {
                if(config.damage.color != Color.White)
                    damage.OverrideColor = config.damage.color;
                if(itemAmmo != null && config.wpnPlusAmmoDmg)
                    damage.Text = Regex.Replace(damage.Text, @"^\d+", keyState.PressingShift() && config.ammoDmgKbSeparate == Mode.Shift || config.ammoDmgKbSeparate == Mode.Always ? player.GetWeaponDamage(item, true) + " + " + player.GetWeaponDamage(itemAmmo, true) : (player.GetWeaponDamage(item, true) + player.GetWeaponDamage(itemAmmo, true)).ToString());
            }
        }
        if(critChance != null)
        {
            if(!config.critChance.on)
                lines.Remove(critChance);
            else if(config.critChance.color != Color.White)
                critChance.OverrideColor = config.critChance.color;
        }
        if(speed != null)
        {
            if(!config.speed.on)
                lines.Remove(speed);
            else
            {
                if(config.speed.color != Color.White)
                    speed.OverrideColor = config.speed.color;
                if(config.betterSpeed)
                    speed.Text = Math.Round(60d / item.useAnimation, 2) + GetTextValue("Speed");
            }
        }
        if(noSpeedScaling != null)
        {
            if(!config.noSpeedScaling.on)
                lines.Remove(noSpeedScaling);
            else if(config.noSpeedScaling.color != Color.White)
                noSpeedScaling.OverrideColor = config.noSpeedScaling.color;
        }
        if(specialSpeedScaling != null)
        {
            if(!config.specialSpeedScaling.on)
                lines.Remove(specialSpeedScaling);
            else if(config.specialSpeedScaling.color != Color.White)
                specialSpeedScaling.OverrideColor = config.specialSpeedScaling.color;
        }
        if(knockback != null)
        {
            if(!config.knockback.on)
                lines.Remove(knockback);
            else
            {
                if(config.knockback.color != Color.White)
                    knockback.OverrideColor = config.knockback.color;
                if(config.betterKnockback)
                    knockback.Text = ((keyState.PressingShift() && config.ammoDmgKbSeparate == Mode.Shift || config.ammoDmgKbSeparate == Mode.Always) && itemAmmo != null && config.wpnPlusAmmoKb ? Math.Round(player.GetWeaponKnockback(item, item.knockBack), 2) + " + " + Math.Round(player.GetWeaponKnockback(itemAmmo, itemAmmo.knockBack), 2) : Math.Round(player.GetWeaponKnockback(item, item.knockBack) + (itemAmmo != null && config.wpnPlusAmmoKb ? player.GetWeaponKnockback(itemAmmo, itemAmmo.knockBack) : 0f), 2)) + GetTextValue("Knockback");
            }
        }
        if(fishingPower != null)
        {
            if(!config.fishingPower.on)
                lines.Remove(fishingPower);
            else if(config.fishingPower.color != Color.White)
                fishingPower.OverrideColor = config.fishingPower.color;
        }
        if(needsBait != null)
        {
            if(!config.needsBait.on)
                lines.Remove(needsBait);
            else if(config.needsBait.color != Color.White)
                needsBait.OverrideColor = config.needsBait.color;
        }
        if(baitPower != null)
        {
            if(!config.baitPower.on)
                lines.Remove(baitPower);
            else if(config.baitPower.color != Color.White)
                baitPower.OverrideColor = config.baitPower.color;
        }
        if(equipable != null)
        {
            if(!config.equipable.on)
                lines.Remove(equipable);
            else if(config.equipable.color != Color.White)
                equipable.OverrideColor = config.equipable.color;
        }
        if(wandConsumes != null)
        {
            if(!config.wandConsumes.on)
                lines.Remove(wandConsumes);
            else if(config.wandConsumes.color != Color.White)
                wandConsumes.OverrideColor = config.wandConsumes.color;
        }
        if(quest != null)
        {
            if(!config.quest.on)
                lines.Remove(quest);
            else if(config.quest.color != Color.White)
                quest.OverrideColor = config.quest.color;
        }
        if(vanity != null)
        {
            if(!config.vanity.on)
                lines.Remove(vanity);
            else if(config.vanity.color != Color.White)
                vanity.OverrideColor = config.vanity.color;
        }
        if(vanityLegal != null)
        {
            if(!config.vanityLegal.on)
                lines.Remove(vanityLegal);
            else if(config.vanityLegal.color != Color.White)
                vanityLegal.OverrideColor = config.vanityLegal.color;
        }
        if(defense != null)
        {
            if(!config.defense.on)
                lines.Remove(defense);
            else if(config.defense.color != Color.White)
                defense.OverrideColor = config.defense.color;
        }
        if(pickPower != null)
        {
            if(!config.pickPower.on)
                lines.Remove(pickPower);
            else if(config.pickPower.color != Color.White)
                pickPower.OverrideColor = config.pickPower.color;
        }
        if(axePower != null)
        {
            if(!config.axePower.on)
                lines.Remove(axePower);
            else if(config.axePower.color != Color.White)
                axePower.OverrideColor = config.axePower.color;
        }
        if(hammerPower != null)
        {
            if(!config.hammerPower.on)
                lines.Remove(hammerPower);
            else if(config.hammerPower.color != Color.White)
                hammerPower.OverrideColor = config.hammerPower.color;
        }
        if(tileBoost != null)
        {
            if(!config.tileBoost.on)
                lines.Remove(tileBoost);
            else if(config.tileBoost.color != Color.White)
                tileBoost.OverrideColor = config.tileBoost.color;
        }
        if(healLife != null)
        {
            if(!config.healLife.on)
                lines.Remove(healLife);
            else if(config.healLife.color != Color.White)
                healLife.OverrideColor = config.healLife.color;
        }
        if(healMana != null)
        {
            if(!config.healMana.on)
                lines.Remove(healMana);
            else if(config.healMana.color != Color.White)
                healMana.OverrideColor = config.healMana.color;
        }
        if(useMana != null)
        {
            if(!config.useMana.on)
                lines.Remove(useMana);
            else if(config.useMana.color != Color.White)
                useMana.OverrideColor = config.useMana.color;
        }
        if(placeable != null)
        {
            if(!config.placeable.on)
                lines.Remove(placeable);
            else if(config.placeable.color != Color.White)
                placeable.OverrideColor = config.placeable.color;
        }
        if(ammo != null)
        {
            if(!config.ammo.on)
                lines.Remove(ammo);
            else if(config.ammo.color != Color.White)
                ammo.OverrideColor = config.ammo.color;
        }
        if(consumable != null)
        {
            if(!config.consumable.on)
                lines.Remove(consumable);
            else if(config.consumable.color != Color.White)
                consumable.OverrideColor = config.consumable.color;
        }
        if(material != null)
        {
            if(!config.material.on)
                lines.Remove(material);
            else if(config.material.color != Color.White)
                material.OverrideColor = config.material.color;
        }
        if(etherianManaWarning != null)
        {
            if(!config.etherianManaWarning.on)
                lines.Remove(etherianManaWarning);
            else if(config.etherianManaWarning.color != Color.White)
                etherianManaWarning.OverrideColor = config.etherianManaWarning.color;
        }
        if(wellFedExpert != null)
        {
            if(!config.wellFedExpert.on)
                lines.Remove(wellFedExpert);
            else if(config.wellFedExpert.color != Color.White)
                wellFedExpert.OverrideColor = config.wellFedExpert.color;
        }
        if(buffTime != null)
        {
            if(!config.buffTime.on)
                lines.Remove(buffTime);
            else if(config.buffTime.color != Color.White)
                buffTime.OverrideColor = config.buffTime.color;
        }
        if(oneDropLogo != null)
        {
            if(!config.oneDropLogo.on)
                lines.Remove(oneDropLogo);
            else if(config.oneDropLogo.color != Color.White)
                oneDropLogo.OverrideColor = config.oneDropLogo.color;
        }
        if(item.prefix > 0)
        {
            if(!config.goodModifier.on)
                lines.RemoveAll(line => line.IsModifier && !line.IsModifierBad);
            if(!config.badModifier.on)
                lines.RemoveAll(line => line.IsModifierBad);
            foreach(TooltipLine line in lines)
            {
                if(line.IsModifier && !line.IsModifierBad && config.goodModifier.color != new Color(120, 190, 120))
                    line.OverrideColor = config.goodModifier.color;
                if(line.IsModifierBad && config.badModifier.color != new Color(190, 120, 120))
                    line.OverrideColor = config.badModifier.color;
            }
        }
        if(setBonus != null)
        {
            if(!config.setBonus.on)
                lines.Remove(setBonus);
            else if(config.setBonus.color != Color.White)
                setBonus.OverrideColor = config.setBonus.color;
        }
        if(expert != null)
        {
            if(!config.expert.on)
                lines.Remove(expert);
            else if(config.expert.color != Color.White)
                expert.OverrideColor = config.expert.color;
        }
        if(master != null)
        {
            if(!config.master.on)
                lines.Remove(master);
            else if(config.master.color != Color.White)
                master.OverrideColor = config.master.color;
        }
        if(journeyResearch != null)
        {
            if(!config.journeyResearch.on)
                lines.Remove(journeyResearch);
            else if(config.journeyResearch.color != new Color(255, 120, 187))
                journeyResearch.OverrideColor = config.journeyResearch.color;
        }
        if(modifiedByMods != null)
        {
            if(!config.modifiedByMods.on)
                lines.Remove(modifiedByMods);
            else if(config.modifiedByMods.color != Color.White)
                modifiedByMods.OverrideColor = config.modifiedByMods.color;
        }
        if(bestiaryNotes != null)
        {
            if(!config.bestiaryNotes.on)
                lines.Remove(bestiaryNotes);
            else if(config.bestiaryNotes.color != Color.White)
                bestiaryNotes.OverrideColor = config.bestiaryNotes.color;
        }

        if(item.useAmmo > 0 && config.itemAmmo)
            lines.Add(new TooltipLine(Mod, "ItemAmmo", itemAmmo != null ? itemAmmo.HoverName + ((keyState.PressingShift() && config.itemMod == Mode.Shift || config.itemMod == Mode.Always) && itemAmmo.ModItem != null ? " - " + itemAmmo.ModItem.Mod.DisplayName : string.Empty) : GetTextValue("No") + item.useAmmo switch { 40 => GetTextValue("Arrow"), 71 => GetTextValue("Coin"), 97 => GetTextValue("Bullet"), 283 => GetTextValue("Dart"), 771 => GetTextValue("Rocket"), 780 => GetTextValue("Solution"), _ => Lang.GetItemNameValue(item.useAmmo) }) { OverrideColor = itemAmmo != null ? RarityColor(itemAmmo) : new Color(120, 120, 120) });

        if(item.shopSpecialCurrency == -1 && item.type is < 71 or > 74 && config.price)
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

                if(config.betterPrice)
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

        if(config.hideTooltip)
            lines.Add(new TooltipLine(Mod, "HideTooltip", Language.GetTextValue("Mods.TrueTooltips.MyGlobalItem.HideTooltip", MyMod.ToggleTooltipKey.GetAssignedKeys().Count > 0 ? string.Join(',', MyMod.ToggleTooltipKey.GetAssignedKeys()) : Lang.menu[195].Value)));

        if(specialPrice != null && !config.specialPrice)
            lines.Remove(specialPrice);

        if(price != null && (value > 0 && config.betterPrice || !config.price))
            lines.Remove(price);
    }

    public override bool PreDrawTooltip(Item item, System.Collections.ObjectModel.ReadOnlyCollection<TooltipLine> lines, ref int effectX, ref int effectY)
    {
        if(blackList.Contains(item.type))
            return false;

        Texture2D sprite = TextureAssets.Item[item.type].Value;
        Rectangle spriteFrame = itemAnimations[item.type]?.GetFrame(sprite) ?? sprite.Frame();
        bool showSprite = (config.sprite == Mode.Always || keyState.PressingShift() && config.sprite == Mode.Shift) && !(item.value == -1 && item.type == 1);
        int x = mouseX + config.xOffset + 20;
        int y = mouseY + config.yOffset + 20;
        int tooltipWidth = 0;
        int tooltipHeight = config.textYOffset - config.spacing;
        int bgLeft = 0;
        int bgRight = 0;
        int bgTop = 0;
        int bgBottom = 0;
        int spriteMax = new[] { spriteFrame.Width + 28, spriteFrame.Height + 28, 64 }.Max();
        int xOffsetFromSprite = showSprite ? spriteMax + 14 : 0;
        int textLeft = Math.Max(-config.textXOffset - xOffsetFromSprite, 0);
        int textTop = Math.Max(-config.textYOffset, 0);

        static Vector2 GetStringSize(string text) => ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One);

        effectX += config.xOffset + config.textXOffset;
        effectY += config.yOffset + config.textYOffset;

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
                tooltipHeight += 24 + config.spacing;
            else
                tooltipHeight += (int)stringSize.Y + config.spacing;
        }

        tooltipWidth += Math.Max(config.textXOffset, -tooltipWidth - 14);

        if(showSprite)
            tooltipHeight = Math.Max(tooltipHeight, spriteMax);

        if(config.background.A > 0)
        {
            x += 4;
            y += 4;
            effectX += 4;
            effectY += 4;

            bgLeft = config.bGPaddingLeft + 14;
            bgRight = config.bGPaddingRight + 14;
            bgTop = config.bGPaddingTop + 14;
            bgBottom = config.bGPaddingBottom + 14;
        }

        if(x < textLeft + bgLeft)
            x = effectX = textLeft + bgLeft;
        if(x > screenWidth - bgRight - tooltipWidth - xOffsetFromSprite)
            x = effectX = screenWidth - bgRight - tooltipWidth - xOffsetFromSprite;
        if(y < textTop + bgTop)
            y = effectY = textTop + bgTop;
        if(y > screenHeight - bgBottom - tooltipHeight)
            y = effectY = screenHeight - bgBottom - tooltipHeight;

        if(config.background.A > 0)
        {
            int bgX = x - textLeft - config.bGPaddingLeft - 4;
            int bgY = y - textTop - config.bGPaddingTop - 4;
            int bgWidth = config.bGPaddingLeft + textLeft + xOffsetFromSprite + tooltipWidth + config.bGPaddingRight + 8;
            int bgHeight = config.bGPaddingTop + textTop + tooltipHeight + config.bGPaddingBottom + 8;

            if(config.spriteBG.A > 0 && showSprite)
            {
                Texture2D background = TextureAssets.InventoryBack13.Value;

                spriteBatch.Draw(background, new Vector2(bgX - 10, bgY - 10), new Rectangle(0, 0, 10, 10), config.background);
                spriteBatch.Draw(background, new Vector2(bgX + bgWidth, bgY - 10), new Rectangle(background.Width - 10, 0, 10, 10), config.background);
                spriteBatch.Draw(background, new Vector2(bgX - 10, bgY + bgHeight), new Rectangle(0, background.Height - 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Vector2(bgX + bgWidth, bgY + bgHeight), new Rectangle(background.Width - 10, background.Height - 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY - 10, bgWidth, 10), new Rectangle(10, 0, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(bgX - 10, bgY, 10, bgHeight), new Rectangle(0, 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(bgX + bgWidth, bgY, 10, bgHeight), new Rectangle(background.Width - 10, 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY + bgHeight, bgWidth, 10), new Rectangle(10, background.Height - 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(bgX, bgY, bgWidth, config.bGPaddingTop + textTop + 4), new Rectangle(10, 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(bgX, y, config.bGPaddingLeft + textLeft + 4, spriteMax), new Rectangle(10, 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(x + spriteMax, y, tooltipWidth + config.bGPaddingRight + 18, spriteMax), new Rectangle(10, 10, 10, 10), config.background);
                spriteBatch.Draw(background, new Rectangle(bgX, y + spriteMax, bgWidth, tooltipHeight + config.bGPaddingBottom - spriteMax + 4), new Rectangle(10, 10, 10, 10), config.background);

                loadCorners?.Invoke();

                if(corners[0] != null)
                    spriteBatch.Draw(corners[0], new Vector2(x, y), config.background);
                if(corners[1] != null)
                    spriteBatch.Draw(corners[1], new Vector2(x + spriteMax - 10, y), config.background);
                if(corners[2] != null)
                    spriteBatch.Draw(corners[2], new Vector2(x, y + spriteMax - 10), config.background);
                if(corners[3] != null)
                    spriteBatch.Draw(corners[3], new Vector2(x + spriteMax - 10, y + spriteMax - 10), config.background);
            }
            else
                Utils.DrawInvBG(spriteBatch, bgX - 10, bgY - 10, bgWidth + 20, bgHeight + 20, config.background);
        }

        if(config.spriteBG.A > 0 && showSprite)
            Utils.DrawInvBG(spriteBatch, x, y, spriteMax, spriteMax, config.spriteBG);

        if(showSprite)
        {
            spriteBatch.Draw(sprite, new Vector2(x, y) + new Vector2(spriteMax, spriteMax) / 2f, spriteFrame, Color.White, 0f, spriteFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);

            effectX += xOffsetFromSprite;
        }

        tooltipHeight = config.textYOffset;

        foreach(TooltipLine line in lines)
        {
            if(line.Name == "OneDropLogo" && line.Mod == "Terraria")
            {
                spriteBatch.Draw(TextureAssets.OneDropLogo.Value, new Vector2(x + xOffsetFromSprite + config.textXOffset, y + tooltipHeight), TextPulse(RarityColor(item, line)));

                tooltipHeight += config.spacing + 24;
            }
            else
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(x + xOffsetFromSprite + config.textXOffset, y + tooltipHeight), TextPulse(RarityColor(item, line)), 0f, Vector2.Zero, Vector2.One, -1f, 2f);

                tooltipHeight += (int)GetStringSize(line.Text).Y + config.spacing;
            }
        }

        return false;
    }
}
