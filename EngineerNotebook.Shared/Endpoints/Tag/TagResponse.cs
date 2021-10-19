namespace EngineerNotebook.Shared.Endpoints.Tag
{
    public class TagResponse : IDtoResponse<Models.Tag>
    {
        public Models.Tag Result { get; set; }
    }
}