namespace WindowsFormsChromaKeyboardSnake
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
            this._mButtonQuit = new System.Windows.Forms.Button();
            this._mTimerGame = new System.Windows.Forms.Timer(this.components);
            this._mComboBoxLayout = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _mButtonQuit
            // 
            this._mButtonQuit.Location = new System.Drawing.Point(160, 81);
            this._mButtonQuit.Name = "_mButtonQuit";
            this._mButtonQuit.Size = new System.Drawing.Size(75, 23);
            this._mButtonQuit.TabIndex = 0;
            this._mButtonQuit.Text = "Quit";
            this._mButtonQuit.UseVisualStyleBackColor = true;
            this._mButtonQuit.Click += new System.EventHandler(this.ButtonQuit_Click);
            // 
            // _mTimerGame
            // 
            this._mTimerGame.Tick += new System.EventHandler(this.TimerGame_Tick);
            // 
            // _mComboBoxLayout
            // 
            this._mComboBoxLayout.FormattingEnabled = true;
            this._mComboBoxLayout.Location = new System.Drawing.Point(12, 12);
            this._mComboBoxLayout.Name = "_mComboBoxLayout";
            this._mComboBoxLayout.Size = new System.Drawing.Size(227, 21);
            this._mComboBoxLayout.TabIndex = 4;
            this._mComboBoxLayout.SelectedIndexChanged += new System.EventHandler(this._mComboBoxLayout_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 116);
            this.Controls.Add(this._mComboBoxLayout);
            this.Controls.Add(this._mButtonQuit);
            this.Name = "Form1";
            this.Text = "Chroma Snake";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _mButtonQuit;
        private System.Windows.Forms.Timer _mTimerGame;
        private System.Windows.Forms.ComboBox _mComboBoxLayout;
    }
}

