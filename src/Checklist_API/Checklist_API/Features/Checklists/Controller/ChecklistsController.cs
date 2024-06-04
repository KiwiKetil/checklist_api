using Microsoft.AspNetCore.Mvc;

<<<<<<<< HEAD:src/Checklist_API/Checklist_API/Features/Checklists/Controller/ChecklistsController.cs
namespace Checklist_API.Features.Checklists.Controller;
========
namespace Checklist_API.Content.Checklists;
>>>>>>>> main:src/Checklist_API/Checklist_API/Content/Checklists/ChecklistsController.cs
[Route("api/v1/checklists")]
[ApiController]
public class CheckListsController : ControllerBase
{
    // GET: api/<CheckListController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<CheckListController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<CheckListController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<CheckListController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<CheckListController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
