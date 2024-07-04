namespace Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientsController(ConfigurationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        var clients = await context.Clients.ToListAsync();
        return Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult> CreateClient([FromBody] Client client)
    {
        context.Clients.Add(client);
        await context.SaveChangesAsync();
        return Ok(client);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateClient(int id, [FromBody] Client client)
    {
        var existingClient = await context.Clients.FindAsync(id);
        if (existingClient == null)
        {
            return NotFound();
        }

        existingClient.ClientId = client.ClientId;
        // Update other properties as needed
        await context.SaveChangesAsync();
        return Ok(existingClient);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteClient(int id)
    {
        var client = await context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync();
        return Ok();
    }
}
