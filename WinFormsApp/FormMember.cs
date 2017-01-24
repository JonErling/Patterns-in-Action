using System;
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
    // form in which members are added or edited

    public partial class FormMember : Form, IMemberView
    {
        private readonly MemberPresenter _memberPresenter;
        private bool _cancelClose;

        public FormMember()
        {
            InitializeComponent();
            Closing += FormMember_Closing;

            // initialize presenter.

            _memberPresenter = new MemberPresenter(this);
        }

        public int MemberId { get; set; }

        public string Email
        {
            get { return textBoxEmail.Text.Trim(); }
            set { textBoxEmail.Text = value; }
        }

        public new string CompanyName
        {
            get { return textBoxCompany.Text.Trim(); }
            set { textBoxCompany.Text = value; }
        }

        public string City
        {
            get { return textBoxCity.Text.Trim(); }
            set { textBoxCity.Text = value; }
        }

        public string Country
        {
            get { return textBoxCountry.Text.Trim(); }
            set { textBoxCountry.Text = value; }
        }

        // validates user input and, if valid, closes window
        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CompanyName) ||
                string.IsNullOrEmpty(City) ||
                string.IsNullOrEmpty(Country))
            {
                // do not close the dialog 
                MessageBox.Show("All fields are required");
                return;
            }

            try
            {
                _memberPresenter.Save();
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Save failed");
                _cancelClose = true;
            }
        }

        
        // provides opportunity to cancel window close event
        
        private void FormMember_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = _cancelClose;
            _cancelClose = false;
        }

        
        // checks for new member or edit existing member

        private void FormMember_Load(object sender, EventArgs e)
        {
            if (MemberId == 0)
                Text = "New Member";
            else
                Text = "Edit Member";

            _memberPresenter.Display(MemberId);
        }
    }
}
