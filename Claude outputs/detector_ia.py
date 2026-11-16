# -*- coding: utf-8 -*-
"""Detector heurístico de estilo 'IA' para textos en español (docx). No es un veredicto: marca patrones."""
import re, sys, statistics, html
from docx import Document
W='{http://schemas.openxmlformats.org/wordprocessingml/2006/main}'

# frases/muletillas típicas de texto generado en español (peso)
FRASES = {
 r'\ben definitiva\b':3, r'\bcabe destacar\b':3, r'\bcabe mencionar\b':3, r'\basimismo\b':2, r'\ben este sentido\b':2,
 r'\bno solo\b.{0,60}\bsino también\b':3, r'\bde manera (eficiente|coherente|integral|ordenada|efectiva|segura)\b':2,
 r'\bgarantiz\w+':2, r'\boptimiz\w+':2, r'\bintegral\b':1, r'\bsignificativ\w+':2, r'\bfortalec\w+':2,
 r'\bconsolid\w+':2, r'\bexcelencia\b':2, r'\breferente\b':2, r'\boportunidad de crecimiento\b':3,
 r'\btoma de decisiones\b':1, r'\bdecisiones estratégicas\b':2, r'\bplanificación estratégica\b':2,
 r'\bamigable\b':1, r'\bintuitiv\w+':1, r'\brobust\w+':2, r'\bsinergi\w+':3, r'\bclave\b(?! (maestra|de|del|contra|en))':0,
 r'\bpermitiendo\b':1, r'\basegurando\b':2, r'\bpromoviendo\b':1, r'\bfacilitando\b':1, r'\bmejorando\b':1,
 r'\bnúcleo\b':2, r'\beliminando la dependencia\b':3, r'\ben tiempo real\b':1, r'\bvisión integral\b':3,
 r'\binherentes?\b':2, r'\bcrucial\w*\b':2, r'\bfundamental\w*\b':1, r'\bpotenci\w+':1, r'\bpanorama\b':2,
 r'\bexperiencia del (cliente|usuario)\b':2, r'\bde calidad\b':1, r'\bdatos confiables\b':1, r'\btransform\w+':2,
 r'\bademás,':1, r'\bpor otro lado\b':1, r'\bpor último\b':1, r'\ben resumen\b':3, r'\ben conclusión\b':3,
}
GER = re.compile(r'\b\w+(ando|iendo|yendo)\b', re.I)
TRIADA = re.compile(r'\b[\wáéíóúñ ]{3,40}, [\wáéíóúñ ]{3,40},? y [\wáéíóúñ ]{3,40}\b', re.I)
# errores/rasgos humanos (restan): tildes faltantes típicas, dobles espacios, minúscula tras punto
HUMANO = [r'\besta registrado\b', r'\bdiagnostico\b', r'\bque insumos\b', r'\bcuales son\b', r'  ', r'\. [a-záéíóú]']

def oraciones(t):
    return [s for s in re.split(r'(?<=[.!?])\s+', t) if len(s.split()) > 2]

def analizar(t):
    hits = []
    score = 0.0
    for pat, w in FRASES.items():
        for m in re.finditer(pat, t, re.I):
            if w:
                hits.append((m.start(), m.end(), 'frase')); score += w
    ger = list(GER.finditer(t))
    for m in ger:
        if m.group(0).lower() not in ('cuando',):
            hits.append((m.start(), m.end(), 'ger'))
    words = len(t.split())
    score += max(0, len(ger) - 1) * 0.8
    tri = [m for m in TRIADA.finditer(t) if not re.search(r'\d|CU[NS]', m.group(0))]
    score += len(tri) * 1.0
    sl = [len(s.split()) for s in oraciones(t)]
    if len(sl) >= 3:
        cv = statistics.pstdev(sl) / statistics.mean(sl)
        if cv < 0.25: score += 2       # oraciones de largo muy parejo
    if sl and statistics.mean(sl) > 28: score += 1.5
    hum = sum(len(re.findall(p, t)) for p in HUMANO)
    score -= hum * 1.5
    dens = score / max(words, 40) * 100      # puntos cada 100 palabras
    return dens, score, hits, sl

def resaltar(t, hits):
    hits = sorted(set(hits))
    out, i = [], 0
    for a, b, k in hits:
        if a < i: continue
        out.append(html.escape(t[i:a]))
        out.append('<mark class="%s">%s</mark>' % (k, html.escape(t[a:b])))
        i = b
    out.append(html.escape(t[i:]))
    return ''.join(out)

def extraer(path):
    d = Document(path)
    sec, res = 'Inicio', []
    for el in d.element.body.iter(W + 'p'):
        t = ''.join(x.text or '' for x in el.iter(W + 't')).strip()
        if not t: continue
        st = el.find(W + 'pPr/' + W + 'pStyle')
        st = st.get(W + 'val') if st is not None else ''
        if st.startswith('Heading') or st.startswith('Ttulo'):
            sec = t; continue
        intbl = any(a.tag == W + 'tc' for a in el.iterancestors())
        res.append((sec, intbl, t))
    return res

if __name__ == '__main__':
    src, dst = sys.argv[1], sys.argv[2]
    paras = extraer(src)
    rows, secs = [], {}
    for sec, intbl, t in paras:
        if len(t.split()) < 12 or sec == 'Inicio': continue
        dens, sc, hits, sl = analizar(t)
        rows.append((sec, intbl, t, dens, sc, hits))
        s = secs.setdefault(sec, [0, 0]); s[0] += sc; s[1] += len(t.split())
    tot_w = sum(len(r[2].split()) for r in rows)
    tot = sum(max(r[4], 0) for r in rows) / tot_w * 100
    def nivel(d): return 'alto' if d >= 6 else ('medio' if d >= 3 else 'bajo')
    top = sorted(rows, key=lambda r: -r[3])
    alto = [r for r in rows if nivel(r[3]) == 'alto']
    medio = [r for r in rows if nivel(r[3]) == 'medio']
    H = ['<!doctype html><html lang="es"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>Chequeo de estilo IA</title><style>',
         ':root{--bg:#fff;--fg:#1f2328;--mut:#5a6270;--card:#f6f8fa;--bd:#d0d7de;--hi:#ffd8a8;--ge:#d0ebff;--al:#c92a2a;--me:#e67700;--ba:#2b8a3e}',
         '@media (prefers-color-scheme:dark){:root{--bg:#161b22;--fg:#e6edf3;--mut:#9aa4b1;--card:#1f2630;--bd:#30363d;--hi:#7a4b12;--ge:#1c3d5a}}',
         'body{background:var(--bg);color:var(--fg);font:15px/1.55 system-ui,sans-serif;max-width:900px;margin:auto;padding:16px}',
         'h1{font-size:22px}h2{font-size:17px;margin-top:28px}.mut{color:var(--mut)}.card{background:var(--card);border:1px solid var(--bd);border-radius:8px;padding:12px 14px;margin:10px 0}',
         'mark.frase{background:var(--hi);color:inherit;border-radius:3px}mark.ger{background:var(--ge);color:inherit;border-radius:3px}',
         '.tag{font-size:12px;font-weight:600;padding:2px 8px;border-radius:10px;color:#fff}.alto{background:var(--al)}.medio{background:var(--me)}.bajo{background:var(--ba)}',
         'table{border-collapse:collapse;width:100%}td,th{border-bottom:1px solid var(--bd);padding:6px;text-align:left;font-size:14px}</style></head><body>']
    H.append('<h1>Chequeo de estilo "IA" — %s</h1>' % html.escape(src.split('/')[-1]))
    H.append('<p class="mut">Heurístico: marca muletillas, gerundios encadenados, enumeraciones de a tres y oraciones de largo muy parejo. '
             'No prueba que un texto sea de IA; sirve para decidir qué reescribir con tus palabras.</p>')
    H.append('<div class="card"><b>Índice global:</b> %.1f puntos cada 100 palabras (%d párrafos analizados, %d palabras). '
             '<span class="tag alto">alto</span> %d &nbsp; <span class="tag medio">medio</span> %d</div>' % (tot, len(rows), tot_w, len(alto), len(medio)))
    H.append('<p><mark class="frase">naranja</mark> = frase típica de texto generado · <mark class="ger">celeste</mark> = gerundio</p>')
    H.append('<h2>Secciones con más marcas</h2><table><tr><th>Sección</th><th>Palabras</th><th>Índice</th></tr>')
    for s, (sc, w) in sorted(secs.items(), key=lambda kv: -kv[1][0] / max(kv[1][1], 1))[:15]:
        if w >= 40:
            H.append('<tr><td>%s</td><td>%d</td><td>%.1f</td></tr>' % (html.escape(s), w, max(sc, 0) / w * 100))
    H.append('</table><h2>Párrafos a revisar (ordenados por índice)</h2>')
    for sec, intbl, t, dens, sc, hits in top:
        if nivel(dens) == 'bajo': break
        H.append('<div class="card"><span class="tag %s">%s %.1f</span> <span class="mut">%s%s</span><p>%s</p></div>' % (
            nivel(dens), nivel(dens), dens, html.escape(sec), ' · tabla' if intbl else '', resaltar(t, hits)))
    H.append('</body></html>')
    open(dst, 'w', encoding='utf-8').write('\n'.join(H))
    print('global %.1f | alto %d | medio %d | parrafos %d' % (tot, len(alto), len(medio), len(rows)))
    for r in top[:25]:
        print('%.1f\t%s\t%s' % (r[3], r[0][:40], r[2][:110]))
