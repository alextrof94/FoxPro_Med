namespace FoxPro_Med
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.buSave = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbUrlHttp = new System.Windows.Forms.TextBox();
			this.tbUrlHttps = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbClientId = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbClientSecret = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.buCertOur = new System.Windows.Forms.Button();
			this.buUrlTest = new System.Windows.Forms.Button();
			this.buUrlSandbox = new System.Windows.Forms.Button();
			this.buUrlWork = new System.Windows.Forms.Button();
			this.buCertForeign = new System.Windows.Forms.Button();
			this.cbLogs = new System.Windows.Forms.CheckBox();
			this.tbUserCert = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbCertForeign = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbLogDir = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tbTempDir = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.buLogDir = new System.Windows.Forms.Button();
			this.buTempDir = new System.Windows.Forms.Button();
			this.tbProxyAddress = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.nudProxyPort = new System.Windows.Forms.NumericUpDown();
			this.cbProxy = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).BeginInit();
			this.SuspendLayout();
			// 
			// buSave
			// 
			this.buSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buSave.Location = new System.Drawing.Point(413, 367);
			this.buSave.Name = "buSave";
			this.buSave.Size = new System.Drawing.Size(152, 62);
			this.buSave.TabIndex = 0;
			this.buSave.Text = "Сохранить";
			this.buSave.UseVisualStyleBackColor = true;
			this.buSave.Click += new System.EventHandler(this.buSave_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "URL http:";
			// 
			// tbUrlHttp
			// 
			this.tbUrlHttp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUrlHttp.Location = new System.Drawing.Point(116, 6);
			this.tbUrlHttp.Name = "tbUrlHttp";
			this.tbUrlHttp.Size = new System.Drawing.Size(449, 20);
			this.tbUrlHttp.TabIndex = 2;
			this.tbUrlHttp.TextChanged += new System.EventHandler(this.tbUrlHttp_TextChanged);
			// 
			// tbUrlHttps
			// 
			this.tbUrlHttps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUrlHttps.Location = new System.Drawing.Point(116, 35);
			this.tbUrlHttps.Name = "tbUrlHttps";
			this.tbUrlHttps.Size = new System.Drawing.Size(449, 20);
			this.tbUrlHttps.TabIndex = 4;
			this.tbUrlHttps.TextChanged += new System.EventHandler(this.tbUrlHttps_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "URL https:";
			// 
			// tbClientId
			// 
			this.tbClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbClientId.Location = new System.Drawing.Point(116, 90);
			this.tbClientId.Name = "tbClientId";
			this.tbClientId.Size = new System.Drawing.Size(449, 20);
			this.tbClientId.TabIndex = 6;
			this.tbClientId.TextChanged += new System.EventHandler(this.tbClientId_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 93);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Client Id:";
			// 
			// tbClientSecret
			// 
			this.tbClientSecret.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbClientSecret.Location = new System.Drawing.Point(116, 116);
			this.tbClientSecret.Name = "tbClientSecret";
			this.tbClientSecret.Size = new System.Drawing.Size(449, 20);
			this.tbClientSecret.TabIndex = 8;
			this.tbClientSecret.TextChanged += new System.EventHandler(this.tbClientSecret_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 119);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(70, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "Client Secret:";
			// 
			// buCertOur
			// 
			this.buCertOur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.buCertOur.Location = new System.Drawing.Point(15, 367);
			this.buCertOur.Name = "buCertOur";
			this.buCertOur.Size = new System.Drawing.Size(193, 62);
			this.buCertOur.TabIndex = 9;
			this.buCertOur.Text = "Выбрать НАШ сертификат";
			this.buCertOur.UseVisualStyleBackColor = true;
			this.buCertOur.Click += new System.EventHandler(this.buCertOur_Click);
			// 
			// buUrlTest
			// 
			this.buUrlTest.Location = new System.Drawing.Point(15, 61);
			this.buUrlTest.Name = "buUrlTest";
			this.buUrlTest.Size = new System.Drawing.Size(158, 23);
			this.buUrlTest.TabIndex = 11;
			this.buUrlTest.Text = "URL\'ы тестовые";
			this.buUrlTest.UseVisualStyleBackColor = true;
			this.buUrlTest.Click += new System.EventHandler(this.buUrlTest_Click);
			// 
			// buUrlSandbox
			// 
			this.buUrlSandbox.Location = new System.Drawing.Point(179, 61);
			this.buUrlSandbox.Name = "buUrlSandbox";
			this.buUrlSandbox.Size = new System.Drawing.Size(158, 23);
			this.buUrlSandbox.TabIndex = 12;
			this.buUrlSandbox.Text = "URL\'ы песочницы";
			this.buUrlSandbox.UseVisualStyleBackColor = true;
			this.buUrlSandbox.Click += new System.EventHandler(this.buUrlSandbox_Click);
			// 
			// buUrlWork
			// 
			this.buUrlWork.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buUrlWork.Location = new System.Drawing.Point(343, 61);
			this.buUrlWork.Name = "buUrlWork";
			this.buUrlWork.Size = new System.Drawing.Size(222, 23);
			this.buUrlWork.TabIndex = 13;
			this.buUrlWork.Text = "URL\'ы продуктивного контура";
			this.buUrlWork.UseVisualStyleBackColor = true;
			this.buUrlWork.Click += new System.EventHandler(this.buUrlWork_Click);
			// 
			// buCertForeign
			// 
			this.buCertForeign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.buCertForeign.Location = new System.Drawing.Point(214, 367);
			this.buCertForeign.Name = "buCertForeign";
			this.buCertForeign.Size = new System.Drawing.Size(193, 62);
			this.buCertForeign.TabIndex = 14;
			this.buCertForeign.Text = "Выбрать сертификат ЧЗ";
			this.buCertForeign.UseVisualStyleBackColor = true;
			this.buCertForeign.Click += new System.EventHandler(this.buCertForeign_Click);
			// 
			// cbLogs
			// 
			this.cbLogs.AutoSize = true;
			this.cbLogs.Location = new System.Drawing.Point(12, 139);
			this.cbLogs.Name = "cbLogs";
			this.cbLogs.Size = new System.Drawing.Size(250, 17);
			this.cbLogs.TabIndex = 15;
			this.cbLogs.Text = "Вести логи и сохранять файлы в папку temp";
			this.cbLogs.UseVisualStyleBackColor = true;
			// 
			// tbUserCert
			// 
			this.tbUserCert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUserCert.Location = new System.Drawing.Point(116, 309);
			this.tbUserCert.Name = "tbUserCert";
			this.tbUserCert.Size = new System.Drawing.Size(449, 20);
			this.tbUserCert.TabIndex = 17;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 312);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "НАШ сертификат:";
			// 
			// tbCertForeign
			// 
			this.tbCertForeign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCertForeign.Location = new System.Drawing.Point(116, 335);
			this.tbCertForeign.Name = "tbCertForeign";
			this.tbCertForeign.Size = new System.Drawing.Size(449, 20);
			this.tbCertForeign.TabIndex = 19;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 338);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(89, 13);
			this.label6.TabIndex = 18;
			this.label6.Text = "Сертификат ЧЗ:";
			// 
			// tbLogDir
			// 
			this.tbLogDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLogDir.Location = new System.Drawing.Point(116, 162);
			this.tbLogDir.Name = "tbLogDir";
			this.tbLogDir.Size = new System.Drawing.Size(336, 20);
			this.tbLogDir.TabIndex = 21;
			this.tbLogDir.TextChanged += new System.EventHandler(this.tbLogDir_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 165);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(95, 13);
			this.label7.TabIndex = 20;
			this.label7.Text = "Папка для логов:";
			// 
			// tbTempDir
			// 
			this.tbTempDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTempDir.Location = new System.Drawing.Point(116, 188);
			this.tbTempDir.Name = "tbTempDir";
			this.tbTempDir.Size = new System.Drawing.Size(336, 20);
			this.tbTempDir.TabIndex = 23;
			this.tbTempDir.TextChanged += new System.EventHandler(this.tbTempDir_TextChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 191);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 13);
			this.label8.TabIndex = 22;
			this.label8.Text = "Папка для файлов:";
			// 
			// buLogDir
			// 
			this.buLogDir.Location = new System.Drawing.Point(458, 162);
			this.buLogDir.Name = "buLogDir";
			this.buLogDir.Size = new System.Drawing.Size(107, 20);
			this.buLogDir.TabIndex = 24;
			this.buLogDir.Text = "Выбрать";
			this.buLogDir.UseVisualStyleBackColor = true;
			this.buLogDir.Click += new System.EventHandler(this.buLogDir_Click);
			// 
			// buTempDir
			// 
			this.buTempDir.Location = new System.Drawing.Point(458, 188);
			this.buTempDir.Name = "buTempDir";
			this.buTempDir.Size = new System.Drawing.Size(107, 20);
			this.buTempDir.TabIndex = 25;
			this.buTempDir.Text = "Выбрать";
			this.buTempDir.UseVisualStyleBackColor = true;
			this.buTempDir.Click += new System.EventHandler(this.buTempDir_Click);
			// 
			// tbProxyAddress
			// 
			this.tbProxyAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbProxyAddress.Location = new System.Drawing.Point(116, 249);
			this.tbProxyAddress.Name = "tbProxyAddress";
			this.tbProxyAddress.Size = new System.Drawing.Size(449, 20);
			this.tbProxyAddress.TabIndex = 27;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 252);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(69, 13);
			this.label9.TabIndex = 26;
			this.label9.Text = "Proxy адрес:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(12, 276);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(62, 13);
			this.label10.TabIndex = 28;
			this.label10.Text = "Proxy порт:";
			// 
			// nudProxyPort
			// 
			this.nudProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nudProxyPort.Location = new System.Drawing.Point(116, 275);
			this.nudProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudProxyPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudProxyPort.Name = "nudProxyPort";
			this.nudProxyPort.Size = new System.Drawing.Size(449, 20);
			this.nudProxyPort.TabIndex = 29;
			this.nudProxyPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// cbProxy
			// 
			this.cbProxy.AutoSize = true;
			this.cbProxy.Location = new System.Drawing.Point(15, 223);
			this.cbProxy.Name = "cbProxy";
			this.cbProxy.Size = new System.Drawing.Size(127, 17);
			this.cbProxy.TabIndex = 30;
			this.cbProxy.Text = "Использовать proxy";
			this.cbProxy.UseVisualStyleBackColor = true;
			// 
			// FormSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(577, 442);
			this.Controls.Add(this.cbProxy);
			this.Controls.Add(this.nudProxyPort);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.tbProxyAddress);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.buTempDir);
			this.Controls.Add(this.buLogDir);
			this.Controls.Add(this.tbTempDir);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.tbLogDir);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.tbCertForeign);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbUserCert);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbLogs);
			this.Controls.Add(this.buCertForeign);
			this.Controls.Add(this.buUrlWork);
			this.Controls.Add(this.buUrlSandbox);
			this.Controls.Add(this.buUrlTest);
			this.Controls.Add(this.buCertOur);
			this.Controls.Add(this.tbClientSecret);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.tbClientId);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbUrlHttps);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbUrlHttp);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buSave);
			this.Name = "FormSettings";
			this.Text = "Settings FoxPro_Med";
			this.Shown += new System.EventHandler(this.FormSettings_Shown);
			((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buSave;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbUrlHttp;
		private System.Windows.Forms.TextBox tbUrlHttps;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbClientId;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbClientSecret;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button buCertOur;
		private System.Windows.Forms.Button buUrlTest;
		private System.Windows.Forms.Button buUrlSandbox;
		private System.Windows.Forms.Button buUrlWork;
		private System.Windows.Forms.Button buCertForeign;
		private System.Windows.Forms.CheckBox cbLogs;
		private System.Windows.Forms.TextBox tbUserCert;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbCertForeign;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbLogDir;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox tbTempDir;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button buLogDir;
		private System.Windows.Forms.Button buTempDir;
		private System.Windows.Forms.TextBox tbProxyAddress;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.NumericUpDown nudProxyPort;
		private System.Windows.Forms.CheckBox cbProxy;
	}
}