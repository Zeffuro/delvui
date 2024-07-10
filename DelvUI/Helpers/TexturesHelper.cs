using System;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Utility;
using Lumina.Data.Files;
using Lumina.Excel;
using static Dalamud.Plugin.Services.ITextureProvider;

namespace DelvUI.Helpers
{
    public class TexturesHelper
    {
        private static string ResolvePath(string path) => Plugin.TextureSubstitutionProvider.GetSubstitutedPath(path);

        public static IDalamudTextureWrap? GetTexture<T>(uint rowId, uint stackCount = 0, bool hdIcon = true) where T : ExcelRow
        {
            var sheet = Plugin.DataManager.GetExcelSheet<T>();
            return sheet == null ? null : GetTexture<T>(sheet.GetRow(rowId), stackCount, hdIcon);
        }

        public static IDalamudTextureWrap? GetTexture<T>(dynamic? row, uint stackCount = 0, bool hdIcon = true) where T : ExcelRow
        {
            if (row == null)
            {
                return null;
            }

            var iconId = row.Icon;
            return GetTextureFromIconId(iconId, stackCount, hdIcon);
        }

        public static IDalamudTextureWrap? GetTextureFromIconId(uint iconId, uint stackCount = 0, bool hdIcon = true)
        {
            GameIconLookup lookup = new GameIconLookup(iconId + stackCount, false, hdIcon);
            string path = Plugin.TextureProvider.GetIconPath(lookup);
            string resolvePath = ResolvePath(path);

            if (iconId == 62042)
            {
                IDalamudTextureWrap? wrap = Plugin.TextureProvider.GetFromFile(resolvePath).GetWrapOrDefault();
                var tex = Plugin.DataManager.GameData.GetFileFromDisk<TexFile>(resolvePath);
                return Plugin.TextureProvider.CreateFromRaw(RawImageSpecification.Rgba32(tex.Header.Width, tex.Header.Width), tex.GetRgbaImageData());
            }

            return Plugin.TextureProvider.GetFromGame(path).GetWrapOrDefault();
        }

        public static IDalamudTextureWrap? GetTextureFromPath(string path)
        {
            string resolvePath = ResolvePath(path);
            return Plugin.TextureProvider.GetFromGame(resolvePath).GetWrapOrDefault();
        }
    }
}
