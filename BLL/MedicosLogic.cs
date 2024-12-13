using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;
using Entities;
using Medicos = Entities.Medicos;

namespace BLL
{
    public class MedicosLogic
    {
        public Medicos Create(Medicos medico)
        {
            Medicos _medico = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Medicos _result = repository.Retrieve<Medicos>
                    (m => m.Nombre == medico.Nombre && m.Especialidad == medico.Especialidad);
                if (_result == null)
                {
                    _medico = repository.Create(medico);
                }
                else
                {
                    throw new Exception("El médico ya existe");
                }
            }
            return medico;
        }

        public Medicos RetrieveById(int id)
        {
            Medicos _medico = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                _medico = repository.Retrieve<Medicos>(m => m.MedicoID == id);
            }
            return _medico;
        }

        public bool Update(Medicos medico)
        {
            bool _updated = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Medicos _result = repository.Retrieve<Medicos>
                    (m => m.Nombre == medico.Nombre && m.MedicoID != medico.MedicoID);
                if (_result == null)
                {
                    _updated = repository.Update(medico);
                }
                else
                {
                    throw new Exception("Otro médico con el mismo nombre ya existe");
                }
            }
            return _updated;
        }

        public bool Delete(int id)
        {
            bool _delete = false;
            var _medico = RetrieveById(id);
            if (_medico != null)
            {
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    _delete = repository.Delete(_medico);
                }
            }
            return _delete;
        }

        public List<Medicos> RetrieveAll()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Usar una expresión lambda para obtener todos los médicos
                return repository.Filter<Medicos>(m => m.MedicoID > 0).ToList();
            }
        }
    }
}