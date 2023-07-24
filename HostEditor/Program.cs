using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace HostEditor
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                string HKLMWinNTCurrent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                string osBuild = Registry.GetValue(HKLMWinNTCurrent, "CurrentBuildNumber", "").ToString();
                if (Int32.Parse(osBuild) >= 5112) //Windows Vista 이상 OS부터 관리자 모드가 적용됨. 
                {
                    if (!AdministratorConfirmed())
                    {
                        ProcessStartInfo info = new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            FileName = Application.ExecutablePath,
                            WorkingDirectory = Environment.CurrentDirectory,
                            Verb = "runas"
                        };

                        Process.Start(info);
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                    }
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
        
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
          
        }


        #region["관리자 모드 실행 메소드"] 
        public static bool AdministratorConfirmed()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            bool administratorconfirm = false;
            if (identity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                administratorconfirm = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return administratorconfirm;
        }
        #endregion

    }
}
