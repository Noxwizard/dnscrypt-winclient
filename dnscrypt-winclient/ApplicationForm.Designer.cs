namespace dnscrypt_winclient
{
	partial class ApplicationForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationForm));
			this.DNSlistbox = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.startstop_button = new System.Windows.Forms.Button();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.notifyIconContextOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIconContextExit = new System.Windows.Forms.ToolStripMenuItem();
			this.portBox = new System.Windows.Forms.ComboBox();
			this.protoUDP = new System.Windows.Forms.RadioButton();
			this.protoTCP = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.hideAdaptersCheckbox = new System.Windows.Forms.CheckBox();
			this.gatewayCheckbox = new System.Windows.Forms.CheckBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabNICs = new System.Windows.Forms.TabPage();
			this.tabConfig = new System.Windows.Forms.TabPage();
			this.button_save = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.combobox_provider = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textbox_key = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.textbox_fqdn = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textbox_server_addr = new System.Windows.Forms.TextBox();
			this.autostartCheckBox = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ipv4Radio = new System.Windows.Forms.RadioButton();
			this.ipv6Radio = new System.Windows.Forms.RadioButton();
			this.parentalControlsRadio = new System.Windows.Forms.RadioButton();
			this.tabPlugins = new System.Windows.Forms.TabPage();
			this.button_plugins_refresh = new System.Windows.Forms.Button();
			this.pluginDescriptionTextBox = new System.Windows.Forms.TextBox();
			this.pluginListBox = new System.Windows.Forms.CheckedListBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textbox_plugin_params = new System.Windows.Forms.TextBox();
			this.tabAbout = new System.Windows.Forms.TabPage();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.buttonInstall = new System.Windows.Forms.Button();
			this.serviceTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.tooltip_plugin = new System.Windows.Forms.ToolTip(this.components);
			this.notifyIconContextMenu.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabNICs.SuspendLayout();
			this.tabConfig.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabPlugins.SuspendLayout();
			this.tabAbout.SuspendLayout();
			this.SuspendLayout();
			// 
			// DNSlistbox
			// 
			this.DNSlistbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DNSlistbox.CheckOnClick = true;
			this.DNSlistbox.FormattingEnabled = true;
			this.DNSlistbox.HorizontalScrollbar = true;
			this.DNSlistbox.Location = new System.Drawing.Point(9, 19);
			this.DNSlistbox.Name = "DNSlistbox";
			this.DNSlistbox.Size = new System.Drawing.Size(389, 154);
			this.DNSlistbox.TabIndex = 0;
			this.DNSlistbox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.DNSlisbox_itemcheck_statechanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(225, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select network devices for use with DNSCrypt";
			// 
			// startstop_button
			// 
			this.startstop_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.startstop_button.Location = new System.Drawing.Point(93, 239);
			this.startstop_button.Name = "startstop_button";
			this.startstop_button.Size = new System.Drawing.Size(75, 23);
			this.startstop_button.TabIndex = 3;
			this.startstop_button.Text = "Start";
			this.startstop_button.UseVisualStyleBackColor = true;
			this.startstop_button.Click += new System.EventHandler(this.startstop_button_Click);
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenu;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "DNSCrypt Proxy Client";
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// notifyIconContextMenu
			// 
			this.notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyIconContextOpen,
            this.notifyIconContextExit});
			this.notifyIconContextMenu.Name = "notifyIconContextMenu";
			this.notifyIconContextMenu.Size = new System.Drawing.Size(104, 48);
			// 
			// notifyIconContextOpen
			// 
			this.notifyIconContextOpen.Name = "notifyIconContextOpen";
			this.notifyIconContextOpen.Size = new System.Drawing.Size(103, 22);
			this.notifyIconContextOpen.Text = "Open";
			this.notifyIconContextOpen.Click += new System.EventHandler(this.notifyIcon_Open);
			// 
			// notifyIconContextExit
			// 
			this.notifyIconContextExit.Name = "notifyIconContextExit";
			this.notifyIconContextExit.Size = new System.Drawing.Size(103, 22);
			this.notifyIconContextExit.Text = "Exit";
			this.notifyIconContextExit.Click += new System.EventHandler(this.notifyIcon_Exit);
			// 
			// portBox
			// 
			this.portBox.FormattingEnabled = true;
			this.portBox.Items.AddRange(new object[] {
            "53",
            "443",
            "5353"});
			this.portBox.Location = new System.Drawing.Point(337, 114);
			this.portBox.Name = "portBox";
			this.portBox.Size = new System.Drawing.Size(59, 21);
			this.portBox.TabIndex = 6;
			// 
			// protoUDP
			// 
			this.protoUDP.AutoSize = true;
			this.protoUDP.Checked = true;
			this.protoUDP.Location = new System.Drawing.Point(3, 4);
			this.protoUDP.Name = "protoUDP";
			this.protoUDP.Size = new System.Drawing.Size(48, 17);
			this.protoUDP.TabIndex = 7;
			this.protoUDP.TabStop = true;
			this.protoUDP.Text = "UDP";
			this.protoUDP.UseVisualStyleBackColor = true;
			// 
			// protoTCP
			// 
			this.protoTCP.AutoSize = true;
			this.protoTCP.Location = new System.Drawing.Point(51, 4);
			this.protoTCP.Name = "protoTCP";
			this.protoTCP.Size = new System.Drawing.Size(46, 17);
			this.protoTCP.TabIndex = 8;
			this.protoTCP.Text = "TCP";
			this.protoTCP.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(302, 117);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Port:";
			// 
			// hideAdaptersCheckbox
			// 
			this.hideAdaptersCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hideAdaptersCheckbox.AutoSize = true;
			this.hideAdaptersCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.hideAdaptersCheckbox.Location = new System.Drawing.Point(266, 176);
			this.hideAdaptersCheckbox.Name = "hideAdaptersCheckbox";
			this.hideAdaptersCheckbox.Size = new System.Drawing.Size(132, 17);
			this.hideAdaptersCheckbox.TabIndex = 11;
			this.hideAdaptersCheckbox.Text = "Show hidden adapters";
			this.hideAdaptersCheckbox.UseVisualStyleBackColor = true;
			this.hideAdaptersCheckbox.CheckedChanged += new System.EventHandler(this.refreshNICList);
			// 
			// gatewayCheckbox
			// 
			this.gatewayCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gatewayCheckbox.AutoSize = true;
			this.gatewayCheckbox.Location = new System.Drawing.Point(6, 149);
			this.gatewayCheckbox.Name = "gatewayCheckbox";
			this.gatewayCheckbox.Size = new System.Drawing.Size(143, 17);
			this.gatewayCheckbox.TabIndex = 13;
			this.gatewayCheckbox.Text = "Act as a gateway device";
			this.gatewayCheckbox.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabNICs);
			this.tabControl1.Controls.Add(this.tabConfig);
			this.tabControl1.Controls.Add(this.tabPlugins);
			this.tabControl1.Controls.Add(this.tabAbout);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(412, 221);
			this.tabControl1.TabIndex = 14;
			// 
			// tabNICs
			// 
			this.tabNICs.Controls.Add(this.label1);
			this.tabNICs.Controls.Add(this.DNSlistbox);
			this.tabNICs.Controls.Add(this.hideAdaptersCheckbox);
			this.tabNICs.Location = new System.Drawing.Point(4, 22);
			this.tabNICs.Name = "tabNICs";
			this.tabNICs.Padding = new System.Windows.Forms.Padding(3);
			this.tabNICs.Size = new System.Drawing.Size(404, 195);
			this.tabNICs.TabIndex = 0;
			this.tabNICs.Text = "NICs";
			this.tabNICs.UseVisualStyleBackColor = true;
			// 
			// tabConfig
			// 
			this.tabConfig.Controls.Add(this.button_save);
			this.tabConfig.Controls.Add(this.label6);
			this.tabConfig.Controls.Add(this.combobox_provider);
			this.tabConfig.Controls.Add(this.label5);
			this.tabConfig.Controls.Add(this.label4);
			this.tabConfig.Controls.Add(this.textbox_key);
			this.tabConfig.Controls.Add(this.panel2);
			this.tabConfig.Controls.Add(this.textbox_fqdn);
			this.tabConfig.Controls.Add(this.label2);
			this.tabConfig.Controls.Add(this.textbox_server_addr);
			this.tabConfig.Controls.Add(this.autostartCheckBox);
			this.tabConfig.Controls.Add(this.panel1);
			this.tabConfig.Controls.Add(this.portBox);
			this.tabConfig.Controls.Add(this.gatewayCheckbox);
			this.tabConfig.Controls.Add(this.label3);
			this.tabConfig.Location = new System.Drawing.Point(4, 22);
			this.tabConfig.Name = "tabConfig";
			this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
			this.tabConfig.Size = new System.Drawing.Size(404, 195);
			this.tabConfig.TabIndex = 2;
			this.tabConfig.Text = "Config";
			this.tabConfig.UseVisualStyleBackColor = true;
			// 
			// button_save
			// 
			this.button_save.Location = new System.Drawing.Point(6, 36);
			this.button_save.Name = "button_save";
			this.button_save.Size = new System.Drawing.Size(75, 23);
			this.button_save.TabIndex = 27;
			this.button_save.Text = "Save";
			this.button_save.UseVisualStyleBackColor = true;
			this.button_save.Click += new System.EventHandler(this.button_save_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(139, 91);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 13);
			this.label6.TabIndex = 25;
			this.label6.Text = "Public Key:";
			// 
			// combobox_provider
			// 
			this.combobox_provider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combobox_provider.FormattingEnabled = true;
			this.combobox_provider.Location = new System.Drawing.Point(6, 9);
			this.combobox_provider.Name = "combobox_provider";
			this.combobox_provider.Size = new System.Drawing.Size(111, 21);
			this.combobox_provider.TabIndex = 26;
			this.combobox_provider.SelectionChangeCommitted += new System.EventHandler(this.combobox_provider_SelectionChangeCommitted);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(122, 65);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(77, 13);
			this.label5.TabIndex = 24;
			this.label5.Text = "Name (FQDN):";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(253, 148);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(49, 13);
			this.label4.TabIndex = 19;
			this.label4.Text = "Protocol:";
			// 
			// textbox_key
			// 
			this.textbox_key.Location = new System.Drawing.Point(205, 88);
			this.textbox_key.Name = "textbox_key";
			this.textbox_key.Size = new System.Drawing.Size(191, 20);
			this.textbox_key.TabIndex = 23;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.protoUDP);
			this.panel2.Controls.Add(this.protoTCP);
			this.panel2.Location = new System.Drawing.Point(305, 141);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(93, 27);
			this.panel2.TabIndex = 18;
			// 
			// textbox_fqdn
			// 
			this.textbox_fqdn.Location = new System.Drawing.Point(205, 62);
			this.textbox_fqdn.Name = "textbox_fqdn";
			this.textbox_fqdn.Size = new System.Drawing.Size(191, 20);
			this.textbox_fqdn.TabIndex = 22;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(117, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "Server Address:";
			// 
			// textbox_server_addr
			// 
			this.textbox_server_addr.Location = new System.Drawing.Point(205, 36);
			this.textbox_server_addr.Name = "textbox_server_addr";
			this.textbox_server_addr.Size = new System.Drawing.Size(191, 20);
			this.textbox_server_addr.TabIndex = 20;
			// 
			// autostartCheckBox
			// 
			this.autostartCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.autostartCheckBox.AutoSize = true;
			this.autostartCheckBox.Checked = true;
			this.autostartCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autostartCheckBox.Enabled = false;
			this.autostartCheckBox.Location = new System.Drawing.Point(6, 172);
			this.autostartCheckBox.Name = "autostartCheckBox";
			this.autostartCheckBox.Size = new System.Drawing.Size(189, 17);
			this.autostartCheckBox.TabIndex = 15;
			this.autostartCheckBox.Text = "Start service when Windows starts";
			this.autostartCheckBox.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.ipv4Radio);
			this.panel1.Controls.Add(this.ipv6Radio);
			this.panel1.Controls.Add(this.parentalControlsRadio);
			this.panel1.Location = new System.Drawing.Point(178, 6);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(218, 24);
			this.panel1.TabIndex = 16;
			// 
			// ipv4Radio
			// 
			this.ipv4Radio.AutoSize = true;
			this.ipv4Radio.Checked = true;
			this.ipv4Radio.Location = new System.Drawing.Point(4, 3);
			this.ipv4Radio.Name = "ipv4Radio";
			this.ipv4Radio.Size = new System.Drawing.Size(47, 17);
			this.ipv4Radio.TabIndex = 0;
			this.ipv4Radio.TabStop = true;
			this.ipv4Radio.Text = "IPv4";
			this.ipv4Radio.UseVisualStyleBackColor = true;
			this.ipv4Radio.CheckedChanged += new System.EventHandler(this.ipRadios_CheckedChanged);
			// 
			// ipv6Radio
			// 
			this.ipv6Radio.AutoSize = true;
			this.ipv6Radio.Location = new System.Drawing.Point(57, 3);
			this.ipv6Radio.Name = "ipv6Radio";
			this.ipv6Radio.Size = new System.Drawing.Size(47, 17);
			this.ipv6Radio.TabIndex = 1;
			this.ipv6Radio.Text = "IPv6";
			this.ipv6Radio.UseVisualStyleBackColor = true;
			this.ipv6Radio.CheckedChanged += new System.EventHandler(this.ipRadios_CheckedChanged);
			// 
			// parentalControlsRadio
			// 
			this.parentalControlsRadio.AutoSize = true;
			this.parentalControlsRadio.Location = new System.Drawing.Point(110, 3);
			this.parentalControlsRadio.Name = "parentalControlsRadio";
			this.parentalControlsRadio.Size = new System.Drawing.Size(104, 17);
			this.parentalControlsRadio.TabIndex = 2;
			this.parentalControlsRadio.Text = "Parental controls";
			this.parentalControlsRadio.UseVisualStyleBackColor = true;
			this.parentalControlsRadio.CheckedChanged += new System.EventHandler(this.ipRadios_CheckedChanged);
			// 
			// tabPlugins
			// 
			this.tabPlugins.Controls.Add(this.button_plugins_refresh);
			this.tabPlugins.Controls.Add(this.pluginDescriptionTextBox);
			this.tabPlugins.Controls.Add(this.pluginListBox);
			this.tabPlugins.Controls.Add(this.label8);
			this.tabPlugins.Controls.Add(this.label7);
			this.tabPlugins.Controls.Add(this.textbox_plugin_params);
			this.tabPlugins.Location = new System.Drawing.Point(4, 22);
			this.tabPlugins.Name = "tabPlugins";
			this.tabPlugins.Padding = new System.Windows.Forms.Padding(3);
			this.tabPlugins.Size = new System.Drawing.Size(404, 195);
			this.tabPlugins.TabIndex = 1;
			this.tabPlugins.Text = "Plugins";
			this.tabPlugins.UseVisualStyleBackColor = true;
			// 
			// button_plugins_refresh
			// 
			this.button_plugins_refresh.BackgroundImage = global::dnscrypt_winclient.Properties.Resources.refresh;
			this.button_plugins_refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_plugins_refresh.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.button_plugins_refresh.Location = new System.Drawing.Point(92, 3);
			this.button_plugins_refresh.Margin = new System.Windows.Forms.Padding(0);
			this.button_plugins_refresh.Name = "button_plugins_refresh";
			this.button_plugins_refresh.Size = new System.Drawing.Size(18, 16);
			this.button_plugins_refresh.TabIndex = 6;
			this.button_plugins_refresh.UseVisualStyleBackColor = true;
			// 
			// pluginDescriptionTextBox
			// 
			this.pluginDescriptionTextBox.Location = new System.Drawing.Point(9, 107);
			this.pluginDescriptionTextBox.Multiline = true;
			this.pluginDescriptionTextBox.Name = "pluginDescriptionTextBox";
			this.pluginDescriptionTextBox.ReadOnly = true;
			this.pluginDescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.pluginDescriptionTextBox.Size = new System.Drawing.Size(389, 57);
			this.pluginDescriptionTextBox.TabIndex = 5;
			// 
			// pluginListBox
			// 
			this.pluginListBox.FormattingEnabled = true;
			this.pluginListBox.Location = new System.Drawing.Point(9, 21);
			this.pluginListBox.Name = "pluginListBox";
			this.pluginListBox.Size = new System.Drawing.Size(389, 79);
			this.pluginListBox.TabIndex = 4;
			this.pluginListBox.SelectedIndexChanged += new System.EventHandler(this.pluginListBox_SelectedIndexChanged);
			this.pluginListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pluginListBox_MouseMove);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 4);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(86, 13);
			this.label8.TabIndex = 3;
			this.label8.Text = "Available plugins";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 173);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(63, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "Parameters:";
			// 
			// textbox_plugin_params
			// 
			this.textbox_plugin_params.Location = new System.Drawing.Point(72, 170);
			this.textbox_plugin_params.Name = "textbox_plugin_params";
			this.textbox_plugin_params.Size = new System.Drawing.Size(326, 20);
			this.textbox_plugin_params.TabIndex = 1;
			this.textbox_plugin_params.Leave += new System.EventHandler(this.textbox_plugin_params_Leave);
			// 
			// tabAbout
			// 
			this.tabAbout.Controls.Add(this.richTextBox1);
			this.tabAbout.Location = new System.Drawing.Point(4, 22);
			this.tabAbout.Name = "tabAbout";
			this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
			this.tabAbout.Size = new System.Drawing.Size(404, 195);
			this.tabAbout.TabIndex = 3;
			this.tabAbout.Text = "About";
			this.tabAbout.UseVisualStyleBackColor = true;
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
			this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.Location = new System.Drawing.Point(4, 7);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.richTextBox1.Size = new System.Drawing.Size(394, 143);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
			// 
			// buttonInstall
			// 
			this.buttonInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonInstall.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonInstall.Location = new System.Drawing.Point(12, 239);
			this.buttonInstall.Name = "buttonInstall";
			this.buttonInstall.Size = new System.Drawing.Size(75, 23);
			this.buttonInstall.TabIndex = 15;
			this.buttonInstall.Text = "Install";
			this.buttonInstall.UseVisualStyleBackColor = true;
			this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
			this.buttonInstall.MouseHover += new System.EventHandler(this.buttonInstall_MouseHover);
			// 
			// ApplicationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 265);
			this.Controls.Add(this.buttonInstall);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.startstop_button);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ApplicationForm";
			this.Text = "DNSCrypt Proxy Client";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_closing);
			this.Resize += new System.EventHandler(this.form_resized);
			this.notifyIconContextMenu.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabNICs.ResumeLayout(false);
			this.tabNICs.PerformLayout();
			this.tabConfig.ResumeLayout(false);
			this.tabConfig.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabPlugins.ResumeLayout(false);
			this.tabPlugins.PerformLayout();
			this.tabAbout.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox DNSlistbox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button startstop_button;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip notifyIconContextMenu;
		private System.Windows.Forms.ToolStripMenuItem notifyIconContextOpen;
		private System.Windows.Forms.ToolStripMenuItem notifyIconContextExit;
		private System.Windows.Forms.ComboBox portBox;
		private System.Windows.Forms.RadioButton protoUDP;
		private System.Windows.Forms.RadioButton protoTCP;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox hideAdaptersCheckbox;
		private System.Windows.Forms.CheckBox gatewayCheckbox;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabNICs;
		private System.Windows.Forms.TabPage tabPlugins;
		private System.Windows.Forms.TabPage tabConfig;
		private System.Windows.Forms.Button buttonInstall;
		private System.Windows.Forms.ToolTip serviceTooltip;
		private System.Windows.Forms.CheckBox autostartCheckBox;
		private System.Windows.Forms.TabPage tabAbout;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textbox_plugin_params;
		private System.Windows.Forms.CheckedListBox pluginListBox;
		private System.Windows.Forms.TextBox pluginDescriptionTextBox;
		private System.Windows.Forms.Button button_plugins_refresh;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textbox_key;
		private System.Windows.Forms.TextBox textbox_fqdn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textbox_server_addr;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton ipv4Radio;
		private System.Windows.Forms.RadioButton ipv6Radio;
		private System.Windows.Forms.RadioButton parentalControlsRadio;
		private System.Windows.Forms.ComboBox combobox_provider;
		private System.Windows.Forms.Button button_save;
		private System.Windows.Forms.ToolTip tooltip_plugin;
	}
}

