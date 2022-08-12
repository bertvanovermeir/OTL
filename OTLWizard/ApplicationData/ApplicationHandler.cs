using OTLWizard.ApplicationData;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
                ViewHandler.Show(Language.Get("oldversion"), Language.Get("oldversionheader"), MessageBoxIcon.Exclamation);
            ViewHandler.Start();
        }

        public static bool CheckVersion()
        {
            var downloadpath = System.IO.Path.GetTempPath() + "otlappversie\\";
            // create the folder if it does not exist
            Directory.CreateDirectory(downloadpath);
            // download the TTL file
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
                return true;
            }
        }

        public static async Task ImportArtefact(string subsetPath, string artefactPath)
        {
            await ImportSubset(subsetPath);
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, Language.Get("gaimport"));
            artefactConn = new ArtefactImporter(artefactPath);
            try
            {
                await Task.Run(() => { artefactConn.Import(GetSubsetClassNames()); });
            }
            catch
            {
                ViewHandler.Show(Language.Get("gafail"), Language.Get("errorheader"), MessageBoxIcon.Error);
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
        public static async Task<bool> ImportSubset(string dbPath, bool keuzelijsten = false)
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
            return CheckDeprecated();
        }

        /// <summary>
        /// check if any of these classes are deprecated
        /// this will generate a purely cosmetic message to the user. Action is in his hands.
        /// </summary>
        private static bool CheckDeprecated()
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
            if (showWarning)
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
                        await Task.Run(() => { artefactConn.Import(GetSubsetClassNames()); });
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
                        await Task.Run(() => { artefactConn.Import(GetSubsetClassNames()); });
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
            return subsetConn.GetOTLObjectTypes().Select(x => x.otlName);
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

        public static RealDataImporter realImporter = new RealDataImporter();
        public static List<OTL_Relationship> relationships;

        public static void R_DestroyOnClose()
        {
            relationships = new List<OTL_Relationship>();
            realImporter = new RealDataImporter();
        }

        public static async Task R_ImportRealRelationDataAsync(string[] paths)
        {
            relationships = new List<OTL_Relationship>();

            foreach (var path in paths)
            {
                if (path.ToUpper().EndsWith(".CSV"))
                {
                    try
                    {
                        await Task.Run(() => { realImporter.Import(path, Enums.ImportType.CSV); });
                    }
                    catch (Exception e)
                    {
                        ViewHandler.Show(Language.Get("fileimporterror") + path + "\n\rError: " + e.Message, Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (path.ToUpper().EndsWith(".XLS"))
                {
                    try
                    {
                        await Task.Run(() => { realImporter.Import(path, Enums.ImportType.XLS); });
                    }
                    catch (Exception e)
                    {
                        ViewHandler.Show(Language.Get("fileimporterror") + path + "\n\rError: " + e.Message, Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (path.ToUpper().EndsWith(".XLSX"))
                {
                    try
                    {
                        await Task.Run(() => { realImporter.Import(path, Enums.ImportType.XLSX); });
                    }
                    catch (Exception e)
                    {
                        ViewHandler.Show(Language.Get("fileimporterror") + path + "\n\rError: " + e.Message, Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else if (path.ToUpper().EndsWith(".JSON"))
                {
                    try
                    {
                        await Task.Run(() => { realImporter.Import(path, Enums.ImportType.JSON); });
                    }
                    catch (Exception e)
                    {
                        ViewHandler.Show(Language.Get("fileimporterror") + path + "\n\rError: " + e.Message, Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    ViewHandler.Show(Language.Get("fileimporterror") + path, Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                }
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
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    entry.ExtractToFile(tempPath + entry.FullName);
                    switch (entry.FullName)
                    {
                        case "R.xml":
                            relationships = (XmlSerialization.ReadFromXmlFile<List<OTL_Relationship>>(tempPath + "R.xml"));
                            break;
                        case "E.xml":
                            realImporter = new RealDataImporter();
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
                        foreach (var item in relationships)
                        {
                            // normal creation direction
                            if (entity.AssetId.Equals(item.bronID) && connector.AssetId.Equals(item.doelID)
                                && item.relationshipURI.Equals(rel.relationshipURI))
                            {
                                relnotfound = false;
                                break;
                            }
                            // opposite direction if directional is false (both directions count in that case)
                            else if(entity.AssetId.Equals(item.doelID) && connector.AssetId.Equals(item.bronID)
                                && item.relationshipURI.Equals(rel.relationshipURI) && item.isDirectional == false)
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
                        ceh.typeuri = rel.relationshipURI;
                        if (rel.isDirectional)
                        {
                            ceh.DisplayName = rel.relationshipName + " --> " + connector.AssetId + " | " + connector.Name;
                            result.Add(ceh);
                        }
                        else
                        {
                            ceh.DisplayName = rel.relationshipName + " <--> " + connector.AssetId + " | " + connector.Name;
                            result.Add(ceh);
                        }
                    }
                }
            }
            result = result.OrderBy(o=>o.relationName).ToList();
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

        public static void R_CreateNewRealRelation(OTL_ConnectingEntityHandle ceh1)
        {
            var temp = new OTL_Relationship();
            Guid g = Guid.NewGuid();
            temp.assetID = g.ToString();
            temp.doelID = ceh1.doelId;
            temp.bronID = ceh1.bronId;
            temp.relationshipURI = ceh1.typeuri;
            temp.isDirectional = ceh1.isDirectional;
            relationships.Add(temp);
        }

        public static void R_RemoveRealRelation(string relID)
        {
            relationships.Remove(relationships.Where(x => x.assetID.Equals(relID)).FirstOrDefault());

        }

        public static OTL_Relationship[] R_GetRealRelationsObjects()
        {
            if (relationships == null)
            {
                return null;
            }
            else
            {
                return relationships.ToArray();
            }
        }

        public static List<OTL_RelationshipType> R_GetAllRelationshipTypes()
        {
            return subsetConn.GetOTLRelationshipTypes();
        }



        public static List<OTL_Relationship> R_GetRealRelations()
        {
            List<OTL_Relationship> temp = new List<OTL_Relationship>();

            foreach (var rel in relationships)
            {
                if (rel.isDirectional)
                {
                    rel.DisplayName = rel.relationshipURI.Split('#')[1] + " | " + rel.bronID + " --> " + rel.doelID;

                }
                else
                {
                    rel.DisplayName = rel.relationshipURI.Split('#')[1] + " | " + rel.bronID + " <--> " + rel.doelID;
                }
                temp.Add(rel);
            }

            temp = temp.OrderBy(o=>o.DisplayName).ToList();
            return temp;
        }

        public static async Task<bool> R_ImportSubsetAsync(string[] vs)
        {
            string subsetpath = vs[0];
            return await ImportSubset(subsetpath);
        }

        public static void R_AddEntity(OTL_Entity e)
        {
            realImporter.AddEntity(e);
        }
    }
}
