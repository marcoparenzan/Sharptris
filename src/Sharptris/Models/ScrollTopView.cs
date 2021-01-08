using System;
using System.Drawing;

namespace Sharptris.Models
{
    public class ScrollTopView
    {
        private Stage stage;
        private StageResources resources;

        private Vec pos;
        private Rectangle rect;

        public ScrollTopView(Stage stage, StageResources resources)
        {
            this.stage = stage;
            this.resources = resources;
        }

        public Vec Pos { get => pos; private set => pos = value; }

        public void Reset(Rectangle rect)
        {
            this.Pos = resources.InitialPos;
            this.rect = rect;
        }

        public void Update(Stage world)
        {
        }

        public void Render(Graphics g)
        {
            //
            // render world
            //

            // how many rects in the viewport
            var ys = rect.Height / resources.TileSize.y;
            var xs = rect.Width / resources.TileSize.x;

            // the x,y converted to offset in the map
            var yt = (int)(Math.Max(pos.y, 0) / resources.TileSize.y); var ym = pos.y % resources.TileSize.y;
            var xt = (int)(Math.Max(pos.x, 0) / resources.TileSize.x); var xm = pos.x % resources.TileSize.x;

            // render
            var offset = 0;
            var yp = 0;
            for (var y = 0; y < resources.Size.y; y++)
            {
                var xp = 0;
                for (var x = 0; x < resources.Size.x; x++)
                {
                    var ghostId = resources.GhostLayer[offset];
                    var tileId = resources.DefaultLayer[offset++];
                    var rect = new RectangleF((int)xp, (int)yp, resources.TileSize.x, resources.TileSize.y);
                    g.DrawImage(resources.Set, rect, resources.TileRectCache[ghostId > 0 ? ghostId : tileId], GraphicsUnit.Pixel);
                    xp += resources.TileSize.x;
                }
                yp += resources.TileSize.y;
            }

        }
    }
}
