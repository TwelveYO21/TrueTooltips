using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TrueTooltips;

[Label("Config")]
internal class MyModConfig : ModConfig
{
    internal class Line
    {
        [Label("")]
        public bool on = true;

        [Label("")]
        [ColorNoAlpha]
        public Color color = Color.White;
    }

    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Label("$Mods.TrueTooltips.MyModConfig.XOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int xOffset;

    [Label("$Mods.TrueTooltips.MyModConfig.YOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int yOffset;

    [Label("$Mods.TrueTooltips.MyModConfig.Background")]
    [DefaultValue(typeof(Color), "21,23,74,235")]
    public Color background;

    [Label("$Mods.TrueTooltips.MyModConfig.BGPaddingLeft")]
    [Range(-4, int.MaxValue)]
    public int bGPaddingLeft;

    [Label("$Mods.TrueTooltips.MyModConfig.BGPaddingRight")]
    [Range(-4, int.MaxValue)]
    public int bGPaddingRight;

    [Label("$Mods.TrueTooltips.MyModConfig.BGPaddingTop")]
    [Range(-4, int.MaxValue)]
    public int bGPaddingTop;

    [Label("$Mods.TrueTooltips.MyModConfig.BGPaddingBottom")]
    [Range(-4, int.MaxValue)]
    public int bGPaddingBottom;

    [Label("$Mods.TrueTooltips.MyModConfig.TextXOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int textXOffset;

    [Label("$Mods.TrueTooltips.MyModConfig.TextYOffset")]
    [Range(int.MinValue, int.MaxValue)]
    public int textYOffset;

    [Label("$Mods.TrueTooltips.MyModConfig.Sprite")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.SpriteTooltip")]
    [DefaultValue(TrueTooltips.Mode.Always)]
    public Mode sprite;

    [Label("$Mods.TrueTooltips.MyModConfig.SpriteBG")]
    [DefaultValue(typeof(Color), "169,171,222,235")]
    public Color spriteBG;

    [Label("$Mods.TrueTooltips.MyModConfig.Spacing")]
    [Range(int.MinValue, int.MaxValue)]
    public int spacing;

    [Label("$Mods.TrueTooltips.MyModConfig.TextPulse")]
    public bool textPulse;

    [Header("$Mods.TrueTooltips.MyModConfig.Header1")]

    [Label("$Mods.TrueTooltips.MyModConfig.ItemAmmo")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.ItemAmmoTooltip")]
    [DefaultValue(true)]
    public bool itemAmmo;

    [Label("$Mods.TrueTooltips.MyModConfig.ItemMod")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.ItemModTooltip")]
    [DefaultValue(TrueTooltips.Mode.Shift)]
    public Mode itemMod;

    [Label("$Mods.TrueTooltips.MyModConfig.BetterPrice")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.BetterPriceTooltip")]
    [DefaultValue(true)]
    public bool betterPrice;

    [Label("$Mods.TrueTooltips.MyModConfig.BetterSpeed")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.BetterSpeedTooltip")]
    [DefaultValue(true)]
    public bool betterSpeed;

    [Label("$Mods.TrueTooltips.MyModConfig.BetterKnockback")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.BetterKnockbackTooltip")]
    [DefaultValue(true)]
    public bool betterKnockback;

    [Label("$Mods.TrueTooltips.MyModConfig.WpnPlusAmmoDmg")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.WpnPlusAmmoDmgTooltip")]
    [DefaultValue(true)]
    public bool wpnPlusAmmoDmg;

    [Label("$Mods.TrueTooltips.MyModConfig.WpnPlusAmmoKb")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.WpnPlusAmmoKbTooltip")]
    [DefaultValue(true)]
    public bool wpnPlusAmmoKb;

    [Label("$Mods.TrueTooltips.MyModConfig.AmmoDmgKbSeparate")]
    [Tooltip("$Mods.TrueTooltips.MyModConfig.AmmoDmgKbSeparateTooltip")]
    [DefaultValue(TrueTooltips.Mode.Shift)]
    public Mode ammoDmgKbSeparate;

    [Label("$Mods.TrueTooltips.MyModConfig.HideTooltip")]
    [DefaultValue(true)]
    public bool hideTooltip;

    [Header("$Mods.TrueTooltips.MyModConfig.Header2")]

    [Label("$LegacyTooltip.56")]
    public Line favorite = new()
    {
        on = false
    };

    [Label("$LegacyTooltip.57")]
    public Line favoriteDesc = new()
    {
        on = false
    };

    [Label("$UI.ItemCannotBePlacedInsideItself")]
    public Line noTransfer = new()
    {
        on = false
    };

    [Label("$LegacyTooltip.0")]
    public Line social = new()
    {
        on = false
    };

    [Label("$LegacyTooltip.1")]
    public Line socialDesc = new()
    {
        on = false
    };

    [Label("$Mods.TrueTooltips.MyModConfig.Damage")]
    public Line damage = new();

    [Label("$Mods.TrueTooltips.MyModConfig.CritChance")]
    public Line critChance = new();

    [Label("$Mods.TrueTooltips.MyModConfig.Speed")]
    public Line speed = new();

    [Label("$tModLoader.NoAttackSpeedScaling")]
    public Line noSpeedScaling = new();

    [Label("$Mods.TrueTooltips.MyModConfig.SpecialSpeedScaling")]
    public Line specialSpeedScaling = new();

    [Label("$Mods.TrueTooltips.MyModConfig.Knockback")]
    public Line knockback = new();

    [Label("$Mods.TrueTooltips.MyModConfig.FishingPower")]
    public Line fishingPower = new();

    [Label("$GameUI.BaitRequired")]
    public Line needsBait = new();

    [Label("$Mods.TrueTooltips.MyModConfig.BaitPower")]
    public Line baitPower = new();

    [Label("$LegacyTooltip.23")]
    public Line equipable = new();

    [Label("$Mods.TrueTooltips.MyModConfig.WandConsumes")]
    public Line wandConsumes = new();

    [Label("$LegacyInterface.65")]
    public Line quest = new();

    [Label("$LegacyTooltip.24")]
    public Line vanity = new();

    [Label("$Misc.CanBePlacedInVanity")]
    public Line vanityLegal = new();

    [Label("$Mods.TrueTooltips.MyModConfig.Defense")]
    public Line defense = new();

    [Label("$Mods.TrueTooltips.MyModConfig.PickPower")]
    public Line pickPower = new();

    [Label("$Mods.TrueTooltips.MyModConfig.AxePower")]
    public Line axePower = new();

    [Label("$Mods.TrueTooltips.MyModConfig.HammerPower")]
    public Line hammerPower = new();

    [Label("$Mods.TrueTooltips.MyModConfig.TileBoost")]
    public Line tileBoost = new();

    [Label("$Mods.TrueTooltips.MyModConfig.HealLife")]
    public Line healLife = new();

    [Label("$Mods.TrueTooltips.MyModConfig.HealMana")]
    public Line healMana = new();

    [Label("$Mods.TrueTooltips.MyModConfig.UseMana")]
    public Line useMana = new();

    [Label("$LegacyTooltip.33")]
    public Line placeable = new();

    [Label("$LegacyTooltip.34")]
    public Line ammo = new();

    [Label("$LegacyTooltip.35")]
    public Line consumable = new();

    [Label("$LegacyTooltip.36")]
    public Line material = new();

    [Label("$LegacyMisc.104")]
    public Line etherianManaWarning = new();

    [Label("$LegacyMisc.40")]
    public Line wellFedExpert = new();

    [Label("$Mods.TrueTooltips.MyModConfig.BuffTime")]
    public Line buffTime = new();

    [Label("$Mods.TrueTooltips.MyModConfig.OneDropLogo")]
    public Line oneDropLogo = new()
    {
        on = false
    };

    [Label("$Mods.TrueTooltips.MyModConfig.GoodModifier")]
    public Line goodModifier = new()
    {
        color = new Color(120, 190, 120)
    };

    [Label("$Mods.TrueTooltips.MyModConfig.BadModifier")]
    public Line badModifier = new()
    {
        color = new Color(190, 120, 120)
    };

    [Label("$Mods.TrueTooltips.MyModConfig.SetBonus")]
    public Line setBonus = new();

    [Label("$GameUI.Expert")]
    public Line expert = new();

    [Label("$GameUI.Master")]
    public Line master = new();

    [Label("$Mods.TrueTooltips.MyModConfig.JourneyResearch")]
    public Line journeyResearch = new()
    {
        color = new Color(255, 120, 187)
    };

    [Label("$tModLoader.ModifiedByModsHoldSHIFT")]
    public Line modifiedByMods = new();

    [Label("$Mods.TrueTooltips.MyModConfig.BestiaryNotes")]
    public Line bestiaryNotes = new();

    [Label("$Mods.TrueTooltips.MyModConfig.SpecialPrice")]
    [DefaultValue(true)]
    public bool specialPrice;

    [Label("$Mods.TrueTooltips.MyModConfig.Price")]
    [DefaultValue(true)]
    public bool price;
}
