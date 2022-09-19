// <copyright file="MembershipController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Altinn.AccessGroups.Core.Models;
using Altinn.AccessGroups.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Altinn.AccessGroups.Controllers
{
    /// <summary>
    /// Controller responsible for operations regarding GroupMemberships.
    /// </summary>
    [Route("authorization/api/v1/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMemberships membership;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipController"/> class.
        /// </summary>
        /// <param name="membership">The membership interface.</param>
        public MembershipController(IMemberships membership)
        {
            this.membership = membership;
        }

        /// <summary>
        /// Method for listing GroupMemberships based on search parameters.
        /// </summary>
        /// <param name="search">AccessGroupSearch model containing the search parameters.</param>
        /// <returns>The response from the ListGroupMemberships method.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/ListGroupMemberships")]
        public async Task<ActionResult> ListGroupMemberships([FromBody] AccessGroupSearch search)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            List<AccessGroup> result = await this.membership.ListGroupMemberships(search);

            return this.Ok(result);
        }

        /// <summary>
        /// Method for adding GroupMembership.
        /// </summary>
        /// <param name="input">The GroupMembership model to be inserted.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/AddMembership")]
        public async Task<ActionResult> AddMembership([FromBody] GroupMembership input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            bool result = await this.membership.AddMembership(input);

            if (result)
            {
                return this.Created("www.altinn.no", "Created");
            }
            else
            {
                return this.BadRequest("Membership was not created");
            }
        }

        /// <summary>
        /// Method for revoking GroupMembership.
        /// </summary>
        /// <param name="input">The GroupMembership model to be deleted.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        [HttpPost]
        [Route("authorization/api/v1/[controller]/RevokeMembership")]
        public async Task<ActionResult> RevokeMembership([FromBody] GroupMembership input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            bool result = await this.membership.RevokeMembership(input);

            return this.Ok(result);
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
