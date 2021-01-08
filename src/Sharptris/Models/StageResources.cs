using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;

namespace Sharptris.Models
{
    public class StageResources
    {
        public string Title { get; private set; }
        public int[] DefaultLayer { get; private set; }
        public int[] GhostLayer { get; private set; }
        public int[] GhostLayer1 { get; private set; }
        public Vec Size { get; private set; }
        public Vec TileSize { get; private set; }
        public Vec InitialPos { get; private set; }
        public Bitmap Set { get; private set; }
        public int SetTilesPerRow { get; private set; }
        public Rectangle[] TileRectCache { get; private set; }
        public Bitmap SpriteSheet { get; private set; }
        public Dictionary<string, Rectangle> Frames { get; } = new Dictionary<string, Rectangle>();

        public static StageResources Get()
        {
            var stage = new StageResources();

            stage.Title = $"Sharptris";
            stage.Size = (9, 20);
            stage.DefaultLayer = new int[stage.Size.x * stage.Size.y];
            stage.GhostLayer = new int[stage.Size.x * stage.Size.y];
            stage.GhostLayer1 = new int[stage.Size.x * stage.Size.y];

            var setStream = typeof(StageResources).Assembly.GetManifestResourceStream(typeof(StageResources), $"TileMap.png");
            stage.Set = (Bitmap)Image.FromStream(setStream);
            stage.SetTilesPerRow = 8;
            stage.TileSize = (24, 24);

            var tileRectCache = new List<Rectangle>();
            for(var tileId = 0; tileId<8; tileId++)
            {
                var (tx, ty) = (tileId % stage.SetTilesPerRow, tileId / stage.SetTilesPerRow);
                tileRectCache.Add(new Rectangle(tx * stage.TileSize.x, ty * stage.TileSize.y, stage.TileSize.x, stage.TileSize.y));
            }
            stage.TileRectCache = tileRectCache.ToArray();

            return stage;
        }

        public void SwapGhosts()
        {
            var x = GhostLayer;
            GhostLayer = GhostLayer1;
            GhostLayer1 = x;
        }

        public void ClearGhost()
        {
            for (var i = 0; i < GhostLayer.Length; i++) GhostLayer[i] = 0;
        }

        public void ClearGhost1()
        {
            for (var i = 0; i < GhostLayer1.Length; i++) GhostLayer1[i] = 0;
        }
    }
}
