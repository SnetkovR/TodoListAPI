using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Notes;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Route("api/notes")]
    public class NotesController : Controller
    {
        private readonly NoteService _noteService;

        public NotesController(NoteService notesService)
        {
            _noteService = notesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetNoteAsync([FromRoute] string noteId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var note = await _noteService.GetAsync(noteId, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                //TODO офк, ловить нужно только notenotfound
                return this.NotFound();
            }

            return this.Ok()
        }

        [HttpGet]
        public ActionResult<List<Note>> Get()
        {
            return _noteService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Note> Get(string id)
        {
            var note = _noteService.Get(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        [HttpPost]
        public ActionResult<Note> Create(Note note)
        {
            _noteService.Create(note);

            return CreatedAtRoute("GetBook", new { id = note.Id.ToString() }, note);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Note noteIn)
        {
            var note = _noteService.Get(id);

            if (note == null)
            {
                return NotFound();
            }

            _noteService.Update(id, noteIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var note = _noteService.Get(id);

            if (note == null)
            {
                return NotFound();
            }

            _noteService.Remove(note);

            return NoContent();
        }
    }
}