using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using Utils.Classes;

namespace GPOOHLT
{
    public partial class frmLogin : Form
    {
        private string m_strFilePath = Application.StartupPath + @"\logininfo.xml";

        public frmLogin()
        {
            InitializeComponent();
            LoadRemembrance();
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

        private void LoadRemembrance()
        {
            bool blnCheck = false;
            if (File.Exists(m_strFilePath))
            {
                using (DataSet objDS = new DataSet())
                {
                    objDS.ReadXml(m_strFilePath);
                    if (objDS.Tables["Credentials"] != null && objDS.Tables["Credentials"].Rows.Count > 0)
                    {
                        string macAddress = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(objDS.Tables["Credentials"].Rows[0]["MacAddress"].ToString()));
                        if (macAddress == GetMACAddress())
                        {
                            txtUserName.Text = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(objDS.Tables["Credentials"].Rows[0]["Username"].ToString()));
                            txtPassword.Text = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(objDS.Tables["Credentials"].Rows[0]["Password"].ToString()));
                            blnCheck = true;
                        }
                    }
                }
            }
            chkRemember.Checked = blnCheck;
        }

        private void SaveRemembrance(bool pCheck)
        {
            if (pCheck)
            {
                using (DataSet objDS = new DataSet("LoginInfo"))
                {
                    string[] strLog = new string[3];
                    objDS.Tables.Add("Credentials");
                    strLog[0] = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(txtUserName.Text.Trim()));
                    strLog[1] = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(txtPassword.Text.Trim()));
                    strLog[2] = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(GetMACAddress()));
                    
                    objDS.Tables["Credentials"].Columns.Add("Username");
                    objDS.Tables["Credentials"].Columns.Add("Password");
                    objDS.Tables["Credentials"].Columns.Add("MacAddress");
                    objDS.Tables["Credentials"].Rows.Add(strLog);
                    objDS.WriteXml(m_strFilePath);
                    strLog = null;
                }
            }
            else
                File.Delete(m_strFilePath);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "Validating your credentials please wait...";

                // Validate User

                if (!ValidateFields())       //Validate input fields
                    return;

                Cursor.Current = Cursors.WaitCursor;
                if (!ValidateUser())         // Authentication agaisnt credentials (Zeeshan)
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Invalid credentials. Please try again...";
                    return;
                }

                //if (!ValidateFileVersion())
                //{
                //    lblMessage.ForeColor = Color.Red;
                //    lblMessage.Text = "Invalid file version. Please contact to DBA to upload the sheet into database";
                //    return;
                //}

                this.DialogResult = DialogResult.OK;
                SaveRemembrance(chkRemember.Checked);
                Logger.WriteActivity("frmLogin", "btnOK_Click", "UserName and Password authenticated");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            this.Close();    
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Please enter username";
                return false;
            }
            
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Please enter password";
                return false;
            }
            
            //if (nudFileVersion.Value <=0)
            //{
            //    lblMessage.ForeColor = Color.Red;
            //    lblMessage.Text = "Please enter file version";
            //    return false;
            //}
            return true;
        }

        private bool ValidateUser()
        {            
            bool goAhead = false;
            string errorText = "";
            goAhead = Common.AuthendicateUser(txtUserName.Text.TrimEnd(), txtPassword.Text.TrimEnd(), out errorText);

            if (errorText.Length > 0)
                lblMessage.Text = errorText;
            return goAhead;
        }

        //private bool ValidateFileVersion()
        //{
        //    bool goAhead = false;
        //    string errorText = "";
        //    goAhead = Common.AuthendicateFileVersion(nudFileVersion.Value, out errorText);
        //    if (errorText.Length > 0)
        //        lblMessage.Text = errorText;
        //    return goAhead;
        //}

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
