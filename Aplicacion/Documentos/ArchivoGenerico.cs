using System;

namespace Aplicacion.Documentos
{
    public class ArchivoGenerico
    {
        public Guid? ObjetorReferencia{set;get;}

        public Guid? DocumentoId{set;get;}

        public string Nombre{set;get;}

        public string Data{set;get;}

        public string Extension{set;get;}
    }
}