using System;
using System.Collections;
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
        Graphics backgroundImageGraphics;
        Bitmap background;
        Bitmap pointMap;

        Dictionary<int, ITuple> dataPoints;

        private static readonly object lockRoot = new object();

        bool Drawing;
        public _2D_DisplayField()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲\

            dataPoints = new Dictionary<int, ITuple>();

            panelGraphics = pl_Canvas.CreateGraphics();

            EventType[] acceptedEventTypeList = { EventType.NewTupleArrive, EventType.InlierBecomeOutlier, EventType.OutlierBecomeInlier, EventType.OldTupleDepart, EventType.NoMoreTuple};

            EventDistributor.GetInstance().SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventTypeList);

            RefreshDataPointsTimer.Start();
        }

        private void pl_Canvas_Paint(object sender, PaintEventArgs e)
        {
            panelGraphics.Clear(SystemColors.Control);
            panelGraphics = pl_Canvas.CreateGraphics();
            /*CoordinateDrawer coordinate = new CoordinateDrawer(pl_Canvas.CreateGraphics(), new Point(25, 25));
            coordinate.DrawingX(pl_Canvas.Width - 10).DrawingY(pl_Canvas.Height - 10);*/
            DrawPivot();
        }

        private void ReadDataFromSource()
        {

        }

        public void DrawDataPoint(Graphics graphics, PointF Origin, float XPivotLength, float YPivotLength)
        {
            List<ITuple> listToDraw;
            
            lock (lockRoot)
            {
                listToDraw = dataPoints.Values.ToList();
            }
            Brush burshForType;
            ITuple tuple;
            for (int i = 0; i < listToDraw.Count; i++)
            {
                tuple = listToDraw[i];
                burshForType = (tuple.IsOutlier) ? Brushes.Red : Brushes.Black;
                graphics.FillEllipse(burshForType, new RectangleF(((float)Convert.ToDouble(tuple.Data[0]) * XPivotLength) + Origin.X, ((float)Convert.ToDouble(tuple.Data[1]) * YPivotLength) + Origin.Y, 5f, 5f));
            }
        }

        public void DrawPivot()
        {
            background = null;
            background = new Bitmap(pl_Canvas.Width, pl_Canvas.Height);
            backgroundImageGraphics = Graphics.FromImage(background);
            using (backgroundImageGraphics)
            {
                backgroundImageGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                CoordinateDrawer coordinate = new CoordinateDrawer(backgroundImageGraphics, new Point(45, 25));
                coordinate.DrawingX(pl_Canvas.Width - 5).DrawingY(pl_Canvas.Height - 10);
                panelGraphics.DrawImage(background, 0, 0);
            }
        }

        public void OnEvent(IEvent anEvent)
        {
            int tupleID = 0;
            ITuple tuple;
            switch (anEvent.Type)
            {
                case EventType.NewTupleArrive:
                    tuple = (ITuple)anEvent.GetAttribute(EventAttributeType.Tuple);
                    lock (lockRoot)
                    {
                        dataPoints.Add(tuple.ID, new COD_Base.Dynamic.Entity.Tuple(tuple));
                    }
                    break;
                case EventType.InlierBecomeOutlier:
                    tupleID = (int)anEvent.GetAttribute(EventAttributeType.TupleID);
                    lock (lockRoot)
                    {
                        dataPoints[tupleID].IsOutlier = true;
                    }
                    break;
                case EventType.OutlierBecomeInlier:
                    tupleID = (int)anEvent.GetAttribute(EventAttributeType.TupleID);
                    lock (lockRoot)
                    {
                        dataPoints[tupleID].IsOutlier = false;
                    }
                    break;
                case EventType.OldTupleDepart:
                    tupleID = (int)anEvent.GetAttribute(EventAttributeType.TupleID);
                    lock (lockRoot)
                    {
                        if (dataPoints.ContainsKey(tupleID))
                        {
                            dataPoints.Remove(tupleID);
                        }
                        else
                        {
                            throw new Exception("No Such tuple In dataPoints");
                        }
                    }
                    
                   
                    break;

                case EventType.NoMoreTuple:
                    RefreshDataPointsTimer.Stop();
                    break;
            }
        }

        private void RefreshDataPointsTimer_Tick(object sender, EventArgs e)
        {

            panelGraphics.Clear(SystemColors.Control);
            DrawPivot();
            pointMap = new Bitmap(pl_Canvas.Width, pl_Canvas.Height);
            Graphics pointMapGraphics = Graphics.FromImage(pointMap);
            float xPivotLength = pl_Canvas.Width - 5 - 45;
            float yPivotLength = pl_Canvas.Height - 10 - 25;
            DrawDataPoint(pointMapGraphics, new PointF(45, 25), pl_Canvas.Width - 5, pl_Canvas.Height - 10);
            panelGraphics.DrawImage(pointMap, 0, 0);
        }

        public void Reset()
        {

        }
    }
}
