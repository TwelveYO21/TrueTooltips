global using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TrueTooltips;

internal class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public class Line
    {
        [Label("")]
        public bool On = true;

        [Label("")]
        [ColorNoAlpha]
        public Color Color = Color.White;
    }

    public enum State
    {
        Always,
        Shift,
        Off
    }

    [Label("$Mods.TrueTooltips.Config.XOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int XOffset;

    [Label("$Mods.TrueTooltips.Config.YOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int YOffset;

    [Label("$Mods.TrueTooltips.Config.Background")]
    [DefaultValue(typeof(Color), "21,23,74,235")]
    public Color Background;

    [Label("$Mods.TrueTooltips.Config.BGXOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int BGXOffset;

    [Label("$Mods.TrueTooltips.Config.BGYOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int BGYOffset;

    [Label("$Mods.TrueTooltips.Config.BGPaddingRight")]
    [Range(int.MinValue, int.MaxValue)]
    public int BGPaddingRight;

    [Label("$Mods.TrueTooltips.Config.BGPaddingBottom")]
    [Range(int.MinValue, int.MaxValue)]
    public int BGPaddingBottom;

    [Label("$Mods.TrueTooltips.Config.TextXOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int TextXOffset;

    [Label("$Mods.TrueTooltips.Config.TextYOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int TextYOffset;

    [Label("$Mods.TrueTooltips.Config.Spacing")]
    [Range(int.MinValue, int.MaxValue)]
    public int Spacing;

    [Label("$Mods.TrueTooltips.Config.Sprite")]
    [Tooltip("$Mods.TrueTooltips.Config.SpriteTooltip")]
    [DefaultValue(State.Always)]
    public State Sprite;

    [Label("$Mods.TrueTooltips.Config.SpriteBG")]
    [DefaultValue(typeof(Color), "169,171,222,235")]
    public Color SpriteBG;

    [Label("$Mods.TrueTooltips.Config.TextPulse")]
    public bool TextPulse;

    // [Label("$Mods.TrueTooltips.Config.VanillaOrder")]
    // public bool VanillaOrder;

    [Header("$Mods.TrueTooltips.Config.Header1")]

    [Label("$Mods.TrueTooltips.Config.ItemAmmo")]
    [Tooltip("$Mods.TrueTooltips.Config.ItemAmmoTooltip")]
    [DefaultValue(true)]
    public bool ItemAmmo;

    [Label("$Mods.TrueTooltips.Config.ItemMod")]
    [Tooltip("$Mods.TrueTooltips.Config.ItemModTooltip")]
    [DefaultValue(State.Shift)]
    public State ItemMod;

    [Label("$Mods.TrueTooltips.Config.BetterPrice")]
    [Tooltip("$Mods.TrueTooltips.Config.BetterPriceTooltip")]
    [DefaultValue(true)]
    public bool BetterPrice;

    [Label("$Mods.TrueTooltips.Config.BetterSpeed")]
    [Tooltip("$Mods.TrueTooltips.Config.BetterSpeedTooltip")]
    [DefaultValue(true)]
    public bool BetterSpeed;

    [Label("$Mods.TrueTooltips.Config.BetterKnockback")]
    [Tooltip("$Mods.TrueTooltips.Config.BetterKnockbackTooltip")]
    [DefaultValue(true)]
    public bool BetterKnockback;

    [Label("$Mods.TrueTooltips.Config.WpnPlusAmmoDmg")]
    [Tooltip("$Mods.TrueTooltips.Config.WpnPlusAmmoDmgTooltip")]
    [DefaultValue(true)]
    public bool WpnPlusAmmoDmg;

    [Label("$Mods.TrueTooltips.Config.WpnPlusAmmoKb")]
    [Tooltip("$Mods.TrueTooltips.Config.WpnPlusAmmoKbTooltip")]
    [DefaultValue(true)]
    public bool WpnPlusAmmoKb;

    [Label("$Mods.TrueTooltips.Config.AmmoDmgKbSeparate")]
    [Tooltip("$Mods.TrueTooltips.Config.AmmoDmgKbSeparateTooltip")]
    [DefaultValue(State.Shift)]
    public State AmmoDmgKbSeparate;

    [Header("$Mods.TrueTooltips.Config.Header2")]

    [Label("$LegacyTooltip.56")]
    public Line Favorite = new()
    {
        On = false
    };

    [Label("$LegacyTooltip.57")]
    public Line FavoriteDesc = new()
    {
        On = false
    };

    [Label("$UI.ItemCannotBePlacedInsideItself")]
    public Line NoTransfer = new()
    {
        On = false
    };

    [Label("$LegacyTooltip.0")]
    public Line Social = new()
    {
        On = false
    };

    [Label("$LegacyTooltip.1")]
    public Line SocialDesc = new()
    {
        On = false
    };

    [Label("$Mods.TrueTooltips.Config.Damage")]
    public Line Damage = new();

    [Label("$Mods.TrueTooltips.Config.CritChance")]
    public Line CritChance = new();

    [Label("$Mods.TrueTooltips.Config.Speed")]
    public Line Speed = new();

    [Label("$tModLoader.NoAttackSpeedScaling")]
    public Line NoSpeedScaling = new();

    [Label("$Mods.TrueTooltips.Config.SpecialSpeedScaling")]
    public Line SpecialSpeedScaling = new();

    [Label("$Mods.TrueTooltips.Config.Knockback")]
    public Line Knockback = new();

    [Label("$Mods.TrueTooltips.Config.FishingPower")]
    public Line FishingPower = new();

    [Label("$GameUI.BaitRequired")]
    public Line NeedsBait = new();

    [Label("$Mods.TrueTooltips.Config.BaitPower")]
    public Line BaitPower = new();

    [Label("$LegacyTooltip.23")]
    public Line Equipable = new();

    [Label("$Mods.TrueTooltips.Config.WandConsumes")]
    public Line WandConsumes = new();

    [Label("$LegacyInterface.65")]
    public Line Quest = new();

    [Label("$LegacyTooltip.24")]
    public Line Vanity = new();

    [Label("$Misc.CanBePlacedInVanity")]
    public Line VanityLegal = new();

    [Label("$Mods.TrueTooltips.Config.Defense")]
    public Line Defense = new();

    [Label("$Mods.TrueTooltips.Config.PickPower")]
    public Line PickPower = new();

    [Label("$Mods.TrueTooltips.Config.AxePower")]
    public Line AxePower = new();

    [Label("$Mods.TrueTooltips.Config.HammerPower")]
    public Line HammerPower = new();

    [Label("$Mods.TrueTooltips.Config.TileBoost")]
    public Line TileBoost = new();

    [Label("$Mods.TrueTooltips.Config.HealLife")]
    public Line HealLife = new();

    [Label("$Mods.TrueTooltips.Config.HealMana")]
    public Line HealMana = new();

    [Label("$Mods.TrueTooltips.Config.UseMana")]
    public Line UseMana = new();

    [Label("$LegacyTooltip.33")]
    public Line Placeable = new();

    [Label("$LegacyTooltip.34")]
    public Line Ammo = new();

    [Label("$LegacyTooltip.35")]
    public Line Consumable = new();

    [Label("$LegacyTooltip.36")]
    public Line Material = new();

    [Label("$LegacyMisc.104")]
    public Line EtherianManaWarning = new();

    [Label("$LegacyMisc.40")]
    public Line WellFedExpert = new();

    [Label("$Mods.TrueTooltips.Config.BuffTime")]
    public Line BuffTime = new();

    [Label("$Mods.TrueTooltips.Config.OneDropLogo")]
    public Line OneDropLogo = new()
    {
        On = false
    };

    [Label("$Mods.TrueTooltips.Config.GoodModifier")]
    public Line GoodModifier = new()
    {
        Color = new Color(120, 190, 120)
    };

    [Label("$Mods.TrueTooltips.Config.BadModifier")]
    public Line BadModifier = new()
    {
        Color = new Color(190, 120, 120)
    };

    [Label("$Mods.TrueTooltips.Config.SetBonus")]
    public Line SetBonus = new();

    [Label("$GameUI.Expert")]
    public Line Expert = new();

    [Label("$GameUI.Master")]
    public Line Master = new();

    [Label("$Mods.TrueTooltips.Config.JourneyResearch")]
    public Line JourneyResearch = new()
    {
        Color = new Color(255, 120, 187)
    };

    [Label("$tModLoader.ModifiedByModsHoldSHIFT")]
    public Line ModifiedByMods = new();

    [Label("$Mods.TrueTooltips.Config.BestiaryNotes")]
    public Line BestiaryNotes = new();

    [Label("$Mods.TrueTooltips.Config.SpecialPrice")]
    [DefaultValue(true)]
    public bool SpecialPrice;

    [Label("$Mods.TrueTooltips.Config.Price")]
    [DefaultValue(true)]
    public bool Price;
}
