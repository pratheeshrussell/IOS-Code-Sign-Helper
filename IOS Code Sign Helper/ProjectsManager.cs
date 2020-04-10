using System;
using System.IO;
using System.Collections.Generic;

namespace IOS_Code_Sign_Helper
{
    class ProjectsManager
    {
        public void Initialize()
        {
            string subPath = Directory.GetCurrentDirectory() + "/projects";
            bool exists = Directory.Exists(subPath);
            if (!exists)
            {
                Directory.CreateDirectory(subPath);
            }
        }

        public string CreateNewProject(String projectname)
        {
            if (!IsValidProject(Directory.GetCurrentDirectory() + "/projects/" + projectname))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/projects/" + projectname);
                return "done";
            }
            return "already exists";
        }

       

        public List<string> GetProjects()
        {
            List<string> allProjects = new List<string>();
            String[] allFolders = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/projects");
            foreach (String folder in allFolders)
            {
                if (IsValidProject(folder))
                {
                    allProjects.Add(folder.Replace(Directory.GetCurrentDirectory() + "/projects\\", ""));
                }
            }
            return allProjects;
        }

        public bool IsValidProject(String path)
        {
            if (!File.Exists(path + "/privateKey.key"))
            {
                return false;
            }
            return true;
        }
    }
}
