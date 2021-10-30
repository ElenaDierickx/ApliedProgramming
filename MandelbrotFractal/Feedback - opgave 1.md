Aangepast: Package toegevoegd

## Feedback Applied Programming

### Opgave 1: Mandelbrot fractaal

#### Algemeen

#### Architectuur (15%)

***Modulair, meerlagenmodel***

- [x] *Meerlagenmodel via mappen of klassebibliotheken*
- [x] *Dependency injection*
- [x] *Gebruik  MVVM Design pattern*

***'Separation of concern'***

- [x] *Domein-logica beperkt tot logische laag*
- [ ] *Logische laag onafhankelijk van presentatielaag*

> Je logica bevat aspecten van de presentatielaag (berekening van kleuren).


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

> Verzamel je datavelden en properties vóór de andere methoden. 
> Momenteel zitten er verspreid tussen andere methoden (zoomFactor, tokenSource)

#### User interface, functionaliteit, UX (15%) 

***Ergonomie***

- [x] *Layout UI*
- [x] *Goede UX*

***functionaliteit***

- [x] *Goede weergave fractaal*
- [x] *Weergave numerieke resultaten*
- [x] *Zooming*
- [x] *Panning*
- [x] *Aanpasbare iteratielimiet*
- [x] *instelbare kleurenweergave*

> weergegeven corner is X-coord van linksboven en Y-coord van rechtsonder?
> Voor X is dat in overeenstemming met de aangegeven cursorpositie, voor Y niet...

#### Goede werking, snelheid, bugs (25%)

***juiste technieken gebruikt***9

- [x] *Juiste berekening fractaal*
- [x] *Zooming & Panning goed verwerkt*

***Juiste werking***

- [x] *Goede werking*

***Snelheid, efficiëntie, concurrency***

- [x] *Goed gebruik concurrency*
- [x] *Efficiënte berekeningen*

***Bugs***

- [x] *Geen bugs*

> soms een traagheid zichtbaar als lijntjes bij snel in- of uitzoomen

#### Installeerbare package voor distributie (10%)

- [ ] *Installable package beschikbaar in repo*

> wel een packaging project, maar geen MSIX bundle (met bijhorend certificaat)

#### Correct gebruik GIT (10%)

- [x] *Gebruik 'atomaire' commits*
- [x] *zinvolle commit messages*

> Gebruik iets meer dan 1 woord in je commit. Welke actie heb je op 'XXX' ondernomen?

#### Rapportering (15%)

- [x] *Structuur*
- [x] *Volledigheid*
- [x] *Technische diepgang*
- [x] *Professionele stijl*

> Je documentatie is een oplijsting van de functies in de verschillende bestanden.
> Bijkomende uitleg over de onderlinge samenhang welkom
