namespace DocumentsDBCaller
{
    public class Document
    {
        public int Id { get; set; }
     
        public string Type { get; set; }
        
        public string Name { get; set; }
        
        public string Number { get; set; }
        
        public string ReleaseDate { get; set; }
        
        public string EntryDate { get; set; }
        
        public string? KeyWords { get; set; }
    }
}
