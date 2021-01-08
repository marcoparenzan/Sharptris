using Sharptris.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharptris
{
    public partial class DoubleBufferForm : Form
    {
        public DoubleBufferForm()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Font = new Font("C64 Pro Mono Normale", 12);
        }

        BufferedGraphics bufferedGraphics;

        public DoubleBufferForm Initialize(Stage stage, StageResources resources)
        {
            this.stage = stage;
            this.resources = resources;
            view = new ScrollTopView(stage, resources);
            this.ClientSize = new Size(resources.Size.x * resources.TileSize.x + 8, (resources.Size.y + 1) * resources.TileSize.y);
            this.Text = resources.Title;
            return this;
        }

        private Stage stage;
        private StageResources resources;
        private ScrollTopView view;

        public bool Suspended { get;  set; } = true;
        public int FrameRate { get; set; }

        public ButtonTrigger Fire { get; } = new ButtonTrigger();
        public ButtonTrigger MoveLeft { get; } = new ButtonTrigger();
        public ButtonTrigger MoveRight { get; } = new ButtonTrigger();
        public ButtonTrigger MoveUp { get; } = new ButtonTrigger();
        public ButtonTrigger MoveDown { get; } = new ButtonTrigger();

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            if (this.bufferedGraphics != null)
            {
                this.Suspended = true;
                this.bufferedGraphics.Dispose();
                this.bufferedGraphics = null;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.bufferedGraphics == null)
            {
                this.bufferedGraphics = BufferedGraphicsManager.Current.Allocate(e.Graphics,
                    //new Rectangle(resources.TileSize.x, resources.TileSize.y, this.ClientRectangle.Width - resources.TileSize.x * 2, this.ClientRectangle.Height - resources.TileSize.y * 2)
                    this.ClientRectangle
                );

                view.Reset(this.ClientRectangle);
                this.Suspended = false;
            }
            else
            {
                this.bufferedGraphics.Render(e.Graphics);
            }
        }

        public void FrameUpdate()
        {
            if (MoveLeft.Triggered())
            {
                stage.MoveLeft();
            }
            if (MoveRight.Triggered())
            {
                stage.MoveRight();
            }
            if (MoveUp.Triggered())
            {
                stage.MoveUp();
            }
            if (MoveDown.Triggered())
            {
                stage.MoveDown();
            }
            if (Fire.Triggered())
            {
                stage.Fire();
            }

            stage.Update();

            view.Update(stage);
        }

        public void FrameRender()
        {
            bufferedGraphics.Graphics.Clear(Color.Black);
            view.Render(bufferedGraphics.Graphics);
            //bufferedGraphics.Graphics.DrawString($"FrameRate={FrameRate} View=({view.Pos.x},{view.Pos.y}) Car=({stage.Current.x},{stage.Current.y})", this.Font, Brushes.White, 32, 32);
            bufferedGraphics.Graphics.DrawString($"Current={stage.Current.pos}", this.Font, Brushes.White, 32, 32);
            this.Invalidate();
        }
    }
}
