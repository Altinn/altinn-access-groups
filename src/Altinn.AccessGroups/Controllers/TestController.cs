// <copyright file="TestController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace Altinn.AccessGroups.Controllers
{
    /// <summary>
    /// Controller for test related methods.
    /// </summary>
    [ApiController]
    [Route("accessgroups/api/v1/[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        public TestController()
        {
        }

        /// <summary>
        /// Test method.
        /// </summary>
        /// <returns>test string</returns>
        [HttpGet]
        public string Get()
        {
            return "Hello world!";
        }
    }
}
