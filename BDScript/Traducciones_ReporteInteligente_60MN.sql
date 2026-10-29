/*
    VetCare - Traducciones del Reporte Inteligente (Reporte 2)
    ----------------------------------------------------------
    Script de actualización para una base de datos ya creada. Agrega al idioma
    inglés (IdiomaID = 2) las claves que usa FrmReporteInteligente_60MN.
    Las claves genéricas (btnGenerar, btnImprimir, btnExportar, btnCerrar,
    lblDesde, lblHasta) ya existen y las reutiliza el traductor.

    El español es el idioma base (EsBase = 1) y no lleva filas en Traduccion:
    si una clave no está traducida, el formulario muestra el texto del diseñador.
*/
USE VetCareBD_60MN;
GO

DECLARE @Nuevas TABLE (Clave VARCHAR(100), Texto NVARCHAR(400));

INSERT INTO @Nuevas (Clave, Texto) VALUES
('FrmReporteInteligente_60MN',             N'Smart report'),
('FrmReporteInteligente_60MN.grpFiltros',  N'Period'),
('lblTop',                                 N'Products in the ranking'),
('msg.DemandaPorCategoria',                N'Supply demand by month and category (units issued)'),
('msg.TopProductosEntregados',             N'Most delivered products in consultations'),
('msg.GenerarParaVer',                     N'Choose a period and press Generate.'),
('msg.SinMovimientos',                     N'No stock movements found in the selected period.');

-- Alta de las que faltan y actualización de las que ya estaban.
INSERT INTO Traduccion (IdiomaID, Clave, Texto)
SELECT 2, n.Clave, n.Texto
  FROM @Nuevas n
 WHERE NOT EXISTS (SELECT 1 FROM Traduccion t WHERE t.IdiomaID = 2 AND t.Clave = n.Clave);

UPDATE t
   SET t.Texto = n.Texto
  FROM Traduccion t
  JOIN @Nuevas n ON n.Clave = t.Clave
 WHERE t.IdiomaID = 2;

SELECT Clave, Texto FROM Traduccion WHERE IdiomaID = 2 AND Clave IN (SELECT Clave FROM @Nuevas);
GO
