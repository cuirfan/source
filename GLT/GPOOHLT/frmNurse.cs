using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Utils;
using Utils.Classes;
using Utils.DataObjects;
using System.Data.OleDb;
using System.Threading;

namespace GPOOHLT
{
    public partial class frmNurse : Form
    {
        List<Nurse> m_objList = new List<Nurse>();

        public frmNurse()
        {
            InitializeComponent();
            this.Text = GlobalData.UserName;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (ValidateFields() == false)
                    return;

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), GlobalData.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(UserControlBrowse.FilePath))
            {
                MessageBox.Show("Please select file", GlobalData.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (string.IsNullOrEmpty(userControlBrowse1.SheetName))
            {
                MessageBox.Show("Please select the sheet name", GlobalData.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (ValidateFields() == false)
                    return;

                using (DataTable objDT = grdData.DataSource as DataTable)
                {
                    if (objDT != null && objDT.Rows.Count > 0)
                    {
                        ReadAndSetData(objDT);
                    }
                    else
                    {
                        MessageBox.Show("Please load the data in grid", GlobalData.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (m_objList.Count > 0)
                {
                    bgw.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalData.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadPatientData()
        {
            string errorText = "";
            DataSet objDS = Common.GetPatientDS(out errorText);

            if (!string.IsNullOrEmpty(errorText))
            {
                MessageBox.Show(errorText, GlobalData.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearData();
                return;
            }

            if (objDS == null || objDS.Tables.Count == 0)
            {
                ClearData();
                return;
            }

            grdData.DataSource = objDS.Tables[0];
        }

        private void ClearData()
        {
            userControlBrowse1.Clear();
            grdData.DataSource = null;
        }
        
        private void ReadAndSetData(DataTable objDT)
        {
            m_objList.Clear();

            foreach (DataRow row in objDT.Rows)
            {
                string appointmentTime = string.Empty;
                try
                {
                    appointmentTime = Convert.ToString(row["Appointment Time"]);
                }
                catch
                {
                    appointmentTime = Convert.ToString(row["Appt# Time"]);
                }

                if (appointmentTime != string.Empty)
                    appointmentTime = GlobalData.GetDate(appointmentTime);

                m_objList.Add(new Nurse()
                {
                    Age = Convert.ToString(row["Patient Age"]),
                    strDOB = Convert.ToString(row["Patient Dateof Birth"]),
                    Firstname = Convert.ToString(row["Patient Forename"]),
                    LastName = Convert.ToString(row["Patient Surname"]),
                    Gender = Convert.ToString(row["Patient Gender"]),
                    CallBackNumber = Convert.ToString(row["Patient Call Back Number"]),
                    MobileNumber = Convert.ToString(row["Patient Mobile Phone Number"]),
                    OtherPhoneNumber = Convert.ToString(row["Patient Other Phone Number"]),
                    OtherNumber = Convert.ToString(row["Patient Other Number"]),
                    PatientID = Convert.ToInt32(row["Patient Patient ID"]),
                    HomeAddress = Convert.ToString(row["Patient Home Address"]),
                    HomeAddressPostcode = Convert.ToString(row["Patient Home Address Postcode"]),
                    CallTakerSymptoms = Convert.ToString(row["Call Taker Symptoms"]),
                    Priority = Convert.ToString(row["Priority"]),
                    CaseType = Convert.ToString(row["Case Type"]),
                    NurseSymptoms = Convert.ToString(row["Nurse Symptoms"]),
                    TreatmentCentre = Convert.ToString(row["Treatment Centre"]),
                    NurseOutcome = Convert.ToString(row["Nurse Outcome"]),
                    DoctorOutcome = Convert.ToString(row["Doctor Outcome"]),
                    Allergy = Convert.ToString(row["Allergy"]),
                    
                    // ---------------------------------------------------------
                    Sending =  Convert.ToString(row["Sending"]),
                    AppointmentTime = appointmentTime
                    // ---------------------------------------------------------
                });
            } 
        }

        public void LoadData()
        {
            GlobalData.Delay = userControlBrowse1.Delay;
            string extendedProperties = "Excel 12.0;HDR=YES;IMEX=1";
            string strConnXls = string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1}\"",
                UserControlBrowse.FilePath,
                extendedProperties);

            using (OleDbConnection objConn = new OleDbConnection(strConnXls))
            {
                using (OleDbCommand dbComm = objConn.CreateCommand())
                {
                    if (objConn.State != ConnectionState.Open)
                    {
                        objConn.Open();
                    }

                    dbComm.CommandText = "Select * from [" + userControlBrowse1.SheetName + "]";

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(dbComm))
                    using (DataSet columnDataSet = new DataSet())
                    using (DataSet dataSet = new DataSet())
                    {
                        columnDataSet.Locale = System.Globalization.CultureInfo.CurrentCulture;
                        adapter.Fill(columnDataSet);

                        if (columnDataSet.Tables.Count == 1)
                        {
                            var worksheet = columnDataSet.Tables[0];

                            // Now that we have a valid worksheet read in, with column names, we can create a
                            // new DataSet with a table that has preset columns that are all of type string.
                            // This fixes a problem where the OLEDB provider is trying to guess the data types
                            // of the cells and strange data appears, such as scientific notation on some cells.
                            dataSet.Tables.Add("WorksheetData");
                            DataTable tempTable = dataSet.Tables[0];

                            foreach (DataColumn column in worksheet.Columns)
                            {
                                tempTable.Columns.Add(column.ColumnName, typeof(string));
                            }

                            adapter.Fill(dataSet, "WorksheetData");

                            if (dataSet.Tables.Count == 1)
                            {
                                worksheet = dataSet.Tables[0];
                                grdData.DataSource = worksheet;
                            }
                        }
                    }
                }
            }
        }

        private string SaveNurseData()
        {
            userControlBrowse1.SaveInXML();

            string result = string.Empty;
            int count = 1;
            foreach (Nurse obj in m_objList)
            {
                lbl.Invoke((MethodInvoker)delegate
                {
                    lbl.Text = "Processing record " + count + " of " + m_objList.Count;
                });

                Common.SaveNurseConsultation(obj, ref count);
                Thread.Sleep(Convert.ToInt32(GlobalData.Delay) * 1000);
            }

            lbl.Invoke((MethodInvoker)delegate
            {
                result = "Total number of record(s) saved " + (count - 1) + " out of " + m_objList.Count;
                Logger.WriteActivity("Common", "SaveNurseData", result);
                lbl.Text = result;
            });

            return result;
        }

        private void CheckMultipleDelegates(Button objbtn, bool value)
        {
            if(objbtn.InvokeRequired)
            {
                MethodInvoker inv = delegate
                {
                    objbtn.Enabled = value;
                };
                this.Invoke(inv);
            }
            else
                objbtn.Enabled = value;                
        }
        
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (prgrsbar.InvokeRequired)
            {
                MethodInvoker inv = delegate
                {
                    prgrsbar.Visible = true;
                };
                this.Invoke(inv);
            }

            CheckMultipleDelegates(btnClearData, false);
            CheckMultipleDelegates(btnSaveData, false);
            CheckMultipleDelegates(btnClose, false);
            CheckMultipleDelegates(btnLoadPatient, false);
            string result = SaveNurseData();
            //MessageBox.Show(result, GlobalData.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CheckMultipleDelegates(btnClearData, true);
            CheckMultipleDelegates(btnSaveData, true);
            CheckMultipleDelegates(btnClose, true);
            CheckMultipleDelegates(btnLoadPatient, true);

            prgrsbar.Visible = false;
        }

        private void frmNurse_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSaveData.Enabled == false)
                e.Cancel = true;
        }

        //private string GetSheetName(OleDbConnection objConn)
        //{
        //    DataTable sheetTable = objConn.GetSchema("Tables");
        //    DataRow rowSheetName = sheetTable.Rows[2];
        //    String sheetName = rowSheetName[2].ToString();
        //    return sheetName;
        //}
    }
}
