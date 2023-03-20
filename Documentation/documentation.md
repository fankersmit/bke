# Boter, Kaas en eieren

1. Twee spelers.
2. Bord van 3 X 3 velden.
3. speler kiest X of 0.  
3. Omstebeurt zet speler 0 of X in een veld.
4. Een zet kan niet gewijzigd worden.
5. Op een gevuld veld kan niet gespeeld worden.
5. De eerste speler met 3 X of 3 0, horizontaal, verticaal of diagonaal wint.
6. ALs geen van de spelers 3 op een rij krijgt, is het gelijk spel.   

## Spelbegrippen
 - Speler
 - Zet
 - Kruisje
 - Nulletje
 - Bord
 - Rij 
 - Kolom
 - Diagonaal
 - Winst
 - Gelijk spel of Onbeslist

## Regels
1. spelers spelen omstebeurt, twee keer zelfde speler mag niet.
2. een zet is definitief, tweemaal op zelfde veld spelen mag niet.
3. een zet is definitief, een zet terugnemen mag niet.
4. Het spel eindigt met een winnaar bij drie op een rij, kolom of diagonaal.
5. Het spel eindigt onbeslist als alle velden gvuld zijn zonder 3 op een rij, kolom of diagonaal.
6. Na elk spel wisselt beginnende speler.

## Analyse
1. Er zijn minimaal 5 zetten nodig om een mogelijke winnaar te bepalen
2. Na 9 zetten is het spel altijd geeindigd.
3. Of eerst een 0 of een X  wordt gezet is niet relevant voor het spel. 
4. De volgorde waarin rij, kolom of diagonaal gevuld wordt, is niet  relevant voor het bepalen van de winnaar.
5. het spel kent een winnaar, of eindigt onbeslist

## Initiele Ontwerpkeuzes
1. het speelveld is een 2D array van int
2. geldige invoer is 0 (ZERO) of 1 (CROSS)
3. Het maakt niet uit wie begint.
4. Bij het niet volgen van een regel wordt exceptie opgegooid.
5. Eindgebruiker wordt iet lastig gevallen met exceptie.
6. Om een winnaar te bepalen kijken we alleen naar kolom, rij of diagonaal bij laatst gespeelde zet
7. Keuze 6 wordt niet geoptimaliseerd.
8. Winnende rij heeft altijd waarde `0` of `3`
8. Na elke zet bepalen we of we een winnaar kunnen aanwijzen
9. Een winnaar wordt aangewezen door te valideren of en  een reeks van 3 X of 0 is.
10. Spel spelen wordt gecodeerd in console app.
11. Spel en spelregels uitvoeren wordt gedoceerd in library. 

# Nieuwe Wensen na eerste oplevering
1. Naast standaard bord 3X3 ook andere maten zoals 4X5 of 6X6.
2. Regels blijven hetzelfde: winst is 3 aansluitende zelfde waarden van ZERO of CROSS 
2. Minmale maat is 3X3, maximale maat is 8X8
3. Bij meerdere spelletjes winnaars bijhouden: wie heeft de meeste gewonnen?

## Analyse
1. Drie aansluitende velden kan nu overal gebeuren, is geen complete rij, kolomn of diagonaal meer.
2. Zet zelf moet gebruikt worden om de winnaar te bepalen.
3. Bord krijgt meer eigenschappen en complexiteit.
4. Weergeven borden zetten wordt complexer, console nog de juiste keuze?. 
5. Resultaat van  spel en spelers bewaren en opvragen.
6. Meer logica moet van app naar lib, specifiej, speelrondes. 
7. We hoeven pas een winnaar te zoeken vanaf de vijfde zet. 

## Ontwerpkeuzes
1. Bord word zelfstanding en in spel opgevoerd (DI).
2. Zet wordt zelfstandige readonly struct (Immutable).  
2. Spelers kunnen voor een spel begint bord grootte opgeven.
3. Voor nu blijven we bij console.
4. Voor het bepalen van de winnende zet kunnen we hetzelfde algoritme gebruiken, met de volgende aanpassing: 
   - tel tot 3 opeenvolgende zelfde zetten.
   - stop niet als binnen 3 een andere zet gevonden wordt maar begin opnieuw.
   - stop als einde van rij of kolom bereikt wordt
   - het is nog steeds niet interessant om te optimaliseren, maar we gaan wel meten!
   - voor een diagonaal bepalen we eerst startrij en sartkolom.

# Gewenste volgende features
1. Spelers worden toegeveogd.
2. Spellen en Spelers worden opgeslagen en kunnen wordenop gevraagd.
3. Velden op bord zijn nu alleen data, geef ze gedrag: er is een zet op mij gedaan
4. versimpel main prograam loop, events vanuit zet is een optie.
