using MongoDB.Driver;
using AssetRental.Infrastructure.Logs.Collections;
using AssetRental.Infrastructure.Logs.Contexts;

namespace AssetRental.Infrastructure.Logs.Persistence
{
    public class LogMensagensPersistence
    {
        private readonly MongoDBContext? _mongoDBContext;

        public LogMensagensPersistence(MongoDBContext? mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        public void Add(LogMensagens log)
        {
            _mongoDBContext.LogMensagens
                .InsertOne(log);
        }

        public List<LogMensagens> GetAll()
        {

            var logMensagensCollection = _mongoDBContext.LogMensagens.Database.GetCollection<LogMensagens>("logmensagens");
            var logDocumentList = logMensagensCollection.Find(Builders<LogMensagens>.Filter.Empty).ToList();

            foreach (var logDocument in logDocumentList)
            {
                // Aqui você pode acessar as propriedades individuais do documento
                var id = logDocument.Id;
                var dataHoraEnvio = logDocument.DataHoraEnvio;
                var emailPara = logDocument.EmailPara;
                var assunto = logDocument.Assunto;
                var conteudo = logDocument.Conteudo;

                // Faça o que precisar com as informações do log
            }

            var filter = Builders<LogMensagens>.Filter.Where(l => true);
            return _mongoDBContext.LogMensagens
                .Find(filter)
                .SortByDescending(l => l.DataHoraEnvio)
                .ToList();
        }
    }
}



