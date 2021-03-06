﻿namespace ImgShareDemo.BLL
{
    using BO.LinkedInResponse;
    using ImgShareDemo.BO.DataTransfer;
    using ImgShareDemo.BO.Entities;
    using ImgShareDemo.DAL;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserService : IDisposable
    {
        private IUnitOfWork _uow;
        private LinkedInApiService _linkedInApiService;
        public UserService()
        {
            _uow = new UnitOfWork();
        }
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<LinkedInUser> InitializeUserFromLinkedIn(LinkedInTokenResponse token)
        {
            LinkedInApiService api = new LinkedInApiService(token.access_token);
            IncomingApiDataResponse<LinkedInPersonResponse> personResponse = await api.GetPerson(token.access_token).ConfigureAwait(false);
            LinkedInUser linkedInUser = (await _uow.LinkedInUserRepository.GetAsync(lu => lu.LinkedInId == personResponse.Data.id).ConfigureAwait(false)).FirstOrDefault();

            if(linkedInUser == null)
            {
                linkedInUser = new LinkedInUser
                {
                    LinkedInId = personResponse.Data.id,
                };
                User user = new User();
                _uow.UserRepository.Insert(user);
                linkedInUser.User = user;
                _uow.LinkedInUserRepository.Insert(linkedInUser);
            }

            linkedInUser.ProfileImageUrl = personResponse.Data.pictureUrl;
            linkedInUser.ProfileRequestUrl = personResponse.Data.publicProfileUrl;
            linkedInUser.Token = token.access_token;
            linkedInUser.TokenExpires = DateTime.UtcNow.AddSeconds(token.expires_in);

            linkedInUser.User.Email = personResponse.Data.emailAddress;
            linkedInUser.User.FirstName = personResponse.Data.firstName;
            linkedInUser.User.LastName = personResponse.Data.lastname;
            linkedInUser.User.ImageUrl = personResponse.Data.pictureUrl;
            if(String.IsNullOrEmpty(linkedInUser.User.Username)
                && !String.IsNullOrEmpty(personResponse.Data.emailAddress)
                && (await _uow.UserRepository.GetAsync(u => u.Username == personResponse.Data.emailAddress).ConfigureAwait(false)) == null)
            {
                linkedInUser.User.Username = personResponse.Data.emailAddress;
            }
            else
            {
                linkedInUser.User.Username = linkedInUser.LinkedInId;
            }

            await _uow.SaveChangesAsync().ConfigureAwait(false);

            return linkedInUser;
        }
        public void Dispose() => _uow.Dispose();
    }
}
