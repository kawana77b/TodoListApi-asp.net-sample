using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Common.Token;
using TodoApi.Models.Attributes;

namespace TodoApi.Models.Auth
{
    [Record]
    public record RefreshToken
    {
        /// <summary>
        /// Token validity period
        /// </summary>
        public static readonly TimeSpan ExpiresInterval = TimeSpan.FromDays(30);

        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Required]
        public string UserId { get; set; } = string.Empty;

        public RefreshToken(string userId)
        {
            UserId = userId;
            UpdateToken();
        }

        /// <summary>
        /// Determine if the token has expired.
        /// </summary>
        /// <param name="isUtc">Whether to use UTC time now or regular <c>DateTime.Now</c></param>
        /// <returns></returns>
        public bool IsExpired(bool isUtc = true)
        {
            return isUtc ? DateTime.UtcNow > ExpiresAt : DateTime.Now > ExpiresAt;
        }

        /// <summary>
        /// Update the token content of this model
        /// </summary>
        public void UpdateToken()
        {
            Token = new RefreshTokenGenerator().Generate();
            ExpiresAt = DateTime.UtcNow.Add(ExpiresInterval);
        }
    }
}