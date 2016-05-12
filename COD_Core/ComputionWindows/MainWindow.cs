using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COD_Base.Core;
using COD_Base.Interface;

namespace ComputionWindows {
    public partial class MainWindow : Form, IListener {

        
        public Dictionary<string, string> modelInfo = new Dictionary<string, string>();
        public string workspace = null;
        public string dataFilePath = null;
        public bool isAlogrithmAdd = false;
        public bool isPropertiesAdd = false;
        public bool isRunSimulation = false;

        protected AlgorithmMgr algorithmHandler;

        private Form displayField;

        //用以标记”添加算法“按钮是否可以按下
        private bool isDLLPathSet = false;
        public MainWindow() {
            InitializeComponent();
            algorithmHandler = new AlgorithmMgr();

            EventType[] acceptedEventTypeList = { EventType.NoMoreTuple , EventType.Error , EventType.OldTupleDepart};
            EventDistributor.GetInstance().SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventTypeList);
        }

        

        private void label9_Click(object sender, EventArgs e) {//无用

        }

        #region 添加算法信息
        //
        /// <summary>
        /// 添加算法主文件的全路径，保存全路径AssemblyPath和WorkingDirectory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_assemblyPath_scan_Click(object sender, EventArgs e) {//点击浏览，选中算法主文件
            string assemblyPath = null;
            string fileName = null;
            openFileDialog_assemblyPath.Filter = "*.dll|*.dll";
            openFileDialog_assemblyPath.Title = "选择算法主文件";
            if (openFileDialog_assemblyPath.ShowDialog() == DialogResult.OK) {
                assemblyPath = openFileDialog_assemblyPath.FileName;
                string[] fp = assemblyPath.Split('\\');
                fileName = fp[fp.Length - 1];
                int workspaceEndIndex = assemblyPath.Length - (fp[fp.Length - 1].Length+1);
                workspace = assemblyPath.Substring(0, workspaceEndIndex);
                
                modelInfo["assemblyPath"] = assemblyPath;//保存主文件路径
                modelInfo["workingDirectory"] = workspace;//保存当前仿真计算工作目录

                tb_workspacePath.Text = workspace;
                tb_assemblyPath.Text = fileName;

                isDLLPathSet = true;
            }
        }

        /// <summary>
        /// 仅在bt_assemblyPath_scan_Click中调用
        /// </summary>
        private int FillClassNameIntoCombox()
        {
            COD_Base.Util.AssemblySupport.LoadAssembly(modelInfo["workingDirectory"], modelInfo["assemblyPath"]);
            List<string> classnames = COD_Base.Util.AssemblySupport.GetClassNameImplementedWithType(typeof(COD_Base.Interface.IAlgorithm));
            foreach(string className in classnames)
            {
                bool alreadyInList = false;
                foreach(string oldName in cb_ClassNames.Items)
                {
                    if (oldName.Equals(className))
                    {
                        alreadyInList = true;
                        break;
                    }
                }
                if (!alreadyInList)
                {
                    cb_ClassNames.Items.Add(className);
                }
            }
            if(classnames != null && classnames.Count > 0)
            {
                cb_ClassNames.Text = classnames[0];
            }

            return classnames.Count;
        }

        //扫描算法标准类测试函数
        private void bt_ClassName_save_Click(object sender, EventArgs e) {//保存当前算法标准类类名
            if (isDLLPathSet)
            {

                int algorithmCount = FillClassNameIntoCombox();
                //该函数验证填写的路径中的引擎类是否为IAlgorithm类
                if (algorithmCount > 0)
                {
                    cb_ClassNames.Text = cb_ClassNames.Items[0].ToString();
                    this.bt_modelInfo_testEngineClass.BackColor = Color.LightGreen;
                    this.bt_modelInfo_add.Enabled = true;
                    MessageBox.Show("扫描完毕，共扫描出" + algorithmCount + "个算法类");
                }
                else
                {
                    this.bt_modelInfo_add.Enabled = false;
                    this.bt_modelInfo_add.BackColor = Color.Yellow;
                    this.bt_modelInfo_testEngineClass.BackColor = Color.Yellow;
                    MessageBox.Show("没有找到算法类，该种类\"" + cb_ClassNames.Text + "\"不存在或不符合要求，可能由以下原因导致：\n 1. 没有按照要求填写，请检查是否填写命名空间\n 2. 拼写错误，请检查拼写是否有错误，命名空间和类名为大小写敏感\n 3. 该类没有继承COD_Base.Interface.IAlgorithm类，算法标准类必须继承该类");
                }
            }
            else if (!isDLLPathSet)
            {
                MessageBox.Show("算法信息没有填写完成，不能进行标准类扫描");
            }
            else if(cb_ClassNames.Text == "Namespace.ClassName" || cb_ClassNames.Text == "" || cb_ClassNames.Text == null)
            {
                MessageBox.Show("请填写算法引擎类\"命名空间名.算法标准类名\"");
            }
        }

        private void bt_modelInfo_add_Click(object sender, EventArgs e) {//添加算法信息
            modelInfo["algorithm"] = cb_ClassNames.Text;
            if (isAlogrithmAdd) {

                DialogResult dr = MessageBox.Show("当前已有算法添加，请确认是否覆盖当前算法信息。", "确认添加", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK) {

                    if (modelInfo["workingDirectory"] == null || modelInfo["workingDirectory"] == "") {
                        MessageBox.Show("工作区信息不得为空！！！");
                    } else if (modelInfo["assemblyPath"] == null || modelInfo["assemblyPath"] == "") {
                        MessageBox.Show("算法主文件信息不得为空！！！");
                    } else if (modelInfo["algorithm"] == null || modelInfo["algorithm"] == "") {
                        MessageBox.Show("算法标准引擎类信息不得为空！！！");
                    } else {
                        if (algorithmHandler.AssembleAlgorithmInstance(modelInfo)) {

                            MessageBox.Show("算法信息覆盖成功");
                        } else {
                            this.bt_modelInfo_add.BackColor = Color.Yellow;
                            MessageBox.Show("算法信息覆盖失败！！！！");
                        }
                    }
                } 
            } else {

                if (modelInfo["workingDirectory"] == null || modelInfo["workingDirectory"] == "") {
                    MessageBox.Show("工作区信息不得为空！！！");
                } else if (modelInfo["assemblyPath"] == null || modelInfo["assemblyPath"] == "") {
                    MessageBox.Show("算法主文件信息不得为空！！！");
                } else if (modelInfo["algorithm"] == null || modelInfo["algorithm"] == "") {
                    MessageBox.Show("算法标准引擎类信息不得为空！！！");
                } else {
                    if (algorithmHandler.AssembleAlgorithmInstance(modelInfo)) {
                        this.bt_modelInfo_add.BackColor = Color.LightGreen;
                        MessageBox.Show("算法信息添加成功" + algorithmHandler._algorithm.Description);
                        isAlogrithmAdd = true;
                    } else {
                        MessageBox.Show("算法信息添加失败！！！！");
                    }

                }
            
            }
            
        }
        private void bt_modelInfo_reset_Click(object sender, EventArgs e) {//重置算法信息

            tb_assemblyPath.Text = "";
            cb_ClassNames.Text = "";
            tb_workspacePath.Text = "";

            modelInfo["algorithm"] = "";
            workspace = null;
            modelInfo.Clear();

            this.bt_modelInfo_add.Enabled = false;

            this.bt_modelInfo_add.BackColor = Color.Yellow;
            this.bt_modelInfo_testEngineClass.BackColor = Color.Yellow;

            isDLLPathSet = false;
            isAlogrithmAdd = false;
        }

        #endregion

       

        #region 添加数据及信息

        private void bt_propertyFilePath_scan_Click(object sender, EventArgs e) {//保存参数文件路径
            openFileDialog_propertyFile.Filter = "*.dll,*.txt,*.swm|*.dll;*.txt;*.swm";
            openFileDialog_propertyFile.Title = "选择数据文件";
            if (openFileDialog_propertyFile.ShowDialog() == DialogResult.OK) {
                dataFilePath = openFileDialog_propertyFile.FileName;
                tb_dataFilePath.Text = dataFilePath;
            }
            //读取文件信息
            ReadPropertyInFile(dataFilePath);
        }

        /// <summary>
        /// 读取数据文件的信息并显示
        /// </summary>
        /// <param name="propertyFilePath"></param>
        private void ReadPropertyInFile(string propertyFilePath) {//读取选中的参数文件
            
        }
        #endregion
  


        #region 仿真计算

        private void bt_simulationComputing_Click(object sender, EventArgs e) {//触发仿真计算
            Configuration config = SetupConfiguration();
            if(config != null)
            {
                algorithmHandler.InitComponent(config);
                if (algorithmHandler.IsReadyToRun())
                {
                    //二维数据可以可视化
                    if (tb_DataDimension.Text == "2")
                    {
                        if(displayField != null)
                        {
                            displayField.Enabled = false;
                            displayField.Visible = false;
                            displayField.Dispose();
                        }
                        displayField = new _2D_DisplayField();
                        displayField.Show(this);
                    }

                    algorithmHandler.Start();
                    return;
                }
            }
            else
            {
                ShowErrorDialog("配置初始化失败，请检查是否有必要的参数信息没有填写");
            }
        }

        private Configuration SetupConfiguration()
        {
            if (    tb_dataFilePath.Text != "" && tb_DataDimension.Text != "" && tb_DataDelimiter.Text != "" 
                && tb_WindowSize.Text != "" && tb_SlideSpan.Text != "" && tb_QueryRange.Text != "" && tb_ThesholdK.Text != "")
            {
                Configuration configuration = new Configuration();

                configuration.SetProperty(PropertiesType.DataFilePath, tb_dataFilePath.Text);
                configuration.SetProperty(PropertiesType.DataDimension, Convert.ToInt32(tb_DataDimension.Text));
                configuration.SetProperty(PropertiesType.Delimiter, Convert.ToChar(tb_DataDelimiter.Text));
                configuration.SetProperty(PropertiesType.WindowSize, Convert.ToInt32(tb_WindowSize.Text));
                configuration.SetProperty(PropertiesType.SlideSpan, Convert.ToInt32(tb_SlideSpan.Text));
                configuration.SetProperty(PropertiesType.QueryRange, Convert.ToDouble(tb_QueryRange.Text));
                configuration.SetProperty(PropertiesType.KNeighbourThreshold, Convert.ToInt32(tb_ThesholdK.Text));
                return configuration;
            }
            else
            {
                return null;
            }
        }

        private void ShowResultDialog()
        {
            MessageBox.Show("运行成功");
        }

        private void ShowErrorDialog(string errorMsg)
        {
            MessageBox.Show(errorMsg);
            tb_SimulationResult.Text = "运行出现问题";
        }

        private void bt_simulationCompution_reset_Click(object sender, EventArgs e) {//重置仿真计算有关参数
            bt_modelInfo_add.Enabled = true;
        }

        public void OnEvent(IEvent anEvent)
        {
            switch (anEvent.Type)
            {
                case EventType.NoMoreTuple:
                    ShowResultDialog();
                    break;
                case EventType.Error:
                    ShowErrorDialog((string)anEvent.GetAttribute(EventAttributeType.Message));
                    break;
            }
        }

        #endregion
    }
}

       