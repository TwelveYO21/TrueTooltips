namespace TrueTooltips;

internal class TTModPlayer : ModPlayer
{
    public override void OnEnterWorld(Player player) => TTGlobalItem.LoadCorners();
}
