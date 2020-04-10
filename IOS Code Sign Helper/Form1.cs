using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace IOS_Code_Sign_Helper
{
	public partial class Form1 : Form
	{
		private readonly SignRequestGenerator reqGenerator = new SignRequestGenerator();
		private readonly P12Generator p12Generator = new P12Generator();
		private readonly ProjectsManager myProjects = new ProjectsManager();

        #region "common"
        public Form1()
		{
			InitializeComponent();
			CheckDependancy();

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			myProjects.Initialize();
			AddComboboxItems();
			button2.Enabled = false;
			button3.Enabled = false;
			button4.Enabled = false;
			button5.Enabled = false;
			button6.Visible = false;
			label5.Text = ""; label6.Text = "";
		}

		private void AddComboboxItems()
		{
			List<String> allProjects =  myProjects.GetProjects();
			comboBox1.Items.Clear();
			comboBox2.Items.Clear();
			comboBox1.Text = "";comboBox2.Text = "";
			foreach (String project in allProjects)
			{
				comboBox1.Items.Add(project);
				comboBox2.Items.Add(project);
			}
		}


		private void CheckDependancy()
		{
			if (!File.Exists(Directory.GetCurrentDirectory() + "/openssl/openssl.exe"))
			{			
				MessageBox.Show("OpenSSL not found","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
				if (Application.MessageLoop){ 
					Application.Exit(); 
				} else {
					System.Environment.Exit(0);
				}

			}
		}
        #endregion

        #region "tab1"
        private void button1_Click(object sender, EventArgs e)
		{
			String projName = Interaction.InputBox("Enter a name for your project");
			if (projName.Length > 3)
			{
				if(myProjects.CreateNewProject(projName) == "done")
				{
					//create privateKey.key									
					reqGenerator.projectPath = Directory.GetCurrentDirectory() + "/projects/" + projName;
					reqGenerator.generatePrivateKey();
					AddComboboxItems();
					comboBox1.Text = projName;
					MessageBox.Show("Project Created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);				
				} else
				{
					MessageBox.Show("Project Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			tab1buttonEnabler();
		}
		private void button3_Click(object sender, EventArgs e)
		{
			initiateCopyCsr();
		}

		private void button2_Click(object sender, EventArgs e)
		{		
			if (textBox1.Text != "")
			{
				reqGenerator.emailId = textBox1.Text;
				if (reqGenerator.generateCSR() == "done")
				{
					initiateCopyCsr();
					//MessageBox.Show("CSR generated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				} else
				{
					MessageBox.Show("CSR not generated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Email is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			tab1buttonEnabler();
		}
		 
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			reqGenerator.projectPath = Directory.GetCurrentDirectory() + "/projects/" + comboBox1.Text;
			tab1buttonEnabler();
		}

		private void initiateCopyCsr()
		{
			if (reqGenerator.copyCSR() == "done")
			{
				MessageBox.Show("CSR file Copied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void tab1buttonEnabler(){
			if (File.Exists(Directory.GetCurrentDirectory() + "/projects/" + comboBox1.Text + "/SigningRequest.certSigningRequest")){
				button2.Enabled = false;
				button3.Enabled = true;
			} else{
				if (File.Exists(Directory.GetCurrentDirectory() + "/projects/" + comboBox1.Text + "/privateKey.key"))
				{
					button2.Enabled = true;
					button3.Enabled = false;
				} else
				{
					AddComboboxItems();
					button2.Enabled = false;
					button3.Enabled = false;
				}

			}
		}

        #endregion

        #region "tab2"
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			p12Generator.projectPath = Directory.GetCurrentDirectory() + "/projects/" + comboBox2.Text;
			handletab2Status();
		}

		private void handletab2Status()
		{
			button4.Enabled = false; button5.Enabled = false;
			String curPath = Directory.GetCurrentDirectory() + "/projects/" + comboBox2.Text;
			bool isPrivateKey = File.Exists(curPath + "/privateKey.key"); 
			bool isCerFound = File.Exists(curPath + "/certificate.cer");
			bool isp12Found = File.Exists(curPath + "/codemagicKey.p12");

			if (isPrivateKey == true) {  label5.Text = "Private Key Found"; } else
			{
				label5.Text = "Private Key Not Found";
			}
			if (isCerFound == true)   { label6.Text = "Apple Certificate Found"; 
				button6.Visible = false; } else
			{
				label6.Text = "Apple Certificate Not Found";
				button6.Visible = true;
			}


			if (isPrivateKey == true && isCerFound == true)
			{
				if (isp12Found == true) { 
					button5.Enabled = true; } else {
					button4.Enabled = true;
				}	
			}
		}
		private void button6_Click(object sender, EventArgs e)
		{
			p12Generator.importcerFile();
			handletab2Status();
		}
		private void button4_Click(object sender, EventArgs e)
		{
			p12Generator.password = textBox2.Text;

			if (p12Generator.generateP12Cert() == "done")
			{
				MessageBox.Show("p12 file generated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} else
			{
				MessageBox.Show("Unable to generate p12 file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
			
			handletab2Status();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (p12Generator.exportP12() == "done")
			{
				MessageBox.Show("p12 file Copied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		#endregion

		#region "tab3"
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://pratheeshrussell.blogspot.com/2020/04/build-flutter-app-for-ios-using.html");
		}


		#endregion

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/pratheeshrussell/IOS-Code-Sign-Helper");
		}
	}
}
