Template:

-------------------------------------------------------------------------------------------------
- (34): Titel
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Das ist eine Beschreibung
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
   ___                    _                         
  /___\_ __   ___ _ __   (_)___ ___ _   _  ___  ___ 
 //  // '_ \ / _ \ '_ \  | / __/ __| | | |/ _ \/ __|
/ \_//| |_) |  __/ | | | | \__ \__ \ |_| |  __/\__ \
\___/ | .__/ \___|_| |_| |_|___/___/\__,_|\___||___/
      |_|                                           
-------------------------------------------------------------------------------------------------


-------------------------------------------------------------------------------------------------
- (1): Wir brauchen ein MergeXDL Kommando. 
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Das Problem, dass ich mit dem CLK beim partiellen Konfigurieren hatte entstand dadurch,
dass die Konfiguration für die BUFG clock Treiber nicht in seperaten Konfigurationsframes
geschrieben wird, sondern in die CLB Logik gequetscht wurde.
Bei meinem Minisystem auf dem LX16 ist die Konfiguration logisch in der Kachel CLKC_X11Y31
abgelegt (Mach mal Tile Filter "CLKC"). Die Konfigurationsdaten liegen aber an der Stelle
INT_X11Y32.
Also nutze ich AddToSelectionXY UpperLeftX=30 UpperLeftY=36 LowerRightX=30 LowerRightY=36;
CutOffFromDesign XDLInFile=top_routed.xdl XDLOutFile=top_clk_cut.xdl KeepSelection=True RemoveModules=True;
um die Konfiguration der BUFGs aus dem Static top auszuschneiden.
Dieses muss dann ins Partielle Modul rein.
Im Grunde würde ein Konkartinieren reichen, wenn da nicht das CLK25 Netz wäre,
welches es im Partial und Static gibt (und in der aus dem Static rausgeschnittenen Kachel).
Ich brauche also so etwas wie
MergeXDL InFile1=partial_cut.xdl InFile2=top_clk_cut.xdl OutFile=...
welches etwas intelligent mergen kann:
Instanzen konkatinieren (eventuell so Sortieren, dass instanzen vor den Netzen kommen,
was wir aktuell nicht brauchen, da die Eingabe XDLs schon sortiert sind)
Netze konkatinieren, wobei Netze mit gleichem Bezeichner fusioniert werden müssen.
Das ist im Moment nur bei Clock-Netzen erforderlich und deshalb einfach, generell
kann ein Netzbezeichner aber in Prefix haben.
Bei meinen Copy&Paste Versuchen schien es, dass die Reihenfolge von inpin, outpin und
pip statements bei den Netzen egal ist.
Ich weiß nicht was der Parser kann aber man könnte noch sinnige Sachen einbauen dass
Instanzen mehrfach vorkommen dürfen, wenn sie die gleiche Konfiguration haben
(sonst ist es meistens ein Fehler), wobei es bei den Namen wegen unterschiedlicher Prefixe
krachen kann und ich würde pragmatisch sagen dass der Bezeichner des 1. Files
quasi per suchen&ersetzen im 2. File übernommen wird.
BEi einem quickshot war auch mehrfach instanzen möglich, wenn die unterschiedliche
Namen haben... 
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (4): Umgebungsvariblen gesetzt?
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Man könnte eine Warnung beim Programmstart und bei allen Kommandos die mit Xilinx zusammen arbeiten 
(Launch FPGA Editor, Partgen) wenn die Xilinx Umgebung (XILINX= und %PATH% hat was mit Xilinx) nicht gesetzt ist. 
Update: Die Prüfung sollte auch in der GUI wiederholt werden
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (5): Das Blocken dauert böse lang;
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Das Blocken dauert böse lang; ist noch OK bei unseren Hello World Beispielen, taugt aber nicht
für die Sachen die ich mit der Maxeler HW vor Augen habe.
Etwas verwunderlich, warum es so lange dauert: wenn wir 1M Taktzyklen pro Kachel brauchen sollte
es pro 1000 Kacheln in einer Sek. gehen.
Bei vielen Kacheln könnte man an sich Copy/Paste machen, bzw wir spielen einfach noch etwas mehr
mit dem Trick rum, den wir für BRAM/MUL angewendet haben.
An sich kann man ja mit der aktuellen Version die Templates bauen, diese dann einfach nutzen
wenn der Hashwert passt und alles andere lässt man laufen wie gehabt.
Man müsste sich dann aber noch überlegen, wie man mit Macros oder MarkPortsInSelectionAsUsed
umgeht (inverses grep, eigenes Template oder algorithmisch wie gehabt)
Ist mal was für ne ruhige Minute. 
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (6): Blank_nach_komma.zip 
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Update: Zeile in Bat-Datei löschen und testen

Es wird immer noch irgendwie ein Blank (" ") eingebaut.
Nach meinem schon verwendeten:
cat %1_blocked.xdl | grep -v dummy.*CLK | sed 's/PAD, /PAD,/g' > tmp.xdl
Bugfix habe ich noch das Folgende eingebaut
cat %1_cut.xdl | sed 's/, /,/g' > tmp.xdl
Sprich beim Rausschreiben des ausgeschnittenen Bereichs kommt manchmal ein Blank nach dem ","
In dem Archiv gibt es eine XDL Eingabedatei und das cut_out GOA Script. DAs Resultat lässt
sich nicht zurüch nach NCD konvertieren.
Noch ein Tip: Die Zeilennummern in der XDL Ausgabe sind etwas Schätzwerte!
Das Konvertieren stört sich an der letzten Zeile in den INST Statements:
_INST_PROP::XDL_SHAPE_MEMBER:Shape_0:0, 1 "
sollte/muss
_INST_PROP::XDL_SHAPE_MEMBER:Shape_0:0,1 "
sein. 
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (14): Output Window
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Bei dem Output Window: <Ctrl> A um alles selektieren zu können (<Ctrl> geht im Moment
  immer an die Tileview. Dann wäre eine clear funktion nett (Auch als Kommando ClearOutput)
  Nicht wichtig, aber unter Windows würde man in einer Textbox ein Kontextmenu mit Copy erwarten
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (19): UCF Area Constraints
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
UCF Area Constraints sind sinnvoll, gehen aber nicht mit BRAM/DSP48 - sollte ähnlich wie bei den Prohibits gehen, muss ich aber mal nen Blick drauf werfen...
Update BRAMSITE MACSITE sind noch hardkoidiert. Wie sollen Clock-Tiles behandelt werden?
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (21): Sortieren
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Dann weiß ich nicht was Du beim Sortieren machst?!
Ich kann nicht zweimal nacheinander das Sortieren aufrufen.
Fehlermeldung:
 An item with the same key has already been added.
Keine Ahnung was Du treibst aber Hinzuzufügen ist nichts.
Im Grunde ist doch nichts anderes zu tun als mehrmals über die Datei zu rumpeln und nach den 
verschiedenen Objekten suchen. OK ist ein Multi-Line Grep aber man muss sich  doch nur an die 
Gramatik von XDL halten
Wenn das alles zu kompliziert ist, schneid einfach beim MergeBlockerAndConnectClock Kommando
das Netz mit dem spezifizierten BUFG treiber aus und häng es hinten an.
Genau das mache ich jetzt manuel.
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (22): CLK-Blocking-Bug bei BRAMs:
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
1)
cat StaticBlocker.xdl | grep -v -e "INT_BRAM.*GCLK[0|1].-" > tmpblocker.xdl 
blockiert immer noch sprich die geblockten PIPs:
        pip INT_BRAM_X21Y24 GCLK1 -> GFAN1, # added by BlockRAMInterconnectsWithArcsOnly
        pip INT_BRAM_X21Y23 GCLK0 -> GFAN0, # added by BlockRAMInterconnectsWithArcsOnly
		...
Haben das BRAM CLK routing nicht unterbunden!

2)
cat StaticBlocker.xdl |  grep -v -e "pip.*INT_BRAM.*CLK.*BRK" > tmpblocker.xdl 
blockiert immer noch sprich die geblockten PIPs:
        pip INT_BRAM_BRK_X3Y16 GCLK0_BRK -> GFAN0, # added by BlockRAMInterconnectsWithArcsOnly
        pip INT_BRAM_BRK_X3Y16 GCLK10_BRK -> SR0, # added by BlockRAMInterconnectsWithArcsOnly
        pip INT_BRAM_BRK_X3Y16 GCLK11_BRK -> SR1, # added by BlockRAMInterconnectsWithArcsOnly
		...
Haben das BRAM CLK routing nicht unterbunden!
3)
ABER 1) und 2) pips gemeinsam rausgeschmissen erlauben das Routing!
Als workaround ist das Folgende in das static.bat script eingefügt worden:
cat staticblocker.xdl | grep -v "pip.INT_BRAM.*GCLK" > tmp_GOA_staticblocker.xdl
Aber komisch lässt man ihn ohne Blocker-PipStatements routen, so gibt es 
keine Einträge mit INT_BRAM_BRK*...
siehe auch die DAtei clk25.xdl
4)
Warum auch immer, aber 
pip INT_BRAM_X21Y17 LOGICIN_B43 -> CLK1, # added by BlockRAMInterconnectsWithArcsOnly
verhindert das CLK routing nicht! LOGICIN_B43 is als stopover nutzbar...
5)
Wenn man das design OHNE Blocker routet und sich das clk25 Nets anschaut, findet man:
  pip INT_BRAM_X21Y17 GCLK1 -> CLK0 , 
  pip INT_BRAM_X21Y17 GCLK1 -> CLK1 , 
  pip INT_BRAM_X21Y21 GCLK1 -> CLK0 , 
  pip INT_BRAM_X21Y21 GCLK1 -> CLK1 , 
  pip INT_BRAM_X21Y25 GCLK1 -> CLK0 , 
  pip INT_BRAM_X21Y25 GCLK1 -> CLK1 , 
  pip INT_BRAM_X21Y29 GCLK1 -> CLK0 , 
  pip INT_BRAM_X21Y29 GCLK1 -> CLK1 , 
  pip INT_BRAM_X3Y17 GCLK1 -> CLK0 , 
  pip INT_BRAM_X3Y17 GCLK1 -> CLK1 , 
  pip INT_BRAM_X3Y21 GCLK1 -> CLK0 , 
  pip INT_BRAM_X3Y21 GCLK1 -> CLK1 , 
  pip INT_BRAM_X3Y25 GCLK1 -> CLK0 , 
  pip INT_BRAM_X3Y25 GCLK1 -> CLK1 , 
  pip INT_BRAM_X3Y29 GCLK1 -> CLK0 , 
  pip INT_BRAM_X3Y29 GCLK1 -> CLK1 , 
(Achtung anschluss in der zweiten Zeile eines BRAM Blocks.)
Workaround (in static.bat):
cat %2 | grep -v "pip.INT_BRAM.*GCLK" > tmp_GOA_%2
@ECHO MergeBlockerAndConnectClock XDLInFile=%1_blocked.xdl XDLOutFile=%1_blocked.xdl BUFGInstanceName=NULL XDLBlockerFiles=tmp_GOA_%2;  >> append_blocker.goa
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (23): AddSingleMacroInstantiationByTile
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Bei AddSingleMacroInstantiationByTile muss man noch das Slice angeben.
Die Idee war das Placement altbekannt via Slice-Koordinaten oder neu via Tile-Koordinaten vorgeben zu können,
was auch funktioniert. Nur ist die Angabe des Slices bei AddSingleMacroInstantiationByTile doppelt gemoppelt
und macht das Ganze nur unnütz kompliziert.
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (26): FileDLG in PrintProhibtStatement
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
FileDLG soll den Paramter FileName setzen
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (27): Leerzeilen zwischen UCF Constrains
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
siehe Titel
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (28): Randwertbehandlung 
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
wir müssen im Blocker um die partielle Fläche herum "einige" Pips vom Blocken ausnehmen. "Einige" 
muss noch präzisiert werden
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (27): Anchor bim Macro-Einlesen 
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Wenn das Macro 
module "BM_S6_L4_R4_single" "left" , cfg "_SYSTEM_MACRO::FALSE" ; 
  port "RH0" "right" "A6";
  port "RH1" "right" "B6";
  port "RH2" "right" "C6";
  port "RH3" "right" "D6";
  port "LH0" "left" "A6";
 eingelesen wird, muss der explizit angegebene Anker verwendet werden
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (28): FileDLG in PrintAreaConstraint
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
FileDLG soll den Paramter FileName setzen
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (29): FileDLG in PrintLocationConstraint
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
FileDLG soll den Paramter FileName setzen
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (30): UCF-Area-Constraint
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
INST "reconfig_blue" AREA_GROUP = "pblock_reconfig_blue"; # generated_by_GoAhead
AREA_GROUP "pblock_reconfig_blue" RANGE = SLICE_X2Y48:SLICE_X7Y51; # generated_by_GoAhead
INST "InstMIPS_CPU*" AREA_GROUP = "AG_CPU"; # generated_by_GoAhead
AREA_GROUP "AG_CPU" RANGE = DSP48_X0Y3:DSP48_X0Y5; # generated_by_GoAhead
AREA_GROUP "AG_CPU" RANGE = SLICE_X6Y9:SLICE_X17Y31; # generated_by_GoAhead

Paramter GroupName wird eingespart. Dazu werden Vorkommen von * aus InstanceName entfernt
und GroupName= AG_InstanceName


Diese Zeilen hinzufügen
AREA_GROUP "AG_CPU" GROUP=CLOSED;
AREA_GROUP "AG_CPU" PLACE=CLOSED;
-------------------------------------------------------------------------------------------------
}


-------------------------------------------------------------------------------------------------
- (31): RESET Funktion sollte mehrmaliges Ausführen eines Scripts erlauben
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
"Reset" sollte die Textausgabefenster (VHDL/UCF/Error Trace) löschen (alles bis auf den Command-Trace)

-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (32): Single Char Port Mapping in PrintVHDLMacroInstantiation
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: TODO IsMatch oder Equals?
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Es sieht so aus, als ob der erste Port beim PortMapping String mehr als ein Zeichen haben muss (also mindestens 2).
Sprich, das Folgende tut was es soll, wenn ich aber "dummy" (gibt es nicht im Macro) rauswerfe so fehlt mir das Mapping I:Connect_I
PrintVHDLMacroInstantiation PortMapping=dummy:dummy,I:Connect_I,O:Connect_O,H:1 InstantiationFilter=instMacro Append=True;
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (33): Macro Cross-FPGA-Kompatibilität
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: TODO ist das noch aktuell?
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Macro auf LX16 in der Mitte erzeugt. Tut nicht weil es das ursprüngliche CLEXM_X8Y33 auf nem Kleinen LX9 nicht gibt:
Failed to run command AddLibraryElementFromXDL XDLMacro=C:\WORK\GoAheadMacros\Connect4_S6_double.xdl ParseXDLPortStatements=True;: Location CLEXM_X8Y33 not found
Sprich ist die Überprüfung des Tiles erforderlich?
Aber kein echtes Problem, da man auf nem' großen Baustein das Macro als XDL einlesen kann, dann dieses als binMacro abspeichern kann.
Dieses binMacro kann man dann in die Macro Lib auch auf dem kleinen (jeden?) Baustein einlesen.
Wenn man dann aber später dieses Macro instaziieren möchte, so gibt es eine Fehlermeldung dass es das Referenztile nicht gibt (was auch stimmt)
AddSingleMacroInstantiationBySlice MacroName=Connect4_S6_double InstanceName=instMacroLeft_6 SliceName=SLICE_X15Y8;
Man kann dann zwar trotzdem noch den VHDL-Code erzeugen (das Macro existier dann in den "Macro Instantiations", aber das Malen mag nicht mehr...
Auch ein Verschieben des Start-Slices tat es nicht da das "Schachbrett" auf dem kleinen Baustein (LX9) mit CLEXM anfängt.
Auf dem etwas größeren LX15 geht es mit CLEXL los (jeweils von links).
Hier ist vielleicht noch ein weiterer BUG, da sehr wahrscheinlich CLEXL auch auf CLEXM gehen sollte.
Nebenbei benutzt das Macro lediglich ein CLEXX Slice und sollte überall (an pos Slice-1) tun...

-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (34): PartGenDevice
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
PartGenAll-GUI erweitern um DeviceAuswahl
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (35): ToolTip-Hypertonie
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Der Tooltip kommt immer wieder neu, ohne dass sich die Maus bewegt.
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (36): Ressourcen in Auswahl anzeigen (Slices, BRAMS, etc)
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Die Info kann in die Tile-Info mit rein
Optional werden #CLBs, #SLICES#, #BRAMS sowie #DSPs in der Tile-Info eingeblendet
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (37): ExpandSelectionToClockRegion-Kommando
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (37): Kommentare hinter Befehlen
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Geht aktuell nicht:
SelectAll; # alles markieren 
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (37): Bei der Wiederherstellung der Position eines Fenster muss die Auflösung berücksichtigt werden
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
tritt auf, wenn man von 2 bildschirmen auf einen wechselt
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (38): Im Macro-Manager neben den Add-Button einen Add-And-Select Button setzen
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------


-------------------------------------------------------------------------------------------------
- (39): Neumalen nach Select-All grotten langsam
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------
- (39): PartGen Packages können nicht ausgewhält werden
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Im Menu PartGen All in PartGen umbenennen
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (40): Design-Statistik
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Nach ReadDesignIntoMacro kann man max., durchschnittliche, Pfadlänge in Pips und Anzahl belegter 
Slice mit Kommando rausprinten könne,
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (41): Aus -= wird ->
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Beim Einlesen von Pips kann aus -= der Operator -= werden.
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (42): GUI für ExplicitAnchor
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
In der GUI fehlt bei AddXDLMacro eine Auswahlmöglichkeit für ExplicitAnchor= ExplicitName=
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (43): Reload fuer den Debugger
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------
}


-------------------------------------------------------------------------------------------------
- (44): Place Multi Macros in Selection zaehlt den Instanznamen nicht automatisch hoch
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Geht nicht: Waehle 2x2 aus und platziere Macros, waehle woanders 2x2 aus und platziere Macros
-------------------------------------------------------------------------------------------------
}
-------------------------------------------------------------------------------------------------
- (45): Multi-Selection
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------

AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=row-wise Horizontal=left-to-right Vertical=top-down AddToCurrentlySelectedMacro=False;
foo_0	foo_1
foo_2	foo_3
foo_4	foo_5
foo_6	foo_7

AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=row-wise Horizontal=left-to-right Vertical=bottom-up AddToCurrentlySelectedMacro=False;
foo_6	foo_7
foo_4	foo_5
foo_2	foo_3
foo_0	foo_1

AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=row-wise Horizontal=right-to-left Vertical=top-down AddToCurrentlySelectedMacro=False;
foo_1	foo_0
foo_3	foo_2
foo_5	foo_4
foo_7	foo_6

AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=row-wise Horizontal=right-to-left Vertical=bottom-up AddToCurrentlySelectedMacro=False;
foo_7	foo_6
foo_5	foo_4
foo_3	foo_2
foo_1	foo_0

AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=column-wise Horizontal=left-to-right Vertical=top-down AddToCurrentlySelectedMacro=False;
foo_0	foo_4
foo_1	foo_5
foo_2	foo_6
foo_3	foo_7

The next one bitte DEFAULT
AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=column-wise Horizontal=left-to-right Vertical=bottom-up AddToCurrentlySelectedMacro=False;
foo_3	foo_7
foo_2	foo_6
foo_1	foo_5
foo_0	foo_4

AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=column-wise Horizontal=right-to-left Vertical=top-down AddToCurrentlySelectedMacro=False;
foo_4	foo_0
foo_5	foo_1
foo_6	foo_2
foo_7	foo_3

AddMacroInstantiationInSelectedTiles SliceNumber=1 InstanceName=foo MacroName=Connect4_S6_double Mode=column-wise Horizontal=right-to-left Vertical=bottom-up AddToCurrentlySelectedMacro=False;
foo_7	foo_3
foo_6	foo_2
foo_5	foo_1
foo_4	foo_0
-------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------
- (46): Slicevergleich für DRC über Regeln steuern
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
else if (requiredSliceType.Equals("SLICEX") && targetSliceType.Equals("SLICEX")) { sliceTypesAreCompatible = true; }
else if (requiredSliceType.Equals("SLICEL") && targetSliceType.Equals("SLICEX")) { sliceTypesAreCompatible = false; }
In Kommandos überführen
-------------------------------------------------------------------------------------------------
}


-------------------------------------------------------------------------------------------------
- (47): CutOffFromDesign generiert direkt ein binFPGA
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Bei CutOffFromDesign speichern wir per XDL den Anker mit ab : Der Anker kommt nach oben links.
Wir können aber per Paramter AlternativAnchor den Anker auch selbst setzen.
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (48): Per Parameter "Routen durch LUTs" erlauben
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (49): Quoten von Parametern
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
If Condition=ShowGUI_after_script=True Then="ShowGUI;" Else="NOP;";
[8/28/2013 7:27:13 PM] diko: geht nicht
[8/28/2013 7:27:21 PM] diko: Set Variable=ShowGUI_after_script   Value=True;
[8/28/2013 7:27:41 PM] Christian Beckhoff: bei Condition fehlen die %%
[8/28/2013 7:27:55 PM] Christian Beckhoff: in deinem Skript partial.goa funktioniert der Aufruf
[8/28/2013 7:27:58 PM] diko: selber gfesehn
[8/28/2013 7:28:26 PM] diko: da fehlt noch irgendwie eine Fhlermeldung
[8/28/2013 7:28:40 PM] diko: ist nicht einfach
[8/28/2013 7:29:27 PM] diko: man könnte es vieleicht so machen dass text immer gequotet werden muss ""
[8/28/2013 7:29:43 PM] diko: Dann kannst du erkennen ob es ein symbol oder ein test ist...
[8/28/2013 7:30:47 PM] Christian Beckhoff: Ich könnte auch eine Warning auf die Konsole schreiben, wenn Value=%foo% aufgelöst werden könnte, Value=foo aber nicht
[8/28/2013 7:30:52 PM] Christian Beckhoff: einfacher, oder?
[8/28/2013 7:31:40 PM] diko: ich war beo Apfel=Birne <-> Apfel=%Birne%

-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (50): Logfile in GOAHEAD_HOME
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Command-Trace zusaetzlich in GOAHEAD_HOME\goa.log abspeichern (eher fuer Debug als fuer Production Use)
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (51): Routing-Model erweitern
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Nach dem Einlesen einer (stat) Netzliste verfolge ich alles Netze und ggf. so im Routing-Modell nicht vorhandenen
Einträge "lernen". Sie dazu zum Bespiel das Netz
net "io_data_in<0>",
	outpin "io_data_in<1>" AQ, 
	inpin "iobar2_datain<0><3>" A1, 
	inpin "mergejoin_0/s_left1_out<9>" A4, 
	inpin "mergejoin_0/s_regs_in<0><3>" A1, 
	inpin "mergejoin_0/s_regs_in<1><3>" A4, 
	inpin "mergejoin_0/s_regs_in<2><3>" A1, 
	inpin "mergejoin_0/s_regs_in<3><3>" A6, 
	inpin "mergejoin_0/s_right0_out<1>" D1, 
	pip CLBLL_X46Y79 CLBLL_IMUX_B21 -> CLBLL_LL_A1,
	pip CLBLL_X54Y92 CLBLL_IMUX_B37 -> CLBLL_LL_D1,
	pip CLBLL_X54Y92 CLBLL_IMUX_B7 -> CLBLL_L_A4,
	pip CLBLL_X56Y90 CLBLL_IMUX_B7 -> CLBLL_L_A4,
	pip CLBLM_X55Y121 CLBLM_L_AQ -> CLBLM_LOGIC_OUTS0,
	pip CLBLM_X55Y83 CLBLM_IMUX_B20 -> CLBLM_L_A1,
	pip CLBLM_X55Y92 CLBLM_IMUX_B2 -> CLBLM_L_A6,
	pip CLBLM_X58Y89 CLBLM_IMUX_B20 -> CLBLM_L_A1,
	pip INT_X45Y101 SS2END0 -> SS4BEG0,
	pip INT_X45Y103 SS2END0 -> SS2BEG0,
	pip INT_X45Y105 SS2END0 -> SS2BEG0,
	pip INT_X45Y107 SS4END0 -> SS2BEG0,
	pip INT_X45Y111 SS4END0 -> SS4BEG0,
	pip INT_X45Y115 SS2END0 -> SS4BEG0,
	pip INT_X45Y117 WW4END1 -> SS2BEG0,
	pip INT_X45Y77 LV0 -> NE4BEG0,
	pip INT_X45Y85 LV8 -> EE4BEG1,
	pip INT_X45Y93 SW4END0 -> EE4BEG0,
	pip INT_X45Y93 SW4END0 -> LV16,
	pip INT_X45Y97 SS4END0 -> SE4BEG0,
	pip INT_X46Y79 WL1END2 -> IMUX_B21,
	pip INT_X47Y79 NE4END0 -> NR1BEG0,
	pip INT_X47Y79 SL1END3 -> WL1BEG2,
	pip INT_X53Y111 SW4END0 -> LV16,
	pip INT_X53Y95 LV0 -> SE4BEG0,
	pip INT_X53Y95 SE4END0 -> SW4BEG0,
	pip INT_X54Y92 FAN3 -> FAN_BOUNCE3,
	pip INT_X54Y92 FAN_BOUNCE3 -> IMUX_B37,
	pip INT_X54Y92 WL1END3 -> FAN3,
	pip INT_X54Y92 WL1END3 -> IMUX_B7,
	pip INT_X55Y113 SS4END0 -> SE4BEG0,
	pip INT_X55Y113 SS4END0 -> SW4BEG0,
	pip INT_X55Y117 SS4END0 -> SS4BEG0,
	pip INT_X55Y117 SS4END0 -> WW4BEG1,
	pip INT_X55Y121 LOGIC_OUTS0 -> SS4BEG0,
	pip INT_X55Y83 SS2END1 -> IMUX_B20,
	pip INT_X55Y85 EE4END1 -> SS2BEG1,
	pip INT_X55Y92 WL1END0 -> IMUX_B2,
	pip INT_X55Y93 EE4END0 -> ER1BEG1,
	pip INT_X55Y93 SE4END0 -> EL1BEG_N3,
	pip INT_X55Y93 SW4END0 -> WL1BEG_N3,
	pip INT_X56Y90 SS2END3 -> IMUX_B7,
	pip INT_X56Y92 EL1END3 -> SS2BEG3,
	pip INT_X56Y92 SL1END1 -> WL1BEG0,
	pip INT_X56Y93 ER1END1 -> SE2BEG1,
	pip INT_X56Y93 ER1END1 -> SL1BEG1,
	pip INT_X57Y111 SE4END0 -> LV16,
	pip INT_X57Y92 SE2END1 -> SE4BEG1,
	pip INT_X57Y95 LV0 -> SW4BEG0,
	pip INT_X58Y89 SW2END1 -> IMUX_B20,
	pip INT_X59Y90 SE4END1 -> SW2BEG1,
;
Z.B: Wie komme ich auf SW2END1??
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (52): GOAHEAD_HOME in Pfaden nicht auflösen
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: geschlossen
-------------------------------------------------------------------------------------------------
Im Command-Trace wird für GOAHEAD_HOME durch den tatsächlichen pfad ersetzt.
Das erschwert die Portierbakrkeit auf andere Maschinen.
-------------------------------------------------------------------------------------------------
}

-------------------------------------------------------------------------------------------------
- (53): Wir nutzen Reflection, um bei allen deserialisierten Objekten Null-Pointer zu erkennen.
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------
}


-------------------------------------------------------------------------------------------------
- (53): DRC: In der gerouteten Netzliste wollen wir den Tunnel wiederfinden?
-------------------------------------------------------------------------------------------------
{
-------------------------------------------------------------------------------------------------
- Dirk: offen
-------------------------------------------------------------------------------------------------
- Christian: offen
-------------------------------------------------------------------------------------------------
Wie ist der definiert? Ueber AddAlias? oder uber ein neues Kommando PrintExpectedRouting?
-------------------------------------------------------------------------------------------------
}




