using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUser(UserRegisterModel model)
        {
            var user = await _userRepository.GetUserByEmail(model.Email);
            if(user != null)
            {
                throw new ConflictException("Email already exists, please try to login");
            }

            var salt = GetRandomSalt();
            var hashedPassword = GetHashedPassword(model.Password, salt);

            var newUser = new User
            {
                Email = model.Email,
                HashedPassword = hashedPassword,
                Salt = salt,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            };

            var savedUser = await _userRepository.Add(newUser);
            if(savedUser.Id > 1)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if(user == null)
            {
                throw new Exception("Email does not exist");
            }

            var hashedPassword = GetHashedPassword(password, user.Salt);

            if(hashedPassword == user.HashedPassword)
            {
                return true;
            }
            return false;
        }

        private string GetRandomSalt()
        {
            var randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        private string GetHashedPassword(string password, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password,
           Convert.FromBase64String(salt),
           KeyDerivationPrf.HMACSHA512,
           10000,
           256 / 8));
            return hashed;
        }
    }
}
