﻿using CloudFoundry.CloudController.V2;
using CloudFoundry.CloudController.V2.Client.Data;
using CloudFoundry.UAA;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFoundry.Build.Tasks
{
    public class CreateApp : BaseTask
    {
        [Required]
        public string Name { get; set;}

        [Required]
        public string Space { get; set; }

        [Required]
        public string Stack { get; set; }

        public int Memory { get; set; }
        public int Instances { get; set; }

        public string Buildpack { get; set; }

        [Output]
        public string AppGuid { get; set; }

        public override bool Execute()
        {
            logger = new Microsoft.Build.Utilities.TaskLoggingHelper(this);
            CloudFoundryClient client = InitClient();

            Guid? spaceGuid = null;
            Guid? stackGuid = null;

            if (Space != string.Empty)
            {
                PagedResponseCollection<ListAllSpacesResponse> spaceList = client.Spaces.ListAllSpaces().Result;

                var space = spaceList.Where(o => o.Name == Space).FirstOrDefault();

                if (space == null)
                {
                    logger.LogError("Space {0} not found", Space);
                    return false;
                }
                spaceGuid = new Guid(space.EntityMetadata.Guid);
            }

            if (Stack != string.Empty)
            {
                PagedResponseCollection<ListAllStacksResponse> stackList = client.Stacks.ListAllStacks().Result;

                var stackInfo = stackList.Where(o => o.Name == Stack).FirstOrDefault();

                if (stackInfo == null)
                {
                    logger.LogError("Stack {0} not found", Stack);
                    return false;
                }
                stackGuid = new Guid(stackInfo.EntityMetadata.Guid);
            }

            if (stackGuid.HasValue && spaceGuid.HasValue)
            {
                CreateAppRequest request = new CreateAppRequest();
                request.Name = Name;
                request.SpaceGuid = spaceGuid;
                request.StackGuid = stackGuid;
                if (Memory > 0)
                {
                    request.Memory = Memory;
                }
                if (Instances > 0)
                {
                    request.Instances = Instances;
                }
                if (Buildpack != string.Empty)
                {
                    request.Buildpack = Buildpack;
                }

                CreateAppResponse response = client.Apps.CreateApp(request).Result;
                AppGuid = response.EntityMetadata.Guid;
                logger.LogMessage("Created app {0} with guid {1}", Name, AppGuid);
            }

            return true;
        }

    }
}