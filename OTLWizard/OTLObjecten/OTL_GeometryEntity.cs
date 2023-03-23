using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public class OTL_GeometryEntity
    {
        private OTL_Entity entity;
        private string color = "";
        private string name = "";
        private string id = "";
        private bool isMapReferencePoint = false;
        private bool isForeground = false;

        public OTL_GeometryEntity(OTL_Entity entity, string color, string name, bool isMapReferencePoint)
        {
            this.entity = entity;
            this.color = color;
            this.name = name;
            this.id = entity.AssetId;
            this.isMapReferencePoint = isMapReferencePoint;
        }

        public OTL_Entity GetEntity()
        {
            return entity;
        }

        public bool IsMapReferencePoint()
        {
            return isMapReferencePoint;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public string GetColor()
        {
            return color;
        }

        public void SetColor(string color)
        {
            this.color=color;
        }

        public string GetId()
        {
            return id;
        }

        public void setForeground(bool state)
        {
            isForeground = state;
        }

        public bool getForeground()
        {
            return isForeground;
        }

    }
}
