using Dapper;
using DocumentsDBCaller;
using Newtonsoft.Json;
using Npgsql;
using System.Globalization;
using System.Net;
using System.Xml;

namespace DocumentsDBCaller
{
    public class DocumentCaller : IGetDocument
    {
        const string connectionString = "ConnectionStrings:ConnectionString";

        const string selectAllCall = "SELECT json_agg(\"DocumentsTable\") FROM \"DocumentsTable\"";
        const string selectByIdCall = "SELECT json_agg(\"DocumentsTable\") FROM \"DocumentsTable\" WHERE \"ID\"  = @Id";
        const string sqlDeleteCall = "DELETE FROM \"DocumentsTable\" WHERE \"ID\"  = @Id";
        const string postNewDocumentCall = "INSERT INTO \"DocumentsTable\"" +
            "(\"ID\", \"Type\", \"Name\", \"Number\", \"ReleaseDate\", \"EntryDate\", \"KeyWords\") " +
            "\r\nVALUES (@Id, @Type, @Name, @Number, @ReleaseDate, @EnteryDate, @KeyWords)";

        private readonly IConfiguration _configuration;

        public DocumentCaller(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        //GET
        public async Task<DbResponse> GetAllDocuments()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));

            var docks = await connection.QueryAsync<string>(selectAllCall);

            if (docks == null)
            {
                return new DbResponse() { Status = HttpStatusCode.NotFound };
            }

            return new DbResponse() { Status = HttpStatusCode.OK, Data = docks.ToArray()[0] };
        }

        //GET BY ID
        public async Task<DbResponse> GetDocument(int carId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));

            var docks = await connection.QueryAsync<string>(selectByIdCall,
                new { Id = carId });

            if (docks == null)
            {
                return new DbResponse() { Status = HttpStatusCode.NotFound };
            }

            return new DbResponse() { Status = HttpStatusCode.OK, Data = docks.ToArray()[0] };
        }

        //POST
        public async Task<DbResponse> PostDocument(Document document)
        {

            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));


            var date = document.ReleaseDate.Replace('.', '/');
            var releaseDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            date = document.EntryDate.Replace('.', '/');
            var enteryDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);




            var affected = await connection.ExecuteAsync(postNewDocumentCall,
                            new { document.Id, document.Type, document.Name, document.Number, releaseDate, enteryDate, document.KeyWords });

            if (affected == 0)
            {
                return new DbResponse() { Status = HttpStatusCode.Conflict };
            }

            return new DbResponse() { Status = HttpStatusCode.OK, Data = true };
        }

        //DELETE
        public async Task<DbResponse> RemoveDocument(int idToDelete)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>(connectionString));

            var affected = await connection.ExecuteAsync(sqlDeleteCall,
                new { Id = idToDelete });

            if (affected == 0)
            {
                return new DbResponse() { Status = HttpStatusCode.Conflict };
            }

            return new DbResponse() { Status = HttpStatusCode.OK, Data = true };
        }
    }
}