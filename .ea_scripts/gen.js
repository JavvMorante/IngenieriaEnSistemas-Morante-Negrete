// ===== Generador de diagramas VetCare IS V3 (Enterprise Architect, JScript) =====
var OUTDIR = "C:\\Users\\jmora\\Desktop\\RepoFacultad\\IngenieriaEnSistemas-Morante-Negrete\\Diagramas-EA-V3\\";
var ROOTNAME = "Ingeniería de Software V3 - VetCare";
var LOG = [];
function log(s){ LOG.push(s); Session.Output(s); }

function delPkg(parent, name){
  for (var i = parent.Packages.Count - 1; i >= 0; i--) {
    if (parent.Packages.GetAt(i).Name == name) { parent.Packages.DeleteAt(i, true); }
  }
  parent.Packages.Refresh();
}
function newPkg(parent, name){
  var p = parent.Packages.AddNew(name, ""); p.Update(); parent.Packages.Refresh(); return p;
}
function newEl(pkg, name, type, stereo){
  var e = pkg.Elements.AddNew(name, type);
  if (stereo) e.Stereotype = stereo;
  e.Update(); pkg.Elements.Refresh(); return e;
}
function newDiag(pkg, name, type){
  var d = pkg.Diagrams.AddNew(name, type); d.Update(); pkg.Diagrams.Refresh(); return d;
}
function place(d, e, l, t, w, h, seq){
  var o = d.DiagramObjects.AddNew("l=" + Math.round(l) + ";r=" + Math.round(l + w) + ";t=" + Math.round(-t) + ";b=" + Math.round(-(t + h)) + ";", "");
  o.ElementID = e.ElementID;
  if (seq) o.Sequence = seq;
  o.Update(); return o;
}
function conn(a, b, type, name, stereo){
  var c = a.Connectors.AddNew(name || "", type);
  c.SupplierID = b.ElementID;
  if (stereo) c.Stereotype = stereo;
  c.Update(); a.Connectors.Refresh(); return c;
}
function strw(s){ return s.length * 6.4; }
function maxw(lines, min){ var m = min; for (var i = 0; i < lines.length; i++){ var w = strw(lines[i]) + 40; if (w > m) m = w; } return m; }

// ---------- Casos de uso ----------
// spec: {name, actor, main, ext:[[label,'include'|'extend']]}
function buildUC(pkg, cu){
  var d = newDiag(pkg, cu.code + "_60MN - " + cu.title, "Use Case");
  var actor = newEl(pkg, cu.actor, "Actor");
  var sys = newEl(pkg, "Sistema VetCare", "Boundary");
  var main = newEl(pkg, cu.main, "UseCase");
  var n = cu.ext.length;
  var ucW = 190, ucH = 72, gap = 34;
  var bx = 230, by = 50, bw = ucW + 80, bh = 60 + (n + 1) * (ucH + gap);
  place(d, sys, bx, by, bw, bh, 50);
  var mainTop = by + 45;
  place(d, main, bx + 40, mainTop, ucW, ucH, 10);
  place(d, actor, 70, mainTop - 8, 50, 90, 11);
  conn(actor, main, "UseCase", "", "use");
  for (var i = 0; i < n; i++){
    var x = newEl(pkg, cu.ext[i][0], "UseCase");
    place(d, x, bx + 40, mainTop + (i + 1) * (ucH + gap), ucW, ucH, 9 - i);
    if (cu.ext[i][1] == "include") conn(main, x, "UseCase", "", "include");
    else conn(x, main, "UseCase", "", "extend");
  }
  d.Update();
  return d;
}

// ---------- Secuencia ----------
// spec: {lifelines:[[name, kind]], msgs:[[from,to,text,isReturn]], frags:[[kind,label,fromIdx,toIdx]]}
function buildSD(pkg, cu, sd){
  var d = newDiag(pkg, cu.code + "_60MN - " + cu.title, "Sequence");
  var ll = {}, x = 30, ord = [];
  var top = 45;
  for (var i = 0; i < sd.lifelines.length; i++){
    var nm = sd.lifelines[i][0], kind = sd.lifelines[i][1], e;
    if (kind == "actor") e = newEl(pkg, nm, "Actor");
    else e = newEl(pkg, nm, "Sequence", kind == "boundary" ? "boundary" : (kind == "control" ? "control" : ""));
    var w = (kind == "actor") ? 90 : Math.max(110, strw(nm) + 24);
    ll[nm] = {e: e, x: x, w: w, cx: x + w / 2};
    ord.push(nm);
    x += w + 45;
  }
  // posiciones verticales de los mensajes
  var y = 150, ys = [], fragStart = {}, fragEnd = {};
  for (var f = 0; f < sd.frags.length; f++){ fragStart[sd.frags[f][2]] = true; fragEnd[sd.frags[f][3]] = true; }
  for (var m = 0; m < sd.msgs.length; m++){
    if (fragStart[m]) y += 30;
    ys.push(y);
    var self = (sd.msgs[m][0] == sd.msgs[m][1]);
    y += self ? 64 : 50;
    if (fragEnd[m]) y += 24;
  }
  var bottom = y + 40;
  var totalW = x;
  // fragmentos primero (quedan detrás)
  for (var f2 = 0; f2 < sd.frags.length; f2++){
    var fr = sd.frags[f2];
    var fe = newEl(pkg, fr[1], "InteractionFragment");
    fe.Subtype = (fr[0] == "loop") ? 4 : (fr[0] == "opt" ? 1 : 0);
    fe.Update();
    var y1 = ys[fr[2]] - 42, y2 = ys[fr[3]] + 30;
    var minx = 1e9, maxx = 0;
    for (var k = fr[2]; k <= fr[3]; k++){
      var a = ll[sd.msgs[k][0]], b = ll[sd.msgs[k][1]];
      minx = Math.min(minx, a.cx, b.cx); maxx = Math.max(maxx, a.cx, b.cx);
    }
    place(d, fe, Math.max(10, minx - 45), y1, (maxx - minx) + 110, y2 - y1, 100 + f2);
  }
  for (var j = 0; j < ord.length; j++){
    var L = ll[ord[j]];
    place(d, L.e, L.x, top, L.w, bottom - top, 50 - j);
  }
  d.Update();
  for (var m2 = 0; m2 < sd.msgs.length; m2++){
    var ms = sd.msgs[m2], A = ll[ms[0]], B = ll[ms[1]];
    var c = A.e.Connectors.AddNew(ms[2], "Sequence");
    c.SupplierID = B.e.ElementID;
    c.SequenceNo = m2 + 1;
    c.DiagramID = d.DiagramID;
    c.StartPointX = Math.round(A.cx); c.StartPointY = -ys[m2];
    c.EndPointX = Math.round(B.cx);
    c.EndPointY = (ms[0] == ms[1]) ? -(ys[m2] + 15) : -ys[m2];
    c.Update();
    Repository.Execute("UPDATE t_connector SET PDATA1='Synchronous', PDATA2='retval=void;', PDATA3='Call', PDATA4='" + (ms[3] ? "1" : "") + "', PtStartX=" + Math.round(A.cx) + ", PtStartY=" + (-ys[m2]) + ", PtEndX=" + Math.round(B.cx) + ", PtEndY=" + ((ms[0] == ms[1]) ? -(ys[m2] + 15) : -ys[m2]) + " WHERE Connector_ID=" + c.ConnectorID);
  }
  d.Update();
  return d;
}

// ---------- Clases (estilo componentes por capa) ----------
// spec: {rows:[[ [layerName, [classDef...]] , ...], ...], deps:[[from,to,stereo,type]]}
// classDef: {n, st, abs, atts:[vis,name,type,static], ops:[vis,name,params,ret,abstract,static]}
function visStr(v){ return v == "-" ? "Private" : (v == "#" ? "Protected" : "Public"); }
function classLines(c){
  var L = [c.n];
  var i;
  for (i = 0; i < (c.atts || []).length; i++) L.push(c.atts[i][0] + " " + c.atts[i][1] + " : " + c.atts[i][2]);
  for (i = 0; i < (c.ops || []).length; i++){
    var ps = c.ops[i][2] ? c.ops[i][2].split(",") : [], ts = [];
    for (var q = 0; q < ps.length; q++){ var ix = ps[q].indexOf(":"); ts.push(ix >= 0 ? ps[q].substring(ix + 1).replace(/^\s+|\s+$/g, "") : ps[q]); }
    L.push("+ " + c.ops[i][1] + "(" + ts.join(", ") + "): " + c.ops[i][3]);
  }
  return L;
}
function classSize(c){
  var na = (c.atts || []).length, no = (c.ops || []).length;
  var h = 34 + 15 * (na + no) + (na ? 10 : 0) + (no ? 10 : 0) + (c.st ? 14 : 0);
  var L = classLines(c), m = 150;
  for (var k = 0; k < L.length; k++){ var w = L[k].length * 6.0 + 45; if (w > m) m = w; }
  return {w: Math.min(360, m), h: Math.max(60, h)};
}
function mkClass(pkg, c){
  var type = c.iface ? "Interface" : "Class";
  var e = newEl(pkg, c.n, type, c.st || "");
  if (c.abs) { e.Abstract = "1"; e.Update(); }
  var i;
  for (i = 0; i < (c.atts || []).length; i++){
    var a = e.Attributes.AddNew(c.atts[i][1], c.atts[i][2]);
    a.Visibility = visStr(c.atts[i][0]); if (c.atts[i][3]) a.IsStatic = true; a.Pos = i; a.Update();
  }
  e.Attributes.Refresh();
  for (i = 0; i < (c.ops || []).length; i++){
    var o = c.ops[i];
    var m = e.Methods.AddNew(o[1], o[3]);
    m.Visibility = visStr(o[0]); if (o[4]) m.Abstract = true; if (o[5]) m.IsStatic = true; m.Pos = i;
    m.Update();
    if (o[2]){
      var ps = o[2].split(",");
      for (var p = 0; p < ps.length; p++){
        var s = ps[p].replace(/^\s+|\s+$/g, ""); if (!s) continue;
        var pn = s, pt = "";
        var ix = s.indexOf(":");
        if (ix >= 0){ pn = s.substring(0, ix).replace(/\s+$/,""); pt = s.substring(ix + 1).replace(/^\s+/,""); }
        var pr = m.Parameters.AddNew(pn, pt); pr.Position = p; pr.Update();
      }
      m.Parameters.Refresh();
    }
  }
  e.Methods.Refresh(); e.Update();
  return e;
}
function buildDC(pkg, cu, dc){
  var d = newDiag(pkg, "DC " + cu.code + "_60MN - " + cu.title, "Component");
  var reg = {}, y = 50, seq = 200;
  for (var r = 0; r < dc.rows.length; r++){
    var row = dc.rows[r], x = 30, rowH = 0;
    for (var l = 0; l < row.length; l++){
      var layer = row[l], classes = layer[1];
      var innerW = 0, innerH = 0, sizes = [];
      for (var k = 0; k < classes.length; k++){ var sz = classSize(classes[k]); sizes.push(sz); innerW += sz.w + 25; innerH = Math.max(innerH, sz.h); }
      var bw = Math.max(innerW + 15, strw(layer[0]) + 40), bh = innerH + 60;
      var be = newEl(pkg, layer[0], "Boundary");
      place(d, be, x, y, bw, bh, seq--);
      var cx = x + 20;
      for (var k2 = 0; k2 < classes.length; k2++){
        var ce = mkClass(pkg, classes[k2]);
        place(d, ce, cx, y + 34, sizes[k2].w, sizes[k2].h, seq--);
        reg[classes[k2].n] = ce;
        cx += sizes[k2].w + 25;
      }
      x += bw + 40; rowH = Math.max(rowH, bh);
    }
    y += rowH + 45;
  }
  for (var i = 0; i < dc.deps.length; i++){
    var dp = dc.deps[i];
    conn(reg[dp[0]], reg[dp[1]], dp[3] || "Dependency", dp[4] || "", dp[2] || "");
  }
  d.Update();
  return d;
}

// ---------- DER ----------
// tables: [{n, cols:[[name,type,pk,notnull,fk]], fks:[[col, parentTable, parentCol]]}]
function buildDER(pkg, cu, tables, cols){
  var d = newDiag(pkg, "DER " + cu.code + "_60MN - " + cu.title, "Logical");
  d.MetaType = "Extended::Data Modeling"; d.Update();
  var reg = {}, perRow = cols || 3, x0 = 40, y0 = 60, colW = 270, i, j;
  var rowH = [], heights = [];
  for (i = 0; i < tables.length; i++){
    var t = tables[i];
    var nf = (t.fks || []).length, nc = t.cols.length;
    var h = 60 + 17 * nc + 45 + (nf ? 22 + 17 * nf : 0);
    heights.push(h);
    var rr = Math.floor(i / perRow); rowH[rr] = Math.max(rowH[rr] || 0, h);
  }
  for (i = 0; i < tables.length; i++){
    var tb = tables[i];
    var e = newEl(pkg, tb.n, "Class", "table");
    e.Gentype = "SQL Server 2012"; e.Update();
    var pkcols = [], pktypes = [];
    for (j = 0; j < tb.cols.length; j++){
      var c = tb.cols[j];
      var a = e.Attributes.AddNew(c[0], c[1]);
      a.Stereotype = "column"; a.IsOrdered = !!c[2]; a.AllowDuplicates = !!c[3]; a.IsCollection = !!c[4]; a.Pos = j;
      a.Update();
      if (c[2]) { pkcols.push(c[0]); pktypes.push(c[1]); }
    }
    e.Attributes.Refresh();
    var m = e.Methods.AddNew("PK_" + tb.n, ""); m.Stereotype = "PK"; m.Visibility = "Public"; m.Update();
    for (j = 0; j < pkcols.length; j++){ var p = m.Parameters.AddNew(pkcols[j], pktypes[j]); p.Position = j; p.Update(); }
    for (j = 0; j < (tb.fks || []).length; j++){
      var fk = tb.fks[j];
      var fm = e.Methods.AddNew("FK_" + tb.n + "_" + fk[1] + (fk[3] ? fk[3] : ""), ""); fm.Stereotype = "FK"; fm.Visibility = "Public"; fm.Update();
      var fp = fm.Parameters.AddNew(fk[0], "INT"); fp.Update();
    }
    e.Methods.Refresh(); e.Update();
    var r = Math.floor(i / perRow), cc = i % perRow, yy = y0;
    for (var q = 0; q < r; q++) yy += rowH[q] + 70;
    var w = 0; for (j = 0; j < tb.cols.length; j++) w = Math.max(w, strw(tb.cols[j][0] + ": " + tb.cols[j][1]) + 70);
    w = Math.max(210, Math.min(300, w));
    place(d, e, x0 + cc * (colW + 70), yy, w, heights[i], 100 - i);
    reg[tb.n] = e;
  }
  for (i = 0; i < tables.length; i++){
    var tb2 = tables[i];
    for (j = 0; j < (tb2.fks || []).length; j++){
      var f = tb2.fks[j];
      var cn = conn(reg[tb2.n], reg[f[1]], "Association", "(" + f[0] + " = " + f[2] + ")", "FK");
      cn.ClientEnd.Cardinality = "0..*"; cn.ClientEnd.Role = "FK_" + tb2.n + "_" + f[1] + (f[3] ? f[3] : ""); cn.ClientEnd.Update();
      cn.SupplierEnd.Cardinality = "1"; cn.SupplierEnd.Role = "PK_" + f[1]; cn.SupplierEnd.Update();
      cn.Update();
    }
  }
  d.Update();
  return d;
}

// ---------- Actividad ----------
// nodes: [[id, kind, label, x, y]] kind: init|final|action|decision ; flows: [[from,to,guard]]
function buildACT(pkg, cu, act){
  var d = newDiag(pkg, cu.code + "_60MN - " + cu.title, "Activity");
  var reg = {}, seq = 300;
  for (var p = 0; p < (act.parts || []).length; p++){
    var pt = act.parts[p];
    var pe = newEl(pkg, pt[0], "ActivityPartition");
    place(d, pe, pt[1], pt[2], pt[3], pt[4], 400 + p);
  }
  for (var i = 0; i < act.nodes.length; i++){
    var n = act.nodes[i], e, w, h;
    if (n[1] == "init"){ e = newEl(pkg, "Inicio", "StateNode"); e.Subtype = 100; e.Update(); w = 26; h = 26; }
    else if (n[1] == "final"){ e = newEl(pkg, "Fin", "StateNode"); e.Subtype = 101; e.Update(); w = 26; h = 26; }
    else if (n[1] == "decision"){ e = newEl(pkg, n[2], "Decision"); w = 40; h = 40; }
    else if (n[1] == "merge"){ e = newEl(pkg, "", "MergeNode"); w = 30; h = 30; }
    else { e = newEl(pkg, n[2], "Action"); w = n[5] || 190; h = n[6] || 46; }
    place(d, e, n[3] - w / 2, n[4] - h / 2, w, h, seq--);
    reg[n[0]] = e;
  }
  for (var j = 0; j < act.flows.length; j++){
    var f = act.flows[j];
    var c = conn(reg[f[0]], reg[f[1]], "ControlFlow", "", "");
    if (f[2]) { c.TransitionGuard = f[2]; c.Update(); }
  }
  d.Update();
  return d;
}

function exportDiag(d, file){
  var ok = Repository.GetProjectInterface().PutDiagramImageToFile(d.DiagramGUID, OUTDIR + file, 1);
  log("EXPORT " + file + " " + ok);
}
// ===== Datos de los diagramas =====
var CUS = [
 {code:"CU01", title:"Crear Usuario", actor:"Administrador", main:"Crear Usuario", ext:[]},
 {code:"CU02", title:"Iniciar Sesión", actor:"Usuario", main:"Iniciar Sesión", ext:[["Cambiar Clave (primer ingreso)","extend"],["Reparar inconsistencia (CU11)","extend"]]},
 {code:"CU03", title:"Desbloquear Usuario", actor:"Administrador", main:"Desbloquear Usuario", ext:[]},
 {code:"CU04", title:"Modificar Usuario", actor:"Administrador", main:"Modificar Usuario", ext:[]},
 {code:"CU05", title:"Cambiar Clave", actor:"Usuario", main:"Cambiar Clave", ext:[]},
 {code:"CU06", title:"Logout", actor:"Usuario", main:"Logout", ext:[]},
 {code:"CU07", title:"Activar-Desactivar Usuario", actor:"Administrador", main:"Activar/Desactivar Usuario", ext:[]},
 {code:"CU08", title:"Consultar Bitácora de Eventos", actor:"Administrador", main:"Consultar Bitácora de Eventos", ext:[["Exportar PDF","extend"],["Exportar XML","extend"]]},
 {code:"CU09", title:"Crear Perfil", actor:"Administrador", main:"Crear Perfil", ext:[["Asignar patentes y familias","include"]]},
 {code:"CU10", title:"Cambiar Idioma", actor:"Usuario", main:"Cambiar Idioma", ext:[]},
 {code:"CU11", title:"Reparar Inconsistencia", actor:"Administrador", main:"Reparar Inconsistencia", ext:[["Verificar dígitos verificadores","include"]]},
 {code:"CU12", title:"Consultar Bitácora de Cambios", actor:"Administrador", main:"Consultar Bitácora de Cambios", ext:[["Comparar versiones","extend"],["Restaurar versión","extend"]]}
];

// ---------------- SECUENCIAS ----------------
var SD = {};
SD.CU04 = {lifelines:[["Administrador","actor"],["FrmGestionUsuarios_60MN","boundary"],["UsuarioBLL_60MN",""],["SesionActual_60MN",""],["CriptografiaHandler_60MN",""],["UsuarioDAL_60MN",""],["GestorDigitoVerificador_60MN",""],["BitacoraBLL_60MN",""]],
 msgs:[
  ["Administrador","FrmGestionUsuarios_60MN","Modificar(usuarioSeleccionado)"],
  ["FrmGestionUsuarios_60MN","UsuarioBLL_60MN","ObtenerPorId(id)"],
  ["UsuarioBLL_60MN","UsuarioDAL_60MN","ObtenerPorId(id)"],
  ["UsuarioDAL_60MN","UsuarioBLL_60MN","Usuario_60MN",1],
  ["UsuarioBLL_60MN","CriptografiaHandler_60MN","Descifrar(DNI, Email)"],
  ["UsuarioBLL_60MN","FrmGestionUsuarios_60MN","Usuario_60MN",1],
  ["FrmGestionUsuarios_60MN","FrmGestionUsuarios_60MN","MostrarDatos(usuario)"],
  ["Administrador","FrmGestionUsuarios_60MN","Aceptar(datosEditados)"],
  ["FrmGestionUsuarios_60MN","UsuarioBLL_60MN","Modificar(usuario)"],
  ["UsuarioBLL_60MN","SesionActual_60MN","RequierePermiso(UsuarioModificar)"],
  ["UsuarioBLL_60MN","UsuarioBLL_60MN","ValidarUsuario(usuario)"],
  ["UsuarioBLL_60MN","UsuarioDAL_60MN","ExisteDni(dni, id)"],
  ["UsuarioDAL_60MN","UsuarioBLL_60MN","false",1],
  ["UsuarioBLL_60MN","CriptografiaHandler_60MN","Cifrar(DNI, Email)"],
  ["UsuarioBLL_60MN","UsuarioDAL_60MN","Actualizar(usuario)"],
  ["UsuarioBLL_60MN","GestorDigitoVerificador_60MN","ActualizarFila(Usuario, id)"],
  ["UsuarioBLL_60MN","BitacoraBLL_60MN","Registrar(UsuarioModificado, descripcion)"],
  ["UsuarioBLL_60MN","FrmGestionUsuarios_60MN","OK",1],
  ["FrmGestionUsuarios_60MN","Administrador","Informar(\"Usuario modificado correctamente\")"],
  ["FrmGestionUsuarios_60MN","UsuarioBLL_60MN","Modificar(usuario)"],
  ["UsuarioBLL_60MN","UsuarioDAL_60MN","ExisteDni(dni, id)"],
  ["UsuarioDAL_60MN","UsuarioBLL_60MN","true",1],
  ["UsuarioBLL_60MN","FrmGestionUsuarios_60MN","NegocioException_60MN",1],
  ["FrmGestionUsuarios_60MN","Administrador","Informar(\"Ya existe un usuario con ese DNI\")"]
 ],
 frags:[["alt","DNI duplicado",19,23]]};

SD.CU05 = {lifelines:[["Usuario","actor"],["FrmCambiarClave_60MN","boundary"],["UsuarioBLL_60MN",""],["SessionManager_60MN",""],["PasswordHasher_60MN",""],["PoliticaClave_60MN",""],["UsuarioDAL_60MN",""],["GestorDigitoVerificador_60MN",""],["BitacoraBLL_60MN",""]],
 msgs:[
  ["Usuario","FrmCambiarClave_60MN","Aceptar(actual, nueva, confirmacion)"],
  ["FrmCambiarClave_60MN","UsuarioBLL_60MN","CambiarClave(actual, nueva, confirmacion)"],
  ["UsuarioBLL_60MN","SessionManager_60MN","Instancia.Usuario"],
  ["SessionManager_60MN","UsuarioBLL_60MN","Usuario_60MN",1],
  ["UsuarioBLL_60MN","PasswordHasher_60MN","Verificar(actual, usuario.ClaveHash)"],
  ["PasswordHasher_60MN","UsuarioBLL_60MN","true",1],
  ["UsuarioBLL_60MN","PoliticaClave_60MN","Validar(nueva)"],
  ["PoliticaClave_60MN","UsuarioBLL_60MN","OK",1],
  ["UsuarioBLL_60MN","PasswordHasher_60MN","Hashear(nueva)"],
  ["PasswordHasher_60MN","UsuarioBLL_60MN","\"sal.hash\"",1],
  ["UsuarioBLL_60MN","UsuarioDAL_60MN","ActualizarClave(id, \"sal.hash\")"],
  ["UsuarioBLL_60MN","GestorDigitoVerificador_60MN","ActualizarFila(Usuario, id)"],
  ["UsuarioBLL_60MN","BitacoraBLL_60MN","Registrar(ClaveCambiada, descripcion)"],
  ["UsuarioBLL_60MN","FrmCambiarClave_60MN","OK",1],
  ["FrmCambiarClave_60MN","Usuario","Informar(\"Contraseña cambiada con éxito\")"],
  ["FrmCambiarClave_60MN","UsuarioBLL_60MN","CambiarClave(actual, nueva, confirmacion)"],
  ["UsuarioBLL_60MN","PasswordHasher_60MN","Verificar(actual, usuario.ClaveHash)"],
  ["PasswordHasher_60MN","UsuarioBLL_60MN","false",1],
  ["UsuarioBLL_60MN","FrmCambiarClave_60MN","NegocioException_60MN",1],
  ["FrmCambiarClave_60MN","Usuario","Informar(\"La contraseña actual es incorrecta\")"]
 ],
 frags:[["alt","Contraseña actual incorrecta",15,19]]};

SD.CU07 = {lifelines:[["Administrador","actor"],["FrmGestionUsuarios_60MN","boundary"],["UsuarioBLL_60MN",""],["SesionActual_60MN",""],["UsuarioDAL_60MN",""],["GestorDigitoVerificador_60MN",""],["BitacoraBLL_60MN",""]],
 msgs:[
  ["Administrador","FrmGestionUsuarios_60MN","SeleccionarFiltro(\"Todos\")"],
  ["FrmGestionUsuarios_60MN","UsuarioBLL_60MN","Listar(soloActivos = false)"],
  ["UsuarioBLL_60MN","FrmGestionUsuarios_60MN","List<Usuario_60MN>",1],
  ["Administrador","FrmGestionUsuarios_60MN","ActivarDesactivar(usuarioSeleccionado)"],
  ["FrmGestionUsuarios_60MN","Administrador","SolicitarConfirmacion()"],
  ["Administrador","FrmGestionUsuarios_60MN","Confirmar()"],
  ["FrmGestionUsuarios_60MN","UsuarioBLL_60MN","CambiarEstado(id)"],
  ["UsuarioBLL_60MN","SesionActual_60MN","RequierePermiso(UsuarioBaja)"],
  ["UsuarioBLL_60MN","UsuarioDAL_60MN","ObtenerPorId(id)"],
  ["UsuarioDAL_60MN","UsuarioBLL_60MN","Usuario_60MN",1],
  ["UsuarioBLL_60MN","UsuarioBLL_60MN","ValidarCambioEstado(usuario)"],
  ["UsuarioBLL_60MN","UsuarioDAL_60MN","ActualizarEstado(id, !usuario.Activo)"],
  ["UsuarioBLL_60MN","GestorDigitoVerificador_60MN","ActualizarFila(Usuario, id)"],
  ["UsuarioBLL_60MN","BitacoraBLL_60MN","Registrar(UsuarioActivado/Desactivado, descripcion)"],
  ["UsuarioBLL_60MN","FrmGestionUsuarios_60MN","OK",1],
  ["FrmGestionUsuarios_60MN","FrmGestionUsuarios_60MN","CargarGrilla()"],
  ["FrmGestionUsuarios_60MN","Administrador","Informar(\"Usuario activado/desactivado correctamente\")"],
  ["FrmGestionUsuarios_60MN","UsuarioBLL_60MN","CambiarEstado(id)"],
  ["UsuarioBLL_60MN","UsuarioBLL_60MN","ValidarCambioEstado(usuario)"],
  ["UsuarioBLL_60MN","FrmGestionUsuarios_60MN","NegocioException_60MN",1],
  ["FrmGestionUsuarios_60MN","Administrador","Informar(\"No se puede desactivar a sí mismo ni al último administrador\")"]
 ],
 frags:[["alt","Propio usuario o último administrador activo",17,20]]};

SD.CU09 = {lifelines:[["Administrador","actor"],["FrmPerfiles_60MN","boundary"],["FamiliaBLL_60MN",""],["SesionActual_60MN",""],["Familia_60MN",""],["FamiliaDAL_60MN",""],["GestorDigitoVerificador_60MN",""],["BitacoraBLL_60MN",""]],
 msgs:[
  ["Administrador","FrmPerfiles_60MN","NuevoPerfil()"],
  ["FrmPerfiles_60MN","FamiliaBLL_60MN","ListarPatentes() / ListarFamilias()"],
  ["FamiliaBLL_60MN","FrmPerfiles_60MN","árbol de permisos",1],
  ["Administrador","FrmPerfiles_60MN","Guardar(nombre, descripcion, permisos)"],
  ["FrmPerfiles_60MN","FamiliaBLL_60MN","CrearFamilia(nombre, descripcion, hijos)"],
  ["FamiliaBLL_60MN","SesionActual_60MN","RequierePermiso(PerfilAlta)"],
  ["FamiliaBLL_60MN","FamiliaDAL_60MN","ExisteNombre(nombre)"],
  ["FamiliaDAL_60MN","FamiliaBLL_60MN","false",1],
  ["FamiliaBLL_60MN","Familia_60MN","AgregarHijo(componente)"],
  ["Familia_60MN","Familia_60MN","ContieneFamilia(id)"],
  ["FamiliaBLL_60MN","FamiliaDAL_60MN","Insertar(familia)"],
  ["FamiliaBLL_60MN","GestorDigitoVerificador_60MN","ActualizarFila(Familia / FamiliaPatente / FamiliaFamilia)"],
  ["FamiliaBLL_60MN","BitacoraBLL_60MN","Registrar(PerfilCreado, descripcion)"],
  ["FamiliaBLL_60MN","FrmPerfiles_60MN","OK",1],
  ["FrmPerfiles_60MN","Administrador","Informar(\"Perfil creado correctamente\")"],
  ["FrmPerfiles_60MN","FamiliaBLL_60MN","CrearFamilia(nombre, descripcion, hijos)"],
  ["FamiliaBLL_60MN","Familia_60MN","AgregarHijo(familia)"],
  ["Familia_60MN","FamiliaBLL_60MN","CicloFamiliasException_60MN",1],
  ["FamiliaBLL_60MN","FrmPerfiles_60MN","CicloFamiliasException_60MN",1],
  ["FrmPerfiles_60MN","Administrador","Informar(\"La familia generaría un ciclo\")"]
 ],
 frags:[["loop","Por cada permiso seleccionado",8,9],["alt","Nombre repetido o ciclo detectado",15,19]]};

SD.CU10 = {lifelines:[["Usuario","actor"],["FrmIdioma_60MN","boundary"],["GestorIdioma_60MN",""],["IdiomaDAL_60MN",""],["FormBase_60MN",""],["TraductorFormularios_60MN",""],["BitacoraBLL_60MN",""]],
 msgs:[
  ["Usuario","FrmIdioma_60MN","Aplicar(idioma)"],
  ["FrmIdioma_60MN","GestorIdioma_60MN","Instancia.CambiarIdioma(idioma)"],
  ["GestorIdioma_60MN","IdiomaDAL_60MN","ObtenerTraducciones(idiomaId)"],
  ["IdiomaDAL_60MN","GestorIdioma_60MN","Dictionary<clave, texto>",1],
  ["GestorIdioma_60MN","GestorIdioma_60MN","Notificar()"],
  ["GestorIdioma_60MN","FormBase_60MN","ActualizarIdioma(idioma)"],
  ["FormBase_60MN","TraductorFormularios_60MN","Traducir(this)"],
  ["FormBase_60MN","FormBase_60MN","AlCambiarIdioma()"],
  ["GestorIdioma_60MN","BitacoraBLL_60MN","Registrar(IdiomaCambiado, descripcion)"],
  ["GestorIdioma_60MN","FrmIdioma_60MN","OK",1],
  ["FrmIdioma_60MN","Usuario","Pantallas traducidas en vivo"]
 ],
 frags:[["loop","Por cada formulario suscripto",5,7]]};

// ---------------- CLASES ----------------
var DC = {};
var C_DV = {n:"GestorDigitoVerificador_60MN", ops:[["+","ActualizarFila","tabla : TablaDV_60MN, id : int","void"]]};
var C_BIT = {n:"BitacoraBLL_60MN", ops:[["+","Registrar","evento : EventoSistema_60MN, descripcion : string","void"]]};
var C_SES = {n:"SesionActual_60MN", ops:[["+","RequierePermiso","codigo : string","void",0,1]]};
DC.CU04 = {rows:[
  [["VetCare (GUI)",[{n:"FrmGestionUsuarios_60MN", atts:[["-","usuarioBLL","UsuarioBLL_60MN"]], ops:[["-","btnModificar_Click","","void"],["-","CargarGrilla","","void"]]}]]],
  [["BLL_60MN",[{n:"UsuarioBLL_60MN", atts:[["-","mapper","UsuarioDAL_60MN"]], ops:[["+","ObtenerPorId","id : int","Usuario_60MN"],["+","Modificar","u : Usuario_60MN","void"],["-","ValidarUsuario","u : Usuario_60MN","void"]]}, C_BIT]],
   ["Servicios_60MN",[C_SES]]],
  [["DAL_60MN",[{n:"UsuarioDAL_60MN", ops:[["+","ObtenerPorId","id : int","Usuario_60MN"],["+","ExisteDni","dni : string, id : int","bool"],["+","Actualizar","u : Usuario_60MN","void"]]}]],
   ["Entidades_60MN",[{n:"Usuario_60MN", atts:[["+","Id","int"],["+","NombreUsuario","string"],["+","Nombre","string"],["+","Apellido","string"],["+","DNI","string"],["+","Email","string"],["+","Activo","bool"],["+","Bloqueado","bool"]]}]],
   ["Seguridad_60MN",[{n:"CriptografiaHandler_60MN", ops:[["+","Cifrar","texto : string","string"],["+","Descifrar","texto : string","string"]]}, C_DV]]]
 ],
 deps:[["FrmGestionUsuarios_60MN","UsuarioBLL_60MN"],["UsuarioBLL_60MN","UsuarioDAL_60MN"],["UsuarioBLL_60MN","CriptografiaHandler_60MN"],["UsuarioBLL_60MN","GestorDigitoVerificador_60MN"],["UsuarioBLL_60MN","BitacoraBLL_60MN"],["UsuarioBLL_60MN","SesionActual_60MN","use"],["UsuarioDAL_60MN","Usuario_60MN","use"]]};

DC.CU05 = {rows:[
  [["VetCare (GUI)",[{n:"FrmCambiarClave_60MN", ops:[["-","btnAceptar_Click","","void"],["-","btnCancelar_Click","","void"]]}]]],
  [["BLL_60MN",[{n:"UsuarioBLL_60MN", atts:[["-","mapper","UsuarioDAL_60MN"]], ops:[["+","CambiarClave","actual : string, nueva : string, confirmacion : string","void"]]}, C_BIT]],
   ["Servicios_60MN",[{n:"SessionManager_60MN", st:"Singleton", atts:[["-","instancia","SessionManager_60MN","1"]], ops:[["+","Instancia","","SessionManager_60MN",0,1],["+","Usuario","","Usuario_60MN"]]}]]],
  [["DAL_60MN",[{n:"UsuarioDAL_60MN", ops:[["+","ActualizarClave","id : int, claveHash : string","void"]]}]],
   ["Seguridad_60MN",[{n:"PasswordHasher_60MN", ops:[["+","Hashear","password : string","string",0,1],["+","Verificar","password : string, hash : string","bool",0,1]]},{n:"PoliticaClave_60MN", ops:[["+","Validar","password : string","void"]]}, C_DV]]]
 ],
 deps:[["FrmCambiarClave_60MN","UsuarioBLL_60MN"],["UsuarioBLL_60MN","UsuarioDAL_60MN"],["UsuarioBLL_60MN","SessionManager_60MN","use"],["UsuarioBLL_60MN","PasswordHasher_60MN"],["UsuarioBLL_60MN","PoliticaClave_60MN"],["UsuarioBLL_60MN","GestorDigitoVerificador_60MN"],["UsuarioBLL_60MN","BitacoraBLL_60MN"]]};

DC.CU07 = {rows:[
  [["VetCare (GUI)",[{n:"FrmGestionUsuarios_60MN", atts:[["-","rbActivos","RadioButton"],["-","rbTodos","RadioButton"]], ops:[["-","btnActivarDesactivar_Click","","void"],["-","CargarGrilla","","void"]]}]]],
  [["BLL_60MN",[{n:"UsuarioBLL_60MN", atts:[["-","mapper","UsuarioDAL_60MN"]], ops:[["+","Listar","soloActivos : bool","List<Usuario_60MN>"],["+","CambiarEstado","id : int","void"],["-","ValidarCambioEstado","u : Usuario_60MN","void"]]}, C_BIT]],
   ["Servicios_60MN",[C_SES]]],
  [["DAL_60MN",[{n:"UsuarioDAL_60MN", ops:[["+","ObtenerPorId","id : int","Usuario_60MN"],["+","ActualizarEstado","id : int, activo : bool","void"],["+","ContarAdministradoresActivos","","int"]]}]],
   ["Entidades_60MN",[{n:"Usuario_60MN", atts:[["+","Id","int"],["+","Activo","bool"],["+","Bloqueado","bool"]]}]],
   ["Seguridad_60MN",[C_DV]]]
 ],
 deps:[["FrmGestionUsuarios_60MN","UsuarioBLL_60MN"],["UsuarioBLL_60MN","UsuarioDAL_60MN"],["UsuarioBLL_60MN","GestorDigitoVerificador_60MN"],["UsuarioBLL_60MN","BitacoraBLL_60MN"],["UsuarioBLL_60MN","SesionActual_60MN","use"],["UsuarioDAL_60MN","Usuario_60MN","use"]]};

DC.CU09 = {rows:[
  [["VetCare (GUI)",[{n:"FrmPerfiles_60MN", ops:[["-","btnNuevo_Click","","void"],["-","btnGuardar_Click","","void"],["-","CargarArbol","","void"]]}]]],
  [["BLL_60MN",[{n:"FamiliaBLL_60MN", ops:[["+","ListarPatentes","","List<Patente_60MN>"],["+","ListarFamilias","","List<Familia_60MN>"],["+","CrearFamilia","nombre : string, descripcion : string, hijos : List<Componente_60MN>","void"]]}, C_BIT]]],
  [["DAL_60MN",[{n:"FamiliaDAL_60MN", ops:[["+","ExisteNombre","nombre : string","bool"],["+","Insertar","f : Familia_60MN","void"]]}]],
   ["Servicios_60MN",[{n:"Componente_60MN", abs:1, atts:[["+","Id","int"],["+","Nombre","string"],["+","EsFamilia","bool"]], ops:[["+","TienePatente","codigo : string","bool",1],["+","ContieneFamilia","id : int","bool"]]},
                      {n:"Patente_60MN", atts:[["+","Codigo","string"]]},
                      {n:"Familia_60MN", atts:[["-","hijos","List<Componente_60MN>"]], ops:[["+","AgregarHijo","c : Componente_60MN","void"],["+","ObtenerHijos","","List<Componente_60MN>"]]}]]]
 ],
 deps:[["FrmPerfiles_60MN","FamiliaBLL_60MN"],["FamiliaBLL_60MN","FamiliaDAL_60MN"],["FamiliaBLL_60MN","BitacoraBLL_60MN"],["FamiliaBLL_60MN","Familia_60MN","use"],
       ["Patente_60MN","Componente_60MN","","Generalization"],["Familia_60MN","Componente_60MN","","Generalization"],["Familia_60MN","Componente_60MN","","Aggregation","hijos"]]};

DC.CU10 = {rows:[
  [["VetCare (GUI)",[{n:"FormBase_60MN", ops:[["#","OnLoad","e : EventArgs","void"],["+","ActualizarIdioma","idioma : IIdioma_60MN","void"],["#","AlCambiarIdioma","","void"]]},
                     {n:"FrmIdioma_60MN", ops:[["-","btnAplicar_Click","","void"]]},
                     {n:"TraductorFormularios_60MN", ops:[["+","Traducir","form : Form","void",0,1]]}]]],
  [["Interfaces_60MN",[{n:"IObservadorIdioma_60MN", iface:1, ops:[["+","ActualizarIdioma","idioma : IIdioma_60MN","void",1]]}]],
   ["Servicios_60MN",[{n:"GestorIdioma_60MN", st:"Singleton", atts:[["-","observadores","List<IObservadorIdioma_60MN>"],["+","IdiomaActual","IIdioma_60MN"]], ops:[["+","Instancia","","GestorIdioma_60MN",0,1],["+","Suscribir","o : IObservadorIdioma_60MN","void"],["+","Desuscribir","o : IObservadorIdioma_60MN","void"],["+","CambiarIdioma","idioma : IIdioma_60MN","void"],["-","Notificar","","void"]]}]]],
  [["DAL_60MN",[{n:"IdiomaDAL_60MN", ops:[["+","Listar","","List<Idioma_60MN>"],["+","ObtenerTraducciones","idiomaId : int","Dictionary<string,string>"]]}]],
   ["Entidades_60MN",[{n:"Idioma_60MN", atts:[["+","Id","int"],["+","Nombre","string"],["+","Codigo","string"]]}]],
   ["BLL_60MN",[C_BIT]]]
 ],
 deps:[["FormBase_60MN","IObservadorIdioma_60MN","","Realisation"],["FrmIdioma_60MN","FormBase_60MN","","Generalization"],["FormBase_60MN","TraductorFormularios_60MN","use"],
       ["FrmIdioma_60MN","GestorIdioma_60MN"],["GestorIdioma_60MN","IObservadorIdioma_60MN","","Aggregation","observadores"],["GestorIdioma_60MN","IdiomaDAL_60MN"],["GestorIdioma_60MN","BitacoraBLL_60MN"],["IdiomaDAL_60MN","Idioma_60MN","use"]]};

// ---------------- DER ----------------
var T_USUARIO = {n:"Usuario", cols:[["UsuarioID","INT",1,1],["Usuario","NVARCHAR(50)",0,1],["Nombre","NVARCHAR(80)",0,1],["Apellido","NVARCHAR(80)",0,1],["DNI","NVARCHAR(200)",0,1],["Email","NVARCHAR(400)",0,0],["Clave","NVARCHAR(200)",0,1],["Activo","BIT",0,1],["Bloqueado","BIT",0,1],["IntentosFallidos","INT",0,1],["DVH","NVARCHAR(64)",0,0]]};
var T_BITACORA = {n:"Bitacora", cols:[["BitacoraID","INT",1,1],["FechaHora","DATETIME",0,1],["UsuarioID","INT",0,0,1],["Modulo","NVARCHAR(50)",0,1],["Evento","NVARCHAR(80)",0,1],["Criticidad","INT",0,1],["Descripcion","NVARCHAR(MAX)",0,0],["DVH","NVARCHAR(64)",0,0]], fks:[["UsuarioID","Usuario","UsuarioID"]]};
var T_DVV = {n:"DVV", cols:[["Tabla","NVARCHAR(50)",1,1],["DVV","NVARCHAR(64)",0,1]]};
var T_FAMILIA = {n:"Familia", cols:[["FamiliaID","INT",1,1],["Nombre","NVARCHAR(80)",0,1],["Descripcion","NVARCHAR(200)",0,0],["DVH","NVARCHAR(64)",0,0]]};
var T_PATENTE = {n:"Patente", cols:[["PatenteID","INT",1,1],["Codigo","NVARCHAR(50)",0,1],["Nombre","NVARCHAR(80)",0,1],["DVH","NVARCHAR(64)",0,0]]};
var T_UF = {n:"UsuarioFamilia", cols:[["UsuarioID","INT",1,1,1],["FamiliaID","INT",1,1,1],["DVH","NVARCHAR(64)",0,0]], fks:[["UsuarioID","Usuario","UsuarioID"],["FamiliaID","Familia","FamiliaID"]]};
var DER = {};
DER.CU04 = [T_USUARIO, T_UF, T_FAMILIA, T_BITACORA, T_DVV];
DER.CU05 = [T_USUARIO, T_BITACORA, T_DVV];
DER.CU07 = [T_USUARIO, T_BITACORA, T_DVV];
DER.CU09 = [T_FAMILIA, T_PATENTE,
  {n:"FamiliaPatente", cols:[["FamiliaID","INT",1,1,1],["PatenteID","INT",1,1,1],["DVH","NVARCHAR(64)",0,0]], fks:[["FamiliaID","Familia","FamiliaID"],["PatenteID","Patente","PatenteID"]]},
  {n:"FamiliaFamilia", cols:[["FamiliaPadreID","INT",1,1,1],["FamiliaHijaID","INT",1,1,1],["DVH","NVARCHAR(64)",0,0]], fks:[["FamiliaPadreID","Familia","FamiliaID","_Padre"],["FamiliaHijaID","Familia","FamiliaID","_Hija"]]},
  {n:"UsuarioFamilia", cols:[["UsuarioID","INT",1,1,1],["FamiliaID","INT",1,1,1],["DVH","NVARCHAR(64)",0,0]], fks:[["FamiliaID","Familia","FamiliaID"]]}];
DER.CU10 = [{n:"Idioma", cols:[["IdiomaID","INT",1,1],["Nombre","NVARCHAR(50)",0,1],["Codigo","NVARCHAR(10)",0,1]]},
  {n:"Traduccion", cols:[["TraduccionID","INT",1,1],["IdiomaID","INT",0,1,1],["Clave","NVARCHAR(150)",0,1],["Texto","NVARCHAR(500)",0,1]], fks:[["IdiomaID","Idioma","IdiomaID"]]},
  {n:"Bitacora", cols:[["BitacoraID","INT",1,1],["FechaHora","DATETIME",0,1],["UsuarioID","INT",0,0],["Evento","NVARCHAR(80)",0,1],["Criticidad","INT",0,1],["Descripcion","NVARCHAR(MAX)",0,0],["DVH","NVARCHAR(64)",0,0]]}];

// ---------------- ACTIVIDAD CU02 ----------------
var M = 330, R = 640, R2 = 900;
var ACT = {
 parts:[["Usuario",30,20,1060,150],["Sistema",30,170,1060,1180]],
 nodes:[
  ["ini","init","",M,60],
  ["a1","action","Ingresar usuario y contraseña",M,105],
  ["a2","action","Presionar \"Entrar\"",M,160],
  ["d1","decision","¿Es el usuario de emergencia?",M,230],
  ["e1","action","Validar firma HMAC del XML, que no haya administrador activo y la clave (SHA-256 + sal)",R,230,230,60],
  ["de","decision","¿Válido?",R2,230],
  ["fe","final","",R2,320],
  ["a3","action","Buscar usuario y verificar DVH de su fila",M,310],
  ["d2","decision","¿Existe y DVH válido?",M,380],
  ["r2","action","Rechazar y registrar LoginFallido / RegistroAlterado",R,380,230,50],
  ["f2","final","",R2,380],
  ["d3","decision","¿Activo y no bloqueado?",M,450],
  ["r3","action","Informar: contactar a un administrador",R,450,230,46],
  ["f3","final","",R2,450],
  ["a4","action","Validar contraseña (SHA-256 + sal)",M,520],
  ["d4","decision","¿Contraseña correcta?",M,590],
  ["r4","action","Sumar intento fallido",R,590,200,40],
  ["d5","decision","¿4.º intento consecutivo?",R,670],
  ["r5","action","Bloquear usuario y registrar UsuarioBloqueado",R2,670,210,50],
  ["r6","action","Mostrar intentos restantes",R,750,200,40],
  ["f4","final","",R2,750],
  ["a5","action","Cargar familias y patentes (Composite)",M,670],
  ["a6","action","Verificar dígitos verificadores de la base",M,740],
  ["d6","decision","¿Base consistente?",M,810],
  ["d7","decision","¿Tiene patente SEG_INTEGRIDAD?",R,850],
  ["r7","action","Denegar acceso",R2,850,170,40],
  ["f5","final","",R2,930],
  ["r8","action","Habilitar CU11 Reparar inconsistencia",R,930,220,46],
  ["mg","merge","",M,1010],
  ["a7","action","Reiniciar intentos y abrir sesión (SessionManager_60MN)",M,1070,230,50],
  ["a8","action","Registrar evento Login",M,1140],
  ["a9","action","Mostrar menú principal según permisos",M,1210],
  ["fin","final","",M,1280]
 ],
 flows:[["ini","a1"],["a1","a2"],["a2","d1"],["d1","e1","sí"],["e1","de"],["de","fe","no"],["de","mg","sí"],["d1","a3","no"],["a3","d2"],["d2","r2","no"],["r2","f2"],["d2","d3","sí"],
        ["d3","r3","no"],["r3","f3"],["d3","a4","sí"],["a4","d4"],["d4","r4","no"],["r4","d5"],["d5","r5","sí"],["d5","r6","no"],["r5","f4"],["r6","f4"],["d4","a5","sí"],["a5","a6"],["a6","d6"],
        ["d6","d7","no"],["d7","r7","no"],["r7","f5"],["d7","r8","sí"],["r8","mg"],["d6","mg","sí"],["mg","a7"],["a7","a8"],["a8","a9"],["a9","fin"]]
};
// ===== Programa principal =====
var model = Repository.Models.GetAt(0);
delPkg(model, ROOTNAME);
var root = newPkg(model, ROOTNAME);
var fso = new ActiveXObject("Scripting.FileSystemObject");
if (!fso.FolderExists(OUTDIR)) fso.CreateFolder(OUTDIR);
var ONLY = (typeof GEN_ONLY != "undefined") ? GEN_ONLY : null;
for (var i = 0; i < CUS.length; i++){
  var cu = CUS[i];
  if (ONLY && ONLY != cu.code) continue;
  var p = newPkg(root, cu.code + " - " + cu.title);
  var low = cu.code.toLowerCase();
  exportDiag(buildUC(p, cu), low + "_cu.png");
  if (cu.code == "CU02") exportDiag(buildACT(p, cu, ACT), "cu02_act.png");
  if (SD[cu.code]) exportDiag(buildSD(p, cu, SD[cu.code]), low + "_seq.png");
  if (DC[cu.code]) exportDiag(buildDC(p, cu, DC[cu.code]), low + "_cls.png");
  if (DER[cu.code]) exportDiag(buildDER(p, cu, DER[cu.code], cu.code == "CU09" ? 3 : 3), low + "_der.png");
}
Repository.RefreshModelView(0);
var st = new ActiveXObject("ADODB.Stream"); st.Type = 2; st.Charset = "utf-8"; st.Open(); st.WriteText(LOG.join("\r\n")); st.SaveToFile(OUTDIR + "_log.txt", 2); st.Close();
Session.Output("GEN OK");
