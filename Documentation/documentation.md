# Boter, Kaas en eieren

1. Twee spelers.
2. Bord van 3 X 3 velden.
3. speler kiest X of 0.  
3. Omstebeurt zet speler 0 of X in een veld.
4. Een zet kan niet gewijzigd worden.
5. Op een gevuld veld kan niet gespeeld worden.
5. De eerste speler met 3 X of 3 0, horizontaal, verticaal of diagonaal wint.
6. ALs geen van de spelers 3 op een rij krijgt, is het gelijk spel.   

# Spelbegrippen
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

# Regels
1. spelers spelen omstebeurt, twee keer zelfde speler mag niet.
2. een zet is definitief, tweemaal op zelfde veld spelen mag niet.
3. een zet is definitief, een zet terugnemen mag niet.
4. Het spel eindigt met een winnaar bij drie op een rij, kolom of diagonaal.
5. Het spel eindigt onbeslist als alle velden gvuld zijn zonder 3 op een rij, kolom of diagonaal.
6. Na elk spel wisselt beginnende speler.

# Analyse
1. Er zijn minimaal 5 zetten nodig om een mogelijke winnaar te bepalen
2. Na 9 zetten is het spel altijd geeindigd.
3. Of eerst een 0 of een X  wordt gezet is niet relevant voor het spel. 
4. De volgorde waarin rij, kolom of diagonaal gevuld wordt, is niet  relevant voor het bepalen van de winnaar.
5. het spel kent een winnaar, of eindigt onbeslist

# Ontwerpkeuzes
1. het speelveld is een 2D array van int
2. geldige invoer is 0 (ZERO) of 1 (CROSS)
3. Het maakt niet uit wie begint.
4. Bij het niet volgen van een regel wordt exceptie opgegooid. 
5. Na elke zet wordt bepaald of er een winnaar is. 
6. Om een winnaar te bepalen kijken we alleen naar kolom, rij of diagonaal bij laatst gespeelde zet
7. Winnende rij heeft altijd waarde `0` of `3`
8. Na elke zet bepalen we of we een winnar kunnen aanwijzen
9. Een winnaar wordt aangewezen door te valideren of en  een reeks van 3 X of 0 is.
10. Spel spelen wordt gecodeerd in console app.
11. Spelregels uitvoeren wordt gedoceerd in library.
12. 
