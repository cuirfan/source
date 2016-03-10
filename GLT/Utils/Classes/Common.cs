using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils.DataObjects;


namespace Utils.Classes
{
    public class Common
    {
        public static int GetVisit(int PaitentID)
        {
            int visitId = 0;
            string errorText = "";
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                sqlProxy.AddCommand("[DATA].[uspGetVisitID]", System.Data.CommandType.StoredProcedure);
                sqlProxy.AddParameter("@pPatientID", System.Data.SqlDbType.Int, PaitentID);
                System.Data.SqlClient.SqlParameter objParameter = sqlProxy.AddOutputParameter("@pVisitID", System.Data.SqlDbType.Int);
                sqlProxy.ExecuteNonQuery(out errorText);

                if (objParameter.Value != null && objParameter.Value != DBNull.Value)
                    visitId = Convert.ToInt32(objParameter.Value);

                Logger.WriteActivity("Common", "GetVisit", string.Format("PatientID: {0} - VisitID: {1}", PaitentID, visitId));

                return visitId;
            }
        }

        private static int GetPriorityID(string text)
        {
            int priorityID = 0;
            string errorText = "";
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                sqlProxy.AddCommand("[DATA].[uspGetPriorityID]", System.Data.CommandType.StoredProcedure);
                sqlProxy.AddParameter("@pStatus", System.Data.SqlDbType.VarChar, text);
                System.Data.SqlClient.SqlParameter objParameter = sqlProxy.AddOutputParameter("@pPriorityID", System.Data.SqlDbType.Int);
                sqlProxy.ExecuteNonQuery(out errorText);

                if (errorText != "")
                    Logger.WriteActivity("Common", "GetPriorityID", errorText);

                if (objParameter.Value != null && objParameter.Value != DBNull.Value)
                    priorityID = Convert.ToInt32(objParameter.Value);
                return priorityID;
            }
        }

        private static int GetCallTypeID(string text)
        {
            int callTypeID = 0;
            string errorText = "";
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                sqlProxy.AddCommand("[DATA].[uspGetCallTypeID]", System.Data.CommandType.StoredProcedure);
                sqlProxy.AddParameter("@pDesc", System.Data.SqlDbType.VarChar, text);
                System.Data.SqlClient.SqlParameter objParameter = sqlProxy.AddOutputParameter("@pCallTypeID", System.Data.SqlDbType.Int);
                sqlProxy.ExecuteNonQuery(out errorText);

                if (errorText != "")
                    Logger.WriteActivity("Common", "GetCallTypeID", errorText);

                if (objParameter.Value != null && objParameter.Value != DBNull.Value)
                    callTypeID = Convert.ToInt32(objParameter.Value);

                return callTypeID;
            }
        }

        private static int GetTreatmentCeterID(string text)
        {
            int treatmentCeterID = 0;
            string errorText = "";
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                sqlProxy.AddCommand("[DATA].[uspGetTreatmentCeterID]", System.Data.CommandType.StoredProcedure);
                sqlProxy.AddParameter("@pTreatmentDesc", System.Data.SqlDbType.VarChar, text);
                System.Data.SqlClient.SqlParameter objParameter = sqlProxy.AddOutputParameter("@pAreaID", System.Data.SqlDbType.Int);
                sqlProxy.ExecuteNonQuery(out errorText);

                if (errorText != "")
                    Logger.WriteActivity("Common", "GetTreatmentCeterID", errorText);

                if (objParameter.Value != null && objParameter.Value != DBNull.Value)
                    treatmentCeterID = Convert.ToInt32(objParameter.Value);

                return treatmentCeterID;
            }
        }

        private static int GetAge(string strAge)
        {
            int age = 0;
            try
            {
                strAge = strAge.Split(' ')[0].Trim();
                age = Convert.ToInt32(strAge);
            }
            catch (Exception ex)
            {
                Logger.WriteActivity("Common", "GetAge", "Problem to getting the age." + Environment.NewLine + ex.ToString());
            }
            return age;
        }
      
        public static DataSet GetPatientDS(out String errorText)
        {
            errorText = "";
            DataSet ds = null;
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                sqlProxy.AddCommand("[DATA].[uspGetPatient]", System.Data.CommandType.StoredProcedure);
                ds = sqlProxy.Execute(out errorText);
            }
            Logger.WriteActivity("Common", "GetPatientDS", "Getting Patient DataSet");
            return ds;
        }

        public static bool AuthendicateUser(string UserName, string Password, out string errorText)
        {
            bool isAuthendicated = false;
            errorText = String.Empty;
            DataSet objDS = new DataSet();
            objDS.Locale = System.Globalization.CultureInfo.InvariantCulture;
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                sqlProxy.AddCommand("[DATA].[uspAuthenticateUser]", System.Data.CommandType.StoredProcedure);
                sqlProxy.AddParameter("@pUsername", System.Data.SqlDbType.VarChar, UserName);
                sqlProxy.AddParameter("@pPassword", System.Data.SqlDbType.VarChar, Password);
                objDS = sqlProxy.ExecuteDataSet(out errorText);

                if (objDS.Tables[0].Rows.Count > 0)
                {
                    GlobalData.UserID = Convert.ToInt32(objDS.Tables[0].Rows[0]["UserID"]);
                    GlobalData.UserName = Convert.ToString(objDS.Tables[0].Rows[0]["UserName"]);
                    GlobalData.Password = Convert.ToString(objDS.Tables[0].Rows[0]["Password"]);
                    isAuthendicated = true;
                }
                
                return isAuthendicated;
            }
        }

        public static bool AuthendicateFileVersion(decimal fileVersion, out string errorText)
        {
            bool isAuthendicated = false;
            errorText = String.Empty;
            //using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            //{
            //    sqlProxy.AddCommand("uspAuthenticateFileVersion", System.Data.CommandType.StoredProcedure);
            //    sqlProxy.AddParameter("@pFileVersion", System.Data.SqlDbType.VarChar, fileVersion.ToString());
            //    System.Data.SqlClient.SqlParameter objParameter = sqlProxy.AddOutputParameter("@pIsFileVersionExists", System.Data.SqlDbType.Bit);
            //    sqlProxy.ExecuteNonQuery(out errorText);

            //    if (objParameter.Value != null && objParameter.Value != DBNull.Value)
            //        isAuthendicated = (bool)objParameter.Value;
                return isAuthendicated;
            //}
        }

        public static bool CheckPatient(int PaitentID)
        {
            bool isExistPatient = false;
            string errorText = "";
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                sqlProxy.AddCommand("[DATA].[uspIsPatientExist]", System.Data.CommandType.StoredProcedure);
                sqlProxy.AddParameter("@pPatientID", System.Data.SqlDbType.Int, PaitentID);
                System.Data.SqlClient.SqlParameter objParameter = sqlProxy.AddOutputParameter("@pIsPatientExists", System.Data.SqlDbType.Bit);
                sqlProxy.ExecuteNonQuery(out errorText);

                if (objParameter.Value != null && objParameter.Value != DBNull.Value)
                {
                    isExistPatient = (bool)objParameter.Value;
                    Logger.WriteActivity("Common", "CheckPatient", "Patient exists in database");
                }
                return isExistPatient;
            }
        }

        public static void SavePatient(PersonalDetails objPersonalDetails, ref int count)
        {
            try
            {
                int pid = 0;
                string errorText = "";

                DataTable UDTAddress = PatientAddress(objPersonalDetails);
                DataTable UDTContacts = PatientContact(objPersonalDetails);

                using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
                {
                    sqlProxy.AddCommand("[General].[uspSavePatient]", System.Data.CommandType.StoredProcedure);

                    if (CheckPatient(objPersonalDetails.PatientID))
                        sqlProxy.AddParameter("@pPatientID", System.Data.SqlDbType.Int, objPersonalDetails.PatientID);
                    else
                        sqlProxy.AddParameter("@pPatientID", System.Data.SqlDbType.Int, 0);

                    int age = GetAge(objPersonalDetails.Age);

                    sqlProxy.AddParameter("@pAge", System.Data.SqlDbType.Int, age);

                    if (objPersonalDetails.Age.Trim() == "" || age == 0)
                        sqlProxy.AddParameter("@pIsAgeUnknown", System.Data.SqlDbType.Bit, true);
                    else
                        sqlProxy.AddParameter("@pIsAgeUnknown", System.Data.SqlDbType.Bit, false);

                    sqlProxy.AddParameter("@pgender", System.Data.SqlDbType.VarChar, objPersonalDetails.Gender);
                    sqlProxy.AddParameter("@pDob", System.Data.SqlDbType.DateTime, Convert.ToDateTime(objPersonalDetails.strDOB));
                    sqlProxy.AddParameter("@pFirstname", System.Data.SqlDbType.NVarChar, objPersonalDetails.Firstname);
                    sqlProxy.AddParameter("@pLastName", System.Data.SqlDbType.NVarChar, objPersonalDetails.LastName);
                    //--------------- DUMMY VALUE------------------------------------//
                    sqlProxy.AddParameter("@pLivingNowIn", System.Data.SqlDbType.Int, DBNull.Value);
                    sqlProxy.AddParameter("@pPlaceofbirth", System.Data.SqlDbType.Int, DBNull.Value);
                    sqlProxy.AddParameter("@pGPName", System.Data.SqlDbType.NVarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pIsMilitaryPerson", System.Data.SqlDbType.Bit, DBNull.Value);
                    sqlProxy.AddParameter("@pMilitaryNumber", System.Data.SqlDbType.NVarChar, DBNull.Value);
                    //sqlProxy.AddParameter("@pIsCorrespondeceAddress", System.Data.SqlDbType.NVarChar, objPersonalDetails.HomeAddress);
                    //sqlProxy.AddParameter("@pAddressExpiration", System.Data.SqlDbType.DateTime, DBNull.Value);
                    //sqlProxy.AddParameter("@pApplicationIdentity", System.Data.SqlDbType.Int, "");
                    sqlProxy.AddParameter("@pisQuickContactGardian", System.Data.SqlDbType.Bit, false);
                    sqlProxy.AddParameter("@pIsPrimaryContact  ", System.Data.SqlDbType.Bit, false);
                    sqlProxy.AddParameter("@pFatherName", System.Data.SqlDbType.NVarChar, "George Parker");
                    sqlProxy.AddParameter("@pHusbandName", System.Data.SqlDbType.NVarChar, "Ben Walker");
                    sqlProxy.AddParameter("@pPatientCast", System.Data.SqlDbType.NVarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pPatientVisitBasedAddress", System.Data.SqlDbType.Int, DBNull.Value); // not using in sp
                    //sqlProxy.AddParameter("@pIsUnknown", System.Data.SqlDbType.NVarChar, objPersonalDetails.HomeAddress);

                    sqlProxy.AddParameter("@pContact", System.Data.SqlDbType.Structured, UDTContacts);
                    sqlProxy.AddParameter("@pAddress", System.Data.SqlDbType.Structured, UDTAddress);

                    System.Data.SqlClient.SqlParameter IDParameter = sqlProxy.AddOutputParameter("@pPatientOUTID", System.Data.SqlDbType.Int);
                    //--------------- DUMMY VALUE------------------------------------//            

                    sqlProxy.ExecuteNonQuery(out errorText);

                    if (errorText != "")
                        throw new Exception("Exception while saving patient (" + objPersonalDetails.PatientID + ")" + Environment.NewLine +errorText);

                    count++;

                    if (IDParameter.Value != null && IDParameter.Value != DBNull.Value)
                        pid = (int)IDParameter.Value;
                }

                if (pid > 0)
                {
                    DataSet ds = MakeVisits(pid, objPersonalDetails);
                    SaveAppointments(ds, objPersonalDetails);
                    Logger.WriteActivity("Common", "SavePatient", "Patient, Appointment, and visit saved in database");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteActivity("Common", "SavePatientData", ex.ToString());
            }
        }

        public static DataSet SaveAppointments(DataSet ds, PersonalDetails objPersonalDetails)
        {
            DataSet dsNurses = new DataSet();
            DataTable dtNurses = new DataTable();
            DataTable dtCaseNos = new DataTable("CaseNos");
            string StrCaseNos = "";
            string StrVisitIds = "";
            dtNurses.Columns.Add("NurseID");
            dtNurses.Columns.Add("UserName");
            dtNurses.Columns.Add("CaseNo");
            dtNurses.Columns.Add("CallID");
            dtNurses.Columns.Add("VisitID");
            dtNurses.Columns.Add("PatientName");
            dtNurses.Columns.Add("Priority");
            dtNurses.Columns.Add("PriorityID");
            dtNurses.Columns.Add("PatientID");
            dtNurses.Columns.Add("Extension");
            dtNurses.Columns.Add("IsAvalible");
            dtNurses.Columns.Add("IsMultiple");
            dtNurses.Columns.Add("Flag");
            dtNurses.Columns.Add("NurseName");
            DataSet dsResult = new DataSet();
            using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
            {
                try
                {
                    string errorText = string.Empty;
                    int callID = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        sqlProxy.AddCommand("uspInsertUpdateCallInformation", System.Data.CommandType.StoredProcedure);
                        System.Data.SqlClient.SqlParameter IDParameter = sqlProxy.AddOutputParameter("@pCallIDOut", System.Data.SqlDbType.Int);
                        sqlProxy.AddParameter("@pCallID", System.Data.SqlDbType.Int, ds.Tables[0].Rows[0]["CallID"]);
                        sqlProxy.AddParameter("@pCallername", System.Data.SqlDbType.NVarChar, ds.Tables[0].Rows[0]["Callername"]);
                        sqlProxy.AddParameter("@pCallerRelationID", System.Data.SqlDbType.Int, ds.Tables[0].Rows[0]["CallerRelationID"]);
                        sqlProxy.AddParameter("@pCallerPhone", System.Data.SqlDbType.NVarChar, ds.Tables[0].Rows[0]["CallerPhone"]);
                        sqlProxy.AddParameter("@pCallDateTime", System.Data.SqlDbType.DateTime, ds.Tables[0].Rows[0]["CallDate"]);
                        sqlProxy.AddParameter("@pMarkAsDeleted", System.Data.SqlDbType.Bit, ds.Tables[0].Rows[0]["MarkAsDeleted"]);
                        sqlProxy.AddParameter("@pIsCallerPatient", System.Data.SqlDbType.Bit, ds.Tables[0].Rows[0]["IsCallerPatient"]);
                        sqlProxy.AddParameter("@pUserID", System.Data.SqlDbType.Int, ds.Tables[0].Rows[0]["UserID"]);
                        sqlProxy.AddParameter("@pGardaStationID", System.Data.SqlDbType.Int, ds.Tables[0].Rows[0]["GardaStationID"]);
                        sqlProxy.AddParameter("@pIsGardaMember", System.Data.SqlDbType.Bit, ds.Tables[0].Rows[0]["IsGardaMember"]);

                        sqlProxy.ExecuteNonQuery(out errorText);

                        if (errorText != "")
                            throw new Exception("Exception while saving appointments against patient (" + objPersonalDetails.PatientID + ")" + Environment.NewLine + errorText);

                        if (IDParameter.Value != null && IDParameter.Value != DBNull.Value)
                            callID = (int)IDParameter.Value;
                    }

                    if (ds != null && ds.Tables.Count > 1)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            sqlProxy.AddCommand("[dbo].[uspInsertVisit]", System.Data.CommandType.StoredProcedure);
                            sqlProxy.AddParameter("@pVisitID", System.Data.SqlDbType.Int, 0);
                            sqlProxy.AddParameter("@pPatientID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["PatientID"]);
                            sqlProxy.AddParameter("@pVisitDate", System.Data.SqlDbType.DateTime, ds.Tables[1].Rows[i]["VisitDate"]);
                            sqlProxy.AddParameter("@pVisitStatusID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["VisitStatusID"]);
                            sqlProxy.AddParameter("@pIsLocked", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsLocked"]);
                            sqlProxy.AddParameter("@pLockedBy", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["LockedBy"]);
                            sqlProxy.AddParameter("@pPriorityID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["PriorityID"]);
                            sqlProxy.AddParameter("@pCallType", System.Data.SqlDbType.Int, GetCallTypeID(objPersonalDetails.CaseType));
                            sqlProxy.AddParameter("@pIsCallerPatient", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsCallerPatient"]);
                            sqlProxy.AddParameter("@pCallername", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["Callername"]);
                            sqlProxy.AddParameter("@pCallerRelationID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["CallerRelationID"]);
                            sqlProxy.AddParameter("@pCallerPhone", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["callbacknumber"]);
                            sqlProxy.AddParameter("@pCallTypeComments", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["CallTypeComments"]);
                            sqlProxy.AddParameter("@pPriorityComments", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["PriorityComments"]);
                            sqlProxy.AddParameter("@pSymptomComments", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["SymptomComments"]);
                            sqlProxy.AddParameter("@pIsFinishedFromCallTaker", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsFinishedFromCallTaker"]);
                            sqlProxy.AddParameter("@pCancelReason", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["CancelReason"]);
                            sqlProxy.AddParameter("@pCallID", System.Data.SqlDbType.Int, callID);
                            sqlProxy.AddParameter("@pIsWalkinPatient", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsWalkinPatient"]);

                            sqlProxy.AddParameter("@pNonTriageReasonID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["NonTriageReasonID"]);
                            sqlProxy.AddParameter("@pNurseID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["LockedBy"]);
                            sqlProxy.AddParameter("@pIsTestCase", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsTestCase"]);
                            sqlProxy.AddParameter("@pIsSaveAndTriage", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsSaveAndTriage"]);
                            sqlProxy.AddParameter("@pGardaStationID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["GardaStationID"]);
                            sqlProxy.AddParameter("@pIsGardaMember", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsGardaMember"]);
                            System.Data.SqlClient.SqlParameter IDParameter = sqlProxy.AddOutputParameter("@poutVisitID", System.Data.SqlDbType.Int);

                            sqlProxy.AddParameter("@pIsSendOutcometoOwnGP", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsSendOutcometoOwnGP"]);
                            sqlProxy.AddParameter("@pIsUnknownVisit", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsUknownVisit"]);
                            sqlProxy.AddParameter("@pTrCenterID", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["colTrCenterID"]);
                            sqlProxy.AddParameter("@pAge", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["Age"]);
                            sqlProxy.AddParameter("@pHomeNo", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["HomeNo"]);
                            sqlProxy.AddParameter("@pMobileNo", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["MobileNo"]);
                            sqlProxy.AddParameter("@pOtherPhoneNo", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["OtherPhoneNo"]);
                            sqlProxy.AddParameter("@pCallbackNo", System.Data.SqlDbType.NVarChar, ds.Tables[1].Rows[i]["CallbackNo"]);
                            sqlProxy.AddParameter("@pRegistrationType", System.Data.SqlDbType.Int, ds.Tables[1].Rows[i]["RegType"]);

                            sqlProxy.AddParameter("@pIsAgeUnknown", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsAgeUnknown"]);
                            sqlProxy.AddParameter("@pIsAgeAppoximate", System.Data.SqlDbType.Bit, ds.Tables[1].Rows[i]["IsAgeAppoximate"]);

                            dsResult = sqlProxy.Execute(out errorText);

                            if (errorText != "")
                                throw new Exception("Exception while saving visits against patient (" + objPersonalDetails.PatientID + ")" + Environment.NewLine + errorText);

                            if (dsResult != null && dsResult.Tables.Count > 0)
                            {
                                for (int k = 0; k < dsResult.Tables[1].Rows.Count; k++)
                                {
                                    DataRow dr = dtNurses.NewRow();
                                    if (dsResult.Tables[1].Rows[k]["NurseID"] != DBNull.Value)
                                    {
                                        dr["NurseID"] = dsResult.Tables[1].Rows[k]["NurseID"];
                                        //EmergencyCaseNurseId = Convert.ToInt32(dsResult.Tables[1].Rows[k]["NurseID"]);
                                    }
                                    if (dsResult.Tables[1].Rows[k]["UserName"] != DBNull.Value)
                                        dr["UserName"] = dsResult.Tables[1].Rows[k]["UserName"];
                                    if (dsResult.Tables[1].Rows[k]["CaseNo"] != DBNull.Value)
                                        dr["CaseNo"] = dsResult.Tables[1].Rows[k]["CaseNo"];
                                    if (dsResult.Tables[1].Rows[k]["CallID"] != DBNull.Value)
                                        dr["CallID"] = dsResult.Tables[1].Rows[k]["CallID"];
                                    if (dsResult.Tables[1].Rows[k]["VisitID"] != DBNull.Value)
                                        dr["VisitID"] = dsResult.Tables[1].Rows[k]["VisitID"];

                                    if (dsResult.Tables[1].Rows[k]["PatientName"] != DBNull.Value)
                                        dr["PatientName"] = dsResult.Tables[1].Rows[k]["PatientName"];
                                    if (dsResult.Tables[1].Rows[k]["Priority"] != DBNull.Value)
                                        dr["Priority"] = dsResult.Tables[1].Rows[k]["Priority"];
                                    if (dsResult.Tables[1].Rows[k]["PriorityID"] != DBNull.Value)
                                        dr["PriorityID"] = dsResult.Tables[1].Rows[k]["PriorityID"];
                                    if (dsResult.Tables[1].Rows[k]["PatientID"] != DBNull.Value)
                                        dr["PatientID"] = dsResult.Tables[1].Rows[k]["PatientID"];

                                    if (dsResult.Tables[1].Rows[k]["Extension"] != DBNull.Value)
                                        dr["Extension"] = dsResult.Tables[1].Rows[k]["Extension"];
                                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsResult.Tables[0].Rows[0]["Alert"] != DBNull.Value)
                                            dr["IsAvalible"] = dsResult.Tables[0].Rows[0]["Alert"];
                                        else
                                            dr["IsAvalible"] = 0;
                                    }
                                    else
                                        dr["IsAvalible"] = 0;

                                    if (dsResult.Tables[1].Rows[k]["IsMultiple"] != DBNull.Value)
                                        dr["IsMultiple"] = Convert.ToBoolean(dsResult.Tables[1].Rows[k]["IsMultiple"]);
                                    if (dsResult.Tables[1].Rows[k]["Flag"] != DBNull.Value)
                                        dr["Flag"] = Convert.ToBoolean(dsResult.Tables[1].Rows[k]["Flag"]);
                                    if (dsResult.Tables[1].Rows[k]["NurseName"] != DBNull.Value)
                                        dr["NurseName"] = Convert.ToString(dsResult.Tables[1].Rows[k]["NurseName"]);
                                    dtNurses.Rows.Add(dr);
                                }

                                if (dsResult.Tables[2] != null && dsResult.Tables[2].Rows.Count > 0)
                                {
                                    StrCaseNos += dsResult.Tables[2].Rows[0][0] + ",";
                                    StrVisitIds += dsResult.Tables[2].Rows[0][1] + ",";
                                }
                            }
                        }
                        dsNurses.Tables.Add(dtNurses);

                    }

                    dtCaseNos.Columns.Add("CaseNumber");
                    DataRow row = dtCaseNos.NewRow();
                    row["CaseNumber"] = StrCaseNos;
                    dtCaseNos.Rows.Add(row);

                    DataTable dtVisitAddress = new DataTable();
                    dtVisitAddress.Columns.Add("VisitID", typeof(string));
                    dtVisitAddress.Rows.Add(StrVisitIds);

                    dsNurses.Tables.Add(dtCaseNos);
                    dsNurses.Tables.Add(dtVisitAddress);
                }
                catch (Exception exp)
                {
                    throw exp;
                }

                return dsNurses;
            }
        }
        
        public static DataSet MakeVisits(int PatientID, PersonalDetails objPersonalDetails)
        {
            DataSet ds = new DataSet();
            DateTime VisitDate = DateTime.Now;
            DataTable dtCall = new DataTable();
            #region Add Columns to Table
            dtCall.Columns.Add("CallID");
            dtCall.Columns.Add("Callername");
            dtCall.Columns.Add("CallerRelationID");
            dtCall.Columns.Add("CallerPhone");
            dtCall.Columns.Add("CallDate");
            dtCall.Columns.Add("UserID");
            dtCall.Columns.Add("IsCallerPatient");
            dtCall.Columns.Add("GardaStationID");
            dtCall.Columns.Add("IsGardaMember");
            dtCall.Columns.Add("MarkAsDeleted");
            #endregion        
            DataRow drCall = dtCall.NewRow();
            DataTable dt = new DataTable();
            #region Add Columns to Table
            dt.Columns.Add("VisitID");
            dt.Columns.Add("PatientID");
            dt.Columns.Add("VisitDate");
            dt.Columns.Add("EmergencyID");
            dt.Columns.Add("AppointmentID");
            dt.Columns.Add("VisitStatusID");
            dt.Columns.Add("IsLocked");
            dt.Columns.Add("LockedBy");
            dt.Columns.Add("PriorityID");
            dt.Columns.Add("CallType");
            dt.Columns.Add("IsCallerPatient");
            dt.Columns.Add("Callername");
            dt.Columns.Add("CallerRelationID");
            dt.Columns.Add("CallerPhone");
            dt.Columns.Add("CallTypeComments");
            dt.Columns.Add("PriorityComments");
            dt.Columns.Add("SymptomComments");
            dt.Columns.Add("IsFinishedFromCallTaker");
            dt.Columns.Add("IsNewCase");
            dt.Columns.Add("CancelReason");
            dt.Columns.Add("IsWalkinPatient");
            dt.Columns.Add("NonTriageReasonID");
            dt.Columns.Add("Symptoms");
            dt.Columns.Add("IsTestCase");
            dt.Columns.Add("IsUknownVisit");
            dt.Columns.Add("IsSaveAndTriage");
            dt.Columns.Add("GardaStationID");
            dt.Columns.Add("IsGardaMember");
            dt.Columns.Add("CallBackNumber");
            dt.Columns.Add("IsSendOutcometoOwnGP");
            dt.Columns.Add("colTrCenterID");
            dt.Columns.Add("Age");
            dt.Columns.Add("HomeNo");
            dt.Columns.Add("MobileNo");
            dt.Columns.Add("OtherPhoneNo");
            dt.Columns.Add("CallbackNo");
            dt.Columns.Add("RegType");
            dt.Columns.Add("IsAgeUnknown");
            dt.Columns.Add("IsAgeAppoximate");
            #endregion           

            #region Assign Values to DataRow
            drCall["CallID"] = 0;
            drCall["IsCallerPatient"] = false;
            drCall["Callername"] = "Not Provided  in sheet";
            drCall["CallerRelationID"] = DBNull.Value;
            drCall["CallerPhone"] = objPersonalDetails.CallBackNumber;
            drCall["UserID"] = GlobalData.UserID;
            drCall["CallDate"] = VisitDate.ToString("s");
            drCall["MarkAsDeleted"] = false;
            drCall["GardaStationID"] = DBNull.Value;
            drCall["IsGardaMember"] = false;
            dtCall.Rows.Add(drCall);
            #endregion

            DataRow dr = dt.NewRow();
            #region Assign Values to DataRow
            dr["VisitID"] = 0;
            VisitDate = DateTime.Now;
            dr["PatientID"] = PatientID;
            dr["VisitDate"] = VisitDate.ToString("s");
            dr["EmergencyID"] = DBNull.Value; //// not using in sp
            dr["AppointmentID"] = DBNull.Value;
            dr["VisitStatusID"] = Convert.ToInt32(AppointmentStatus.Triage);
            dr["IsLocked"] = DBNull.Value;
            dr["LockedBy"] =  GlobalData.UserID;
            dr["PriorityID"] = GetPriorityID(objPersonalDetails.Priority);
            dr["CallType"] = DBNull.Value;
            dr["IsCallerPatient"] = false;
            dr["Callername"] = "Not Provided in sheet";
            dr["CallerRelationID"] = 0;
            dr["CallerPhone"] = objPersonalDetails.OtherNumber;
            dr["CallBackNumber"] = objPersonalDetails.CallBackNumber;
            dr["CallTypeComments"] = "GPOOH Load Testing";
            dr["PriorityComments"] = "GPOOH Load Testing";
            dr["SymptomComments"] = "GPOOH Load Testing";
            dr["IsFinishedFromCallTaker"] = false;
            dr["Symptoms"] = objPersonalDetails.NurseSymptoms;
            dr["IsNewCase"] = true;
            dr["CancelReason"] = "";
            dr["IsWalkinPatient"] = DBNull.Value;
            dr["NonTriageReasonID"] = DBNull.Value;
            dr["IsTestCase"] = true;
            dr["IsUknownVisit"] = DBNull.Value;
            dr["IsSendOutcometoOwnGP"] = false;
            dt.Rows.Add(dr);
            dr["IsSaveAndTriage"] = false;
            dr["GardaStationID"] = DBNull.Value;
            dr["IsGardaMember"] = DBNull.Value;

            dr["Age"] = GetAge(objPersonalDetails.Age);
            dr["HomeNo"] = objPersonalDetails.OtherNumber;
            dr["MobileNo"] = objPersonalDetails.MobileNumber;
            dr["OtherPhoneNo"] = objPersonalDetails.OtherPhoneNumber;
            dr["CallbackNo"] = objPersonalDetails.CallBackNumber;
            dr["RegType"] = DBNull.Value;
            dr["IsAgeUnknown"] = DBNull.Value;
            dr["IsAgeAppoximate"] = DBNull.Value;
            #endregion
           
            ds.Tables.Add(dtCall);
            ds.Tables.Add(dt);
            DataTable dtVisitAddress = new DataTable();
            return ds;
        }

        private static int SavePatientAppointments(Nurse objDetails, int visitID)
        {
            string errorText = "";
            int appointmentID = 0;

            try
            {
                int caseTypeID = GetCallTypeID(objDetails.CaseType);
                int areaID = GetTreatmentCeterID(objDetails.TreatmentCentre);
                if (areaID > 0)
                {
                    if (caseTypeID == Convert.ToInt32(CallTypes.TreatmentCentre)) 
                    {
                        using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
                        {
                            sqlProxy.AddCommand("[dbo].[InsertUpdatePatientAppointments]", System.Data.CommandType.StoredProcedure);
                            sqlProxy.AddParameter("@pAppointmentID", System.Data.SqlDbType.Int, 0);
                            sqlProxy.AddParameter("@pVisitID", System.Data.SqlDbType.Int, visitID);
                            sqlProxy.AddParameter("@pTreatmentcenterID", System.Data.SqlDbType.Int, areaID);
                            sqlProxy.AddParameter("@pGPID", System.Data.SqlDbType.Int, DBNull.Value);

                            string strAppointmentStartTime = objDetails.AppointmentTime.Trim();
                            if (strAppointmentStartTime == "")
                                throw new Exception("Appointment Time can not be empty while saving appointment against patient(" + objDetails.PatientID + ")" + Environment.NewLine + errorText);

                            DateTime objDTStartTime = Convert.ToDateTime(strAppointmentStartTime);
                            DateTime objDTEndTime = objDTStartTime.AddMinutes(30);

                            sqlProxy.AddParameter("@pStartTime", System.Data.SqlDbType.DateTime, objDTStartTime);
                            sqlProxy.AddParameter("@pEndTime", System.Data.SqlDbType.DateTime, objDTEndTime);
                            //sqlProxy.AddParameter("@pTreatmentCenterDirection", System.Data.SqlDbType.VarChar, "");
                            sqlProxy.AddParameter("pComments", System.Data.SqlDbType.VarChar, "Not Provided");
                            sqlProxy.AddParameter("@pIsCompleteByDispatcher", System.Data.SqlDbType.Bit, false);
                            sqlProxy.AddParameter("pUserID", System.Data.SqlDbType.Int, GlobalData.UserID);
                            sqlProxy.AddParameter("@pIsMultiCall", System.Data.SqlDbType.Bit, 0);
                            System.Data.SqlClient.SqlParameter outParameter = sqlProxy.AddOutputParameter("@pOutput", System.Data.SqlDbType.Int);

                            sqlProxy.ExecuteNonQuery(out errorText);
                            if (errorText != "")
                                throw new Exception("Exception while saving appointment against patient(" + objDetails.PatientID + ")" + Environment.NewLine + errorText);

                            if (outParameter.Value != null && outParameter.Value != DBNull.Value)
                                appointmentID = (int)outParameter.Value;

                        }
                    }
                    else if (caseTypeID == Convert.ToInt32(CallTypes.HomeVisit) || caseTypeID == Convert.ToInt32(CallTypes.DoctorAdvice))
                    {
                        using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
                        {
                            sqlProxy.AddCommand("[dbo].[uspAssignTreatmentCenter]", System.Data.CommandType.StoredProcedure);
                            sqlProxy.AddParameter("@pVisitID", System.Data.SqlDbType.Int, visitID);
                            sqlProxy.AddParameter("@pTreatmentCenterID", System.Data.SqlDbType.Int, areaID);
                            sqlProxy.AddParameter("@pTreatmentCenterStatus", System.Data.SqlDbType.Bit, false);
                            sqlProxy.AddParameter("pUserID", System.Data.SqlDbType.Int, GlobalData.UserID);
                            sqlProxy.AddParameter("@pAppointmentDate", System.Data.SqlDbType.DateTime, DateTime.Now);

                            sqlProxy.ExecuteNonQuery(out errorText);
                            if (errorText != "")
                                throw new Exception("Exception while Assigning treatment center against patient(" + objDetails.PatientID + ")" + Environment.NewLine + errorText);
                        }
                    }
                }
                else
                    Logger.WriteActivity("Common", "SavePatientAppointments", "Treatment Centre not found for patient (" + objDetails.PatientID + ")");
            }
            catch (Exception ex)
            {
                Logger.WriteActivity("Common", "SavePatientAppointments", ex.ToString());
            }

            return appointmentID;
        }

        public static void SaveNurseConsultation(Nurse objDetails, ref int count)
        {
            try
            {
                string errorText = "";
                using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
                {
                    int visitid = GetVisit(objDetails.PatientID);

                    if (visitid == 0)
                        throw new Exception("Exception while saving nurse consultation against patient(" + objDetails.PatientID + "). visitid can not be 0" + Environment.NewLine);
                    //int areaID = GetTreatmentCeterID(objDetails.TreatmentCentre);
                    //if (areaID > 0)
                    //{
                    //    sqlProxy.AddCommand("[DATA].[uspInsertAppointment]", System.Data.CommandType.StoredProcedure);
                    //    sqlProxy.AddParameter("@pvisitid", System.Data.SqlDbType.Int, GetVisit(objDetails.PatientID));
                    //    sqlProxy.AddParameter("@puserid", System.Data.SqlDbType.Bit, GlobalData.UserID);
                    //    sqlProxy.AddParameter("@pAppointmentTime", System.Data.SqlDbType.DateTime, objDetails.AppointmentTime);
                    //    sqlProxy.AddParameter("@pAppTreatmentCenterID", System.Data.SqlDbType.Int, areaID);
                    //    sqlProxy.ExecuteNonQuery(out errorText);
                    //}
                    //else
                    //    Logger.WriteActivity("Common", "SaveNurseConsultation", "Treatment Centre not found for patient (" + objDetails.PatientID + ")");
                    int appointmentID = SavePatientAppointments(objDetails, visitid);

                    sqlProxy.AddCommand("[dbo].[uspUpdateNurseConsultation]", System.Data.CommandType.StoredProcedure);
                    sqlProxy.AddParameter("@pNurseConsultationID", System.Data.SqlDbType.Int, 0);


                    sqlProxy.AddParameter("@pVisitID", System.Data.SqlDbType.Int, visitid);
                    sqlProxy.AddParameter("@pConsultedBY", System.Data.SqlDbType.Int, GlobalData.UserID);
                    sqlProxy.AddParameter("@pConsultationStartTime", System.Data.SqlDbType.DateTime, DateTime.Now.AddHours(2.0));
                    sqlProxy.AddParameter("@pConsultationEndTime", System.Data.SqlDbType.DateTime, DateTime.Now);

                    sqlProxy.AddParameter("@pHistory", System.Data.SqlDbType.NVarChar, "Not provided");
                    sqlProxy.AddParameter("@pExamination", System.Data.SqlDbType.NVarChar, "Not provided");
                    sqlProxy.AddParameter("@pTreatment", System.Data.SqlDbType.NVarChar, "Not provided");
                    sqlProxy.AddParameter("@pDiagnose", System.Data.SqlDbType.NVarChar, "Not provided");
                    sqlProxy.AddParameter("@pByNurse", System.Data.SqlDbType.Bit, true);
                    sqlProxy.AddParameter("@pInsertedBy", System.Data.SqlDbType.Int, GlobalData.UserID);
                    sqlProxy.AddParameter("@pInsertedAt", System.Data.SqlDbType.DateTime, DateTime.Now);
                    sqlProxy.AddParameter("@pMimimumTriage", System.Data.SqlDbType.Bit, true);
                    ////sqlProxy.AddParameter("@pNurseAssessmentHistoryID", System.Data.SqlDbType.Int, 0);
                    sqlProxy.AddParameter("@psymptoms", System.Data.SqlDbType.VarChar, objDetails.NurseSymptoms);

                    sqlProxy.AddParameter("@pSymptomsIDs", System.Data.SqlDbType.VarChar, DBNull.Value);

                    sqlProxy.AddParameter("@pcharacterstics", System.Data.SqlDbType.VarChar, "Not provided");
                    sqlProxy.AddParameter("@ponset", System.Data.SqlDbType.VarChar, "Not provided");
                    sqlProxy.AddParameter("@plocation", System.Data.SqlDbType.VarChar, "Not provided");
                    sqlProxy.AddParameter("@paggravating", System.Data.SqlDbType.VarChar, "Not provided");
                    sqlProxy.AddParameter("@prelieving", System.Data.SqlDbType.VarChar, "Not provided");
                    sqlProxy.AddParameter("@pCurrentMedication", System.Data.SqlDbType.VarChar, "Not provided");
                    sqlProxy.AddParameter("@pPatientAdvisedToBringPastHistory", System.Data.SqlDbType.Bit, true);

                    bool IsClosed = false;

                    if (string.Equals(objDetails.Sending, "Despatch", StringComparison.InvariantCultureIgnoreCase))
                        GlobalData.ObjAppointmentStatus = AppointmentStatus.WaitingToDespatch;
                    else if (string.Equals(objDetails.Sending, "Transmit", StringComparison.InvariantCultureIgnoreCase))
                        GlobalData.ObjAppointmentStatus = AppointmentStatus.SentToTreatmentCentre;
                    else
                    {
                        GlobalData.ObjAppointmentStatus = AppointmentStatus.Closed;
                        IsClosed = true;
                    }

                    sqlProxy.AddParameter("@pVisitStatus", System.Data.SqlDbType.Int, Convert.ToInt32(GlobalData.ObjAppointmentStatus));
                    sqlProxy.AddParameter("@pOutComeID", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pComments", System.Data.SqlDbType.NVarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pIsNurse", System.Data.SqlDbType.Bit, IsClosed);
                    sqlProxy.AddParameter("@pPriorityLogID", System.Data.SqlDbType.Int, 0);
                    sqlProxy.AddParameter("@pPriorityDate", System.Data.SqlDbType.DateTime, DateTime.Now);
                    sqlProxy.AddParameter("@pPriorityID", System.Data.SqlDbType.Int, GetPriorityID(objDetails.Priority));
                    sqlProxy.AddParameter("@pPriorityComments", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pCallTypeLogID", System.Data.SqlDbType.Int, 0);
                    sqlProxy.AddParameter("@pCallTypeDate", System.Data.SqlDbType.DateTime, DateTime.Now);


                    sqlProxy.AddParameter("@pCallTypeID", System.Data.SqlDbType.Int, GetCallTypeID(objDetails.CaseType));

                    sqlProxy.AddParameter("@pCallTypeComments", System.Data.SqlDbType.VarChar, DBNull.Value);

                    sqlProxy.AddParameter("@pCallType", System.Data.SqlDbType.Int, GetCallTypeID(objDetails.CaseType));
                    sqlProxy.AddParameter("@pIsClosed", System.Data.SqlDbType.Bit, IsClosed);
                    sqlProxy.AddParameter("@pStatus", System.Data.SqlDbType.NVarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pCurrentAlergy", System.Data.SqlDbType.Int, 3);
                    sqlProxy.AddParameter("@pOtherAlergy", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.ExecuteNonQuery(out errorText);

                    if (errorText != "")
                        throw new Exception("Exception while saving nurse consultation against patient(" + objDetails.PatientID + ") and visitid " + visitid + Environment.NewLine + errorText);

                    count++;
                    Logger.WriteActivity("Common", "SaveNurseConsultation", "saving nurse consultation record of patient (" + objDetails.PatientID + ") and visitid " + visitid);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteActivity("Common", "SaveNurseConsultation", ex.ToString());
            }
        }

        public static void SaveDoctorConsultation(Doctor objDetails, ref int count)
        {
            try
            {
                string errorText = "";
                DataSet ds = null;
                using (SqlServerProxy sqlProxy = new SqlServerProxy(GlobalData.GPOOHLTConnectionString))
                {
                    sqlProxy.AddCommand("[Data].[uspGetNurseInformation]", System.Data.CommandType.StoredProcedure);
                    sqlProxy.AddParameter("@pVisitid", System.Data.SqlDbType.Int, GetVisit(objDetails.PatientID));
                    ds = sqlProxy.Execute(out errorText);

                    sqlProxy.AddCommand("[dbo].[uspSaveDrConsultation]", System.Data.CommandType.StoredProcedure);
                    sqlProxy.AddParameter("@pUserID", System.Data.SqlDbType.Int, GlobalData.UserID);
                    sqlProxy.AddParameter("@pVisitID", System.Data.SqlDbType.Int, GetVisit(objDetails.PatientID));

                    sqlProxy.AddParameter("@pNurseConsultationID", System.Data.SqlDbType.Int, 0);
                    sqlProxy.AddParameter("@pConsultationStartTime", System.Data.SqlDbType.DateTime, (ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0]["ConsultationStartTime"] : DBNull.Value);
                    //sqlProxy.AddParameter("@pConsultedBY", System.Data.SqlDbType.Int, (ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0]["ConsultedBY"] : DBNull.Value);
                    sqlProxy.AddParameter("@pConsultedBY", System.Data.SqlDbType.Int, GlobalData.UserID);
                    sqlProxy.AddParameter("@pConsultationEndTime", System.Data.SqlDbType.DateTime, (ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0]["ConsultationEndTime"] : DBNull.Value);
                    sqlProxy.AddParameter("@pExamination", System.Data.SqlDbType.NVarChar, (ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0]["Examination"] : DBNull.Value);
                    sqlProxy.AddParameter("@pTreatment", System.Data.SqlDbType.NVarChar, (ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0]["Treatment"] : DBNull.Value);
                    sqlProxy.AddParameter("@pDiagnose", System.Data.SqlDbType.NVarChar, (ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0]["diagnoses"] : DBNull.Value);

                    sqlProxy.AddParameter("@pCurrentMedication", System.Data.SqlDbType.NVarChar, objDetails.Medication);
                    sqlProxy.AddParameter("@pPriorityID", System.Data.SqlDbType.Int, GetPriorityID(objDetails.Priority));
                    sqlProxy.AddParameter("@pCallTypeID", System.Data.SqlDbType.Int, GetCallTypeID(objDetails.CaseType));
                    sqlProxy.AddParameter("@pIsClosed", System.Data.SqlDbType.Bit, 1);
                    sqlProxy.AddParameter("@pVisitStatus", System.Data.SqlDbType.Int, AppointmentStatus.Closed);
                    sqlProxy.AddParameter("@pPriorityLogID", System.Data.SqlDbType.Int, 0);
                    sqlProxy.AddParameter("@pCallTypeLogID", System.Data.SqlDbType.Int, 0);

                    sqlProxy.AddParameter("@pCunsultationHistory", System.Data.SqlDbType.NVarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pPresentingComplaints", System.Data.SqlDbType.NVarChar, DBNull.Value);
                    sqlProxy.AddParameter("@pID", System.Data.SqlDbType.Int, 0);
                    sqlProxy.AddParameter("@pOutComeID", System.Data.SqlDbType.VarChar, 11);
                    sqlProxy.AddParameter("@pOutComeComments", System.Data.SqlDbType.VarChar, "GPOOH Load Application");
                    sqlProxy.AddParameter("@pPriorityLogComments", System.Data.SqlDbType.VarChar, "GPOOH Load Application");
                    sqlProxy.AddParameter("@pCallTypeLogComments", System.Data.SqlDbType.VarChar, "GPOOH Load Application");
                    sqlProxy.AddParameter("@SendOutcometoOwnGP", System.Data.SqlDbType.Bit, 0);

                    sqlProxy.AddParameter("@SaveExamTemplate", System.Data.SqlDbType.Bit, 0);
                    sqlProxy.AddParameter("@Generalappearance", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Temperature", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@HeartRate", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@BPSys", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@BPDia", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@RespirationRate", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Urine", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Weight", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@PEFR", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Comments1", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Chest", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@CVS", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Abdominal", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@ENT", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@MSK", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Neuro", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Skin", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.AddParameter("@Comments2", System.Data.SqlDbType.VarChar, DBNull.Value);
                    sqlProxy.ExecuteNonQuery(out errorText);

                    if (errorText != "")
                        throw new Exception("Exception while saving doctor consultation against patient(" + objDetails.PatientID + ") " + Environment.NewLine + errorText);

                    count++;
                    Logger.WriteActivity("Common", "SaveDoctorConsultation", "saving doctor consultation record of patient (" + objDetails.PatientID + ") ");

                }
            }
            catch (Exception ex)
            {
                Logger.WriteActivity("Common", "SaveDoctorConsultation", ex.ToString());
            }
            
        }

        
        public static DataTable PatientAddress(PersonalDetails objPersonalDetails)
        {
            DataTable dtAddress = null;
            if (!string.IsNullOrEmpty(objPersonalDetails.HomeAddress))
            {
                dtAddress = new DataTable();
                dtAddress.Columns.Add("VisitAddressID", typeof(int));
                dtAddress.Columns.Add("VisitID", typeof(int));
                dtAddress.Columns.Add("AddressTypeID", typeof(int));
                dtAddress.Columns.Add("AddressLine1", typeof(string));
                dtAddress.Columns.Add("AddressLine2", typeof(string));
                dtAddress.Columns.Add("AddressLine3", typeof(string));
                dtAddress.Columns.Add("PostCode", typeof(string));
                dtAddress.Columns.Add("IsCorrespondence", typeof(bool));
                dtAddress.Columns.Add("AddressLine4", typeof(string));
                dtAddress.Columns.Add("AddressLine5", typeof(string));
                dtAddress.Columns.Add("AddressLine6", typeof(string));
                dtAddress.Columns.Add("CreatedBy", typeof(int));
                dtAddress.Columns.Add("AreaID", typeof(int));
                dtAddress.Columns.Add("AddressExpiration", typeof(DateTime));
                dtAddress.Columns.Add("IsUnknown", typeof(bool));


                //string[] addresType = objPersonalDetails.HomeAddress.Trim().Split(',');
                //string[] L1 = objPersonalDetails.pAddressLine1.Trim().Split('|');
                //string[] L2 = objPersonalDetails.pAddressLine2.Trim().Split('|');
                //string[] L3 = objPersonalDetails.pAddressLine3.Trim().Split('|');
                //string[] L4 = objPersonalDetails.pAddressLine4.Trim().Split('|');
                //string[] L5 = objPersonalDetails.pAddressLine5.Trim().Split('|');
                //string[] L6 = objPersonalDetails.pAddressLine6.Trim().Split('|');
                //string[] PostCode = objPersonalDetails.pPostCode.Trim().Split(',');
                //string[] Correspondence = objPersonalDetails.pICorrespondenceAddress.Trim().Split(',');
                //string[] IsUnknown = objPersonalDetails.pIsUnknownAddress.Split(',');

                string[] addresArray = objPersonalDetails.HomeAddress.Trim().Split(',');

                string AddressLine1 = addresArray.Length > 0 ? addresArray[0] : string.Empty;
                string AddressLine2 = addresArray.Length > 1 ? addresArray[1] : string.Empty;
                string AddressLine3 = addresArray.Length > 2 ? addresArray[2] : string.Empty;
                string AddressLine4 = addresArray.Length > 3 ? addresArray[3] : string.Empty;
                string AddressLine5 = addresArray.Length > 4 ? addresArray[4] : string.Empty;
                string AddressLine6 = addresArray.Length > 5 ? addresArray[5] : string.Empty;


                    DataRow drAddress = dtAddress.NewRow();
                    drAddress["AddressTypeID"] = AddressType.Home;
                    drAddress["AddressLine1"] = AddressLine1;
                    drAddress["AddressLine2"] = AddressLine2;
                    drAddress["AddressLine3"] = AddressLine3;
                    drAddress["AddressLine4"] = AddressLine4;
                    drAddress["AddressLine5"] = AddressLine5;
                    drAddress["AddressLine6"] = AddressLine6;
                    drAddress["AreaID"] = GetTreatmentCeterID(objPersonalDetails.TreatmentCentre);
                    drAddress["PostCode"] = DBNull.Value;

                    //if (Correspondence[i].ToString().Equals("1"))
                    //{
                    //    drAddress["IsCorrespondence"] = true;
                    //}
                    //else
                    drAddress["IsCorrespondence"] = true;
                    
                    //if (IsUnknown[i].ToString().Equals(1))
                    //{
                    //    drAddress["IsUnknown"] = true;
                    //}
                    //else
                        drAddress["IsUnknown"] = false;


                    //if (addresType[i] == "1")
                    //{
                    //    drAddress["AddressExpiration"] = DBNull.Value;
                    //}

                    //if (addresType[i] == "2")
                    //{
                        drAddress["AddressExpiration"] = DateTime.Now.AddHours(12);
                    ////}
                    //if (objPersonalDetails.pVisitId.HasValue)
                        drAddress["VisitID"] = GetVisit(objPersonalDetails.PatientID);
                    //else
                    //    drAddress["VisitID"] = DBNull.Value;
                    //if (objPersonalDetails.pVisitAddressID.HasValue)
                    //    drAddress["VisitAddressID"] = objPersonalDetails.pVisitAddressID;
                    //else
                    drAddress["VisitAddressID"] = 0;
                    drAddress["CreatedBy"] = GlobalData.UserID;
                    dtAddress.Rows.Add(drAddress);
                    dtAddress.AcceptChanges();
                    drAddress = null;
            }
            return dtAddress;
        }
    
        public static DataTable PatientContact(PersonalDetails objPersonalDetails)
        {
            #region tblPatientContact
            DataTable dtContact = null;

                dtContact = new DataTable();
                dtContact.Columns.Add("ContactID", typeof(int));
                dtContact.Columns.Add("ContactNumber", typeof(string));
                dtContact.Columns.Add("ContactTypeID", typeof(int));
                dtContact.Columns.Add("AddressBookID", typeof(int));
                dtContact.Columns.Add("UnKnownContact", typeof(int));
                dtContact.Columns.Add("CorrespondenceContact", typeof(int));
                dtContact.Columns.Add("SpeedDial", typeof(int));
                dtContact.Columns.Add("CreatedBy", typeof(int));


                //string[] ContactTypeID =  ; //To be change in next release //objPersonalDetails.pContactTypePhoneID.Trim().Split(',');
                //string[] ContactPhone = objPersonalDetails.pContactPhone.Trim().Split(',');
                //string[] IsUnknownPhone = objPersonalDetails.pIsUnknownPhone.Trim().Split(',');
                //string[] Correspondence = objPersonalDetails.pIsCorrespondencePhone.Trim().Split(',');


                //for (int i = 0; i < ContactTypeID.Length; i++)
                //{
                    DataRow drContact = dtContact.NewRow();
                    drContact["ContactTypeID"] = 12; // Callback num.. To be change in next release
                    drContact["ContactNumber"] = objPersonalDetails.CallBackNumber;
                    drContact["UnKnownContact"] = 0; // Added by Noman
                    drContact["CorrespondenceContact"] = 0;
                    drContact["SpeedDial"] = DBNull.Value;
                    drContact["CreatedBy"] = GlobalData.UserID;
                    drContact["ContactID"] = 0;
                    drContact["AddressBookID"] = objPersonalDetails.PatientID;


                    DataRow drContact2 = dtContact.NewRow();
                    drContact2["ContactTypeID"] = 3; // Mobile num.. To be change in next release
                    drContact2["ContactNumber"] = objPersonalDetails.MobileNumber;
                    drContact2["UnKnownContact"] = 0; // Added by Noman
                    drContact2["CorrespondenceContact"] =0;
                    drContact2["SpeedDial"] = DBNull.Value;
                    drContact2["CreatedBy"] = GlobalData.UserID;
                    drContact2["ContactID"] = 0;
                    drContact2["AddressBookID"] = objPersonalDetails.PatientID;


                    DataRow drContact3 = dtContact.NewRow();
                    drContact3["ContactTypeID"] = 4; // phone num.. To be change in next release
                    drContact3["ContactNumber"] = objPersonalDetails.OtherPhoneNumber;
                    drContact3["UnKnownContact"] = 0; // Added by Noman
                    drContact3["CorrespondenceContact"] = 0;
                    drContact3["SpeedDial"] = DBNull.Value;
                    drContact3["CreatedBy"] = GlobalData.UserID;
                    drContact3["ContactID"] = 0;
                    drContact3["AddressBookID"] = objPersonalDetails.PatientID;


                    DataRow drContact4 = dtContact.NewRow();
                    drContact4["ContactTypeID"] =11; // other num.. To be change in next release
                    drContact4["ContactNumber"] = objPersonalDetails.OtherNumber;
                    drContact4["UnKnownContact"] = 0; // Added by Noman
                    drContact4["CorrespondenceContact"] = 0;
                    drContact4["SpeedDial"] = DBNull.Value;
                    drContact4["CreatedBy"] = GlobalData.UserID;
                    drContact4["ContactID"] = 0;
                    drContact4["AddressBookID"] = objPersonalDetails.PatientID;


                    dtContact.Rows.Add(drContact);



                    dtContact.AcceptChanges();
                    drContact = null;

                    return dtContact;


            #endregion
        }
    
    }
}
