using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    partial class DangKy
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
            this.cbShowPass = new System.Windows.Forms.CheckBox();
            this.label = new System.Windows.Forms.Label();
            this.btn_register = new System.Windows.Forms.Button();
            this.lb_pass = new System.Windows.Forms.Label();
            this.lb_username = new System.Windows.Forms.Label();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.lb_confirmPass = new System.Windows.Forms.Label();
            this.txtConfirmPass = new System.Windows.Forms.TextBox();
            this.lb_backtologin = new System.Windows.Forms.LinkLabel();
            this.lb_name = new System.Windows.Forms.Label();
            this.txt_fullname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbShowPass
            // 
            this.cbShowPass.AutoSize = true;
            this.cbShowPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowPass.Location = new System.Drawing.Point(364, 210);
            this.cbShowPass.Margin = new System.Windows.Forms.Padding(2);
            this.cbShowPass.Name = "cbShowPass";
            this.cbShowPass.Size = new System.Drawing.Size(129, 22);
            this.cbShowPass.TabIndex = 4;
            this.cbShowPass.Text = "Hiện password";
            this.cbShowPass.UseVisualStyleBackColor = true;
            this.cbShowPass.CheckedChanged += new System.EventHandler(this.cbShowPass_CheckedChanged);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(173, 326);
            this.label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(172, 18);
            this.label.TabIndex = 10;
            this.label.Text = "Bạn đã có tài khoản ?";
            // 
            // btn_register
            // 
            this.btn_register.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn_register.Location = new System.Drawing.Point(176, 252);
            this.btn_register.Margin = new System.Windows.Forms.Padding(2);
            this.btn_register.Name = "btn_register";
            this.btn_register.Size = new System.Drawing.Size(117, 48);
            this.btn_register.TabIndex = 5;
            this.btn_register.Text = "Đăng ký";
            this.btn_register.UseVisualStyleBackColor = true;
            this.btn_register.Click += new System.EventHandler(this.btn_register_Click);
            // 
            // lb_pass
            // 
            this.lb_pass.AutoSize = true;
            this.lb_pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_pass.Location = new System.Drawing.Point(8, 135);
            this.lb_pass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_pass.Name = "lb_pass";
            this.lb_pass.Size = new System.Drawing.Size(104, 25);
            this.lb_pass.TabIndex = 8;
            this.lb_pass.Text = "Password:";
            // 
            // lb_username
            // 
            this.lb_username.AutoSize = true;
            this.lb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_username.Location = new System.Drawing.Point(8, 100);
            this.lb_username.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_username.Name = "lb_username";
            this.lb_username.Size = new System.Drawing.Size(137, 25);
            this.lb_username.TabIndex = 7;
            this.lb_username.Text = "Tên tài khoản:";
            // 
            // txt_username
            // 
            this.txt_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_username.Location = new System.Drawing.Point(172, 102);
            this.txt_username.Margin = new System.Windows.Forms.Padding(2);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(289, 24);
            this.txt_username.TabIndex = 1;
            // 
            // txt_password
            // 
            this.txt_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_password.Location = new System.Drawing.Point(172, 135);
            this.txt_password.Margin = new System.Windows.Forms.Padding(2);
            this.txt_password.Name = "txt_password";
            this.txt_password.PasswordChar = '*';
            this.txt_password.Size = new System.Drawing.Size(289, 24);
            this.txt_password.TabIndex = 2;
            // 
            // lb_confirmPass
            // 
            this.lb_confirmPass.AutoSize = true;
            this.lb_confirmPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_confirmPass.Location = new System.Drawing.Point(8, 168);
            this.lb_confirmPass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_confirmPass.Name = "lb_confirmPass";
            this.lb_confirmPass.Size = new System.Drawing.Size(193, 25);
            this.lb_confirmPass.TabIndex = 9;
            this.lb_confirmPass.Text = "Xác nhận Password:";
            // 
            // txtConfirmPass
            // 
            this.txtConfirmPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPass.Location = new System.Drawing.Point(172, 168);
            this.txtConfirmPass.Margin = new System.Windows.Forms.Padding(2);
            this.txtConfirmPass.Name = "txtConfirmPass";
            this.txtConfirmPass.PasswordChar = '*';
            this.txtConfirmPass.Size = new System.Drawing.Size(289, 24);
            this.txtConfirmPass.TabIndex = 3;
            // 
            // lb_backtologin
            // 
            this.lb_backtologin.AutoSize = true;
            this.lb_backtologin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_backtologin.Location = new System.Drawing.Point(196, 367);
            this.lb_backtologin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_backtologin.Name = "lb_backtologin";
            this.lb_backtologin.Size = new System.Drawing.Size(108, 25);
            this.lb_backtologin.TabIndex = 11;
            this.lb_backtologin.TabStop = true;
            this.lb_backtologin.Text = "Đăng nhập";
            this.lb_backtologin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lb_backtologin_LinkClicked);
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lb_name.Location = new System.Drawing.Point(8, 66);
            this.lb_name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(101, 25);
            this.lb_name.TabIndex = 6;
            this.lb_name.Text = "Họ và tên:";
            // 
            // txt_fullname
            // 
            this.txt_fullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_fullname.Location = new System.Drawing.Point(172, 67);
            this.txt_fullname.Margin = new System.Windows.Forms.Padding(2);
            this.txt_fullname.Name = "txt_fullname";
            this.txt_fullname.Size = new System.Drawing.Size(289, 24);
            this.txt_fullname.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.Location = new System.Drawing.Point(150, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 29);
            this.label3.TabIndex = 12;
            this.label3.Text = "Thông tin cá nhân";
            // 
            // DangKy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 465);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_fullname);
            this.Controls.Add(this.lb_name);
            this.Controls.Add(this.lb_backtologin);
            this.Controls.Add(this.lb_confirmPass);
            this.Controls.Add(this.txtConfirmPass);
            this.Controls.Add(this.cbShowPass);
            this.Controls.Add(this.label);
            this.Controls.Add(this.btn_register);
            this.Controls.Add(this.lb_pass);
            this.Controls.Add(this.lb_username);
            this.Controls.Add(this.txt_username);
            this.Controls.Add(this.txt_password);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DangKy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng ký";
            this.Load += new System.EventHandler(this.DangKy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox cbShowPass;
        private Label label;
        private Button btn_register;
        private Label lb_pass;
        private Label lb_username;
        private TextBox txt_username;
        private TextBox txt_password;
        private Label lb_confirmPass;
        private TextBox txtConfirmPass;
        private LinkLabel lb_backtologin;
        private Label lb_name;
        private TextBox txt_fullname;
        private Label label3;
    }
}