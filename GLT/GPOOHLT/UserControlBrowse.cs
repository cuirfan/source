using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils.Classes;
using System.Data.OleDb;

namespace GPOOHLT
{
    public partial class UserControlBrowse : UserControl
    {
        private string m_strFilePath = Application.StartupPath;
        public static string FilePath { get; set; }  // variable to store file path
        public string SheetName { get; set; }
        public decimal Delay { get; set; }

        public DataTable objDT = new DataTable();

        public UserControlBrowse()
        {
            InitializeComponent();
        }

        //private string m_XMLFileName = string.Empty;
        //public string XMLFileName
        //{
        //    get { return m_XMLFileName; }
        //    set
        //    {
        //        m_XMLFileName = value;
        //    }
        //}

        // public string XMLFileName { get; set; }

        //private string m_strFilePath = Application.StartupPath + @"\NurseInfo.xml";
        public void LoadFromXML()
        {
            string tName = Utils.Classes.GlobalData.ObjRoles.ToString();
            m_strFilePath += @"\" +tName + @".xml";

            if (System.IO.File.Exists(m_strFilePath))
            {
                using (DataSet objDS = new DataSet())
                {
                    objDS.ReadXml(m_strFilePath);
                    if (objDS.Tables[tName] != null && objDS.Tables[tName].Rows.Count > 0)
                    {
                        string fn = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(objDS.Tables[tName].Rows[0]["FilePath"].ToString()));
                        if (System.IO.File.Exists(fn))
                        {
                            txtPathName.Text = fn;
                            try
                            {
                                cboSheet.Text = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(objDS.Tables[tName].Rows[0]["SheetName"].ToString()));
                            }
                            catch
                            {
                            }
                            numDelay.Value = Convert.ToDecimal(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(objDS.Tables[tName].Rows[0]["Delay"].ToString())));
                        }
                    }
                }
            }
        }

        public void SaveInXML()
        {
            string tName = Utils.Classes.GlobalData.ObjRoles.ToString();

            using (DataSet objDS = new DataSet())
            {
                string[] strLog = new string[3];
                objDS.Tables.Add(tName);
                strLog[0] = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(txtPathName.Text));
                try
                {
                    strLog[1] = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(SheetName));
                }
                catch
                {
                }
                
                strLog[2] = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(numDelay.Value.ToString()));

                objDS.Tables[tName].Columns.Add("FilePath");
                objDS.Tables[tName].Columns.Add("SheetName");
                objDS.Tables[tName].Columns.Add("Delay");
                objDS.Tables[tName].Rows.Add(strLog);
                objDS.WriteXml(m_strFilePath);
                strLog = null;
            }
        }

        /// <summary>
        /// Clear the file path of excel sheet
        /// </summary>
        public void Clear()
        {
            FilePath = string.Empty;
            txtPathName.Text = FilePath;
            objDT.Clear();
            //cboSheet.SelectedIndex = -1;
            cboSheet.DataSource = null; ;
        }

        private void OnBrowseClick()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Sheet(*.xls;*.xlsx)|*.xls;*.xlsx";
            ofd.Title = "Excel File";

            // set initial directory of computer system
            //ofd.InitialDirectory = Application.StartupPath + @"\..\..\..\..\";

            // set restore directory
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // store selected file path
                txtPathName.Text = ofd.FileName;
                FilePath = txtPathName.Text.Trim();
                LoadData();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OnBrowseClick();
        }

        public void LoadData()
        {
            string extendedProperties = "Excel 12.0;HDR=YES;IMEX=1";
            string strConnXls = string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1}\"",
                FilePath,
                extendedProperties);

            using (OleDbConnection objConn = new OleDbConnection(strConnXls))
            {
                using (OleDbCommand dbComm = objConn.CreateCommand())
                {
                    if (objConn.State != ConnectionState.Open)
                    {
                        objConn.Open();
                    }

                    DataTable tblSheetTable = GetSheetTable(objConn);
                    //tblSheetTable.Select()
                    FillSheetCombo(tblSheetTable);
                }
            }
        }

        private void FillSheetCombo(DataTable objDataTable)
        {
            DataTable tbl = objDataTable.Clone();
            string filter = string.Empty;
            if (GlobalData.ObjRoles==Roles.CallTaker)
                filter = "TABLE_NAME like '%CT_%'";
            else if (GlobalData.ObjRoles == Roles.Nurse)
                filter = "TABLE_NAME like '%Nur_%'";
            else if (GlobalData.ObjRoles == Roles.Doctor)
                filter = "TABLE_NAME like '%DOC_%'";



            DataRow[] arr = objDataTable.Select(filter);
            foreach (var row in arr)
            {
                tbl.ImportRow(row);
            }
            string valueMember = "TABLE_NAME";
            string displayMember = "TABLE_NAME";
            FillCombo(cboSheet, tbl, displayMember, valueMember);
        }

        private void FillCombo(ComboBox cbo, DataTable DataTableToBind, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
            cbo.DataSource = DataTableToBind;
            cbo.SelectedIndex = -1;
        }

        private DataTable GetSheetTable(OleDbConnection objConn)
        {
            DataTable sheetTable = objConn.GetSchema("Tables");
            return sheetTable;
        }

        private void cboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSheet.SelectedItem == null)
                SheetName = string.Empty;
            else
                SheetName = cboSheet.Text;
        }

        private void UserControlBrowse_Load(object sender, EventArgs e)
        {
            LoadFromXML();

            //if (FilePath == null)
            //    FilePath = "";

            //if (FilePath != string.Empty)
            //{
            //    txtPathName.Text = FilePath;
            //    LoadData();
            //}

            Delay = numDelay.Value;
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            Delay = numDelay.Value;
        }

        private void txtPathName_TextChanged(object sender, EventArgs e)
        {
            FilePath = txtPathName.Text.Trim();
            LoadData();
        }

        private void txtPathName_MouseClick(object sender, MouseEventArgs e)
        {
            OnBrowseClick();
        }
    }
}
