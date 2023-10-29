namespace DocumentsDBCaller
{
    public interface IGetDocument
    {
        Task<DbResponse> GetAllDocuments();

        Task<DbResponse> GetDocument(int Id);

        Task<DbResponse> RemoveDocument(int Id);

        Task<DbResponse> PostDocument(Document lot);
    }
}
