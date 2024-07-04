namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ApiResourcesController(ConfigurationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiResource>>> GetApiResources()
    {
        var apiResources = await context.ApiResources.ToListAsync();
        return Ok(apiResources);
    }

    [HttpPost]
    public async Task<ActionResult> CreateApiResource([FromBody] ApiResource apiResource)
    {
        context.ApiResources.Add(apiResource);
        await context.SaveChangesAsync();
        return Ok(apiResource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateApiResource(int id, [FromBody] ApiResource apiResource)
    {
        var existingApiResource = await context.ApiResources.FindAsync(id);
        if (existingApiResource == null)
        {
            return NotFound();
        }

        existingApiResource.Name = apiResource.Name;
        existingApiResource.DisplayName = apiResource.DisplayName;
        // Update other properties as needed
        await context.SaveChangesAsync();
        return Ok(existingApiResource);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteApiResource(int id)
    {
        var apiResource = await context.ApiResources.FindAsync(id);
        if (apiResource == null)
        {
            return NotFound();
        }

        context.ApiResources.Remove(apiResource);
        await context.SaveChangesAsync();
        return Ok();
    }
}