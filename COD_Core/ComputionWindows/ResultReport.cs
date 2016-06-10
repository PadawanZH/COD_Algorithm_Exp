using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ComputionWindows
{
    public partial class ResultReport : Form
    {
        public ResultReport(string configText,string streamText ,string resultText)
        {
            InitializeComponent();
            tb_configField.Text = configText;
            tb_streamField.Text = streamText;
            tb_resultField.Text = resultText;
        }
    }
}
