namespace TrueTooltips;

internal class TTModPlayer : ModPlayer
{
    public override void OnEnterWorld(Player player) => TTGlobalItem.LoadCorners();

    public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet)
    {
        if(TTMod.ToggleTooltipKey.JustPressed && HoverItem.type != 0)
            TTGlobalItem.ToggleTooltip(HoverItem.type);
    }
}
