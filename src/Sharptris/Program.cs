using Sharptris.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharptris
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var resources = StageResources.Get();
            var stage = new Stage(resources);

            var form =
                new DoubleBufferForm()
            ;
            form.Initialize(stage, resources);
            //form.FormBorderStyle = FormBorderStyle.None;
            //form.WindowState = FormWindowState.Maximized;
            form.StartPosition = FormStartPosition.CenterScreen;
            //form.Size = new Size(960, 640);
            form.KeyDown += (s, e) => {

                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        form.Close();
                        break;
                    case Keys.Space:
                        form.Fire.Set();
                        break;
                    case Keys.Left:
                        form.MoveLeft.Set();
                        break;
                    case Keys.Right:
                        form.MoveRight.Set();
                        break;
                    case Keys.Up:
                        form.MoveUp.Set();
                        break;
                    case Keys.Down:
                        form.MoveDown.Set();
                        break;
                    default:
                        break;
                }
            
            };
            form.KeyUp += (s, e) => {

                switch (e.KeyCode)
                {
                    case Keys.Space:
                        form.Fire.Reset();
                        break;
                    case Keys.Left:
                        form.MoveLeft.Reset();
                        break;
                    case Keys.Right:
                        form.MoveRight.Reset();
                        break;
                    case Keys.Up:
                        form.MoveUp.Reset();
                        break;
                    case Keys.Down:
                        form.MoveDown.Reset();
                        break;
                    default:
                        break;
                }

            };
            form.Show();

            var refrate = (int) Math.Round(1000.0 / 30, 0);

            var timer = new Timer();
            timer.Interval = refrate;
            timer.Tick += (s, e) =>
            {
                var start = DateTime.Now;

                form.FrameRender();
                form.FrameUpdate();

                var stop = DateTime.Now;
                form.FrameRate = (int)Math.Round(1000.0 / (stop - start).TotalMilliseconds, 0);

            };
            timer.Start();

            Application.Run(form);
        }
    }
}
