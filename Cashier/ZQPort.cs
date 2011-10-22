using System.Runtime.InteropServices;

namespace ZQPort
{
	public class CZQPort
			{
				[DllImport("ZQPortDll.dll", EntryPoint = "IsConnect", CallingConvention = CallingConvention.StdCall)]
				public static extern bool IsConnect(string strPort);
				[DllImport("ZQPortDll.dll", EntryPoint = "SendData", CallingConvention = CallingConvention.StdCall)]
				public static extern int SendData(string strPort, string psz, int nLen);
				[DllImport("ZQPortDll.dll", EntryPoint = "GetStatus", CallingConvention = CallingConvention.StdCall)]
				public static extern int GetStatus(string pszIP);
				[DllImport("ZQPortDll.dll", EntryPoint = "IsBusy", CallingConvention = CallingConvention.StdCall)]
				public static extern bool IsBusy(int nStatus);
				[DllImport("ZQPortDll.dll", EntryPoint = "IsPaperEnd", CallingConvention = CallingConvention.StdCall)]
				public static extern bool IsPaperEnd(int nStatus);
				[DllImport("ZQPortDll.dll", EntryPoint = "IsPause", CallingConvention = CallingConvention.StdCall)]
				public static extern bool IsPause(int nStatus);
				[DllImport("ZQPortDll.dll", EntryPoint = "IsCoverOpen", CallingConvention = CallingConvention.StdCall)]
				public static extern bool IsCoverOpen(int nStatus);
				[DllImport("ZQPortDll.dll", EntryPoint = "IsTempOver", CallingConvention = CallingConvention.StdCall)]
				public static extern bool IsTempOver(int nStatus);
				[DllImport("ZQPortDll.dll", EntryPoint = "GetUSBPort", CallingConvention = CallingConvention.StdCall)]
				public static extern int GetUSBPort();
				[DllImport("ZQPortDll.dll", EntryPoint = "GetCOMMax", CallingConvention = CallingConvention.StdCall)]
				public static extern int GetCOMMax();
				[DllImport("ZQPortDll.dll", EntryPoint = "GetLPTMax", CallingConvention = CallingConvention.StdCall)]
				public static extern int GetLPTMax();
				[DllImport("ZQPortDll.dll", EntryPoint = "SetComParam", CallingConvention = CallingConvention.StdCall)]
				public static extern bool SetComParam(string strPort, int nBaudrate, int nBitLen, char szParity, int nStopBit, int nFlowCtrl);
				[DllImport("ZQPortDll.dll", EntryPoint = "SetTimeout", CallingConvention = CallingConvention.StdCall)]
				public static extern bool SetTimeOut(string strPort, int nTimeout);
				[DllImport("ZQPortDll.dll", EntryPoint = "ShowCOMSettingDlg", CallingConvention = CallingConvention.StdCall)]
				public static extern int ShowCOMSettingDlg(string strPort);
				[DllImport("ZQPortDll.dll", EntryPoint = "ClosePort", CallingConvention = CallingConvention.StdCall)]
				public static extern bool ClosePort(string strPort);
			}
}
