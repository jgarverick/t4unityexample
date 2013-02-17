using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using ObliteracyDotNet.Generators;
using ObliteracyDotNet.Generators.Structures;
using System.Data;
using ObliteracyDotNet.Generators.Templates;
using System.IO;
using ObliteracyDotNet.Generators.Templates.Projects;
using ObliteracyDotNet.Generators.Templates.Objects;

namespace ObliteracyDotNet.Generators.PatternBuilders
{
    public class CustomPatternBuilder:IPatternBuilder
    {
        public string ContractSource { get; set; }
        public string ServiceSource { get; set; }
        private PatternBuilderConfig Options;

        public CustomPatternBuilder(PatternBuilderConfig options)
        {
            Options = options;
            InitializeDirectories();
        }

        public void Execute()
        {
            if (System.IO.Directory.Exists(Options.SaveDirectory))
            {
                
                DataSet schema = DatabaseMetadataStore.GetMetadata(Options.DBServer, Options.DBInstance);
                // Get the tables collection
                DataTable tables = schema.Tables["TableColumnView"];
                string currentTableName = "";
                foreach (DataRow row in tables.Rows)
                {
                    if (row[0].ToString() != currentTableName)
                    {
                        currentTableName = row[0].ToString();
                        string normalizedObjectName = currentTableName.Replace("_", "");
                        Console.WriteLine("Generating classes for {0}...", normalizedObjectName);
                        if (Options.MethodStructures.ContainsKey(currentTableName))
                            GetDefaultMethods(currentTableName).ForEach(method =>
                            {
                                if (!(Options.MethodStructures[currentTableName].Contains(method)))
                                    Options.MethodStructures[currentTableName].Add(method);
                            });

                        else
                            Options.MethodStructures.Add(row[0].ToString(), GetDefaultMethods(currentTableName));
                        List<IPatternTemplate> genList = PatternResolver.Resolve(currentTableName, Options);
                        tables.DefaultView.RowFilter = string.Format("TableName = '{0}'", currentTableName);
                        genList.ForEach(generator =>
                        {
                            if (generator.GetType() == typeof(BusinessObject))
                                (generator as BusinessObject).ColumnsTable = tables.DefaultView.ToTable();
                            generator.ReturnType = row[0].ToString();
                            string results = generator.TransformText();
                            WriteFile(Options.SaveDirectory + generator.FileName + ".cs", results);
                        });
                    }
                }
                
            }
        }

        private void WriteFile(string path, string contents)
        {
            using (System.IO.StreamWriter tw = new System.IO.StreamWriter(path))
            {
                tw.Write(contents);
                tw.Close();
            }

        }

        private List<MethodStruct> GetDefaultMethods(string objName)
        {
            List<MethodStruct> methods = new List<MethodStruct>();
            MethodStruct add = new MethodStruct();
            add.Name = "Add" + objName.Replace("_","");
            add.ClassName = objName;
            MethodStruct update = new MethodStruct();
            update.Name = "Update" + objName.Replace("_", "");
            update.ClassName = objName;
            MethodStruct get = new MethodStruct();
            get.Name = "Get" + objName.Replace("_", "");
            get.ClassName = objName;
            methods.Add(add);
            methods.Add(update);
            methods.Add(get);
            return methods;
        }

        public void InitializeDirectories()
        {
            string servicesSln = Options.SaveDirectory + "\\" + Options.ProjectNamespace + ".Services";
            string bizSln = Options.SaveDirectory + "\\" + Options.ProjectNamespace + ".Business";
            string coreSln = Options.SaveDirectory + "\\" + Options.ProjectNamespace + ".Core";
            // Delete everything first
            if (Directory.Exists(Options.SaveDirectory))
                System.IO.Directory.Delete(Options.SaveDirectory, true);
            Directory.CreateDirectory(Options.SaveDirectory);
            Directory.CreateDirectory(servicesSln);
            Directory.CreateDirectory(bizSln);
            Directory.CreateDirectory(coreSln);
            Directory.CreateDirectory(servicesSln + "\\Services");
            Directory.CreateDirectory(servicesSln+ "\\Contracts");
            Directory.CreateDirectory(coreSln + "\\Managers");
            Directory.CreateDirectory(bizSln + "\\Business");
            Directory.CreateDirectory(coreSln + "\\Helpers");
            Directory.CreateDirectory(servicesSln + "\\Returns");
        }

        public void CreateBusinessProject()
        {
            CreateProject("Business", new List<string>() { "Business" });
        }

        public void CreateCoreProject()
        {
            CreateProject("Core", new List<string>() {"Managers","Helpers", "..\\" });
        }

        public void CreateServicesProject()
        {
            CreateProject("Services", new List<string>() { "Services", "Contracts", "Returns" });
        }

        private void CreateProject(string projTypeName, List<string> directories)
        {
            string projBase = Options.SaveDirectory + "\\" + Options.ProjectNamespace + "." + projTypeName + "\\";

            ProjectTemplate proj = new ProjectTemplate();
            proj.ProjectGuid = Guid.NewGuid();
            proj.Namespace = Options.ProjectNamespace;
            proj.IncludedFiles = new List<string>();
            directories.ForEach(dir =>
            {
                Directory.GetFiles(projBase + "\\" + dir).ToList().ForEach(file =>
                {
                    if (string.IsNullOrEmpty(dir))
                        proj.IncludedFiles.Add(Path.GetFileName(file));
                    else
                        proj.IncludedFiles.Add(dir + "\\" + Path.GetFileName(file));
                });
            });
            WriteFile(projBase + Options.ProjectNamespace + "." + projTypeName + ".csproj", proj.TransformText());
        }
    }
}
