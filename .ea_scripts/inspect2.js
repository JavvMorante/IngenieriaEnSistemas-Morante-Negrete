var OUT = "C:\\Users\\jmora\\Desktop\\RepoFacultad\\IngenieriaEnSistemas-Morante-Negrete\\.ea_scripts\\inspect2_out.txt";
var buf = [];
function w(s){ buf.push(s); }
var ids=[69,70,71,68];
for (var k=0;k<ids.length;k++){
  var d=Repository.GetDiagramByID(ids[k]);
  w("######## DIAGRAM "+d.Name+" type="+d.Type+" meta="+d.MetaType+" stereo="+d.Stereotype+" cx="+d.cx+" cy="+d.cy+" showDetails="+d.ShowDetails+" scale="+d.Scale);
  w("styleEx="+d.StyleEx); w("extStyle="+d.ExtendedStyle);
  for (var i=0;i<d.DiagramObjects.Count;i++){
    var o=d.DiagramObjects.GetAt(i); var e=Repository.GetElementByID(o.ElementID);
    w(" OBJ "+e.Name+" | type="+e.Type+" stereo="+e.Stereotype+" sub="+e.Subtype+" gentype="+e.Gentype+" | l="+o.left+" r="+o.right+" t="+o.top+" b="+o.bottom+" seq="+o.Sequence+" style="+o.Style);
    if (k==2 && i<3){
      for (var a=0;a<e.Attributes.Count;a++){var at=e.Attributes.GetAt(a); w("    ATT "+at.Name+" "+at.Type+" st="+at.Stereotype+" ord="+at.IsOrdered+" dup="+at.AllowDuplicates+" len="+at.Length+" coll="+at.IsCollection+" stat="+at.IsStatic+" pos="+at.Pos);}
      for (var m=0;m<e.Methods.Count;m++){var me=e.Methods.GetAt(m); var ps=""; for (var p=0;p<me.Parameters.Count;p++) ps+=me.Parameters.GetAt(p).Name+":"+me.Parameters.GetAt(p).Type+","; w("    METH "+me.Name+" st="+me.Stereotype+" ret="+me.ReturnType+" ("+ps+")");}
    }
  }
  for (var j=0;j<d.DiagramLinks.Count;j++){
    var l=d.DiagramLinks.GetAt(j); var c=Repository.GetConnectorByID(l.ConnectorID);
    w(" LINK "+c.Type+" '"+c.Name+"' st="+c.Stereotype+" seq="+c.SequenceNo+" "+c.ClientID+"->"+c.SupplierID+" sy="+c.StartPointY+" ey="+c.EndPointY+" geom="+l.Geometry+" style="+l.Style+" cc="+c.ClientEnd.Cardinality+" sc="+c.SupplierEnd.Cardinality+" diag="+c.DiagramID);
  }
}
w("==== CONNECTORS SQL d69");
w(Repository.SQLQuery("SELECT Connector_ID, Connector_Type, Name, SeqNo, Start_Object_ID, End_Object_ID, PtStartX, PtStartY, PtEndX, PtEndY, PDATA1, PDATA2, PDATA3, PDATA4, PDATA5, DiagramID, Stereotype, StyleEx, Btm_Mid_Label FROM t_connector WHERE DiagramID=69"));
var st = new ActiveXObject("ADODB.Stream"); st.Type=2; st.Charset="utf-8"; st.Open(); st.WriteText(buf.join("\r\n")); st.SaveToFile(OUT, 2); st.Close();
Session.Output("INSPECT2 OK " + buf.length);
