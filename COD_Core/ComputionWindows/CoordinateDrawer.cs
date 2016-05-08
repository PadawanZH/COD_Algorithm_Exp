using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ComputionWindows
{
    class CoordinateDrawer
    {
        ////// 坐标字体
        private Font _font;
 
        ////// 画图刷子
        private SolidBrush _brush;
 
        //////  字符串格式
        private StringFormat _sf;
 
        ////// 画笔
        private Pen _pen;
       
        ////// 每个单元的间距
        private int _space;
 
        ////// 画图对象
        private Graphics _g;
 
        ////// 0坐标点
        private Point _circle;
 
        ////// 短线
        private int _shortLine;
 
        ////// 中线
        private int _middleLine;
 
        ////// 长线
        private int _longLine;
 
        ////// 构造函数
        public CoordinateDrawer(Graphics g, Point circle)
        {
            _space      = 5;
            _circle     = circle;
            _g          = g;
            _shortLine  = 5;
            _middleLine = 10;
            _longLine   = 15;
            _font       = new Font("Airal", 7);
            _brush      = new SolidBrush(Color.Black);
            _sf         = new StringFormat();
            _pen        = new Pen(Color.Black, 1);
            _drawingCircle();
        }
 
        ////// 画原点
        private void _drawingCircle()
        {
            _sf.FormatFlags      = StringFormatFlags.DirectionRightToLeft;
            _g.DrawString("0", _font, _brush, new PointF(_circle.X - 6, _circle.Y - 10), _sf);
        }

    ////// 画Y标尺
    //////Y轴最大值
    public CoordinateDrawer DrawingY(int maxY)
    {
        int y = 0;
        Point[] line = null;
        _sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
        double total = Math.Floor((maxY - _circle.Y) / _space * 1.0);
        total -= total % 5;
        for (int i = 0, j = -1; i < maxY && j < total; i++)
        {
            if (0 != i % _space)
            {
                continue;
            }
            y = i + _circle.Y;
            j++;
            if (0 != j % 5)
            {
                line = new Point[] {
                        new Point(_circle.X - _shortLine, y),
                        new Point(_circle.X, y)
                    };
                _g.DrawLines(_pen, line);
                continue;
            }
            double coorValue = (double)j / total;
            _g.DrawString(coorValue.ToString("#0.000"), _font, _brush, new PointF(_circle.X-15, y-5), _sf);
            if (0 == j % 10)
            {
                line = new Point[] {
                        new Point(_circle.X - _longLine, y),
                        new Point(_circle.X, y)
                    };
            }
            else {
                line = new Point[] {
                        new Point(_circle.X - _middleLine, y),
                        new Point(_circle.X, y)
                    };
            }
            _g.DrawLines(_pen, line);
        }
        line = new Point[] {
                new Point(_circle.X, y),
                new Point(_circle.X, _circle.Y)
            };
        _g.DrawLines(_pen, line);

        return this;
    }
 
        ////// 画X轴
        /// 
        ///最大X坐标
        public CoordinateDrawer DrawingX(int maxX)
        {
            int x = 0;
            Point[] line = null;
            double total = Math.Floor((maxX - _circle.X) / _space * 1.0);
            
            _sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            for (int i = 0, j = -1; i<maxX && j<total; i++) {
                if (0 != i % _space) {
                    continue;
                }
                x = i + _circle.X;
                j++;
                //画短线
                if (0 != j % 5) {
                    line = new Point[] {
                        new Point(x, _circle.Y - _shortLine),
                        new Point(x, _circle.Y)
                    };
                    _g.DrawLines(_pen, line);
                    continue;
                }
                double coorValue = (double)j / total;
                if (j != 0)
                {
                    _g.DrawString(coorValue.ToString("#0.000"), _font, _brush, new PointF(x + 15f, 2), _sf);
                }
                if (0 == j % 10) {
                    line = new Point[] {
                        new Point(x, _circle.Y - _longLine),
                        new Point(x, _circle.Y)
                    };
                } else {
                    line = new Point[] {
                        new Point(x, _circle.Y - _middleLine),
                        new Point(x, _circle.Y)
                    };
                }
                _g.DrawLines(_pen, line);
                
            }
            line = new Point[] {
                new Point(_circle.X, _circle.Y),
                new Point(x, _circle.Y)
            };
            _g.DrawLines(_pen, line);
 
            return this;
        }
 
    }
}