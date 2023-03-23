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
            XLS,
            SDF
        }

        public enum Views
        {
            Loading,
            Home,
            ArtefactMain,
            ArtefactResult,
            SubsetMain,
            Settings,
            RelationsMain,
            RelationsImport,
            RelationsUserDefined,
            RelationImportSummary,
            SDFMain,
            Tutorial,
            SubsetViewer,
            SubsetViewerImport,
            GeometryViewer,
            DataConversion,
            isNull
        }

        public enum Tutorial
        {
            subsettool,
            artefacttool,
            relationtool,
            sdftool
        }

        public enum SDFAttributeTypes
        {
            Simple,
            List,
            Real,
            Integer,
            Bool,
            Geometry,
            FeatId
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
