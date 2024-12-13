using System.Collections.Generic;
using System.Web.Services;
using BLL;
using Entidades = Entities; // Alias para el espacio de nombres Entities
using System.Xml.Serialization;

namespace SOAP
{
    /// <summary>
    /// Servicio web para la gestión de médicos.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class MedicosService : System.Web.Services.WebService
    {
        private MedicosLogic _logic = new MedicosLogic();

        [WebMethod]
        public List<SerializableMedico> GetAllMedicos()
        {
            var medicos = _logic.RetrieveAll();
            return medicos.ConvertAll(m => new SerializableMedico(m));
        }

        [WebMethod]
        public SerializableMedico GetMedicoById(int id)
        {
            var medico = _logic.RetrieveById(id);
            if (medico == null)
            {
                return null;
            }
            return new SerializableMedico(medico);
        }

        [WebMethod]
        public void CreateMedico(SerializableMedico medico)
        {
            _logic.Create(medico.ToMedico());
        }

        [WebMethod]
        public void UpdateMedico(SerializableMedico medico)
        {
            _logic.Update(medico.ToMedico());
        }

        [WebMethod]
        public void DeleteMedico(int id)
        {
            _logic.Delete(id);
        }
    }

    /// <summary>
    /// Clase serializable para médicos.
    /// </summary>
    [XmlRoot("Medico")]
    public class SerializableMedico
    {
        public int MedicoID { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public string Telefono { get; set; } // Opcional, se serializa como string.

        public SerializableMedico() { }

        public SerializableMedico(Entidades.Medicos medico)
        {
            MedicoID = medico.MedicoID;
            Nombre = medico.Nombre;
            Especialidad = medico.Especialidad;
            Telefono = medico.Telefono;
        }

        public Entidades.Medicos ToMedico()
        {
            return new Entidades.Medicos
            {
                MedicoID = this.MedicoID,
                Nombre = this.Nombre,
                Especialidad = this.Especialidad,
                Telefono = this.Telefono
            };
        }
    }
}