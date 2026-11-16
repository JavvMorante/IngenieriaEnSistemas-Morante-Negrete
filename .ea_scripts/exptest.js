var P="C:\\Users\\jmora\\Desktop\\RepoFacultad\\IngenieriaEnSistemas-Morante-Negrete\\.ea_scripts\\";
var pi=Repository.GetProjectInterface();
var ids=[69,70,71,68];
for (var i=0;i<ids.length;i++){ var d=Repository.GetDiagramByID(ids[i]); var ok=pi.PutDiagramImageToFile(d.DiagramGUID, P+"test_"+ids[i]+".png", 1); Session.Output("exp "+ids[i]+" "+ok); }
