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
			this.service_label = new System.Windows.Forms.Label();
			this.service_button = new System.Windows.Forms.Button();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.tcp_checkbox = new System.Windows.Forms.CheckBox();
			this.tcp_port_number = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// DNSlistbox
			// 
			this.DNSlistbox.CheckOnClick = true;
			this.DNSlistbox.FormattingEnabled = true;
			this.DNSlistbox.HorizontalScrollbar = true;
			this.DNSlistbox.Location = new System.Drawing.Point(12, 25);
			this.DNSlistbox.Name = "DNSlistbox";
			this.DNSlistbox.Size = new System.Drawing.Size(521, 109);
			this.DNSlistbox.TabIndex = 0;
			this.DNSlistbox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.DNSlisbox_itemcheck_statechanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(225, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select network devices for use with DNSCrypt";
			// 
			// service_label
			// 
			this.service_label.AutoSize = true;
			this.service_label.Location = new System.Drawing.Point(93, 173);
			this.service_label.Name = "service_label";
			this.service_label.Size = new System.Drawing.Size(128, 13);
			this.service_label.TabIndex = 2;
			this.service_label.Text = "DNSCrypt is NOT running";
			// 
			// service_button
			// 
			this.service_button.Location = new System.Drawing.Point(12, 168);
			this.service_button.Name = "service_button";
			this.service_button.Size = new System.Drawing.Size(75, 23);
			this.service_button.TabIndex = 3;
			this.service_button.Text = "Start";
			this.service_button.UseVisualStyleBackColor = true;
			this.service_button.Click += new System.EventHandler(this.service_button_Click);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "DNSCrypt Proxy Client";
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// tcp_checkbox
			// 
			this.tcp_checkbox.AutoSize = true;
			this.tcp_checkbox.Location = new System.Drawing.Point(12, 141);
			this.tcp_checkbox.Name = "tcp_checkbox";
			this.tcp_checkbox.Size = new System.Drawing.Size(96, 17);
			this.tcp_checkbox.TabIndex = 4;
			this.tcp_checkbox.Text = "Use TCP port: ";
			this.tcp_checkbox.UseVisualStyleBackColor = true;
			// 
			// tcp_port_number
			// 
			this.tcp_port_number.Location = new System.Drawing.Point(104, 141);
			this.tcp_port_number.MaxLength = 5;
			this.tcp_port_number.Name = "tcp_port_number";
			this.tcp_port_number.Size = new System.Drawing.Size(51, 20);
			this.tcp_port_number.TabIndex = 5;
			this.tcp_port_number.Text = "443";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 197);
			this.Controls.Add(this.tcp_port_number);
			this.Controls.Add(this.tcp_checkbox);
			this.Controls.Add(this.service_button);
			this.Controls.Add(this.service_label);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.DNSlistbox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "DNSCrypt Proxy Client";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_closing);
			this.Resize += new System.EventHandler(this.form_resized);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox DNSlistbox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label service_label;
		private System.Windows.Forms.Button service_button;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.CheckBox tcp_checkbox;
		private System.Windows.Forms.TextBox tcp_port_number;
	}
}

