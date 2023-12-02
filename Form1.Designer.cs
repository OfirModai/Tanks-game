namespace TanksGame
{
    partial class Form1
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.resetBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.testingTable = new System.Windows.Forms.DataGridView();
            this.playerProgressBar = new System.Windows.Forms.ProgressBar();
            this.leftBorder = new System.Windows.Forms.PictureBox();
            this.rightBorder = new System.Windows.Forms.PictureBox();
            this.topBorder = new System.Windows.Forms.PictureBox();
            this.bottomBorder = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.testingTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomBorder)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // resetBtn
            // 
            this.resetBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.resetBtn.ForeColor = System.Drawing.Color.Black;
            this.resetBtn.Location = new System.Drawing.Point(0, 0);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(600, 70);
            this.resetBtn.TabIndex = 3;
            this.resetBtn.Tag = "btn";
            this.resetBtn.Text = "reset battelfield";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.resetBtn_MouseClick);
            // 
            // startBtn
            // 
            this.startBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.startBtn.ForeColor = System.Drawing.Color.Black;
            this.startBtn.Location = new System.Drawing.Point(600, 0);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(600, 70);
            this.startBtn.TabIndex = 4;
            this.startBtn.Tag = "btn";
            this.startBtn.Text = "start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.startBtn_MouseClick);
            // 
            // testingTable
            // 
            this.testingTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testingTable.Location = new System.Drawing.Point(867, 70);
            this.testingTable.Name = "testingTable";
            this.testingTable.Size = new System.Drawing.Size(313, 530);
            this.testingTable.TabIndex = 5;
            this.testingTable.Tag = "testingtable";
            // 
            // playerProgressBar
            // 
            this.playerProgressBar.Location = new System.Drawing.Point(1047, 286);
            this.playerProgressBar.Maximum = 30;
            this.playerProgressBar.Name = "playerProgressBar";
            this.playerProgressBar.Size = new System.Drawing.Size(100, 30);
            this.playerProgressBar.Step = 3;
            this.playerProgressBar.TabIndex = 6;
            this.playerProgressBar.Tag = "progressbar";
            this.playerProgressBar.Visible = false;
            // 
            // leftBorder
            // 
            this.leftBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftBorder.Image = global::TanksGame.Properties.Resources.Block_fixed;
            this.leftBorder.Location = new System.Drawing.Point(0, 70);
            this.leftBorder.Name = "leftBorder";
            this.leftBorder.Size = new System.Drawing.Size(37, 530);
            this.leftBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.leftBorder.TabIndex = 7;
            this.leftBorder.TabStop = false;
            this.leftBorder.Tag = "border";
            this.leftBorder.Image.Tag = Strategy.none;
            // 
            // rightBorder
            // 
            this.rightBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightBorder.Image = global::TanksGame.Properties.Resources.Block_fixed;
            this.rightBorder.Location = new System.Drawing.Point(824, 70);
            this.rightBorder.Name = "rightBorder";
            this.rightBorder.Size = new System.Drawing.Size(47, 530);
            this.rightBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rightBorder.TabIndex = 8;
            this.rightBorder.TabStop = false;
            this.rightBorder.Tag = "border";
            this.rightBorder.Image.Tag = Strategy.none;
            // 
            // topBorder
            // 
            this.topBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.topBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topBorder.Image = global::TanksGame.Properties.Resources.Block_fixed;
            this.topBorder.Location = new System.Drawing.Point(37, 70);
            this.topBorder.Name = "topBorder";
            this.topBorder.Size = new System.Drawing.Size(790, 43);
            this.topBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.topBorder.TabIndex = 9;
            this.topBorder.TabStop = false;
            this.topBorder.Image.Tag = Strategy.none;
            this.topBorder.Tag = "border";
            // 
            // bottomBorder
            // 
            this.bottomBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottomBorder.Image = global::TanksGame.Properties.Resources.Block_fixed;
            this.bottomBorder.Location = new System.Drawing.Point(37, 557);
            this.bottomBorder.Name = "bottomBorder";
            this.bottomBorder.Size = new System.Drawing.Size(790, 43);
            this.bottomBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bottomBorder.TabIndex = 10;
            this.bottomBorder.TabStop = false;
            this.bottomBorder.Image.Tag = Strategy.none;
            this.bottomBorder.Tag = "border";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.ClientSize = new System.Drawing.Size(1184, 861);
            this.Controls.Add(this.bottomBorder);
            this.Controls.Add(this.topBorder);
            this.Controls.Add(this.rightBorder);
            this.Controls.Add(this.leftBorder);
            this.Controls.Add(this.playerProgressBar);
            this.Controls.Add(this.testingTable);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.resetBtn);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ClientSizeChanged += new System.EventHandler(this.Form1_ClientSizeChanged);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.testingTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomBorder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.DataGridView testingTable;
        private System.Windows.Forms.ProgressBar playerProgressBar;
        private System.Windows.Forms.PictureBox leftBorder;
        private System.Windows.Forms.PictureBox rightBorder;
        private System.Windows.Forms.PictureBox topBorder;
        private System.Windows.Forms.PictureBox bottomBorder;
    }
}

