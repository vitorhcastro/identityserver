namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ApiScopesController(ConfigurationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiScope>>> GetApiScopes()
    {
        var apiScopes = await context.ApiScopes.ToListAsync();
        return Ok(apiScopes);
    }

    [HttpPost]
    public async Task<ActionResult> CreateApiScope([FromBody] ApiScope apiScope)
    {
        context.ApiScopes.Add(apiScope);
        await context.SaveChangesAsync();
        return Ok(apiScope);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateApiScope(int id, [FromBody] ApiScope apiScope)
    {
        var existingApiScope = await context.ApiScopes.FindAsync(id);
        if (existingApiScope == null)
        {
            return NotFound();
        }

        existingApiScope.Name = apiScope.Name;
        existingApiScope.DisplayName = apiScope.DisplayName;
        existingApiScope.Description = apiScope.Description;
        existingApiScope.Enabled = apiScope.Enabled;
        // Update other properties as needed
        await context.SaveChangesAsync();
        return Ok(existingApiScope);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteApiScope(int id)
    {
        var apiScope = await context.ApiScopes.FindAsync(id);
        if (apiScope == null)
        {
            return NotFound();
        }

        context.ApiScopes.Remove(apiScope);
        await context.SaveChangesAsync();
        return Ok();
    }
}
