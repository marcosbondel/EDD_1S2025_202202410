# Manual Técnico - Gestión de Repuestos

## Índice
1. [Introducción](#introduccion)
2. [Requisitos del Sistema](#requisitos-del-sistema)
3. [Instalación](#instalacion)
4. [Uso de la Aplicación](#uso-de-la-aplicacion)
   - [Interfaz Gráfica](#interfaz-grafica)
   - [Gestión de Repuestos](#gestion-de-repuestos)
5. [Detalles Técnicos](#detalles-tecnicos)
   - [Estructura del Código](#estructura-del-codigo)
   - [Modelo de Datos](#modelo-de-datos)
   - [Funciones Clave](#funciones-clave)
6. [Consideraciones de Seguridad](#consideraciones-de-seguridad)
7. [Mantenimiento y Futuras Mejoras](#mantenimiento-y-futuras-mejoras)

## Introducción
Esta aplicación permite gestionar repuestos mediante una interfaz gráfica desarrollada en C# con GTK.
Los usuarios pueden agregar, editar y eliminar repuestos, así como realizar cargas masivas de datos.

## Requisitos del Sistema
- **Sistema Operativo:** Windows 10/11 o Linux
- **Entorno de Desarrollo:** .NET 6 o superior
- **Librerías requeridas:**
  - GTK#
  - MSDialog (para diálogos de mensajes)

## Instalación
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/usuario/proyecto-repuestos.git
   ```
2. Acceder a la carpeta del proyecto:
   ```bash
   cd proyecto-repuestos
   ```
3. Compilar el proyecto:
   ```bash
   dotnet build
   ```
4. Ejecutar la aplicación:
   ```bash
   dotnet run
   ```

## Uso de la Aplicación
### Interfaz Gráfica
La aplicación presenta una ventana principal con las siguientes opciones:
- **Agregar Repuesto:** Permite registrar un nuevo repuesto ingresando nombre, detalles y costo.
- **Editar Repuesto:** Permite modificar datos de un repuesto existente.
- **Eliminar Repuesto:** Permite borrar un repuesto por su ID.
- **Carga Masiva:** Importa repuestos desde un archivo externo.

### Gestión de Repuestos
- **Agregar:** Completar los campos y presionar "Guardar".
- **Editar:** Ingresar el ID, modificar los campos y presionar "Guardar".
- **Eliminar:** Ingresar el ID y presionar "Eliminar".

## Detalles Técnicos
### Estructura del Código
- `View/SparePartsView.cs` → Interfaz gráfica con GTK
- `Model/SparePart.cs` → Definición de la estructura `SparePart`
- `Storage/AppData.cs` → Manejo de almacenamiento de datos
- `ADT/SimpleNode.cs` → Implementación de nodos para listas enlazadas

### Modelo de Datos
```csharp
public unsafe struct SparePart {
    public int Id;
    public fixed char SparePartName[50];
    public fixed char Details[200];
    public double Cost;
}
```

### Funciones Clave
- `SetFixedString(char* destination, string source, int maxLength)` → Almacena cadenas en estructuras `fixed char`.
- `GetFixedString(char* source, int maxLength)` → Convierte `fixed char` a `string`.
- `OnSaveClicked(object sender, EventArgs e)` → Maneja la creación y edición de repuestos.
- `OnDeleteClicked(object sender, EventArgs e)` → Elimina un repuesto por su ID.

## Consideraciones de Seguridad
- Validar entradas para evitar valores nulos o incorrectos.
- Manejar excepciones en operaciones de almacenamiento.
- Controlar acceso a la base de datos para evitar inyecciones SQL.


