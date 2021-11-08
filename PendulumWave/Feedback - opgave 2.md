
## Feedback Applied Programming

### Opgave 2: Pendulum Wave

#### Algemeen

#### Architectuur (15%)

***Modulair, meerlagenmodel***

- [x] *Meerlagenmodel via mappen of klassebibliotheken*
- [x] *Dependency injection*
- [x] *Gebruik  MVVM Design pattern*

***'Separation of concern'***

- [x] *Domein-logica beperkt tot logische laag*
- [x] *Logische laag onafhankelijk van presentatielaag*

Gebruik waar mogelijk MVVM patroon met databinding en commands in MainWindow.
Dit ter vervanging van de code behind waar je rechtstreeks methoden van je view model oproept

bv. ValueChanged="Slider_ValueChanged"


Je simulatielus wordt gestuurd vanuit je presenatielaag. Je laat de simulatie beter autonoom in het model draaien.

#### Programmeerstijl, Kwaliteit van de code (10%)


***Naamgeving***

- [x] *Naamgeving volgens C# conventie*
- [x] *Zinvolle, duidelijke namen*

***Korte methodes***

- [x] *maximale lengte ~20 lijnen*

***Programmeerstijl***

- [x] *Layout code*
- [x] *Zinvolle, duidelijke namen*
- [x] *Correct gebruik commentaar*
- [x] *Algemene programmeerstijl*


#### User interface, functionaliteit, UX (15%) 

***Ergonomie***

- [x] *Layout UI*
- [x] *estetische weergave* 
- [x] *Goede UX*

***functionaliteit***

- [x] *Goede weergave view met controllerbare camera*
- [x] *Goede weergave 'Bottom' view*
- [x] *Weergave numerieke resultaten*
- [x] *Instelbaar aantal slingers*
- [x] *Instelbare kleurenweergave*
- [x] *Start, Pauze, Reset*

Toepassing start onmiddellijk met zijn simulatie
Kleuren kunnen verzet worden tijdens de simulatie, alsook tijdens pauze van een simulatie
Geen instelbare draaihoek voorzien


#### Goede werking, snelheid, bugs (25%)


***juiste technieken gebruikt***

- [ ] *Correcte simulatie slinger (o.a. formules)*
- [ ] *Correcte berekening lengte van de slingers (o.a. formule)*
- [ ] *Maximale simulatiefrekwentie*
- [ ] *Realistische renderfrekwentie*

***Juiste werking***

- [ ] *Goede werking*

***Snelheid, efficiëntie, concurrency***

- [ ] *Zinvol gebruik concurrency*
- [ ] *Efficiënte berekeningen*

***Bugs***

- [ ] *Geen bugs*

Simulatie loopt zichtbaar traag. Verhoog het aantal oscillaties per seconde.
Na sinusflow, chaos ... duurt te lang eer bollen weer in één lijn komen
Berekening lengte rope ok
renderloop gestuurd via CompositionTarget.Rendering
foutje in formule berekening nieuwe hoek? Controleer bepaling hoekversnelling (omrekening naar radialen ook terug zetten)

geen instelbaar aantal oscillaties per minuut

#### Installeerbare package voor distributie (10%)

- [x] *Installable package beschikbaar in repo*

package met certificaat beschikbaar
ook gepast icoontje opgenomen

#### Correct gebruik GIT (10%)

- [x] *Gebruik 'atomaire' commits*
- [x] *zinvolle commit messages*

ok

#### Rapportering (15%)

- [ ] *Structuur*
- [ ] *Volledigheid*
- [ ] *Technische diepgang*
- [ ] *Professionele stijl*

Zeer summier
korte oplijsting van de model-klassen en waarvoor ze staan
korte toelichting van de 10 private MVM methoden

Wel kort idee van berekening lengte pendulum vermeld, alsook vermelding CompositionTarget.Rendering
Kortom: Leest niet als een rapport, maar kadert wel (zeer) kort de betekenis van de klassen & methoden