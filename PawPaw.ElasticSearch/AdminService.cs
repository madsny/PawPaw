namespace PawPaw.ElasticSearch
{
    public class AdminService
    {
        private readonly Indexer _indexer;

        public AdminService()
        {
            _indexer = new Indexer();
        }

        public void DeleteIndex()
        {
            _indexer.DeleteIndex();
        }

        public void EnsureIndexExists()
        {
            _indexer.EnsureIndexExists();
        }
    }
}
