import zeep
from zeep.transports import Transport
from requests import Session
import urllib3  # Importar para deshabilitar las advertencias

# Deshabilitar todas las advertencias de SSL
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

# URL del servicio SOAP para médicos
url = 'https://localhost:44371/MedicosService.asmx?WSDL'

# Deshabilitar la verificación del certificado SSL
session = Session()
session.verify = False  # No verificar el certificado SSL

# Actualizar los encabezados de la solicitud si es necesario
session.headers.update({
    'User-Agent': 'ZeepClient',
    'Content-Type': 'text/xml;charset=UTF-8'
})

# Crear un transporte con la sesión modificada
transport = Transport(session=session)

# Crear el cliente SOAP usando la URL del WSDL y el transporte
try:
    client = zeep.Client(url, transport=transport)
    print("Cliente SOAP creado correctamente.")
except Exception as e:
    print(f"Error al crear el cliente SOAP: {e}")
    exit()

# Método para obtener todos los médicos
def obtener_medicos():
    try:
        medicos = client.service.GetAllMedicos()
        for medico in medicos:
            print(f"ID: {medico.MedicoID}, Nombre: {medico.Nombre}, Especialidad: {medico.Especialidad}, Teléfono: {medico.Telefono}")
    except Exception as e:
        print(f"Error al obtener médicos: {e}")

# Método para obtener un médico por ID
def obtener_medico_por_id(id_medico):
    try:
        medico = client.service.GetMedicoById(id_medico)
        if medico:
            print(f"ID: {medico.MedicoID}, Nombre: {medico.Nombre}, Especialidad: {medico.Especialidad}, Teléfono: {medico.Telefono}")
        else:
            print(f"Médico con ID {id_medico} no encontrado.")
    except Exception as e:
        print(f"Error al obtener médico por ID: {e}")

# Método para agregar un nuevo médico
def agregar_medico(nombre, especialidad, telefono):
    try:
        # Primero, verifica si el médico ya existe
        medicos = client.service.GetAllMedicos()
        for medico in medicos:
            if medico.Nombre.lower() == nombre.lower():
                print(f"Error: El médico '{nombre}' ya existe.")
                return
        
        # Si no existe, procede a agregarlo
        nuevo_medico = {
            'MedicoID': 0,  # Asumiendo que el ID se genera automáticamente
            'Nombre': nombre,
            'Especialidad': especialidad,
            'Telefono': telefono
        }
        client.service.CreateMedico(nuevo_medico)
        print(f"Médico '{nombre}' agregado con éxito.")
    except zeep.exceptions.Fault as fault:
        if 'ya existe' in fault.message.lower():
            print(f"Error: El médico '{nombre}' ya existe.")
        else:
            print(f"Error al agregar médico: {fault.message}")
    except Exception as e:
        print(f"Error al agregar médico: {e}")

# Método para actualizar un médico existente
def actualizar_medico(id_medico, nombre, especialidad, telefono):
    try:
        medico = {
            'MedicoID': id_medico,
            'Nombre': nombre,
            'Especialidad': especialidad,
            'Telefono': telefono
        }
        client.service.UpdateMedico(medico)
        print(f"Médico con ID {id_medico} actualizado con éxito.")
    except zeep.exceptions.Fault as fault:
        print(f"Error al actualizar médico: {fault.message}")
    except Exception as e:
        print(f"Error al actualizar médico: {e}")

# Método para eliminar un médico
def eliminar_medico(id_medico):
    try:
        client.service.DeleteMedico(id_medico)
        print(f"Médico con ID {id_medico} eliminado con éxito.")
    except zeep.exceptions.Fault as fault:
        print(f"Error al eliminar médico: {fault.message}")
    except Exception as e:
        print(f"Error al eliminar médico: {e}")

# Menú interactivo
def menu():
    while True:
        print("\nMenú de Gestión de Médicos")
        print("1. Listar todos los médicos")
        print("2. Buscar médico por ID")
        print("3. Agregar nuevo médico")
        print("4. Actualizar médico existente")
        print("5. Eliminar médico")
        print("6. Salir")
        opcion = input("Seleccione una opción: ")

        if opcion == '1':
            obtener_medicos()
        elif opcion == '2':
            try:
                id_medico = int(input("Ingrese el ID del médico: "))
                obtener_medico_por_id(id_medico)
            except ValueError:
                print("ID inválido. Por favor, ingrese un número entero.")
        elif opcion == '3':
            nombre = input("Ingrese el nombre del médico: ").strip()
            especialidad = input("Ingrese la especialidad del médico: ").strip()
            telefono = input("Ingrese el teléfono del médico: ").strip()
            if nombre and especialidad and telefono:
                agregar_medico(nombre, especialidad, telefono)
            else:
                print("Todos los campos son obligatorios.")
        elif opcion == '4':
            try:
                id_medico = int(input("Ingrese el ID del médico: "))
                nombre = input("Ingrese el nuevo nombre del médico: ").strip()
                especialidad = input("Ingrese la nueva especialidad del médico: ").strip()
                telefono = input("Ingrese el nuevo teléfono del médico: ").strip()
                if nombre and especialidad and telefono:
                    actualizar_medico(id_medico, nombre, especialidad, telefono)
                else:
                    print("Todos los campos son obligatorios.")
            except ValueError:
                print("ID inválido. Por favor, ingrese un número entero.")
        elif opcion == '5':
            try:
                id_medico = int(input("Ingrese el ID del médico a eliminar: "))
                eliminar_medico(id_medico)
            except ValueError:
                print("ID inválido. Por favor, ingrese un número entero.")
        elif opcion == '6':
            print("Saliendo del programa...")
            break
        else:
            print("Opción no válida. Intente nuevamente.")

# Iniciar el menú interactivo
if __name__ == "__main__":
    menu()
