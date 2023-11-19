namespace CompanyPMO_.NET.Dto
{
    public class EntityParticipantOrOwnerDTO<T>
    {
        public T Entity { get; set; }
        public bool IsParticipant { get; set; }
        public bool IsOwner { get; set; }
    }
}
