using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Managment_System.Core.DTO;
using Managment_System.Core.Helpers;
using Managment_System.Core.Interfaces;
using Managment_System.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Managment_System.EF.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return new AuthDTO { Message = "Email is already registered!" };
            }
            if (await _userManager.FindByNameAsync(registerDTO.Username) is not null)
            {
                return new AuthDTO { Message = "Username is already registered!" };
            }

            var user = new ApplicationUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthDTO { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);
 
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthDTO
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthDTO> LoginAsync(LoginDTO loginDTO)
        {
            var authDTO = new AuthDTO();

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                authDTO.Message = "Email or Password is incorrect!";
                return authDTO;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authDTO.IsAuthenticated = true;
            authDTO.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDTO.Email = user.Email;
            authDTO.Username = user.UserName;
            authDTO.ExpiresOn = jwtSecurityToken.ValidTo;
            authDTO.Roles = rolesList.ToList();

            return authDTO;
        }

        public async Task<string> AddRoleAsync(AddRoleDTO addRoleDTO)
        {
            var user = await _userManager.FindByEmailAsync(addRoleDTO.Email);

            if (user is null || !await _roleManager.RoleExistsAsync(addRoleDTO.Role))
                return "Invalid user or role";

            if (await _userManager.IsInRoleAsync(user, addRoleDTO.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, addRoleDTO.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }
        public async Task<string> DeleteUserAsync(string email)
        {
           
            var user = await _userManager.FindByEmailAsync(email);

           
            if (user == null)
            {
                return "User not found.";
            }

         
            var result = await _userManager.DeleteAsync(user);

           
            return result.Succeeded ? string.Empty : "Something went wrong";
        }
    }
}
