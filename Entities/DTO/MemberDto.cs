namespace API.Entities.DTO
{
    /// <summary>
    /// Data transfer object for member profile information.
    /// Excludes sensitive data like password hashes.
    /// </summary>
    public class MemberDto
    {
        public required string Id { get; set; }

        public required string DisplayName { get; set; }

        public required string Email { get; set; }

        public string? ImageUrl { get; set; }
    }
}
