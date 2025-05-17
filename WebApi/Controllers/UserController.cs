using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Users;
using Raven.Abstractions.Data;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;
        private readonly IDocumentSession _session;

        public UserController(ICreateUserService createUserService, IDeleteUserService deleteUserService,
            IGetUserService getUserService, IUpdateUserService updateUserService, IDocumentSession session)
        {
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
            _session = session;
        }

        [Route("{userId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Guid userId, [FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var existingUser = _session.Load<User>($"users/{userId}");
            if (existingUser != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "User already exists.");
            }
            var user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            _updateUserService.Update(user, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);

            try
            {
                _session.Advanced.UseOptimisticConcurrency = true;
                _session.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, new UserData(user));
            }
            catch (ConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "User was modified by another process. Please try again.");
            }
        }





        [Route("{userId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateUser(Guid userId, [FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var user = _getUserService.GetUser(userId);
            var userTypeString = model.Type.ToString();
            if (user == null)
            {
                return DoesNotExist();
            }
            _updateUserService.Update(user, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            return Found(new UserData(user));
        }

        [Route("{userId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }
            _deleteUserService.Delete(user);
            return Found();
        }

        [Route("{userId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            return Found(new UserData(user));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetUsers(int skip = 0, int take = 50, UserTypes? type = null, string name = null, string email = null, string tag = null)
        {
            var tags = string.IsNullOrWhiteSpace(tag) ? null : new[] { tag };

            var users = _getUserService.GetUsers(type, name, email, tags)
                                       .Skip(skip)
                                       .Take(take)
                                       .Select(u => new UserData(u))
                                       .ToList();

            return Found(users);
        }

        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllUsers()
        {
            _deleteUserService.DeleteAll();
            return Found();
        }

        [Route("list/tag")]
        [HttpGet]
        public HttpResponseMessage GetUsersByTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var users = _getUserService.GetUsers(null, null, null, new[] { tag });

            if (users == null || !users.Any())
            {
                return DoesNotExist();
            }

            return Found(users);
        }
    }
}