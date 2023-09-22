using Microsoft.AspNetCore.Mvc;
using MongoDBWebApp.Models;
using MongoDBWebApp.Services;

namespace MongoDBWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViewController : ControllerBase
{
    private readonly ViewService _viewService;

    public ViewController(ViewService viewService) =>
        _viewService = viewService;

    [HttpGet]
    public async Task<List<ViewBO>> Get() =>
        await _viewService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ViewBO>> Get(string id)
    {
        var view = await _viewService.GetAsync(id);

        if (view is null)
        {
            return NotFound();
        }

        return view;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ViewBO newView)
    {
        await _viewService.CreateAsync(newView);

        return CreatedAtAction(nameof(Get), new { id = newView.Id }, newView);
    }

    [HttpPut("{id:length(24)}/{type:length(4)}")]
    public async Task<IActionResult> Update(string id, string type, FieldBO updatedView)
    {
        var view = await _viewService.GetAsync(id);

        if (updatedView is null)
        {
            return NotFound();
        }

        await _viewService.UpdateAsync(id, type, updatedView);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var view = await _viewService.GetAsync(id);

        if (view is null)
        {
            return NotFound();
        }

        await _viewService.RemoveAsync(id);

        return NoContent();
    }
}