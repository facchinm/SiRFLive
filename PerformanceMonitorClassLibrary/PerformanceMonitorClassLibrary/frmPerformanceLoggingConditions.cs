﻿namespace PerformanceMonitorClassLibrary
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmPerformanceLoggingConditions : Form
    {
        private Button button_Cancel;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private CheckBox PhysicalMemoryCheckBox;
        private NumericUpDown PhysMemoryUsageNumericUpDown;
        private frmPerformanceMonitor PMF;
        private Button SetLoggingTriggersButton;
        private CheckBox SiRFLiveCPUCheckBox;
        private NumericUpDown SiRFLiveCPUUsageNumericUpDown;
        private CheckBox SiRFLiveMemoryUsageCheckBox;
        private NumericUpDown SiRFLiveMemoryUsageNumericUpDown;
        private CheckBox TimeIntervalCheckBox;
        private NumericUpDown TimeIntervalNumericUpDown;
        private CheckBox TotalCPUUsageCheckBox;
        private NumericUpDown TotalCPUUsageNumericUpDown;
        private CheckBox VirtualmemoryUsageCheckBox;
        private NumericUpDown VirtualMemoryUsageNumericUpDown;

        public frmPerformanceLoggingConditions()
        {
            this.InitializeComponent();
            this.PMF = new frmPerformanceMonitor();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmPerformanceLoggingConditions));
            this.SetLoggingTriggersButton = new Button();
            this.TimeIntervalNumericUpDown = new NumericUpDown();
            this.label1 = new Label();
            this.TotalCPUUsageNumericUpDown = new NumericUpDown();
            this.label2 = new Label();
            this.label3 = new Label();
            this.SiRFLiveCPUUsageNumericUpDown = new NumericUpDown();
            this.label4 = new Label();
            this.PhysMemoryUsageNumericUpDown = new NumericUpDown();
            this.label5 = new Label();
            this.SiRFLiveMemoryUsageNumericUpDown = new NumericUpDown();
            this.label6 = new Label();
            this.VirtualMemoryUsageNumericUpDown = new NumericUpDown();
            this.TimeIntervalCheckBox = new CheckBox();
            this.TotalCPUUsageCheckBox = new CheckBox();
            this.SiRFLiveCPUCheckBox = new CheckBox();
            this.PhysicalMemoryCheckBox = new CheckBox();
            this.SiRFLiveMemoryUsageCheckBox = new CheckBox();
            this.VirtualmemoryUsageCheckBox = new CheckBox();
            this.button_Cancel = new Button();
            this.groupBox1 = new GroupBox();
            this.groupBox2 = new GroupBox();
            this.TimeIntervalNumericUpDown.BeginInit();
            this.TotalCPUUsageNumericUpDown.BeginInit();
            this.SiRFLiveCPUUsageNumericUpDown.BeginInit();
            this.PhysMemoryUsageNumericUpDown.BeginInit();
            this.SiRFLiveMemoryUsageNumericUpDown.BeginInit();
            this.VirtualMemoryUsageNumericUpDown.BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.SetLoggingTriggersButton.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.SetLoggingTriggersButton.Location = new Point(0x26, 0x143);
            this.SetLoggingTriggersButton.Name = "SetLoggingTriggersButton";
            this.SetLoggingTriggersButton.Size = new Size(0x8f, 0x1f);
            this.SetLoggingTriggersButton.TabIndex = 0;
            this.SetLoggingTriggersButton.Text = "Set Logging Triggers";
            this.SetLoggingTriggersButton.UseVisualStyleBackColor = true;
            this.SetLoggingTriggersButton.Click += new EventHandler(this.SetLoggingConditionsButton_Click);
            this.TimeIntervalNumericUpDown.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.TimeIntervalNumericUpDown.Location = new Point(0xbd, 0x1b);
            int[] bits = new int[4];
            bits[0] = 0x270f;
            this.TimeIntervalNumericUpDown.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.TimeIntervalNumericUpDown.Minimum = new decimal(numArray2);
            this.TimeIntervalNumericUpDown.Name = "TimeIntervalNumericUpDown";
            this.TimeIntervalNumericUpDown.Size = new Size(0x33, 0x16);
            this.TimeIntervalNumericUpDown.TabIndex = 3;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.TimeIntervalNumericUpDown.Value = new decimal(numArray3);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0xf5, 0x1d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(60, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "seconds";
            this.TotalCPUUsageNumericUpDown.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.TotalCPUUsageNumericUpDown.Location = new Point(0xb0, 0x19);
            int[] numArray4 = new int[4];
            numArray4[0] = 1;
            this.TotalCPUUsageNumericUpDown.Minimum = new decimal(numArray4);
            this.TotalCPUUsageNumericUpDown.Name = "TotalCPUUsageNumericUpDown";
            this.TotalCPUUsageNumericUpDown.Size = new Size(0x33, 0x16);
            this.TotalCPUUsageNumericUpDown.TabIndex = 6;
            int[] numArray5 = new int[4];
            numArray5[0] = 0x4b;
            this.TotalCPUUsageNumericUpDown.Value = new decimal(numArray5);
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0xe8, 0x1c);
            this.label2.Name = "label2";
            this.label2.Size = new Size(20, 0x10);
            this.label2.TabIndex = 7;
            this.label2.Text = "%";
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0xe8, 0x38);
            this.label3.Name = "label3";
            this.label3.Size = new Size(20, 0x10);
            this.label3.TabIndex = 10;
            this.label3.Text = "%";
            this.SiRFLiveCPUUsageNumericUpDown.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.SiRFLiveCPUUsageNumericUpDown.Location = new Point(0xb0, 0x35);
            int[] numArray6 = new int[4];
            numArray6[0] = 1;
            this.SiRFLiveCPUUsageNumericUpDown.Minimum = new decimal(numArray6);
            this.SiRFLiveCPUUsageNumericUpDown.Name = "SiRFLiveCPUUsageNumericUpDown";
            this.SiRFLiveCPUUsageNumericUpDown.Size = new Size(0x33, 0x16);
            this.SiRFLiveCPUUsageNumericUpDown.TabIndex = 9;
            int[] numArray7 = new int[4];
            numArray7[0] = 50;
            this.SiRFLiveCPUUsageNumericUpDown.Value = new decimal(numArray7);
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0xe8, 0x1d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(20, 0x10);
            this.label4.TabIndex = 13;
            this.label4.Text = "%";
            this.PhysMemoryUsageNumericUpDown.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.PhysMemoryUsageNumericUpDown.Location = new Point(0xb0, 0x1a);
            int[] numArray8 = new int[4];
            numArray8[0] = 1;
            this.PhysMemoryUsageNumericUpDown.Minimum = new decimal(numArray8);
            this.PhysMemoryUsageNumericUpDown.Name = "PhysMemoryUsageNumericUpDown";
            this.PhysMemoryUsageNumericUpDown.Size = new Size(0x33, 0x16);
            this.PhysMemoryUsageNumericUpDown.TabIndex = 12;
            int[] numArray9 = new int[4];
            numArray9[0] = 50;
            this.PhysMemoryUsageNumericUpDown.Value = new decimal(numArray9);
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(0xe8, 0x39);
            this.label5.Name = "label5";
            this.label5.Size = new Size(20, 0x10);
            this.label5.TabIndex = 0x10;
            this.label5.Text = "%";
            this.SiRFLiveMemoryUsageNumericUpDown.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.SiRFLiveMemoryUsageNumericUpDown.Location = new Point(0xb0, 0x36);
            int[] numArray10 = new int[4];
            numArray10[0] = 1;
            this.SiRFLiveMemoryUsageNumericUpDown.Minimum = new decimal(numArray10);
            this.SiRFLiveMemoryUsageNumericUpDown.Name = "SiRFLiveMemoryUsageNumericUpDown";
            this.SiRFLiveMemoryUsageNumericUpDown.Size = new Size(0x33, 0x16);
            this.SiRFLiveMemoryUsageNumericUpDown.TabIndex = 15;
            int[] numArray11 = new int[4];
            numArray11[0] = 50;
            this.SiRFLiveMemoryUsageNumericUpDown.Value = new decimal(numArray11);
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label6.Location = new Point(0xe8, 0x55);
            this.label6.Name = "label6";
            this.label6.Size = new Size(20, 0x10);
            this.label6.TabIndex = 0x13;
            this.label6.Text = "%";
            this.VirtualMemoryUsageNumericUpDown.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.VirtualMemoryUsageNumericUpDown.Location = new Point(0xb0, 0x52);
            int[] numArray12 = new int[4];
            numArray12[0] = 1;
            this.VirtualMemoryUsageNumericUpDown.Minimum = new decimal(numArray12);
            this.VirtualMemoryUsageNumericUpDown.Name = "VirtualMemoryUsageNumericUpDown";
            this.VirtualMemoryUsageNumericUpDown.Size = new Size(0x33, 0x16);
            this.VirtualMemoryUsageNumericUpDown.TabIndex = 0x12;
            int[] numArray13 = new int[4];
            numArray13[0] = 50;
            this.VirtualMemoryUsageNumericUpDown.Value = new decimal(numArray13);
            this.TimeIntervalCheckBox.AutoSize = true;
            this.TimeIntervalCheckBox.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.TimeIntervalCheckBox.Location = new Point(0x23, 0x1c);
            this.TimeIntervalCheckBox.Name = "TimeIntervalCheckBox";
            this.TimeIntervalCheckBox.Size = new Size(0x68, 20);
            this.TimeIntervalCheckBox.TabIndex = 20;
            this.TimeIntervalCheckBox.Text = "Time Interval";
            this.TimeIntervalCheckBox.UseVisualStyleBackColor = true;
            this.TotalCPUUsageCheckBox.AutoSize = true;
            this.TotalCPUUsageCheckBox.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.TotalCPUUsageCheckBox.Location = new Point(0x15, 0x1a);
            this.TotalCPUUsageCheckBox.Name = "TotalCPUUsageCheckBox";
            this.TotalCPUUsageCheckBox.Size = new Size(0x66, 20);
            this.TotalCPUUsageCheckBox.TabIndex = 0x15;
            this.TotalCPUUsageCheckBox.Text = "Total Usage";
            this.TotalCPUUsageCheckBox.UseVisualStyleBackColor = true;
            this.SiRFLiveCPUCheckBox.AutoSize = true;
            this.SiRFLiveCPUCheckBox.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.SiRFLiveCPUCheckBox.Location = new Point(0x15, 0x36);
            this.SiRFLiveCPUCheckBox.Name = "SiRFLiveCPUCheckBox";
            this.SiRFLiveCPUCheckBox.Size = new Size(0x7e, 20);
            this.SiRFLiveCPUCheckBox.TabIndex = 0x16;
            this.SiRFLiveCPUCheckBox.Text = "SiRFLive Usage";
            this.SiRFLiveCPUCheckBox.UseVisualStyleBackColor = true;
            this.PhysicalMemoryCheckBox.AutoSize = true;
            this.PhysicalMemoryCheckBox.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.PhysicalMemoryCheckBox.Location = new Point(0x12, 0x1b);
            this.PhysicalMemoryCheckBox.Name = "PhysicalMemoryCheckBox";
            this.PhysicalMemoryCheckBox.Size = new Size(0x7a, 20);
            this.PhysicalMemoryCheckBox.TabIndex = 0x17;
            this.PhysicalMemoryCheckBox.Text = "Physical Usage";
            this.PhysicalMemoryCheckBox.UseVisualStyleBackColor = true;
            this.SiRFLiveMemoryUsageCheckBox.AutoSize = true;
            this.SiRFLiveMemoryUsageCheckBox.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.SiRFLiveMemoryUsageCheckBox.Location = new Point(0x12, 0x37);
            this.SiRFLiveMemoryUsageCheckBox.Name = "SiRFLiveMemoryUsageCheckBox";
            this.SiRFLiveMemoryUsageCheckBox.Size = new Size(0x7e, 20);
            this.SiRFLiveMemoryUsageCheckBox.TabIndex = 0x18;
            this.SiRFLiveMemoryUsageCheckBox.Text = "SiRFLive Usage";
            this.SiRFLiveMemoryUsageCheckBox.UseVisualStyleBackColor = true;
            this.VirtualmemoryUsageCheckBox.AutoSize = true;
            this.VirtualmemoryUsageCheckBox.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.VirtualmemoryUsageCheckBox.Location = new Point(0x12, 0x53);
            this.VirtualmemoryUsageCheckBox.Name = "VirtualmemoryUsageCheckBox";
            this.VirtualmemoryUsageCheckBox.Size = new Size(0x6c, 20);
            this.VirtualmemoryUsageCheckBox.TabIndex = 0x19;
            this.VirtualmemoryUsageCheckBox.Text = "Virtual Usage";
            this.VirtualmemoryUsageCheckBox.UseVisualStyleBackColor = true;
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.button_Cancel.Location = new Point(0xd9, 0x143);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(0x3a, 0x1f);
            this.button_Cancel.TabIndex = 0x1a;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
            this.groupBox1.Controls.Add(this.TotalCPUUsageCheckBox);
            this.groupBox1.Controls.Add(this.TotalCPUUsageNumericUpDown);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SiRFLiveCPUCheckBox);
            this.groupBox1.Controls.Add(this.SiRFLiveCPUUsageNumericUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupBox1.Location = new Point(13, 0x3e);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x125, 0x5f);
            this.groupBox1.TabIndex = 0x1b;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CPU";
            this.groupBox2.Controls.Add(this.PhysMemoryUsageNumericUpDown);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.SiRFLiveMemoryUsageNumericUpDown);
            this.groupBox2.Controls.Add(this.VirtualmemoryUsageCheckBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.SiRFLiveMemoryUsageCheckBox);
            this.groupBox2.Controls.Add(this.VirtualMemoryUsageNumericUpDown);
            this.groupBox2.Controls.Add(this.PhysicalMemoryCheckBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupBox2.Location = new Point(13, 0xac);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x125, 0x79);
            this.groupBox2.TabIndex = 0x1c;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Memory";
            base.AcceptButton = this.SetLoggingTriggersButton;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(0x143, 0x182);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.button_Cancel);
            base.Controls.Add(this.TimeIntervalCheckBox);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.TimeIntervalNumericUpDown);
            base.Controls.Add(this.SetLoggingTriggersButton);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "frmPerformanceLoggingConditions";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Performance Logging Triggers";
            base.Load += new EventHandler(this.PerformanceLoggingConditionsForm_Load);
            this.TimeIntervalNumericUpDown.EndInit();
            this.TotalCPUUsageNumericUpDown.EndInit();
            this.SiRFLiveCPUUsageNumericUpDown.EndInit();
            this.PhysMemoryUsageNumericUpDown.EndInit();
            this.SiRFLiveMemoryUsageNumericUpDown.EndInit();
            this.VirtualMemoryUsageNumericUpDown.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PerformanceLoggingConditionsForm_Load(object sender, EventArgs e)
        {
            this.TimeIntervalCheckBox.CheckState = this.PMF.PerfTrigger_TimeInterval ? CheckState.Checked : CheckState.Unchecked;
            this.TimeIntervalNumericUpDown.Value = this.PMF.TimeInterval;
            this.TotalCPUUsageCheckBox.CheckState = this.PMF.PerfTrigger_TotalCPUUsage ? CheckState.Checked : CheckState.Unchecked;
            this.TotalCPUUsageNumericUpDown.Value = (decimal) this.PMF.TotalCPUUsagePercentage;
            this.SiRFLiveCPUCheckBox.CheckState = this.PMF.PerfTRigger_SiRFLiveCPUUsage ? CheckState.Checked : CheckState.Unchecked;
            this.SiRFLiveCPUUsageNumericUpDown.Value = (decimal) this.PMF.SiRFLiveCPUUsagePercentage;
            this.PhysicalMemoryCheckBox.CheckState = this.PMF.PerfTrigger_TotalPhysMemory ? CheckState.Checked : CheckState.Unchecked;
            this.PhysMemoryUsageNumericUpDown.Value = (decimal) this.PMF.PhysicalMemoryUsagePercentage;
            this.SiRFLiveMemoryUsageCheckBox.CheckState = this.PMF.PerfTrigger_SiRFLivePhysMemory ? CheckState.Checked : CheckState.Unchecked;
            this.SiRFLiveMemoryUsageNumericUpDown.Value = (decimal) this.PMF.SiRFLivePhysicalMemoryUsagePercentage;
            this.VirtualmemoryUsageCheckBox.CheckState = this.PMF.PerfTrigger_VirtualMemoryUsage ? CheckState.Checked : CheckState.Unchecked;
            this.VirtualMemoryUsageNumericUpDown.Value = (decimal) this.PMF.VirtualMemoryUsagePercentage;
        }

        private void SetLoggingConditionsButton_Click(object sender, EventArgs e)
        {
            this.PMF.PerfTrigger_TimeInterval = this.TimeIntervalCheckBox.CheckState == CheckState.Checked;
            this.PMF.TimeInterval = (uint) this.TimeIntervalNumericUpDown.Value;
            this.PMF.PerfTrigger_TotalCPUUsage = this.TotalCPUUsageCheckBox.CheckState == CheckState.Checked;
            this.PMF.TotalCPUUsagePercentage = (double) this.TotalCPUUsageNumericUpDown.Value;
            this.PMF.PerfTRigger_SiRFLiveCPUUsage = this.SiRFLiveCPUCheckBox.CheckState == CheckState.Checked;
            this.PMF.SiRFLiveCPUUsagePercentage = (double) this.SiRFLiveCPUUsageNumericUpDown.Value;
            this.PMF.PerfTrigger_TotalPhysMemory = this.PhysicalMemoryCheckBox.CheckState == CheckState.Checked;
            this.PMF.PhysicalMemoryUsagePercentage = (double) this.PhysMemoryUsageNumericUpDown.Value;
            this.PMF.PerfTrigger_SiRFLivePhysMemory = this.SiRFLiveMemoryUsageCheckBox.CheckState == CheckState.Checked;
            this.PMF.SiRFLivePhysicalMemoryUsagePercentage = (double) this.SiRFLiveMemoryUsageNumericUpDown.Value;
            this.PMF.PerfTrigger_VirtualMemoryUsage = this.VirtualmemoryUsageCheckBox.CheckState == CheckState.Checked;
            this.PMF.VirtualMemoryUsagePercentage = (double) this.VirtualMemoryUsageNumericUpDown.Value;
            if (((!this.PMF.PerfTrigger_TimeInterval && !this.PMF.PerfTrigger_TotalCPUUsage) && (!this.PMF.PerfTRigger_SiRFLiveCPUUsage && !this.PMF.PerfTrigger_TotalPhysMemory)) && (!this.PMF.PerfTrigger_SiRFLivePhysMemory && !this.PMF.PerfTrigger_VirtualMemoryUsage))
            {
                MessageBox.Show("At least one checkbox must be checked");
            }
            else
            {
                frmPerformanceMonitor.SavePerformanceMonitorParameters();
                base.Close();
            }
        }
    }
}

