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
    (1, "Crear usuario", "SEG_USU_ALTA"),
    (2, "Modificar usuario", "SEG_USU_MODIF"),
    (3, "Dar de baja / reactivar usuario", "SEG_USU_BAJA"),
    (4, "Gestionar familias y permisos", "SEG_ROLES"),
    (5, "Desbloquear usuario", "SEG_USU_DESBLOQ"),
    (6, "Consultar bitácora de eventos", "SEG_BITACORA_EVENTOS"),
    (7, "Consultar bitácora de cambios", "SEG_BITACORA_CAMBIOS"),
    (8, "Verificar integridad (dígitos verificadores)", "SEG_INTEGRIDAD"),
    (9, "Registrar producto", "STK_PROD_ALTA"),
    (10, "Modificar producto", "STK_PROD_MODIF"),
    (11, "Registrar movimiento de stock", "STK_MOVIMIENTO"),
    // Opciones del menú aún no implementadas ("TD"): patente propia para controlar su visibilidad.
    (12, "Backup", "SEG_BACKUP"),
    (13, "Restore", "SEG_RESTORE"),
    (14, "Clientes", "MAE_CLIENTES"),
    (15, "Proveedores", "MAE_PROVEEDORES"),
    (16, "Carrito", "VEN_CARRITO"),
    (17, "Facturar", "VEN_FACTURAR"),
    (18, "Despachar", "VEN_DESPACHAR"),
    (19, "Compras", "COM_COMPRAS"),
    (20, "Reportes", "REP_REPORTES"),
};
var familias = new List<(int Id, string Nombre)>
{
    (1, "Administrador"),
    (2, "Gestión de Usuarios"),
    (3, "Auditoría"),
    (4, "Operario de stock"),
    (5, "Ventas"),
    (6, "Vendedor"),
    (7, "Veterinario"),
};
var familiaPatente = new List<(int A, int B)>
{
    (1, 4), (1, 12), (1, 13), (1, 14), (1, 15), (1, 19), (1, 20), // Administrador: patentes propias
    (2, 1), (2, 2), (2, 3), (2, 5),                                // Gestión de Usuarios
    (3, 6), (3, 7), (3, 8),                                        // Auditoría
    (4, 9), (4, 10), (4, 11),                                      // Operario de stock
    (5, 16), (5, 17), (5, 18),                                     // Ventas
    (6, 14),                                                       // Vendedor: Clientes (+ familia Ventas)
    (7, 14), (7, 20),                                              // Veterinario: Clientes y Reportes
};
var familiaFamilia = new List<(int A, int B)>
{
    (1, 2), (1, 3), (1, 4), (1, 5), // Administrador = acceso total (G04): incluye todas las familias
    (6, 5),                         // Vendedor incluye la familia Ventas
};
var usuarioFamilia = new List<(int A, int B)> { (1, 1), (2, 4) };  // admin → Administrador; operario → Operario de stock
var usuarioPatente = new List<(int A, int B)> { (2, 20) };          // operario: patente individual Reportes

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
// Usuarios: administrador y operario de stock (personal de G04)
// ---------------------------------------------------------------------------
var usuarios = new[]
{
    (Id: 1, Usuario: "admin", Clave: "Admin1234", Nombre: "Mariano", Apellido: "Núñez", Dni: "30111222", Email: "mnunez@vetcare.local"),
    (Id: 2, Usuario: "operario", Clave: "Operario1234", Nombre: "Sofía", Apellido: "Ramírez", Dni: "32444555", Email: "sramirez@vetcare.local"),
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
        true, false, 0, false, false);
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
sql.AppendLine("-- El trigger TR_Productos_BitacoraCambios registra el ALTA de cada producto en Productos_C.");
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
