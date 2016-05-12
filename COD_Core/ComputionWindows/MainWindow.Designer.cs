namespace ComputionWindows {
    partial class MainWindow {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_assemblyPath = new System.Windows.Forms.TextBox();
            this.bt_assemblyPath_scan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_workspacePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_modelInfo_testEngineClass = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_dataFilePath = new System.Windows.Forms.TextBox();
            this.bt_dataFilePath_scan = new System.Windows.Forms.Button();
            this.bt_simulationComputing = new System.Windows.Forms.Button();
            this.gb_modelInfo = new System.Windows.Forms.GroupBox();
            this.cb_ClassNames = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.bt_modelInfo_reset = new System.Windows.Forms.Button();
            this.bt_modelInfo_add = new System.Windows.Forms.Button();
            this.gb_simulationInfo = new System.Windows.Forms.GroupBox();
            this.RunAlgConfigGroup = new System.Windows.Forms.GroupBox();
            this.tb_ThesholdK = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_QueryRange = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_SlideSpan = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_WindowSize = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bt_simulationCompution_reset = new System.Windows.Forms.Button();
            this.tb_SimulationResult = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.openFileDialog_assemblyPath = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_propertyFile = new System.Windows.Forms.OpenFileDialog();
            this.gb_propertyInfo = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_DataDelimiter = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_DataDimension = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ButtonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.InputAreaTip = new System.Windows.Forms.ToolTip(this.components);
            this.BackgroundPanel = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gb_modelInfo.SuspendLayout();
            this.gb_simulationInfo.SuspendLayout();
            this.RunAlgConfigGroup.SuspendLayout();
            this.gb_propertyInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.BackgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "算法主文件：";
            this.InputAreaTip.SetToolTip(this.label1, "该算法主文件应为算法C#封装编译的DLL文件");
            // 
            // tb_assemblyPath
            // 
            this.tb_assemblyPath.Location = new System.Drawing.Point(124, 31);
            this.tb_assemblyPath.Name = "tb_assemblyPath";
            this.tb_assemblyPath.ReadOnly = true;
            this.tb_assemblyPath.Size = new System.Drawing.Size(306, 21);
            this.tb_assemblyPath.TabIndex = 1;
            this.InputAreaTip.SetToolTip(this.tb_assemblyPath, "该算法主文件应为算法C#封装编译的DLL文件");
            // 
            // bt_assemblyPath_scan
            // 
            this.bt_assemblyPath_scan.Location = new System.Drawing.Point(453, 29);
            this.bt_assemblyPath_scan.Name = "bt_assemblyPath_scan";
            this.bt_assemblyPath_scan.Size = new System.Drawing.Size(81, 24);
            this.bt_assemblyPath_scan.TabIndex = 2;
            this.bt_assemblyPath_scan.Text = "浏 览";
            this.ButtonToolTip.SetToolTip(this.bt_assemblyPath_scan, "浏览算法主文件位置");
            this.bt_assemblyPath_scan.UseVisualStyleBackColor = true;
            this.bt_assemblyPath_scan.Click += new System.EventHandler(this.bt_assemblyPath_scan_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "工作区目录：";
            this.InputAreaTip.SetToolTip(this.label2, "工作区目录为算法主文件目录，日志、结果均在此目录下");
            // 
            // tb_workspacePath
            // 
            this.tb_workspacePath.Location = new System.Drawing.Point(124, 75);
            this.tb_workspacePath.Name = "tb_workspacePath";
            this.tb_workspacePath.ReadOnly = true;
            this.tb_workspacePath.Size = new System.Drawing.Size(306, 21);
            this.tb_workspacePath.TabIndex = 5;
            this.InputAreaTip.SetToolTip(this.tb_workspacePath, "工作区目录为算法主文件目录，日志、结果均在此目录下");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "标准算法类：";
            this.InputAreaTip.SetToolTip(this.label3, "*标准类为算法封装中继承IAlgorithm的类");
            // 
            // bt_modelInfo_testEngineClass
            // 
            this.bt_modelInfo_testEngineClass.BackColor = System.Drawing.Color.Yellow;
            this.bt_modelInfo_testEngineClass.Location = new System.Drawing.Point(55, 160);
            this.bt_modelInfo_testEngineClass.Name = "bt_modelInfo_testEngineClass";
            this.bt_modelInfo_testEngineClass.Size = new System.Drawing.Size(82, 34);
            this.bt_modelInfo_testEngineClass.TabIndex = 8;
            this.bt_modelInfo_testEngineClass.Text = "扫描算法类";
            this.ButtonToolTip.SetToolTip(this.bt_modelInfo_testEngineClass, "点击该按钮测试标准算法类是否填写正确");
            this.bt_modelInfo_testEngineClass.UseVisualStyleBackColor = false;
            this.bt_modelInfo_testEngineClass.Click += new System.EventHandler(this.bt_ClassName_save_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "输入数据文件：";
            // 
            // tb_dataFilePath
            // 
            this.tb_dataFilePath.Location = new System.Drawing.Point(124, 24);
            this.tb_dataFilePath.Name = "tb_dataFilePath";
            this.tb_dataFilePath.ReadOnly = true;
            this.tb_dataFilePath.Size = new System.Drawing.Size(306, 21);
            this.tb_dataFilePath.TabIndex = 11;
            this.tb_dataFilePath.Text = "E:\\Workspace\\C#\\COD_Algorithm_Exp\\COD_Core\\NormalizeData\\bin\\Debug\\newData.txt";
            // 
            // bt_dataFilePath_scan
            // 
            this.bt_dataFilePath_scan.Location = new System.Drawing.Point(453, 22);
            this.bt_dataFilePath_scan.Name = "bt_dataFilePath_scan";
            this.bt_dataFilePath_scan.Size = new System.Drawing.Size(81, 24);
            this.bt_dataFilePath_scan.TabIndex = 12;
            this.bt_dataFilePath_scan.Text = "浏 览";
            this.ButtonToolTip.SetToolTip(this.bt_dataFilePath_scan, "浏览参数文件，参数文件格式参看《参数文件格式说明》");
            this.bt_dataFilePath_scan.UseVisualStyleBackColor = true;
            this.bt_dataFilePath_scan.Click += new System.EventHandler(this.bt_propertyFilePath_scan_Click);
            // 
            // bt_simulationComputing
            // 
            this.bt_simulationComputing.BackColor = System.Drawing.Color.Yellow;
            this.bt_simulationComputing.Location = new System.Drawing.Point(55, 153);
            this.bt_simulationComputing.Name = "bt_simulationComputing";
            this.bt_simulationComputing.Size = new System.Drawing.Size(80, 34);
            this.bt_simulationComputing.TabIndex = 22;
            this.bt_simulationComputing.Text = "仿真计算";
            this.bt_simulationComputing.UseVisualStyleBackColor = false;
            this.bt_simulationComputing.Click += new System.EventHandler(this.bt_simulationComputing_Click);
            // 
            // gb_modelInfo
            // 
            this.gb_modelInfo.Controls.Add(this.cb_ClassNames);
            this.gb_modelInfo.Controls.Add(this.label12);
            this.gb_modelInfo.Controls.Add(this.bt_modelInfo_reset);
            this.gb_modelInfo.Controls.Add(this.bt_modelInfo_add);
            this.gb_modelInfo.Controls.Add(this.tb_assemblyPath);
            this.gb_modelInfo.Controls.Add(this.label1);
            this.gb_modelInfo.Controls.Add(this.bt_assemblyPath_scan);
            this.gb_modelInfo.Controls.Add(this.tb_workspacePath);
            this.gb_modelInfo.Controls.Add(this.label2);
            this.gb_modelInfo.Controls.Add(this.bt_modelInfo_testEngineClass);
            this.gb_modelInfo.Controls.Add(this.label3);
            this.gb_modelInfo.Location = new System.Drawing.Point(12, 15);
            this.gb_modelInfo.Name = "gb_modelInfo";
            this.gb_modelInfo.Size = new System.Drawing.Size(573, 229);
            this.gb_modelInfo.TabIndex = 24;
            this.gb_modelInfo.TabStop = false;
            this.gb_modelInfo.Text = "算法信息";
            // 
            // cb_ClassNames
            // 
            this.cb_ClassNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ClassNames.FormattingEnabled = true;
            this.cb_ClassNames.Location = new System.Drawing.Point(124, 119);
            this.cb_ClassNames.Name = "cb_ClassNames";
            this.cb_ClassNames.Size = new System.Drawing.Size(306, 20);
            this.cb_ClassNames.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.SystemColors.Control;
            this.label12.ForeColor = System.Drawing.Color.Crimson;
            this.label12.Location = new System.Drawing.Point(40, 203);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(329, 12);
            this.label12.TabIndex = 19;
            this.label12.Text = "*1. 浏览算法主文件； 2. 扫描算法类； 3. 点击\"加载算法\"";
            // 
            // bt_modelInfo_reset
            // 
            this.bt_modelInfo_reset.BackColor = System.Drawing.Color.Red;
            this.bt_modelInfo_reset.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_modelInfo_reset.ForeColor = System.Drawing.SystemColors.Info;
            this.bt_modelInfo_reset.Location = new System.Drawing.Point(441, 160);
            this.bt_modelInfo_reset.Name = "bt_modelInfo_reset";
            this.bt_modelInfo_reset.Size = new System.Drawing.Size(80, 34);
            this.bt_modelInfo_reset.TabIndex = 15;
            this.bt_modelInfo_reset.Text = "重  置";
            this.ButtonToolTip.SetToolTip(this.bt_modelInfo_reset, "重置填写的算法信息");
            this.bt_modelInfo_reset.UseVisualStyleBackColor = false;
            this.bt_modelInfo_reset.Click += new System.EventHandler(this.bt_modelInfo_reset_Click);
            // 
            // bt_modelInfo_add
            // 
            this.bt_modelInfo_add.BackColor = System.Drawing.Color.Yellow;
            this.bt_modelInfo_add.Enabled = false;
            this.bt_modelInfo_add.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_modelInfo_add.Location = new System.Drawing.Point(245, 160);
            this.bt_modelInfo_add.Name = "bt_modelInfo_add";
            this.bt_modelInfo_add.Size = new System.Drawing.Size(80, 34);
            this.bt_modelInfo_add.TabIndex = 14;
            this.bt_modelInfo_add.Text = "加载算法";
            this.bt_modelInfo_add.UseVisualStyleBackColor = false;
            this.bt_modelInfo_add.Click += new System.EventHandler(this.bt_modelInfo_add_Click);
            // 
            // gb_simulationInfo
            // 
            this.gb_simulationInfo.Controls.Add(this.RunAlgConfigGroup);
            this.gb_simulationInfo.Controls.Add(this.button1);
            this.gb_simulationInfo.Controls.Add(this.label5);
            this.gb_simulationInfo.Controls.Add(this.bt_simulationCompution_reset);
            this.gb_simulationInfo.Controls.Add(this.tb_SimulationResult);
            this.gb_simulationInfo.Controls.Add(this.label9);
            this.gb_simulationInfo.Controls.Add(this.bt_simulationComputing);
            this.gb_simulationInfo.Location = new System.Drawing.Point(12, 497);
            this.gb_simulationInfo.Name = "gb_simulationInfo";
            this.gb_simulationInfo.Size = new System.Drawing.Size(573, 268);
            this.gb_simulationInfo.TabIndex = 25;
            this.gb_simulationInfo.TabStop = false;
            this.gb_simulationInfo.Text = "仿真计算";
            // 
            // RunAlgConfigGroup
            // 
            this.RunAlgConfigGroup.Controls.Add(this.tb_ThesholdK);
            this.RunAlgConfigGroup.Controls.Add(this.label8);
            this.RunAlgConfigGroup.Controls.Add(this.tb_QueryRange);
            this.RunAlgConfigGroup.Controls.Add(this.label10);
            this.RunAlgConfigGroup.Controls.Add(this.tb_SlideSpan);
            this.RunAlgConfigGroup.Controls.Add(this.label7);
            this.RunAlgConfigGroup.Controls.Add(this.tb_WindowSize);
            this.RunAlgConfigGroup.Controls.Add(this.label6);
            this.RunAlgConfigGroup.Location = new System.Drawing.Point(42, 20);
            this.RunAlgConfigGroup.Name = "RunAlgConfigGroup";
            this.RunAlgConfigGroup.Size = new System.Drawing.Size(492, 120);
            this.RunAlgConfigGroup.TabIndex = 30;
            this.RunAlgConfigGroup.TabStop = false;
            this.RunAlgConfigGroup.Text = "算法运行配置";
            // 
            // tb_ThesholdK
            // 
            this.tb_ThesholdK.Location = new System.Drawing.Point(403, 76);
            this.tb_ThesholdK.Name = "tb_ThesholdK";
            this.tb_ThesholdK.Size = new System.Drawing.Size(74, 21);
            this.tb_ThesholdK.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(260, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "邻居数的阈值 k（整数）";
            // 
            // tb_QueryRange
            // 
            this.tb_QueryRange.Location = new System.Drawing.Point(141, 76);
            this.tb_QueryRange.Name = "tb_QueryRange";
            this.tb_QueryRange.Size = new System.Drawing.Size(74, 21);
            this.tb_QueryRange.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "查询范围 R（小数）";
            // 
            // tb_SlideSpan
            // 
            this.tb_SlideSpan.Location = new System.Drawing.Point(403, 29);
            this.tb_SlideSpan.Name = "tb_SlideSpan";
            this.tb_SlideSpan.Size = new System.Drawing.Size(74, 21);
            this.tb_SlideSpan.TabIndex = 3;
            this.tb_SlideSpan.Text = "10";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(260, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "窗口滑动区间  （整数）";
            // 
            // tb_WindowSize
            // 
            this.tb_WindowSize.Location = new System.Drawing.Point(141, 29);
            this.tb_WindowSize.Name = "tb_WindowSize";
            this.tb_WindowSize.Size = new System.Drawing.Size(74, 21);
            this.tb_WindowSize.TabIndex = 1;
            this.tb_WindowSize.Text = "1000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "窗口大小  （整数）";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Yellow;
            this.button1.Location = new System.Drawing.Point(245, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 34);
            this.button1.TabIndex = 29;
            this.button1.Text = "生成报告";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.ForeColor = System.Drawing.Color.Crimson;
            this.label5.Location = new System.Drawing.Point(37, 241);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(461, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "*操作顺序： 1. 配置算法参数； 2. 点击\"仿真计算\"按钮（若数据为2维可生成图像）";
            // 
            // bt_simulationCompution_reset
            // 
            this.bt_simulationCompution_reset.BackColor = System.Drawing.Color.Red;
            this.bt_simulationCompution_reset.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_simulationCompution_reset.ForeColor = System.Drawing.SystemColors.Info;
            this.bt_simulationCompution_reset.Location = new System.Drawing.Point(441, 153);
            this.bt_simulationCompution_reset.Name = "bt_simulationCompution_reset";
            this.bt_simulationCompution_reset.Size = new System.Drawing.Size(80, 34);
            this.bt_simulationCompution_reset.TabIndex = 28;
            this.bt_simulationCompution_reset.Text = "重新设置";
            this.ButtonToolTip.SetToolTip(this.bt_simulationCompution_reset, "重置仿真计算有关参数");
            this.bt_simulationCompution_reset.UseVisualStyleBackColor = false;
            this.bt_simulationCompution_reset.Click += new System.EventHandler(this.bt_simulationCompution_reset_Click);
            // 
            // tb_SimulationResult
            // 
            this.tb_SimulationResult.Location = new System.Drawing.Point(127, 207);
            this.tb_SimulationResult.Name = "tb_SimulationResult";
            this.tb_SimulationResult.ReadOnly = true;
            this.tb_SimulationResult.Size = new System.Drawing.Size(407, 21);
            this.tb_SimulationResult.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(40, 211);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 26;
            this.label9.Text = "仿真计算状态：";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // openFileDialog_assemblyPath
            // 
            this.openFileDialog_assemblyPath.FileName = "openFileDialog1";
            // 
            // openFileDialog_propertyFile
            // 
            this.openFileDialog_propertyFile.FileName = "openFileDialog1";
            // 
            // gb_propertyInfo
            // 
            this.gb_propertyInfo.Controls.Add(this.groupBox1);
            this.gb_propertyInfo.Controls.Add(this.label13);
            this.gb_propertyInfo.Controls.Add(this.tb_dataFilePath);
            this.gb_propertyInfo.Controls.Add(this.label4);
            this.gb_propertyInfo.Controls.Add(this.bt_dataFilePath_scan);
            this.gb_propertyInfo.Location = new System.Drawing.Point(12, 261);
            this.gb_propertyInfo.Name = "gb_propertyInfo";
            this.gb_propertyInfo.Size = new System.Drawing.Size(573, 222);
            this.gb_propertyInfo.TabIndex = 26;
            this.gb_propertyInfo.TabStop = false;
            this.gb_propertyInfo.Text = "数据文件信息";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_DataDelimiter);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.tb_DataDimension);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Location = new System.Drawing.Point(42, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 125);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据信息";
            // 
            // tb_DataDelimiter
            // 
            this.tb_DataDelimiter.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.tb_DataDelimiter.Location = new System.Drawing.Point(403, 53);
            this.tb_DataDelimiter.MaxLength = 1;
            this.tb_DataDelimiter.Name = "tb_DataDelimiter";
            this.tb_DataDelimiter.Size = new System.Drawing.Size(74, 21);
            this.tb_DataDelimiter.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(260, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 12);
            this.label14.TabIndex = 3;
            this.label14.Text = "数据分割符号 （字符）";
            // 
            // tb_DataDimension
            // 
            this.tb_DataDimension.Location = new System.Drawing.Point(141, 53);
            this.tb_DataDimension.Name = "tb_DataDimension";
            this.tb_DataDimension.Size = new System.Drawing.Size(74, 21);
            this.tb_DataDimension.TabIndex = 2;
            this.tb_DataDimension.Text = "2";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(24, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "数据维度  （整数）";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.SystemColors.Control;
            this.label13.ForeColor = System.Drawing.Color.Crimson;
            this.label13.Location = new System.Drawing.Point(39, 200);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(275, 12);
            this.label13.TabIndex = 20;
            this.label13.Text = "*操作顺序： 1. 浏览数据文件； 2. 配置数据信息";
            // 
            // ButtonToolTip
            // 
            this.ButtonToolTip.AutomaticDelay = 100;
            // 
            // InputAreaTip
            // 
            this.InputAreaTip.AutomaticDelay = 100;
            this.InputAreaTip.IsBalloon = true;
            // 
            // BackgroundPanel
            // 
            this.BackgroundPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundPanel.AutoScroll = true;
            this.BackgroundPanel.Controls.Add(this.gb_modelInfo);
            this.BackgroundPanel.Controls.Add(this.gb_propertyInfo);
            this.BackgroundPanel.Controls.Add(this.gb_simulationInfo);
            this.BackgroundPanel.Location = new System.Drawing.Point(0, 0);
            this.BackgroundPanel.Margin = new System.Windows.Forms.Padding(2);
            this.BackgroundPanel.Name = "BackgroundPanel";
            this.BackgroundPanel.Size = new System.Drawing.Size(599, 778);
            this.BackgroundPanel.TabIndex = 28;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(598, 777);
            this.Controls.Add(this.BackgroundPanel);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MainWindow";
            this.Text = "流数据异常值检测算法运行";
            this.gb_modelInfo.ResumeLayout(false);
            this.gb_modelInfo.PerformLayout();
            this.gb_simulationInfo.ResumeLayout(false);
            this.gb_simulationInfo.PerformLayout();
            this.RunAlgConfigGroup.ResumeLayout(false);
            this.RunAlgConfigGroup.PerformLayout();
            this.gb_propertyInfo.ResumeLayout(false);
            this.gb_propertyInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.BackgroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_assemblyPath;
        private System.Windows.Forms.Button bt_assemblyPath_scan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_workspacePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_modelInfo_testEngineClass;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox tb_dataFilePath;
        private System.Windows.Forms.Button bt_dataFilePath_scan;
        private System.Windows.Forms.Button bt_simulationComputing;
        private System.Windows.Forms.GroupBox gb_modelInfo;
        private System.Windows.Forms.GroupBox gb_simulationInfo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_SimulationResult;
        private System.Windows.Forms.Button bt_simulationCompution_reset;
        private System.Windows.Forms.OpenFileDialog openFileDialog_assemblyPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog_propertyFile;
        private System.Windows.Forms.Button bt_modelInfo_reset;
        private System.Windows.Forms.Button bt_modelInfo_add;
        private System.Windows.Forms.GroupBox gb_propertyInfo;
        private System.Windows.Forms.ToolTip ButtonToolTip;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ToolTip InputAreaTip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel BackgroundPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox RunAlgConfigGroup;
        private System.Windows.Forms.TextBox tb_SlideSpan;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_WindowSize;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_ThesholdK;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_QueryRange;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_DataDelimiter;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_DataDimension;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cb_ClassNames;
    }
}

