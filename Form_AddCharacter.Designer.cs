namespace Cooldown_Tracker
{
    partial class Form_AddCharacter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AddCharacter));
            TextBoxCharacterName = new TextBox();
            label1 = new Label();
            Button_SubmitName = new Button();
            SuspendLayout();
            // 
            // TextBoxCharacterName
            // 
            TextBoxCharacterName.Location = new Point(12, 12);
            TextBoxCharacterName.Name = "TextBoxCharacterName";
            TextBoxCharacterName.Size = new Size(228, 23);
            TextBoxCharacterName.TabIndex = 0;
            TextBoxCharacterName.KeyPress += TextBoxCharacterName_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(246, 15);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 1;
            label1.Text = "Character Name";
            // 
            // Button_SubmitName
            // 
            Button_SubmitName.Location = new Point(255, 36);
            Button_SubmitName.Name = "Button_SubmitName";
            Button_SubmitName.Size = new Size(75, 23);
            Button_SubmitName.TabIndex = 2;
            Button_SubmitName.Text = "Submit";
            Button_SubmitName.UseVisualStyleBackColor = true;
            Button_SubmitName.Click += Button_SubmitName_Click;
            // 
            // Form_AddCharacter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(347, 70);
            Controls.Add(Button_SubmitName);
            Controls.Add(label1);
            Controls.Add(TextBoxCharacterName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form_AddCharacter";
            Text = "Character Wizard";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TextBoxCharacterName;
        private Label label1;
        private Button Button_SubmitName;
    }
}