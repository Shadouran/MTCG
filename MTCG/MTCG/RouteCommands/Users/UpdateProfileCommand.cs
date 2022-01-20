using MTCG.Core.Response;
using MTCG.Models;
using System;

namespace MTCG.RouteCommands.Users
{
    class UpdateProfileCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public User UserProfile { get; set; }
        public string Username { get; set; }

        public UpdateProfileCommand(IGameManager gameManager, User userProfile, string username)
        {
            GameManager = gameManager;
            UserProfile = userProfile;
            Username = username;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                GameManager.UpdateUserProfile(User, UserProfile, Username);
                response.Payload = "Successfully updated profile";
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception e)
            {
                response.Payload = e.Message;
                response.StatusCode = StatusCode.BadRequest;
            }
            return response;
        }
    }
}
