using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
    }
}
