// <copyright file="AccessGroupController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Altinn.AccessGroups.Core.Models;
using Altinn.AccessGroups.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Altinn.AccessGroups.Controllers
{
    /// <summary>
    /// Controller for handling AccessGroup related methods.
    /// </summary>
    [ApiController]
    public class AccessGroupController : ControllerBase
    {
        private IAccessGroup accessGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessGroupController"/> class.
        /// </summary>
        /// <param name="accessGroup">The AccessGroup interface.</param>
        public AccessGroupController(IAccessGroup accessGroup)
        {
            this.accessGroup = accessGroup;
        }

        /// <summary>
        /// Method for creating AccessGroup.
        /// </summary>
        /// <param name="accessGroup">AccessGroup model for the AccessGroup to be created.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/CreateGroup")]
        public async Task<ActionResult> CreateGroup([FromBody] AccessGroup accessGroup)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(await this.accessGroup.CreateGroup(accessGroup));
        }

        /// <summary>
        /// Method for updating AccessGroup.
        /// </summary>
        /// <param name="accessGroup">AccessGroup model for the AccessGroup to be updated.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/UpdateGroup")]
        public async Task<ActionResult> UpdateGroup([FromBody] AccessGroup accessGroup)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            bool result = await this.accessGroup.UpdateGroup(accessGroup);

            return this.Ok(result);
        }

        /// <summary>
        /// Method for exporting AccessGroups.
        /// </summary>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpGet]
        [Route("authorization/api/v1/[controller]/ExportAccessGroups")]
        public async Task<ActionResult> ExportAccessGroups()
        {
            return this.Ok(await this.accessGroup.GetAccessGroups());
        }

        /// <summary>
        /// Method for importing AccessGroups.
        /// </summary>
        /// <param name="accessGroups">A list of AccessGroups to be imported.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/ImportAccessGroups")]
        public async Task<ActionResult> ImportAccessGroups([FromBody] List<AccessGroup> accessGroups)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            List<AccessGroup> result = await this.accessGroup.ImportAccessGroups(accessGroups);

            return this.Ok(result);
        }

        /// <summary>
        /// Method for importing ExternalRelationships.
        /// </summary>
        /// <param name="externalRelationships">A list of ExternalRelationships to be imported.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/ImportExternalRelationships")]
        public async Task<ActionResult> ImportExternalRelationships([FromBody] List<ExternalRelationship> externalRelationships)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(await this.accessGroup.ImportExternalRelationships(externalRelationships));
        }

        /// <summary>
        /// Method for exporting ExternalRelationships.
        /// </summary>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpGet]
        [Route("authorization/api/v1/[controller]/ExportExternalRelationships")]
        public async Task<ActionResult> ExportExternalRelationships()
        {
            return this.Ok(await this.accessGroup.GetExternalRelationships());
        }

        /// <summary>
        /// Method for importing Categories.
        /// </summary>
        /// <param name="categories">A list of categories to be imported.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/ImportCategories")]
        public async Task<ActionResult> ImportCategories([FromBody] List<Category> categories)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(await this.accessGroup.ImportCategories(categories));
        }

        /// <summary>
        /// Method for exporting categories.
        /// </summary>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpGet]
        [Route("authorization/api/v1/[controller]/ExportCategories")]
        public async Task<ActionResult> ExportCategories()
        {
            return this.Ok(await this.accessGroup.GetCategories());
        }

        /// <summary>
        /// Hello world method.
        /// </summary>
        /// <returns>Hello world.</returns>
        [HttpGet]
        [Route("authorization/api/v1/[controller]/")]
        public string Get()
        {
            return "Hello world!";
        }
    }
}
