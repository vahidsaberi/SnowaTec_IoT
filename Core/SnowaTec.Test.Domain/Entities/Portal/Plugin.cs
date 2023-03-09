namespace SnowaTec.Test.Domain.Entities.Portal
{
    public class Plugin : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Installed { get; set; }
    }
}
