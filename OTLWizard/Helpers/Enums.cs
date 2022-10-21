namespace OTLWizard.OTLObjecten
{
    public class Enums
    {
        public enum DataType
        {
            Real,
            Integer,
            Text,
            List
        }

        public enum ImportType
        {
            CSV,
            JSON,
            XLSX,
            XLS
        }

        public enum Views
        {
            Loading,
            Home,
            ArtefactMain,
            ArtefactResult,
            SubsetMain,
            Settings,
            Relations,
            RelationsImport,
            RelationsUserDefined,
            RelationImportSummary,
            isNull
        }

        public enum Query
        {
            Objects,
            Parameters,
            Relations,
            Artefact,
            Version
        }
    }
}
