var OUT = "C:\\Users\\jmora\\Desktop\\RepoFacultad\\IngenieriaEnSistemas-Morante-Negrete\\.ea_scripts\\inspect_out.txt";
var buf = [];
function w(s){ buf.push(s); }
function walk(pkg, depth){
  var pad = ""; for (var i=0;i<depth;i++) pad += "  ";
  w(pad + "[P] " + pkg.Name + " id=" + pkg.PackageID);
  for (var d=0; d<pkg.Diagrams.Count; d++){
    var dg = pkg.Diagrams.GetAt(d);
    w(pad + "  [D] " + dg.Name + " | type=" + dg.Type + " | meta=" + dg.MetaType + " | id=" + dg.DiagramID + " | styleEx=" + dg.StyleEx + " | extStyle=" + dg.ExtendedStyle);
  }
  for (var p=0; p<pkg.Packages.Count; p++) walk(pkg.Packages.GetAt(p), depth+1);
}
for (var m=0; m<Repository.Models.Count; m++) walk(Repository.Models.GetAt(m), 0);
w("==== DIAGRAM SQL");
w(Repository.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type, StyleEx, PDATA, ShowDetails, cx, cy, Swimlanes FROM t_diagram"));
var fso = new ActiveXObject("Scripting.FileSystemObject");
var st = new ActiveXObject("ADODB.Stream"); st.Type=2; st.Charset="utf-8"; st.Open(); st.WriteText(buf.join("\r\n")); st.SaveToFile(OUT, 2); st.Close();
Session.Output("INSPECT OK " + buf.length);
