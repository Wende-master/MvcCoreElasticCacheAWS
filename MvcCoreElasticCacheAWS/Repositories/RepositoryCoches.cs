﻿using MvcCoreElasticCacheAWS.Models;
using System.Xml.Linq;

namespace MvcCoreElasticCacheAWS.Repositories
{
    public class RepositoryCoches
    {
        public XDocument document;

        public RepositoryCoches()
        {
            string path = "MvcCoreElasticCacheAWS.Documents.coches.xml";
            Stream stream = this.GetType().Assembly
                .GetManifestResourceStream(path);

            this.document = XDocument.Load(stream);
        }

        public List<Coche> GetCoches()
        {
            var consulta =
                from datos in this.document.Descendants("coche")
                select new Coche
                {
                    IdCoche = int.Parse(datos.Element("idcoche").Value),
                    Marca = datos.Element("marca").Value,
                    Modelo = datos.Element("modelo").Value,
                    Imagen = datos.Element("imagen").Value
                };
            return consulta.ToList();
        }

        public Coche FindCoche(int id)
        {
            return this.GetCoches()
                .FirstOrDefault(x => x.IdCoche == id);

        }
    }
}
