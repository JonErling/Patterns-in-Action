﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinFormsApp.Models;
using WinFormsApp.Presenters;
using WinFormsApp.Views;

namespace WinFormsApp
{
    /// <summary>
    /// Form where users enter login credentials.
    /// </summary>
    /// <remarks>
    /// Valid demo values are: 
    ///   userName: debbie@company.com
    ///   password: secret123
    /// </remarks>
    public partial class FormLogin : Form, ILoginView
    {
        // The Presenter
        private readonly LoginPresenter _loginPresenter;
        private bool _cancelClose;

        /// <summary>
        /// Default contructor of FormLogin.
        /// </summary>
        public FormLogin()
        {
            InitializeComponent();
            Closing += FormLogin_Closing;

            _loginPresenter = new LoginPresenter(this);
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Email => textBoxEmail.Text.Trim();

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password => textBoxPassword.Text.Trim();


        /// <summary>
        /// Performs login and upson success closes dialog.
        /// </summary>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                _loginPresenter.Login();
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Login failed");
                _cancelClose = true;
            }
        }

        /// <summary>
        /// Cancel was requested. Now closes dialog.
        /// </summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Displays valid demo credentials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelValid_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("You can use the following credentials: \r\n\r\n" +
                "    Email:     debbie@company.com \r\n" +
                "    PassWord:  secret123", "Login Credentials");
        }

        /// <summary>
        /// Provides opportunity to cancel the dialog close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLogin_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = _cancelClose;
            _cancelClose = false;
        }
    }
}
