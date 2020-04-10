using System;
using System.IO;
using System.Windows.Forms;

namespace IOS_Code_Sign_Helper
{
    class P12Generator
    {
        public String projectPath = "";
        public String password = "";
        private String opensslConf = Directory.GetCurrentDirectory() + "/openssl/openssl.cnf";
        private String opensslFolder = Directory.GetCurrentDirectory() + "/openssl";

        public String importcerFile()
        {
            var fileSaveDialog1 = new OpenFileDialog();
            fileSaveDialog1.DefaultExt = "cer";
            fileSaveDialog1.FileName = "distribution";
            fileSaveDialog1.Filter = "Security Certificate (*.cer)|*.cer";
            DialogResult result = fileSaveDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sourcefileName = fileSaveDialog1.FileName; 
                string destfileName = projectPath + "/certificate.cer";
                System.IO.File.Copy(sourcefileName, destfileName, true);
                return "done";
            }
            return "";
        }

        public String generateP12Cert()
        {
            if (!generatePemFile()){
                return "pem generation failed";
            }
            String cmd = "/C cd \"" + opensslFolder + "\" && " + "set OPENSSL_CONF=" + opensslConf + " && " +
                 "openssl pkcs12 -export -inkey \"" + projectPath + "\\privateKey.key\" -in \"" + projectPath + 
                 "\\container.pem\" -out \"" + projectPath + "\\codemagicKey.p12\" -passout pass:" + password;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (!File.Exists(projectPath + "/codemagicKey.p12"))
            {
                return "p12 generation failed";
            }

            return "done";
        }

        private bool generatePemFile()
        {
            String cmd = "/C cd \"" + opensslFolder + "\" && " + "set OPENSSL_CONF=" + opensslConf + " && " +
                 "openssl x509 -in \"" + projectPath + "\\certificate.cer\" -inform DER -out \"" + projectPath 
                 + "\\container.pem\" -outform PEM";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (!File.Exists(projectPath + "/container.pem"))
            {
                return false;
            }
            return true;
        }

        public String exportP12()
        {
            var fileSaveDialog1 = new SaveFileDialog();
            fileSaveDialog1.DefaultExt = "p12";
            fileSaveDialog1.FileName = "codemagicKey";
            fileSaveDialog1.Filter = "pkcs (*.p12)|*.p12";
            DialogResult result = fileSaveDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sourcefileName = projectPath + "/codemagicKey.p12";
                string destfileName = fileSaveDialog1.FileName;
                System.IO.File.Copy(sourcefileName, destfileName, true);
                return "done";
            }
            return "";
        }
    }
}
