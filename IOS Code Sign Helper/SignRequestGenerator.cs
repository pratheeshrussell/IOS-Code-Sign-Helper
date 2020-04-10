using System;
using System.IO;
using System.Windows.Forms;

namespace IOS_Code_Sign_Helper
{
    class SignRequestGenerator
    {
       public String projectPath = "";
       public String emailId = "";
       private String opensslConf = Directory.GetCurrentDirectory() + "/openssl/openssl.cnf";
       private String opensslFolder = Directory.GetCurrentDirectory() + "/openssl";
       public String generateCSR()
       {
            String cmd = "/C cd \"" + opensslFolder + "\" && " + "set OPENSSL_CONF=" + opensslConf + " && " +
                 "openssl req -new -key \"" + projectPath + "\\privateKey.key\"  -out \"" + projectPath + 
                 "\\SigningRequest.certSigningRequest\" -subj \"/emailAddress="+ emailId + "\"";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (!File.Exists(projectPath + "/SigningRequest.certSigningRequest")){
                return "failed";  }
            return "done";
       }

        public bool generatePrivateKey()
        {
            String cmd = "/C cd \"" + opensslFolder + "\" && " + "set OPENSSL_CONF=" + opensslConf + " && " +
                 "openssl genrsa -out \"" + projectPath + "\\privateKey.key\" 2048";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }

        public String copyCSR()
        {
            var fileSaveDialog1 = new SaveFileDialog();
            fileSaveDialog1.DefaultExt = "certSigningRequest";
            fileSaveDialog1.FileName = "SigningRequest";
            fileSaveDialog1.Filter = "certSigningRequest (*.certSigningRequest)|*.certSigningRequest";
            DialogResult result = fileSaveDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sourcefileName = projectPath + "/SigningRequest.certSigningRequest";
                string destfileName = fileSaveDialog1.FileName;
                System.IO.File.Copy(sourcefileName, destfileName, true);
                return "done";
            }
            return "";
        }


    }
}
