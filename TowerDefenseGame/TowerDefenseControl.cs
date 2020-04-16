using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TowerDefenseGame
{
    class TowerDefenseControl : FrameworkElement
    {
        TowerDefenseLogic logic;
        TowerDefenseRenderer renderer;
        TowerDefenseModel model;
        Stopwatch stw;

        public TowerDefenseControl()
        {
            Loaded += TowerDefenseControl_Loaded;// += <TAB><ENTER>
        }
        private void TowerDefenseControl_Loaded(object sender, RoutedEventArgs e)
        {
            stw = new Stopwatch();
            model = new TowerDefenseModel(ActualWidth, ActualHeight);
            logic = new TowerDefenseLogic(model);
            renderer = new TowerDefenseRenderer(model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                //win.KeyDown += Win_KeyDown;
                MouseDown += TowerDefenseControl_MouseDown;
            }

            InvalidateVisual();
            stw.Start();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (renderer != null) drawingContext.DrawDrawing(renderer.BuildDrawing());
        }

        private void TowerDefenseControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            Point tilePos = logic.GetTilePos(mousePos);
            MessageBox.Show(mousePos + "\n" + tilePos);
        }
        /*
        private void Win_KeyDown(object sender, KeyEventArgs e)
        {
            bool finished = false;
            switch (e.Key)
            {
                case Key.W: finished = logic.Move(0, -1); break;
                case Key.S: finished = logic.Move(0, 1); break;
                case Key.A: finished = logic.Move(-1, 0); break;
                case Key.D: finished = logic.Move(1, 0); break;
            }
            InvalidateVisual();
            if (finished)
            {
                stw.Stop();
                MessageBox.Show("YAY! " + stw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
                // Not elegant: should use an event!
            }
        }
        */
    }
}
