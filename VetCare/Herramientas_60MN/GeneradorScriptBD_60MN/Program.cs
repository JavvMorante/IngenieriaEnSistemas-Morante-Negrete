using System.Globalization;
using System.Text;
using System.Xml.Linq;
using Seguridad_60MN.Criptografia;
using Seguridad_60MN.Emergencia;
using Seguridad_60MN.Integridad;

// ---------------------------------------------------------------------------
// Ubicación de la solución y claves de seguridad (se leen del App.config de VetCare)
// ---------------------------------------------------------------------------
string raizSolucion = BuscarRaizSolucion();
string appConfig = Path.Combine(raizSolucion, "VetCare", "App.config");
foreach (XElement setting in XDocument.Load(appConfig).Descendants("add"))
{
    string clave = (string?)setting.Attribute("key") ?? "";
    if (clave.StartsWith("VETCARE_CLAVE_"))
        Environment.SetEnvironmentVariable(clave, (string?)setting.Attribute("value"));
}

GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
StringBuilder sql = new StringBuilder();
sql.Append(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Estructura.sql"), Encoding.UTF8));

sql.AppendLine();
sql.AppendLine("/* ---------------------------------------------------------------------");
sql.AppendLine("   DATOS INICIALES (generados: hashes, AES y dígitos verificadores)");
sql.AppendLine("   --------------------------------------------------------------------- */");
sql.AppendLine();

// ---------------------------------------------------------------------------
// Modelo de permisos USUARIO - FAMILIA - PATENTE
// ---------------------------------------------------------------------------
var patentes = new List<(int Id, string Nombre, string Codigo)>
{
    // Seguridad (CUS01 a CUS13)
    (1, "Crear usuario", "SEG_USU_ALTA"),
    (2, "Modificar usuario", "SEG_USU_MODIF"),
    (3, "Dar de baja / reactivar usuario", "SEG_USU_BAJA"),
    (4, "Gestionar familias y permisos", "SEG_ROLES"),
    (5, "Desbloquear usuario", "SEG_USU_DESBLOQ"),
    (6, "Consultar bitácora de eventos", "SEG_BITACORA_EVENTOS"),
    (7, "Consultar bitácora de cambios", "SEG_BITACORA_CAMBIOS"),
    (8, "Verificar integridad (dígitos verificadores)", "SEG_INTEGRIDAD"),
    (9, "Respaldar base de datos (Backup)", "SEG_BACKUP"),
    (10, "Restaurar base de datos (Restore)", "SEG_RESTORE"),
    // Gestión de Pacientes (CUN01 a CUN10)
    (11, "Registrar cliente", "PAC_CLIENTE_ALTA"),
    (12, "Modificar cliente", "PAC_CLIENTE_MODIF"),
    (13, "Consultar cliente", "PAC_CLIENTE_CONSULTA"),
    (14, "Gestionar mascota", "PAC_MASCOTA"),
    (15, "Crear historia clínica", "HC_ALTA"),
    (16, "Consultar historia clínica", "HC_CONSULTA"),
    (17, "Actualizar historia clínica", "HC_MODIF"),
    (18, "Registrar atención", "ATN_REGISTRAR"),
    (19, "Registrar medicación en carrito", "VEN_CARRITO"),
    (20, "Emitir informe de atención", "ATN_INFORME"),
    // Gestión de Stock (CUN11 a CUN15)
    (21, "Registrar producto", "STK_PROD_ALTA"),
    (22, "Modificar producto", "STK_PROD_MODIF"),
    (23, "Configurar margen y alerta de stock mínimo", "STK_CONFIG"),
    (24, "Registrar movimiento de stock", "STK_MOVIMIENTO"),
    (25, "Generar reporte de existencias", "STK_REPORTE"),
    // Opciones del menú fuera del alcance de esta versión ("TD")
    (26, "Proveedores", "MAE_PROVEEDORES"),
    (27, "Facturar", "VEN_FACTURAR"),
    (28, "Despachar", "VEN_DESPACHAR"),
    (29, "Compras", "COM_COMPRAS"),
    (30, "Reportes 2 y 3", "REP_REPORTES"),
    // Bitácora de eventos: exportar e importar en XML (patentes separadas: importar es más delicado)
    (31, "Exportar bitácora de eventos (XML)", "SEG_BITACORA_EXPORT"),
    (32, "Importar bitácora de eventos (XML)", "SEG_BITACORA_IMPORT"),
};
// Familias según G04: un tipo de usuario por rol del negocio, más dos familias de seguridad.
var familias = new List<(int Id, string Nombre)>
{
    (1, "Administrador"),
    (2, "Gestión de Usuarios"),
    (3, "Auditoría"),
    (4, "Operario de stock"),
    (5, "Vendedor"),
    (6, "Veterinario"),
};
var familiaPatente = new List<(int A, int B)>
{
    // Administrador (Gerente): perfiles, backup/restore, CUN13 y opciones fuera de alcance
    (1, 4), (1, 9), (1, 10), (1, 23), (1, 26), (1, 27), (1, 28), (1, 29), (1, 30),
    (1, 32),                                        // Administrador: importar bitácora (XML)
    (2, 1), (2, 2), (2, 3), (2, 5),                 // Gestión de Usuarios
    (3, 6), (3, 7), (3, 8), (3, 31),                // Auditoría (incluye exportar bitácora a XML)
    (4, 21), (4, 22), (4, 24), (4, 25),             // Operario de stock: CUN11, CUN12, CUN14, CUN15
    (5, 11), (5, 12), (5, 13), (5, 14), (5, 19),    // Vendedor: CUN01 a CUN04 y CUN09
    (6, 13), (6, 15), (6, 16), (6, 17), (6, 18), (6, 19), (6, 20), // Veterinario: CUN03, CUN05 a CUN10
};
var familiaFamilia = new List<(int A, int B)>
{
    (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), // Administrador = acceso total (G04): incluye todas las familias
};
var usuarioFamilia = new List<(int A, int B)> { (1, 1), (2, 4), (3, 5), (4, 6) };
var usuarioPatente = new List<(int A, int B)> { (2, 23) }; // operario: patente individual CUN13 (Configurar margen)

List<string> dvhPatente = new List<string>();
sql.AppendLine("SET IDENTITY_INSERT Patente ON;");
foreach (var p in patentes)
{
    string dvh = dv.CalcularDVH(TablaDV_60MN.Patente, p.Id, p.Nombre, p.Codigo);
    dvhPatente.Add(dvh);
    sql.AppendLine($"INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES ({p.Id}, {Txt(p.Nombre)}, {Txt(p.Codigo)}, '{dvh}');");
}
sql.AppendLine("SET IDENTITY_INSERT Patente OFF;");
sql.AppendLine();

List<string> dvhFamilia = new List<string>();
sql.AppendLine("SET IDENTITY_INSERT Familia ON;");
foreach (var f in familias)
{
    string dvh = dv.CalcularDVH(TablaDV_60MN.Familia, f.Id, f.Nombre);
    dvhFamilia.Add(dvh);
    sql.AppendLine($"INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES ({f.Id}, {Txt(f.Nombre)}, '{dvh}');");
}
sql.AppendLine("SET IDENTITY_INSERT Familia OFF;");
sql.AppendLine();

List<string> dvhFamiliaPatente = Relacion(TablaDV_60MN.FamiliaPatente, "FamiliaID", "PatenteID", familiaPatente);
List<string> dvhFamiliaFamilia = Relacion(TablaDV_60MN.FamiliaFamilia, "FamiliaPadreID", "FamiliaHijaID", familiaFamilia);
sql.AppendLine("GO");
sql.AppendLine();

// ---------------------------------------------------------------------------
// Usuarios: uno por perfil del negocio (personal de G04)
// ---------------------------------------------------------------------------
var usuarios = new[]
{
    (Id: 1, Usuario: "admin", Clave: "Admin1234", Nombre: "Mariano", Apellido: "Núñez", Dni: "30111222", Email: "mnunez@vetcare.local"),
    (Id: 2, Usuario: "operario", Clave: "Operario1234", Nombre: "Sofía", Apellido: "Ramírez", Dni: "32444555", Email: "sramirez@vetcare.local"),
    (Id: 3, Usuario: "vendedor", Clave: "Vendedor1234", Nombre: "Lucas", Apellido: "González", Dni: "35000111", Email: "lgonzalez@vetcare.local"),
    (Id: 4, Usuario: "veterinario", Clave: "Veterinario1234", Nombre: "Carla", Apellido: "Domínguez", Dni: "33222111", Email: "cdominguez@vetcare.local"),
};
List<string> dvhUsuario = new List<string>();
sql.AppendLine("SET IDENTITY_INSERT Usuario ON;");
foreach (var u in usuarios)
{
    string hash = PasswordHasher_60MN.Hashear(u.Clave);
    string dni = CriptografiaHandler_60MN.Encriptar(u.Dni);
    string email = CriptografiaHandler_60MN.Encriptar(u.Email);
    // Orden de columnas = TablaDV_60MN.Usuario
    string dvh = dv.CalcularDVH(TablaDV_60MN.Usuario, u.Id, u.Usuario, hash, u.Nombre, u.Apellido, dni, email,
        true, false, 0, false, false, (int?)null); // IdiomaID NULL: el usuario todavía no eligió idioma (español)
    dvhUsuario.Add(dvh);
    sql.AppendLine("INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)");
    sql.AppendLine($"     VALUES ({u.Id}, {Txt(u.Usuario)}, {Txt(hash)}, {Txt(u.Nombre)}, {Txt(u.Apellido)}, {Txt(dni)}, {Txt(email)}, 1, 0, 0, 0, 0, '{dvh}');");
}
sql.AppendLine("SET IDENTITY_INSERT Usuario OFF;");
sql.AppendLine();
List<string> dvhUsuarioFamilia = Relacion(TablaDV_60MN.UsuarioFamilia, "UsuarioID", "FamiliaID", usuarioFamilia);
List<string> dvhUsuarioPatente = Relacion(TablaDV_60MN.UsuarioPatente, "UsuarioID", "PatenteID", usuarioPatente);
sql.AppendLine("GO");
sql.AppendLine();

// Inserta una tabla de relación (clave compuesta de dos columnas) con su DVH, en el orden de la clave.
List<string> Relacion(string tabla, string columnaA, string columnaB, List<(int A, int B)> filas)
{
    List<string> dvhs = new List<string>();
    foreach (var (a, b) in filas.OrderBy(f => f.A).ThenBy(f => f.B))
    {
        string dvh = dv.CalcularDVH(tabla, a, b);
        dvhs.Add(dvh);
        sql.AppendLine($"INSERT INTO {tabla} ({columnaA}, {columnaB}, DVH) VALUES ({a}, {b}, '{dvh}');");
    }
    return dvhs;
}

// ---------------------------------------------------------------------------
// Maestros de stock
// ---------------------------------------------------------------------------
var categorias = new (int Id, string Nombre, decimal? Margen)[]
{
    (1, "Medicamentos", null), (2, "Vacunas", 35m), (3, "Antiparasitarios", null), (4, "Alimentos", 20m), (5, "Accesorios", 40m)
};
sql.AppendLine("SET IDENTITY_INSERT Categoria ON;");
foreach (var c in categorias)
    sql.AppendLine($"INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES ({c.Id}, {Txt(c.Nombre)}, {(c.Margen.HasValue ? Dec(c.Margen.Value) : "NULL")});");
sql.AppendLine("SET IDENTITY_INSERT Categoria OFF;");
sql.AppendLine();
sql.AppendLine("SET IDENTITY_INSERT Proveedor ON;");
sql.AppendLine("INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT) VALUES (1, N'Droguería Veterinaria del Sur S.A.', '30-71234567-1');");
sql.AppendLine("INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT) VALUES (2, N'Distribuidora Pet Health S.R.L.', '30-70987654-3');");
sql.AppendLine("INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT) VALUES (3, N'Laboratorios BioVet S.A.', '30-69876543-9');");
sql.AppendLine("SET IDENTITY_INSERT Proveedor OFF;");
sql.AppendLine();
const decimal margenGeneral = 30m;
sql.AppendLine($"INSERT INTO Configuracion (Clave, Valor) VALUES ('MargenGeneral', '{Dec(margenGeneral)}');");
sql.AppendLine("INSERT INTO Configuracion (Clave, Valor) VALUES ('StockMinimoSugerido', '5');");
sql.AppendLine("GO");
sql.AppendLine();

var productos = new[]
{
    (Id: 1, Nombre: "Amoxicilina 250 mg x 10 comp.", Cat: 1, Prov: 1, Stock: 40, Min: 10, Costo: 1800m, Vto: new DateTime(2027, 6, 30), Lote: "AMX-2401"),
    (Id: 2, Nombre: "Vacuna Séxtuple canina", Cat: 2, Prov: 3, Stock: 25, Min: 8, Costo: 5200m, Vto: new DateTime(2027, 3, 31), Lote: "VSX-118"),
    (Id: 3, Nombre: "Pipeta antipulgas perro 10-20 kg", Cat: 3, Prov: 2, Stock: 6, Min: 10, Costo: 3100m, Vto: new DateTime(2028, 1, 31), Lote: "PIP-7755"),
    (Id: 4, Nombre: "Alimento balanceado gato adulto 3 kg", Cat: 4, Prov: 2, Stock: 15, Min: 5, Costo: 9800m, Vto: new DateTime(2027, 11, 30), Lote: "ALG-3021"),
    (Id: 5, Nombre: "Collar isabelino talle M", Cat: 5, Prov: 2, Stock: 12, Min: 4, Costo: 2500m, Vto: new DateTime(2030, 12, 31), Lote: "COL-M-01"),
};
List<string> dvhProductos = new List<string>();
sql.AppendLine("-- Productos usados por el carrito de medicación (CUN09). El trigger de la bitácora de cambios está deshabilitado por ahora.");
sql.AppendLine("SET IDENTITY_INSERT Productos ON;");
foreach (var p in productos)
{
    decimal margen = categorias.First(c => c.Id == p.Cat).Margen ?? margenGeneral;
    decimal venta = Math.Round(p.Costo * (1 + margen / 100m), 2);
    string codigo = "PRD-" + p.Id.ToString("00000");
    string dvh = dv.CalcularDVH(TablaDV_60MN.Productos, p.Id, codigo, p.Nombre, p.Cat, p.Prov, p.Stock, p.Min, p.Costo, venta, p.Vto, p.Lote, true);
    dvhProductos.Add(dvh);
    sql.AppendLine("INSERT INTO Productos (ProductoID, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo, PrecioCosto, PrecioVenta, FechaVencimiento, Lote, Activo, UsuarioModificacion, DVH)");
    sql.AppendLine($"     VALUES ({p.Id}, {Txt(p.Nombre)}, {p.Cat}, {p.Prov}, {p.Stock}, {p.Min}, {Dec(p.Costo)}, {Dec(venta)}, '{p.Vto:yyyy-MM-dd}', {Txt(p.Lote)}, 1, 1, '{dvh}');");
}
sql.AppendLine("SET IDENTITY_INSERT Productos OFF;");
sql.AppendLine();
sql.AppendLine("-- La pipeta arranca por debajo del stock mínimo: alerta de reposición activa.");
sql.AppendLine("INSERT INTO AlertaReposicion (ProductoID, Activa) VALUES (3, 1);");
sql.AppendLine("GO");
sql.AppendLine();

// ---------------------------------------------------------------------------
// Gestión de Pacientes: clientes, mascotas y una historia clínica de ejemplo
// ---------------------------------------------------------------------------
var clientes = new[]
{
    (Id: 1, Tipo: "DNI", Nro: "28555111", Nombre: "Laura", Apellido: "Pereyra", Direccion: "Av. Hipólito Yrigoyen 8900, Lomas de Zamora", Telefono: "11 4244-1020", Email: (string?)"lpereyra@mail.com"),
    (Id: 2, Tipo: "DNI", Nro: "31777222", Nombre: "Diego", Apellido: "Acosta", Direccion: "Laprida 450, Lomas de Zamora", Telefono: "11 5566-7788", Email: (string?)null),
};
List<string> dvhCliente = new List<string>();
sql.AppendLine("SET IDENTITY_INSERT Cliente ON;");
foreach (var c in clientes)
{
    string dvh = dv.CalcularDVH(TablaDV_60MN.Cliente, c.Id, c.Tipo, c.Nro, c.Nombre, c.Apellido, c.Direccion, c.Telefono, c.Email!, true);
    dvhCliente.Add(dvh);
    sql.AppendLine("INSERT INTO Cliente (ClienteID, TipoDocumento, NroDocumento, Nombre, Apellido, Direccion, Telefono, Email, Activo, DVH)");
    sql.AppendLine($"     VALUES ({c.Id}, {Txt(c.Tipo)}, {Txt(c.Nro)}, {Txt(c.Nombre)}, {Txt(c.Apellido)}, {Txt(c.Direccion)}, {Txt(c.Telefono)}, {Txt(c.Email)}, 1, '{dvh}');");
}
sql.AppendLine("SET IDENTITY_INSERT Cliente OFF;");
sql.AppendLine();

var mascotas = new[]
{
    (Id: 1, Cliente: 1, Nombre: "Toby", Especie: "Perro", Raza: "Caniche", Sexo: "M", Nacimiento: new DateTime(2019, 4, 12)),
    (Id: 2, Cliente: 1, Nombre: "Mora", Especie: "Gato", Raza: "Europeo común", Sexo: "H", Nacimiento: new DateTime(2021, 9, 3)),
    (Id: 3, Cliente: 2, Nombre: "Rocco", Especie: "Perro", Raza: "Labrador", Sexo: "M", Nacimiento: new DateTime(2022, 1, 20)),
};
List<string> dvhMascota = new List<string>();
sql.AppendLine("SET IDENTITY_INSERT Mascota ON;");
foreach (var m in mascotas)
{
    string dvh = dv.CalcularDVH(TablaDV_60MN.Mascota, m.Id, m.Cliente, m.Nombre, m.Especie, m.Raza, m.Sexo, m.Nacimiento, true);
    dvhMascota.Add(dvh);
    sql.AppendLine("INSERT INTO Mascota (MascotaID, ClienteID, Nombre, Especie, Raza, Sexo, FechaNacimiento, Activo, DVH)");
    sql.AppendLine($"     VALUES ({m.Id}, {m.Cliente}, {Txt(m.Nombre)}, {Txt(m.Especie)}, {Txt(m.Raza)}, '{m.Sexo}', '{m.Nacimiento:yyyy-MM-dd}', 1, '{dvh}');");
}
sql.AppendLine("SET IDENTITY_INSERT Mascota OFF;");
sql.AppendLine();

DateTime aperturaHc = new DateTime(2026, 9, 1, 10, 30, 0);
string dvhHc = dv.CalcularDVH(TablaDV_60MN.HistoriaClinica, 1, "HC-00001", 1, aperturaHc, 7.4m, "Sin alergias registradas", null!);
sql.AppendLine("SET IDENTITY_INSERT HistoriaClinica ON;");
sql.AppendLine("INSERT INTO HistoriaClinica (HistoriaClinicaID, MascotaID, FechaApertura, PesoActual, Alergias, Observaciones, UsuarioID, DVH)");
sql.AppendLine($"     VALUES (1, 1, '{aperturaHc:yyyy-MM-ddTHH:mm:ss}', 7.40, N'Sin alergias registradas', NULL, 4, '{dvhHc}');");
sql.AppendLine("SET IDENTITY_INSERT HistoriaClinica OFF;");
sql.AppendLine("GO");
sql.AppendLine();

// ---------------------------------------------------------------------------
// Dígitos verificadores verticales
// ---------------------------------------------------------------------------
var dvvs = new List<(string Tabla, string Dvv)>
{
    (TablaDV_60MN.Usuario, dv.CalcularDVV(TablaDV_60MN.Usuario, dvhUsuario)),
    (TablaDV_60MN.Patente, dv.CalcularDVV(TablaDV_60MN.Patente, dvhPatente)),
    (TablaDV_60MN.Familia, dv.CalcularDVV(TablaDV_60MN.Familia, dvhFamilia)),
    (TablaDV_60MN.FamiliaPatente, dv.CalcularDVV(TablaDV_60MN.FamiliaPatente, dvhFamiliaPatente)),
    (TablaDV_60MN.FamiliaFamilia, dv.CalcularDVV(TablaDV_60MN.FamiliaFamilia, dvhFamiliaFamilia)),
    (TablaDV_60MN.UsuarioFamilia, dv.CalcularDVV(TablaDV_60MN.UsuarioFamilia, dvhUsuarioFamilia)),
    (TablaDV_60MN.UsuarioPatente, dv.CalcularDVV(TablaDV_60MN.UsuarioPatente, dvhUsuarioPatente)),
    (TablaDV_60MN.Bitacora, dv.CalcularDVV(TablaDV_60MN.Bitacora, Array.Empty<string>())),
    (TablaDV_60MN.Productos, dv.CalcularDVV(TablaDV_60MN.Productos, dvhProductos)),
    (TablaDV_60MN.MovimientoStock, dv.CalcularDVV(TablaDV_60MN.MovimientoStock, Array.Empty<string>())),
    (TablaDV_60MN.Cliente, dv.CalcularDVV(TablaDV_60MN.Cliente, dvhCliente)),
    (TablaDV_60MN.Mascota, dv.CalcularDVV(TablaDV_60MN.Mascota, dvhMascota)),
    (TablaDV_60MN.HistoriaClinica, dv.CalcularDVV(TablaDV_60MN.HistoriaClinica, new[] { dvhHc })),
    (TablaDV_60MN.Atencion, dv.CalcularDVV(TablaDV_60MN.Atencion, Array.Empty<string>())),
    (TablaDV_60MN.AtencionDetalle, dv.CalcularDVV(TablaDV_60MN.AtencionDetalle, Array.Empty<string>())),
};
foreach (var (tabla, dvv) in dvvs)
    sql.AppendLine($"INSERT INTO DVV (Tabla, DVV) VALUES ('{tabla}', '{dvv}');");
sql.AppendLine("GO");
sql.AppendLine();
sql.AppendLine("PRINT 'VetCareBD_60MN creada correctamente.';");
sql.AppendLine("GO");

// ---------------------------------------------------------------------------
// Salidas
// ---------------------------------------------------------------------------
string rutaScript = Path.Combine(Directory.GetParent(raizSolucion)!.FullName, "BDScript", "VetCareBD_60MN.sql");
Directory.CreateDirectory(Path.GetDirectoryName(rutaScript)!);
File.WriteAllText(rutaScript, sql.ToString(), new UTF8Encoding(true));
Console.WriteLine("Script generado: " + rutaScript);

string rutaXml = Path.Combine(raizSolucion, "VetCare", UsuarioEmergencia_60MN.NombreArchivo);
UsuarioEmergencia_60MN.Generar(rutaXml, "emergencia", "Usuario", "Emergencia", "Emergencia1234", dv);
UsuarioEmergencia_60MN.Cargar(rutaXml); // valida que la firma recién generada sea correcta
Console.WriteLine("Usuario de emergencia generado: " + rutaXml);

// ---------------------------------------------------------------------------
static string Txt(string? valor) => valor == null ? "NULL" : "N'" + valor.Replace("'", "''") + "'";
static string Bit(bool valor) => valor ? "1" : "0";
static string Dec(decimal valor) => valor.ToString("0.00", CultureInfo.InvariantCulture);

static string BuscarRaizSolucion()
{
    DirectoryInfo? dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir != null && !File.Exists(Path.Combine(dir.FullName, "VetCare.sln")))
        dir = dir.Parent;
    return dir?.FullName ?? throw new DirectoryNotFoundException("No se encontró VetCare.sln (ejecutar dentro de la carpeta de la solución).");
}
