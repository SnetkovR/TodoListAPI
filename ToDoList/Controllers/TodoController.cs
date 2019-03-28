using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ClientModels.Todos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Todo;
using ToDoList.Errors;
using ToDoList.Services;
using Todo = Models.Todos.Todo;

namespace ToDoList.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/todos")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly TodoService _todoService;
        public TodoController(TodoService todosService, UserRepository userRepository)
        {
            _todoService = todosService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateNoteAsync([FromBody]ClientModels.Todos.TodoBuildInfo buildInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (buildInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("buildInfo");
                return this.BadRequest(error);
            }
            var nameIdentifier = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetAsync(nameIdentifier.Value, cancellationToken).ConfigureAwait(false);

            var creationInfo = TodoBuildInfoConverter.Convert(user.Id.ToString(), buildInfo);

            var modelTodoInfo = await _todoService.CreateAsync(creationInfo, cancellationToken).ConfigureAwait(false);

            var clientTodoInfo = TodoInfoConverter.Convert(modelTodoInfo);

            var routeParams = new Dictionary<string, object>
            {
                {"recordId", clientTodoInfo.Id }
            };

            return CreatedAtRoute("GetTodoRoute", routeParams, clientTodoInfo);
        }


        [HttpGet]
        [Route("{recordId}", Name = "GetTodoRoute")]
        public async Task<IActionResult> GetTodoAsync([FromRoute] string recordId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Todo record;
            var nameIdentifier = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetAsync(nameIdentifier.Value, cancellationToken).ConfigureAwait(false);

            if (!Guid.TryParse(recordId, out var modelTodoId))
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return this.NotFound(error);
            }

            if (!await IsSameUser(modelTodoId, cancellationToken))
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return NotFound(error);
            }

            try
            {
                record = await _todoService.GetAsync(modelTodoId, cancellationToken).ConfigureAwait(false);
            }
            catch (RecordNotFoundException e)
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return NotFound(error);
            }

            var clientRecord = TodoConverter.Convert(record);

            return this.Ok(clientRecord);
        }

        [HttpPatch]
        [Route("{recordId}")]
        public async Task<IActionResult> PatchNoteAsync([FromRoute]string recordId, 
            [FromBody]ClientModels.Todos.TodoPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("RecordPatchInfo");
                return this.BadRequest(error);
            }

            if (!Guid.TryParse(recordId, out var recordIdGuid))
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return this.NotFound(error);
            }

            if (!await IsSameUser(recordIdGuid, cancellationToken))
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return NotFound(error);
            }

            var modelPatchInfo = TodoPathcInfoConverter.Convert(recordIdGuid, patchInfo);

            Models.Todos.Todo modelRecord;

            try
            {
                modelRecord = await this._todoService.PatchAsync(modelPatchInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (RecordNotFoundException)
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return this.NotFound(error);
            }

            var clientNote = TodoConverter.Convert(modelRecord);
            return this.Ok(clientNote);
        }

        [HttpDelete]
        [Route("{recordId}")]
        public async Task<IActionResult> DeleteNoteAsync([FromRoute] string recordId,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(recordId, out var recordIdGuid))
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return this.NotFound(error);
            }

            if (!await IsSameUser(recordIdGuid, cancellationToken))
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return NotFound(error);
            }

            try
            {
                await this._todoService.RemoveAsync(recordIdGuid, cancellationToken).ConfigureAwait(false);
            }
            catch (RecordNotFoundException)
            {
                var error = ServiceErrorResponses.TodoNotFound(recordId);
                return this.NotFound(error);
            }

            return this.NoContent();
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchNotesAsync([FromQuery]TodoInfoSearchQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelQuery = TodoInfoSearchQueryConverter.Convert(query ?? new ClientModels.Todos.TodoInfoSearchQuery());
            var nameIdentifier = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetAsync(nameIdentifier.Value, cancellationToken);
            modelQuery.UserId = user.Id;
            var modelNotes = await this._todoService.SearchAsync(modelQuery, cancellationToken).ConfigureAwait(false);
            var clientrecords = modelNotes.Select(record => TodoInfoConverter.Convert(record)).ToList();
            var clientNotesList = new ClientModels.Todos.TodoList()
            {
                Todos = clientrecords
            };

            return this.Ok(clientNotesList);
        }

        private async Task<bool> IsSameUser(Guid recordIdGuid, CancellationToken cancellationToken)
        {
            var nameIdentifier = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetAsync(nameIdentifier.Value, cancellationToken).ConfigureAwait(false);
            var record = await _todoService.GetAsync(recordIdGuid, cancellationToken);
            return record.UserId == user.Id;
        }
    }
}