namespace TrueTooltips;

internal class TTModPlayer : ModPlayer
{
    public override void OnEnterWorld(Player player)
    {
        List<Color[]> cornerData = new() { new Color[100], new Color[100], new Color[100], new Color[100] };
        Texture2D background = TextureAssets.InventoryBack13.Value;
        var bgColor = new Color[1];
        int[] cornerX = { 0, background.Width - 10, 0, background.Width - 10 };
        int[] cornerY = { 0, 0, background.Height - 10, background.Height - 10 };

        background.GetData(0, new Rectangle(10, 10, 1, 1), bgColor, 0, 1);

        for(int i = 0; i < TTGlobalItem.Corners.Length; i++)
            TTGlobalItem.Corners[i] = new Texture2D(spriteBatch.GraphicsDevice, 10, 10);

        for(int i = 0; i < cornerData.Count; i++)
        {
            background.GetData(0, new Rectangle(cornerX[i], cornerY[i], 10, 10), cornerData[i], 0, 100);

            for(int j = 0; j < cornerData[i].Length; j++)
                cornerData[i][j] = cornerData[i][j].A > 0 ? Color.Transparent : bgColor[0];

            TTGlobalItem.Corners[i]?.SetData(cornerData[i]);
        }
    }
}
