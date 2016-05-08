using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using COD_Base.Core;
using COD_Base.Interface;

namespace ComputionWindows
{
    /// <summary>
    /// 实现<see cref="COD_Base.Interface.IListener"/>接口，响应数据变动并显示
    /// </summary>
    public partial class _2D_DisplayField : Form, COD_Base.Interface.IListener
    {
        Graphics panelGraphics;
        Bitmap background;
        List<ITuple> dataPoints;
        public _2D_DisplayField()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲\

            dataPoints = new List<ITuple>();

            panelGraphics = pl_Canvas.CreateGraphics();

            EventType[] acceptedEventTypeList = { EventType.NewTupleArrive, EventType.NoMoreTuple, EventType.InlierBecomeOutlier, EventType.OutlierBecomeInlier, EventType.WindowSlide };

            EventDistributor.GetInstance().SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventTypeList);
        }

        private void pl_Canvas_Paint(object sender, PaintEventArgs e)
        {
            panelGraphics.Clear(SystemColors.Control);
            panelGraphics = pl_Canvas.CreateGraphics();
            /*CoordinateDrawer coordinate = new CoordinateDrawer(pl_Canvas.CreateGraphics(), new Point(25, 25));
            coordinate.DrawingX(pl_Canvas.Width - 10).DrawingY(pl_Canvas.Height - 10);*/
            background = null;
            background = new Bitmap(pl_Canvas.Width, pl_Canvas.Height);
            Graphics backgroundImageGraphics = Graphics.FromImage(background);
            using (backgroundImageGraphics)
            {
                backgroundImageGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                CoordinateDrawer coordinate = new CoordinateDrawer(backgroundImageGraphics, new Point(45, 25));
                coordinate.DrawingX(pl_Canvas.Width - 5).DrawingY(pl_Canvas.Height - 10);

                float xPivotLength = pl_Canvas.Width - 5 - 45;
                float yPivotLength = pl_Canvas.Height - 10 - 25;
                DrawDataPoint(backgroundImageGraphics, new PointF(45, 25), pl_Canvas.Width - 5, pl_Canvas.Height - 10);
                panelGraphics.DrawImage(background, 0, 0);
            }
        }

        private void ReadDataFromSource()
        {

        }

        public void DrawDataPoint(Graphics graphics, PointF Origin, float XPivotLength, float YPivotLength)
        {
            StreamReader sr = new StreamReader(@"E:\Workspace\C#\COD_Algorithm_Exp\COD_Core\NormalizeData\bin\Debug\newData.txt");
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] data = line.Split(' ');
                graphics.FillEllipse(Brushes.Black, new RectangleF(((float)Convert.ToDouble(data[0])* XPivotLength)  + Origin.X, ( (float)Convert.ToDouble(data[1]) * YPivotLength) + Origin.Y, 5f, 5f));
            }
        }

        public void OnEvent(IEvent anEvent)
        {
            throw new NotImplementedException();
        }
    }
}
