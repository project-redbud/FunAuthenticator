namespace Milimoe.FunAuthenticator
{
    partial class FunAuthenticator
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CodeText = new Label();
            RemindText = new Label();
            Timer = new ProgressBar();
            SuspendLayout();
            // 
            // CodeText
            // 
            CodeText.Font = new Font("LanaPixel", 36F, FontStyle.Regular, GraphicsUnit.Point);
            CodeText.Location = new Point(12, 9);
            CodeText.Name = "CodeText";
            CodeText.Size = new Size(313, 87);
            CodeText.TabIndex = 0;
            CodeText.Text = "123456";
            CodeText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RemindText
            // 
            RemindText.Font = new Font("LanaPixel", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            RemindText.Location = new Point(12, 96);
            RemindText.Name = "RemindText";
            RemindText.Size = new Size(313, 28);
            RemindText.TabIndex = 1;
            RemindText.Text = "剩余 30 秒";
            RemindText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Timer
            // 
            Timer.Location = new Point(12, 127);
            Timer.Maximum = 30;
            Timer.Name = "Timer";
            Timer.Size = new Size(313, 23);
            Timer.Step = 1;
            Timer.Style = ProgressBarStyle.Continuous;
            Timer.TabIndex = 2;
            // 
            // FunAuthenticator
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(337, 161);
            Controls.Add(Timer);
            Controls.Add(RemindText);
            Controls.Add(CodeText);
            Name = "FunAuthenticator";
            Text = "FunAuthenticator";
            ResumeLayout(false);
        }

        #endregion

        private Label CodeText;
        private Label RemindText;
        private ProgressBar Timer;
    }
}