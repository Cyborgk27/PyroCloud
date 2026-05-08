using PyroCloud.Core.Domain.Entities.Identity;

namespace PyroCloud.Core.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        ///<summary>
        /// Generates a JWT token for the specified user.
        /// <param name="user">The user for whom the token is being generated.</param>
        /// <returns>A JWT token as a string.</returns>
        string GenerateToken(User user);
    }
}
