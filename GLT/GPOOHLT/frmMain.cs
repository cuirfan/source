using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils.Classes;

namespace GPOOHLT
{
    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadDefaultData();
            if (!Login())
                Application.Exit();
        }

        private bool Login()
        {
            bool access = false;
            frmLogin objFrm = new frmLogin();
            DialogResult res = DialogResult.None;

            while (res != DialogResult.OK || res != DialogResult.Cancel)
            {
                res = objFrm.ShowDialog();
                if (res == DialogResult.OK)
                {
                    access = true;
                    break;
                }
                else if (res == DialogResult.Cancel)
                {
                    access = false;
                    break;
                }
            }

            return access;
        }

        private void LoadDefaultData()
        {
            this.BringToFront();
            GlobalData.ObjRoles = Roles.None; 
            GlobalData.GPOOHLTConnectionString = AppConfigReader.CONN_STRING;
            GlobalData.AppName = AppConfigReader.SystemName;
            GlobalData.RTCSeverIP = AppConfigReader.RTCSeverIP;
            GlobalData.RTCServerPort = int.Parse(AppConfigReader.RTCServerPort);
        }

        private void lnkCallTaker_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalData.ObjRoles = Roles.CallTaker;
            frmCallTaker objFrm = new frmCallTaker();
            objFrm.ShowDialog();
        }

        private void lnkNurse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalData.ObjRoles = Roles.Nurse;
            frmNurse objFrm = new frmNurse();
            objFrm.ShowDialog();
        }

        private void lnkDoctor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalData.ObjRoles = Roles.Doctor;
            frmDoctor objFrm = new frmDoctor();
            objFrm.ShowDialog();
        }


    }
}
