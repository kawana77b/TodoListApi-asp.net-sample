using TodoApi.Data;
using TodoApi.Models.Auth;
using TodoApi.Repository.Base;
using TodoApi.Repository.Contracts;

namespace TodoApi.Repository
{
    public class RefreshTokenRepository(AppDbContext context) : AppDbRepository(context), IRepository<RefreshToken>
    {
        public RefreshToken? FetchByUserId(string userId)
        {
            return Context.RefreshTokens.FirstOrDefault(rt => rt.UserId == userId);
        }

        public RefreshToken? FetchByToken(string token)
        {
            return Context.RefreshTokens.FirstOrDefault(rt => rt.Token == token);
        }

        public void RemoveByUserId(string userId)
        {
            var refreshToken = FetchByUserId(userId);
            if (refreshToken != null)
            {
                Remove(refreshToken);
            }
        }

        public void Add(RefreshToken refreshToken)
        {
            refreshToken.CreatedAt = DateTime.Now;
            refreshToken.UpdatedAt = DateTime.Now;
            WithTransaction(() =>
            {
                Context.RefreshTokens.Add(refreshToken);
                Context.SaveChanges();
            });
        }

        public void Update(RefreshToken refreshToken)
        {
            refreshToken.UpdatedAt = DateTime.Now;
            WithTransaction(() =>
            {
                Context.RefreshTokens.Update(refreshToken);
                Context.SaveChanges();
            });
        }

        public void Remove(RefreshToken refreshToken)
        {
            WithTransaction(() =>
            {
                Context.RefreshTokens.Remove(refreshToken);
                Context.SaveChanges();
            });
        }
    }
}