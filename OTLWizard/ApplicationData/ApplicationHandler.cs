using OTLWizard.ApplicationData;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OTLWizard.OTLObjecten
{
    /// <summary>
    /// Belangrijkste klasse die de werking van het programma illustreert. 
    /// De interface legt connectie met de instatie van deze klasse voor de uitvoering van opdrachten (klikken op knoppen etc..)
    /// </summary>
    public static class ApplicationHandler
    {
        private static SubsetImporter subsetConn;
        private static ArtefactImporter artefactConn;
        private static string newestversion;
        private static string currentversion;

        public static void Start()
        {
            Settings.Init();
            Language.Init();
            if (!CheckVersion())
            {
                ViewHandler.Show(Language.Get("oldversion") + "\n" + GetVersionUpdateHistory(), Language.Get("oldversionheader"), MessageBoxIcon.Exclamation);
                Process.Start("https://github.com/bertvanovermeir/OTL/releases");
            }
            ViewHandler.Start();
        }

        public static string GetVersionUpdateHistory()
        {
            var tempstr = "";
            var downloadpath = System.IO.Path.GetTempPath() + "otlappversioning\\";
            if (Directory.Exists(downloadpath))
            {
                Directory.Delete(downloadpath, true);
                Directory.CreateDirectory(downloadpath);
            }
            else
            {
                Directory.CreateDirectory(downloadpath);
            }
            // download the TTL file
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile("https://raw.githubusercontent.com/bertvanovermeir/OTL/master/OTLWizard/Data/versioning.dat", downloadpath + "versioning.dat");
                }
                string[] lines = File.ReadAllLines(downloadpath + "versioning.dat", System.Text.Encoding.UTF8);
                foreach (string item in lines)
                {
                    tempstr = tempstr + "\n" + item;
                }
            }
            catch
            {
                // could not check for updates, just silently continue. This is the user's responsablility.       
            }
            return tempstr;
        }

        public static bool CheckVersion()
        {
            var downloadpath = System.IO.Path.GetTempPath() + "otlappversie\\";
            // create the folder if it does not exist
            if (Directory.Exists(downloadpath))
            {
                Directory.Delete(downloadpath, true);
                Directory.CreateDirectory(downloadpath);
            }
            else
            {
                Directory.CreateDirectory(downloadpath);
            }
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile("https://raw.githubusercontent.com/bertvanovermeir/OTL/master/OTLWizard/Data/version.dat", downloadpath + "version.dat");
                }
                string[] lines = File.ReadAllLines(downloadpath + "version.dat", System.Text.Encoding.UTF8);
                foreach (string item in lines)
                {
                    newestversion = item;
                }
                string[] lines2 = File.ReadAllLines("data\\version.dat", System.Text.Encoding.UTF8);
                foreach (string item in lines2)
                {
                    currentversion = item;
                }
                if (newestversion.Equals(currentversion))
                    return true;
                else
                    return false;
            }
            catch
            {
                // could not check for updates, just silently continue. This is the user's responsablility.
                return true;
            }
        }

        public static async Task ImportArtefact(string subsetPath, string artefactPath)
        {
            await ImportSubset(subsetPath, false);
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("gaimport"));
            artefactConn = new ArtefactImporter(artefactPath);
            try
            {
                await Task.Run(() => { artefactConn.Import(GetSubsetUris()); });
            }
            catch
            {
                ViewHandler.Show(Language.Get("gafail"), Language.Get("errorheader"), MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        public static void SetArtefactForClass(OTL_ObjectType o)
        {
            var geom = GetArtefactResultData().Where(w => w.URL == o.uri).Select(q => q).FirstOrDefault();
            if (geom != null && geom.geometrie.Length > 2)
            {
                o.geometryRepresentation = geom.geometrie.Trim().Substring(0, geom.geometrie.Length - 1).Split(',');
            }
            else
            {
                o.geometryRepresentation = null;
            }
        }

        public static async Task ImportArtefact(string artefactPath)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("gaimport"));
            artefactConn = new ArtefactImporter(artefactPath);
            try
            {
                await Task.Run(() => { artefactConn.Import(GetSubsetUris()); });
            }
            catch
            {
                ViewHandler.Show(Language.Get("sdfgafail"), Language.Get("errorheader"), MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        public static string GetOTLVersion()
        {
            var temp = subsetConn.GetOTLVersion();
            if (temp == null)
                temp = "";
            return temp;
        }

        /// <summary>
        /// Interface Handle voor het importeren van de OTL database
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="klPath"></param>
        public static async Task<bool> ImportSubset(string dbPath, bool showdeprecationmsg, bool keuzelijsten = false)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("otlimport"));
            subsetConn = new SubsetImporter(dbPath, keuzelijsten);
            try
            {
                await Task.Run(() => { subsetConn.Import(); });
            }
            catch
            {
                ViewHandler.Show(Language.Get("otlfail"), Language.Get("errorheader"), MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            return CheckDeprecated(showdeprecationmsg);
        }

        /// <summary>
        /// check if any of these classes are deprecated
        /// this will generate a purely cosmetic message to the user. Action is in his hands.
        /// </summary>
        private static bool CheckDeprecated(bool showmsg)
        {
            string deprecatedclasses = "";
            string deprecatedparameters = "";
            bool showWarning = false;
            foreach (OTL_ObjectType otlObject in subsetConn.GetOTLObjectTypes())
            {
                if (otlObject.deprecated)
                {
                    deprecatedclasses = deprecatedclasses + otlObject.friendlyName + ", ";
                    showWarning = true;
                }
                foreach (OTL_Parameter p in otlObject.GetParameters())
                {
                    if (p.Deprecated && !otlObject.deprecated)
                    {
                        deprecatedparameters = deprecatedparameters + p.FriendlyName + " in " + otlObject.friendlyName + ", ";
                        showWarning = true;
                    }
                }
            }
            if (showWarning && showmsg)
            {

                ViewHandler.Show(Language.Get("deprecation") + "Classes (incl. parameters):\n"
                    + deprecatedclasses + "\n\nParameters (in classes):\n" + deprecatedparameters, Language.Get("deprecationheader"), MessageBoxIcon.Information);
            }
            return showWarning;
        }

        public static async Task ExportCSVSubset(string exportPath, string wktpath, int amountExamples, Boolean withDescriptions, Boolean withChecklistOptions, Boolean dummyData, Boolean wkt, Boolean deprecated, string[] classes)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("templateexport") + "(CSV).");
            SubsetExporterCSV exp = new SubsetExporterCSV();
            bool successSubset = exp.SetOTLSubset(subsetConn.GetOTLObjectTypes());
            bool successSelection = exp.SetSelectedClassesByUser(classes);
            if (successSubset && successSelection)
            {
                // test if WKT then import artefact
                artefactConn = new ArtefactImporter(wktpath);
                if (wkt)
                {
                    try
                    {
                        await Task.Run(() => { artefactConn.Import(GetSubsetUris()); });
                    }
                    catch
                    {
                        ViewHandler.Show(Language.Get("gafail"), Language.Get("errorheader"), MessageBoxIcon.Error);
                    }
                }
                var result = await Task.Run(() => exp.Export(exportPath, artefactConn.GetOTLArtefactTypes(), amountExamples, withDescriptions, dummyData, wkt, deprecated));
                if (!result)
                {
                    ViewHandler.Show(Language.Get("saveerror"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                }
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show(Language.Get("exportfinished"), Language.Get("successheader"), MessageBoxIcon.Information);
            }
            else
            {
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show(Language.Get("selectionerror"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Interface Handle voor het exporteren van de Template XLS
        /// </summary>
        /// <param name="exportPath"></param>
        /// <param name="withDescriptions"></param>
        /// <param name="withChecklistOptions"></param>
        /// <param name="classes"></param>
        public static async Task ExportXlsSubset(string exportPath, string wktpath, int amountExamples, Boolean withDescriptions, Boolean withChecklistOptions, Boolean dummyData, Boolean wkt, Boolean deprecated, string[] classes)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("templateexport") + "(XLSX).");
            SubsetExporterXLS exp = new SubsetExporterXLS();
            bool successSubset = exp.SetOTLSubset(subsetConn.GetOTLObjectTypes());
            bool successSelection = exp.SetSelectedClassesByUser(classes);
            if (successSubset && successSelection)
            {
                // test if WKT then import artefact
                artefactConn = new ArtefactImporter(wktpath);
                if (wkt)
                {
                    try
                    {
                        await Task.Run(() => { artefactConn.Import(GetSubsetUris()); });
                    }
                    catch
                    {
                        ViewHandler.Show(Language.Get("gafail"), Language.Get("errorheader"), MessageBoxIcon.Error);
                    }
                }
                var result = await Task.Run(() => exp.Export(exportPath, artefactConn.GetOTLArtefactTypes(), amountExamples, withDescriptions, withChecklistOptions, dummyData, wkt, deprecated));
                if (!result)
                {
                    ViewHandler.Show(Language.Get("saveerror"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                }
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show(Language.Get("exportfinished"), Language.Get("successheader"), MessageBoxIcon.Information);
            }
            else
            {
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show(Language.Get("selectionerror"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static async Task ExportXlsArtefact(string exportPath, List<OTL_ArtefactType> artefacten)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("artefactexport"));
            ArtefactExporterXLS exp = new ArtefactExporterXLS();
            var result = await Task.Run(() => exp.Export(exportPath, artefacten));
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            if (!result)
            {
                ViewHandler.Show(Language.Get("saveerror"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                ViewHandler.Show(Language.Get("exportfinished"), Language.Get("successheader"), MessageBoxIcon.Information);
            }
        }

        public static async Task ExportCSVArtefact(string exportPath, List<OTL_ArtefactType> artefacten)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("artefactexport"));
            ArtefactExporterCSV exp = new ArtefactExporterCSV();
            var result = await Task.Run(() => exp.Export(exportPath, artefacten));
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            if (!result)
            {
                ViewHandler.Show(Language.Get("saveerror"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                ViewHandler.Show(Language.Get("exportfinished"), Language.Get("successheader"), MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Interface Handle voor het vullen van de Listbox met alle mogelijke klassen die zich in de subset bevinden.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetSubsetClassNames()
        {
            return subsetConn.GetOTLObjectTypes().Select(x => x.otlName).OrderBy(y => y);
        }

        public static IEnumerable<string> GetSubsetUris()
        {
            return subsetConn.GetOTLObjectTypes().Select(x => x.uri).OrderBy(y => y);
        }

        /// <summary>
        /// Interface Handle voor het opvragen van alle ingeladen artefactdata, dit kan nog gefilterd worden
        /// in een later stadium met de user selection.
        /// </summary>
        /// <returns></returns>
        public static List<OTL_ArtefactType> GetArtefactResultData()
        {
            return artefactConn.GetOTLArtefactTypes();
        }

        /////////////////// RELATION TOOL FUNCTIONS /////////////////////////////
        /////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        public static RealDataImporter realImporter = new RealDataImporter();


        public static void R_DestroyOnClose()
        {
            realImporter = new RealDataImporter();
        }

        public static async Task R_ImportRealRelationDataAsync(string[] paths)
        {
            // set subset relation object
            realImporter.SetRelationTypeData(subsetConn);
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("csvrealimport"));
            foreach (var path in paths)
            {
                try
                {
                    if (path.ToUpper().EndsWith(".CSV"))
                    {
                        await Task.Run(() => { realImporter.Import(path, Enums.ImportType.CSV); });
                    }
                    else if (path.ToUpper().EndsWith(".XLS") || path.ToUpper().EndsWith(".XLSX"))
                    {
                        await Task.Run(() => { realImporter.Import(path, Enums.ImportType.XLS); });
                    }
                    else if (path.ToUpper().EndsWith(".SDF"))
                    {
                        // check if sdf path exists, prompt the user with a dialog to change it
                        if (File.Exists(Settings.Get("sdfpath")) && Settings.Get("sdfpath").ToLower().Contains("fdocmd.exe"))
                        {
                            await Task.Run(() => { realImporter.Import(path, Enums.ImportType.SDF); });
                        }
                        else
                        {
                            ViewHandler.Show(Language.Get("dependencymissing2"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                            setFDODependency();
                            ViewHandler.Show(Language.Get("restartprocess"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        ViewHandler.Show(Language.Get("fileimporterror") + path, Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                catch (Exception e)
                {
                    ViewHandler.Show(Language.Get("fileimporterror") + path + "\n\rError: " + e.Message, Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            realImporter.CheckAssetCompliance();
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            if (realImporter.GetErrors().Count > 0)
            {
                ViewHandler.Show(Enums.Views.RelationImportSummary, Enums.Views.isNull, realImporter.GetErrors().ToArray());
                realImporter.ResetErrors();
            }
        }

        private static void setFDODependency()
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = true;
            fdlg.Title = Language.Get("SelectFDOCMDFilePath");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Executable Files (*.exe)|*.exe";
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                Settings.Update("sdfpath", fdlg.FileName);
                Settings.WriteSettings();
            }
        }

        public static List<OTL_Entity> R_GetImportedEntities()
        {
            return realImporter.GetEntities();
        }

        public static void R_SaveRelationState(string path)
        {
            try
            {
                var tempPath = Path.GetTempPath();
                var randomdirname = Guid.NewGuid().ToString();
                tempPath = tempPath + randomdirname + "\\";
                Directory.CreateDirectory(tempPath);
                // created relationships
                XmlSerialization.WriteToXmlFile<List<OTL_Relationship>>(tempPath + "R.xml", R_GetRealRelationsObjects().ToList());
                // imported entities and properties
                XmlSerialization.WriteToXmlFile<List<OTL_Entity>>(tempPath + "E.xml", R_GetImportedEntities().ToList());
                // possible relationships
                XmlSerialization.WriteToXmlFile<List<OTL_RelationshipType>>(tempPath + "T.xml", subsetConn.GetOTLRelationshipTypes());
                // userdata
                //TODO
                // create ZIP
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                ZipFile.CreateFromDirectory(tempPath, path);
            }
            catch
            {
                ViewHandler.Show(Language.Get("filesaveissue"), Language.Get("filesaveissueheader"), MessageBoxIcon.Error);
            }

        }

        public static void R_LoadRelationState(string fileName)
        {
            try
            {
                ZipArchive zip = ZipFile.OpenRead(fileName);
                var tempPath = Path.GetTempPath();
                var randomdirname = Guid.NewGuid().ToString();
                tempPath = tempPath + randomdirname + "\\";
                Directory.CreateDirectory(tempPath);
                realImporter = new RealDataImporter();

                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    entry.ExtractToFile(tempPath + entry.FullName);
                    switch (entry.FullName)
                    {
                        case "R.xml":
                            realImporter.SetRelationships(XmlSerialization.ReadFromXmlFile<List<OTL_Relationship>>(tempPath + "R.xml"));
                            break;
                        case "E.xml":
                            realImporter.SetEntities(XmlSerialization.ReadFromXmlFile<List<OTL_Entity>>(tempPath + "E.xml"));
                            break;
                        case "T.xml":
                            subsetConn = new SubsetImporter();
                            subsetConn.SetOTLRelationshipTypes(XmlSerialization.ReadFromXmlFile<List<OTL_RelationshipType>>(tempPath + "T.xml"));
                            break;
                        default:
                            throw new Exception("This is not a valid savestate file.");
                    }
                }

            }
            catch (Exception e)
            {
                ViewHandler.Show("loadstateerror" + e.ToString(), "loadstateerrorheader", MessageBoxIcon.Error);
            }
        }



        public static void R_ExportRealRelationData(string path)
        {
            try
            {
                RealDataExporter.Export(path, R_GetRealRelationsObjects().ToList());
            }
            catch
            {
                ViewHandler.Show(Language.Get("exportcsverror"), Language.Get("error"), System.Windows.Forms.MessageBoxIcon.Error);

            }
        }

        public static List<OTL_ConnectingEntityHandle> R_GetPossibleRelations(OTL_Entity entity)
        {
            if (entity != null)
            {
                var rels = subsetConn.GetOTLRelationshipTypes();
                var entities = realImporter.GetEntities();
                List<OTL_ConnectingEntityHandle> result = new List<OTL_ConnectingEntityHandle>();
                var relnotfound = true;

                foreach (var rel in rels)
                {
                    if (rel.bronURI.Equals(entity.TypeUri))
                    {
                        var connectors = entities.Where(x => x.TypeUri.ToLower().Equals(rel.doelURI.ToLower()));
                        // we now have all connections. But need to check if the relation already exist.
                        var remainingconnector = new List<OTL_Entity>();

                        foreach (var connector in connectors)
                        {
                            foreach (var item in realImporter.GetRelationships())
                            {
                                // normal creation direction
                                if (entity.AssetId.Equals(item.bronID) && connector.AssetId.Equals(item.doelID)
                                    && item.relationshipURI.Equals(rel.relationshipURI) && item.Activated != false)
                                {
                                    relnotfound = false;
                                    break;
                                }
                                // opposite direction if directional is false (both directions count in that case)
                                else if (entity.AssetId.Equals(item.doelID) && connector.AssetId.Equals(item.bronID)
                                    && item.relationshipURI.Equals(rel.relationshipURI) && item.isDirectional == false && item.Activated != false)
                                {
                                    relnotfound = false;
                                    break;
                                }
                                else
                                {
                                    relnotfound = true;
                                }
                            }
                            if (relnotfound)
                                remainingconnector.Add(connector);
                        }
                        foreach (var connector in remainingconnector)
                        {
                            OTL_ConnectingEntityHandle ceh = new OTL_ConnectingEntityHandle();
                            ceh.isDirectional = rel.isDirectional;
                            ceh.relationName = rel.relationshipName;
                            ceh.bronId = entity.AssetId;
                            ceh.doelId = connector.AssetId;
                            if (connector.Properties.ContainsKey("naam"))
                                ceh.doelName = connector.Properties["naam"];
                            else
                                ceh.doelName = "";
                            if (entity.Properties.ContainsKey("naam"))
                                ceh.bronName = entity.Properties["naam"];
                            else
                                ceh.bronName = "";
                            ceh.typeuri = rel.relationshipURI;
                            if (rel.isDirectional)
                            {
                                ceh.GenerateDisplayName(" --> ", connector.Name);
                                result.Add(ceh);
                            }
                            else
                            {
                                ceh.GenerateDisplayName(" <--> ", connector.Name);
                                result.Add(ceh);
                            }
                        }
                    }
                }
                result = result.OrderBy(o => o.relationName).ToList();
                // add a placeholder relation that allows user to enter relation type and ID. WITHOUT ANY CHECKS
                OTL_ConnectingEntityHandle c = new OTL_ConnectingEntityHandle();
                c.isDirectional = true;
                c.relationName = "userDefinedRelationship";
                c.bronId = entity.AssetId;
                c.doelId = "userDefinedAssetID";
                c.typeuri = "userDefinedTypeURI";
                c.DisplayName = Language.Get("userdefinedrelation");
                result.Add(c);
                return result;
            }
            else
            {
                return null;
            }

        }

        public static void R_CreateNewRealRelation(OTL_ConnectingEntityHandle ceh1)
        {
            // check if this relation already exists but inactive
            var currentlist = R_GetRealRelations();
            var found = false;
            var assetidfound = "";
            foreach (var item in currentlist)
            {
                if (item.bronID.Equals(ceh1.bronId) && item.doelID.Equals(ceh1.doelId) && item.relationshipURI.Equals(ceh1.typeuri))
                {
                    assetidfound = item.AssetId;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                // do not create a new relation, as it already exists.
                R_ActivateRealRelation(assetidfound);

            }
            else
            {
                var temp = new OTL_Relationship();
                Guid g = Guid.NewGuid();
                temp.AssetId = g.ToString();
                temp.doelID = ceh1.doelId;
                temp.doelName = ceh1.doelName;
                temp.bronName = ceh1.bronName;
                temp.bronID = ceh1.bronId;
                temp.relationshipURI = ceh1.typeuri;
                temp.isDirectional = ceh1.isDirectional;
                temp.Activated = true;
                temp.Properties.Add(Settings.Get("otlidentifier"), temp.AssetId);
                temp.Properties.Add(Settings.Get("otlclassuri"), temp.relationshipURI);
                temp.Properties.Add(Settings.Get("otlsrcrel"), temp.bronID);
                temp.Properties.Add(Settings.Get("otltrgtrel"), temp.doelID);
                temp.GenerateDisplayName();
                realImporter.AddRelationship(temp);
            }
        }

        public static void R_ActivateRealRelation(string relID)
        {
            realImporter.ActivateRelationship(relID);
        }

        public static void R_RemoveRealRelation(string relID, bool softremove)
        {
            realImporter.RemoveRelationship(relID, softremove);
        }

        public static OTL_Relationship[] R_GetRealRelationsObjects()
        {
            if (realImporter.GetRelationships() == null)
            {
                return null;
            }
            else
            {
                return realImporter.GetRelationships().ToArray();
            }
        }

        public static List<OTL_RelationshipType> R_GetAllRelationshipTypes()
        {
            return subsetConn.GetOTLRelationshipTypes();
        }

        public static OTL_Entity R_GetEntityForID(string id)
        {
            return realImporter.GetEntities().Where(e => e.AssetId == id).FirstOrDefault();
        }



        public static List<OTL_Relationship> R_GetRealRelations()
        {
            List<OTL_Relationship> temp = new List<OTL_Relationship>();
            temp = realImporter.GetRelationships().OrderBy(o => o.DisplayName).ToList();
            return temp;
        }

        public static async Task<bool> R_ImportSubsetAsync(string[] vs)
        {
            string subsetpath = vs[0];
            bool success = true;
            if (subsetpath.Equals("download"))
            {
                // dowload the subset from the website to a temp folder
                string filename = "otl.db";
                var localPath = System.IO.Path.GetTempPath() + "otldownload\\";

                // create the folder if it does not exist
                Directory.CreateDirectory(localPath);
                // download the TTL file
                ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("otldownload"));
                try
                {
                    using (var client = new WebClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        client.DownloadFile(Settings.GetRaw("otlpath"), localPath + filename);
                        success = true;
                    }
                    subsetpath = localPath + filename;
                }
                catch
                {
                    success = false;
                    ViewHandler.Show(Language.Get("otldownloadfail"), Language.Get("errorheader"), MessageBoxIcon.Error);
                }
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            }
            if (success)
            {
                await ImportSubset(subsetpath, false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void R_CreateUserAsset(string doelID)
        {
            realImporter.CreateUserAsset(doelID);
        }

        /////////////////// XSD TOOL FUNCTIONS //////////////////////////////////
        /////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////
        public static XsdHandler sdf;
        public static string[] xsdClassNames;

        public static void SDX_ImportSDX(string path, bool onlyClassNames)
        {
            sdf = new XsdHandler();
            if (onlyClassNames)
            {
                xsdClassNames = sdf.GetXSDClasses(path);
            }
            else
            {

            }
        }

        public static string[] getXSDClassNames()
        {
            return xsdClassNames;
        }

        public static void SDX_ExportSDX(string path, string[] classes)
        {
            sdf = new XsdHandler();
            List<OTL_ObjectType> temp = null;

            if (classes == null)
            {
                temp = subsetConn.GetOTLObjectTypes();
            }
            else if (classes.Length == 0)
            {
                temp = subsetConn.GetOTLObjectTypes();
            }
            else
            {
                temp = subsetConn.GetOTLObjectTypesFor(classes);
            }
            var ok = sdf.Export(temp, path);
            if (ok)
                ViewHandler.Show(Language.Get("sdfsuccess"), Language.Get("errorheader"), MessageBoxIcon.Information);
            else
                ViewHandler.Show(Language.Get("sdffail"), Language.Get("errorheader"), MessageBoxIcon.Error);

        }

        /////////////////// VWR TOOL FUNCTIONS //////////////////////////////////
        /////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////


        public static async void VWR_ImportSubset(string path)
        {
            await ImportSubset(path, false);
        }

        public static List<OTL_ObjectType> VWR_GetSubset()
        {
            return subsetConn.GetOTLObjectTypes();
        }

        public static List<OTL_RelationshipType> VWR_GetRelationTypes()
        {
            return subsetConn.GetOTLRelationshipTypes();
        }



    }
}
