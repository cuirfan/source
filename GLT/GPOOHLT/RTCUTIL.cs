using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GPOOHLT
{
    public class RTCUTIL
    {
        //private RTCClientProxy.RTCClientManager m_rtcClient;

        //private void ConnectToRTCServer()
        //{
        //    RTCClientManager.Serverip = Utils.Classes.GlobalData.RTCSeverIP;
        //    RTCClientManager.ServerPort = Utils.Classes.GlobalData.RTCServerPort;

        //    m_rtcClient = RTCClientProxy.RTCClientManager.GetInstance;
        //    m_rtcClient.ConnectRTCServer();
        //}

        /// <summary>
        /// callType 
        /// </summary>
        /// <param name="callType"> 1 for refresh despatcher queue, 2 for despatcher and nurse, 3 for nurse queue, 4 for receptionist queue, 5 for GP queue, 6 for gp and Receptionist, 7 for call taker and nurse
        /// 8 for only calltaker queue</param>
        /// <param name="isRefreshQueue">False, when CaseRefreshType is Arrived</param>
        //public static void RefreshOthers()
        //{
        //    RTCClientManager.Serverip = Utils.Classes.GlobalData.RTCSeverIP;
        //    RTCClientManager.ServerPort = Utils.Classes.GlobalData.RTCServerPort;
        //    RTCClientManager.Signature = Utils.Classes.GlobalData.UserName;

        //    RTCClientProxy.RTCClientManager rtcClient = RTCClientProxy.RTCClientManager.GetInstance;
        //    rtcClient.ConnectRTCServer();

        //    if (rtcClient.ConnectionStatus != RTCClientProxy.ClientStatus.Connected)
        //        return;

        //    RTCClientProxy.Classes.RefreshCase objCase = new RTCClientProxy.Classes.RefreshCase();

        //    objCase.IsRefreshCase = true;
        //    //if (isRefreshQueue)
        //    {
        //        //objCase.RefreshType = RTCClientProxy.CaseRefreshType.arrived;
        //        objCase.RefreshType = RTCClientProxy.CaseRefreshType.RefreshQueue;
        //        //objCase.VisitID = 0;
        //    }
        //    byte[] data = RTCClientProxy.Classes.PacketSerilizer.EncodeClass<RTCClientProxy.Classes.RefreshCase>(objCase, (byte)RTCClientProxy.MessageType.RefreshCase);
        //    string oError = string.Empty;
        //        try
        //        {
        //            {
        //                rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.All, rtcClient.RTCOnlineUserList, true, false, out oError);
        //                //rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.CallTaker_Despatcher, rtcClient.RTCOnlineUserList, false, false, out oError);
        //                //rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.CallTaker_Nurse_Despatcher, rtcClient.RTCOnlineUserList, false, false, out oError);
        //                //rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.Nurse_Despatcher, rtcClient.RTCOnlineUserList, false, false, out oError);

        //                //rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.Nurse, rtcClient.RTCOnlineUserList, false, false, out oError);
        //                //rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.Nurse_Despatcher, rtcClient.RTCOnlineUserList, false, false, out oError);
        //                //rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.CallTaker_Nurse, rtcClient.RTCOnlineUserList, false, false, out oError);
        //                //rtcClient.SendDataToPushServer(data, RTCClientProxy.eMachinetype.CallTaker, rtcClient.RTCOnlineUserList, false, false, out oError);
        //            }
                    
                    
                    
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //}

    }
}
