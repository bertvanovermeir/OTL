using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.ApplicationData
{
    public class RealDataComparer
    {
        private Dictionary<string, OTL_Entity> baseEntitites = new Dictionary<string, OTL_Entity>();
        private Dictionary<string, OTL_Entity> newEntities = new Dictionary<string, OTL_Entity>();
        private Dictionary<string, OTL_Relationship> baseRelationships = new Dictionary<string, OTL_Relationship>();
        private Dictionary<string, OTL_Relationship> newRelationships = new Dictionary<string, OTL_Relationship>();

        private Dictionary<string,OTL_Relationship> acceptedRelationshipsList = new Dictionary<string,OTL_Relationship>();
        private Dictionary<string,OTL_Entity> acceptedEntityList = new Dictionary<string,OTL_Entity>();
        private Dictionary<string,OTL_Relationship> omittedRelationshipsList = new Dictionary<string,OTL_Relationship>();
        private Dictionary<string,OTL_Entity> omittedEntityList = new Dictionary<string,OTL_Entity>();

        private List<OTL_CommentContainer> comments = new List<OTL_CommentContainer>();

        public RealDataComparer()
        {
            ResetComparer();
        }

        public void ResetComparer()
        {
            acceptedRelationshipsList.Clear();
            acceptedEntityList.Clear();
            omittedRelationshipsList.Clear();
            omittedEntityList.Clear();
            baseEntitites.Clear();
            newEntities.Clear();
            baseRelationships.Clear();
            newRelationships.Clear();
            comments.Clear();
        }

        public void SetBaseData(List<OTL_Entity> entities, List<OTL_Relationship> relationships)
        {
            foreach(OTL_Entity entity in entities)
            {
                baseEntitites.Add(entity.AssetId, entity);
            }
            foreach(OTL_Relationship relationship in relationships)
            {
                baseRelationships.Add(relationship.AssetId, relationship);
            }
        }

        public List<OTL_Entity> GetEntities()
        {
            return acceptedEntityList.Values.ToList();
        }

        public List<OTL_Relationship> GetRelationships()
        {
            return acceptedRelationshipsList.Values.ToList();
        }

        public void SetNewData(List<OTL_Entity> entities, List<OTL_Relationship> relationships)
        {
            foreach (OTL_Entity entity in entities)
            {
                newEntities.Add(entity.AssetId, entity);
            }
            foreach (OTL_Relationship relationship in relationships)
            {
                newRelationships.Add(relationship.AssetId, relationship);
            }
        }

        public void CompareDataSets()
        {
            // check entities
            foreach(OTL_Entity newEntity in newEntities.Values)
            {
                var baseEntity = baseEntitites.Where(n => n.Value.AssetId.Equals(newEntity.AssetId)).FirstOrDefault().Value;
                if(baseEntity != null)
                {
                    // start process with bool
                    var areAttributesChanged = false;

                    // check if attributes match
                    foreach(var newAttribute in newEntity.GetProperties())
                    {
                        var baseAttributeValue = baseEntity.GetProperties().Where(p => p.Key.Equals(newAttribute.Key)).FirstOrDefault().Value;
                        if(baseAttributeValue != null)
                        {
                            if(!baseAttributeValue.Equals(newAttribute.Value))
                            {
                                areAttributesChanged = true;
                                comments.Add(new OTL_CommentContainer
                                {
                                    AssetId = baseEntity.AssetId,
                                    Comment = Language.Get("changedattribute"),
                                    Type = "entity",
                                    IsAttribute = true,
                                    AttributeName = newAttribute.Key,
                                    originalAttributeValue = baseAttributeValue,
                                    newAttributeValue = newAttribute.Value
                                }); ;
                            }
                        } else
                        {
                            areAttributesChanged = true;
                            comments.Add(new OTL_CommentContainer
                            {
                                AssetId = baseEntity.AssetId,
                                Comment = Language.Get("addedattribute"),
                                Type = "entity",
                                IsAttribute = true,
                                AttributeName = newAttribute.Key,
                                newAttributeValue = newAttribute.Value
                            });
                        }
                    }
                    if(areAttributesChanged)
                    {
                        acceptedEntityList.Add((string)baseEntity.AssetId,(OTL_Entity)baseEntity);
                    } else
                    {
                        comments.Add(new OTL_CommentContainer
                        {
                            AssetId = newEntity.AssetId,
                            Comment = Language.Get("removedentity"),
                            Type = "entity"
                        });
                        omittedEntityList.Add((string)baseEntity.AssetId,(OTL_Entity)baseEntity);
                    }
                } else
                {
                    comments.Add(new OTL_CommentContainer {
                        AssetId=newEntity.AssetId, 
                        Comment=Language.Get("addedentity"),
                        Type="entity"});
                    acceptedEntityList.Add(newEntity.AssetId, newEntity);
                }
            }
            // relations
            foreach(OTL_Relationship newRelation in newRelationships.Values)
            {
                var baseRelation = baseRelationships.Where(n => n.Value.Equals(newRelation.AssetId)).FirstOrDefault().Value;
                if(baseRelation != null)
                {
                    // check if status has changed isActive
                    if (baseRelation.isActive.Equals(newRelation.isActive))
                    {
                        comments.Add(new OTL_CommentContainer
                        {
                            AssetId = baseRelation.AssetId,
                            Comment = Language.Get("removedrelation"),
                            Type = "relation"
                        });
                        omittedRelationshipsList.Add(newRelation.AssetId, newRelation); 
                    } else
                    {
                        comments.Add(new OTL_CommentContainer
                        {
                            AssetId = newRelation.AssetId,
                            Comment = Language.Get("addedrelationforstatus"),
                            Type = "relation"
                        });
                        acceptedRelationshipsList.Add(newRelation.AssetId, newRelation);
                    }
                } else
                {
                    comments.Add(new OTL_CommentContainer
                    {
                        AssetId = newRelation.AssetId,
                        Comment = Language.Get("addedrelation"),
                        Type = "relation"
                    });
                    acceptedRelationshipsList.Add(newRelation.AssetId, newRelation);
                }
            }
        }

        public List<OTL_CommentContainer> GetReport()
        {
            return comments;
        }
    }
}
