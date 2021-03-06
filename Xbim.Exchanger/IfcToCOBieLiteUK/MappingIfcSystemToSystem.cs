﻿using System.Collections.Generic;
using System.Linq;
using Xbim.Common;
using Xbim.CobieLiteUk;
using Xbim.Ifc4.Interfaces;


namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcSystemToSystem : XbimMappings<IModel, List<Facility>, string, IIfcSystem, Xbim.CobieLiteUk.System>
    {
        protected override Xbim.CobieLiteUk.System Mapping(IIfcSystem ifcSystem, Xbim.CobieLiteUk.System target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcSystem);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSystem);
            target.AltExternalId = ifcSystem.GlobalId;
            target.ExternalSystem = helper.ExternalSystemName(ifcSystem);
            target.Name = ifcSystem.Name;
            target.Description = ifcSystem.Description;
            target.CreatedBy = helper.GetCreatedBy(ifcSystem);
            target.CreatedOn = helper.GetCreatedOn(ifcSystem);
            target.Categories = helper.GetCategories(ifcSystem);
            //Add Assets
            var systemAssignments = helper.GetSystemAssignments(ifcSystem);
            var ifcObjectDefinitions = systemAssignments as IList<IIfcObjectDefinition> ?? systemAssignments.ToList();
            if (ifcObjectDefinitions.Any())
            {
                target.Components = new List<AssetKey>();
                foreach (var ifcObjectDefinition in ifcObjectDefinitions)
                {
                    var assetKey = new AssetKey { Name = ifcObjectDefinition.Name };
                    target.Components.Add(assetKey);
                }
            }

            //Attributes
            target.Attributes = helper.GetAttributes(ifcSystem);

            //Documents
            var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
            helper.AddDocuments(docsMappings, target, ifcSystem);

            //TODO:
            //System Issues
            return target;
        }

        public override Xbim.CobieLiteUk.System CreateTargetObject()
        {
            return new Xbim.CobieLiteUk.System();
        }
    }
}
