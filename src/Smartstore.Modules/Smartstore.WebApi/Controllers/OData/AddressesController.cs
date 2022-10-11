﻿using Smartstore.Core.Common;

namespace Smartstore.Web.Api.Controllers.OData
{
    /// <summary>
    /// The endpoint for operations on Address entity.
    /// </summary>
    public class AddressesController : WebApiController<Address>
    {
        [HttpGet, WebApiQueryable]
        [Permission(Permissions.Customer.Read)]
        public IQueryable<Address> Get()
        {
            return Entities.AsNoTracking();
        }

        [HttpGet, WebApiQueryable]
        [Permission(Permissions.Customer.Read)]
        public Task<IActionResult> Get(int key)
        {
            return GetByIdAsync(key);
        }

        [HttpGet("addresses({key})/{property}")]
        [HttpGet("addresses/{key}/{property}")]
        [Permission(Permissions.Customer.Read)]
        public Task<IActionResult> GetProperty(int key, string property)
        {
            return GetPropertyValueAsync(key, property);
        }

        [HttpPost]
        [Permission(Permissions.Customer.Create)]
        public async Task<IActionResult> Post([FromBody] Address entity)
        {
            return await PostAsync(entity);
        }

        [HttpPut]
        [Permission(Permissions.Customer.Update)]
        public async Task<IActionResult> Put(int key, Delta<Address> model)
        {
            return await PutAsync(key, model);
        }

        [HttpPatch]
        [Permission(Permissions.Customer.Update)]
        public async Task<IActionResult> Patch(int key, Delta<Address> model)
        {
            return await PatchAsync(key, model);
        }

        [HttpDelete]
        [Permission(Permissions.Customer.Delete)]
        public async Task<IActionResult> Delete(int key)
        {
            return await DeleteAsync(key);
        }
    }
}