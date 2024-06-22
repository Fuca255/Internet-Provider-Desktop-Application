# Tim 02

Ovaj projekat je namenjen Internet i Tv provajderima.<br>
Da bi se aplikacija koristila potrebno je da se u konfiguracioni fajl(config.txt) unesu podaci i to:<br>
	&nbsp;- U prvoj liniji fajla je potrbno uneti naziv provajdera koji koristi sistem<br>
	&nbsp;- U drugoj liniji fajla je potrebno uneti konekcioni string do baze podataka koju ce provajder koristi<br>
<br>

**NOTE: Aplikacija ima podrsku za Sqlite i MySql baze podataka.**<br>
&nbsp;	  Potrebno je instalirati sledece pakete kako bi podrska za navedene baze bila moguca**.<br>
		&nbsp;Paketi:<br>
		&ensp;- System.Data.SqLite<br>
		&ensp;- MySql.Data<br>

Aplikacija se sastoji iz intuitivnog korisnickog interfejsa i baze podataka,logera.<br>
Graficki interfejs funkcionalnosti:<br>
	&nbsp;- Dodavanje novih Internet, Tv i Kombinovanih paketa<br>
	&nbsp;- Registrovanje novih korisnika usluga paketa<br>
	&nbsp;- Aktiviranje/Deaktiviranje paketa korsnicima usluga<br>
	&nbsp;- Brisanje paketa<br>
	&nbsp;- Brisanje postojecih korisnika usluga<br>
	&nbsp;- Prikaz svih postojecih paketa po kategorijama (Tv Paket, Internet Paket, Kombinovani Paket)<br>
	&nbsp;- Pretraga korisnika usluge po username-u<br>
	&nbsp;- Prikaz ugovora (aktivnih paketa) za izabranog korisnika usluga. Prikazani su po kategorijama paketa i posebno po broju ugovora.<br>
Graficki interfejs prikaz:<br>
	&nbsp;- Prikaz svih registorvanih korisnika usluga<br>
	&nbsp;- Prikaz svih postojecih paketa<br>
	&nbsp;- Prikaz ugovora (aktivnih paketa) za izabranog korisnika usluga. Prikazani su po kategorijama paketa i posebno po broju ugovora.<br>
	&nbsp;- Prikaz detalja selektovanog paketa<br>
	&nbsp;- Prikaz detalja ugovora (aktiviranog paketa) za selektovanog korisnika usluga<br>
	&nbsp;- Prikaz podataka korisnika usluga.<br>


Korisceni paterni:<br>
	&nbsp;- FactoryMethod<br>
	&nbsp;- Singleton<br>
	&nbsp;- Builder<br>
	&nbsp;- Facade<br>
	&nbsp;- Proxy<br>
	&nbsp;- Observer<br>
	&nbsp;- TemplateMethod<br>

# Kreacioni patterni
1. **Singleton** - ovaj pattern omogucava kreiranje tacno jedne instance neke klase.To se postize tako sto klasa sadrzi jedan private static field koji je tipa **Database**,
takodje sadrzi metodu preko koje se dobija ta instanca u slucaju da je instancirana i ima privatan konstruktor. U projektu taj mehanizam je upotrebljen za klasu koja predstavlja bazu.

2. **Factory method** - ovaj patern nam sluzi za obezbedjivanje postojanja neke vrste podrske koja je neophodna za konkrektni objekat.
Na primer u nasem projektu factory method je koriscen za obezbedjivanje podrske za konekciju na vise tipova baza. Database klasa u sebi
sadrzi field koji je tipa **IConnection**, a **IConnection** je interfejs koji podrzava odredjene vrste konekcija na zeljenu bazu (u nasem slucaju sqlite i mysql) i uzavisnosti od konekcionog
stringa koji se procita iz config fajla, klasa **ConnectionProviderFactory** ce napraviti konekciju koja odgovara prosledjenom konekcionom stringu.

3. **Builder** - ovaj pattern smo koristili za kreiranje paketa. Pri kreiranju Buildera kreira se jedan prazan paket (_Packet.cs_), koji nema setovan ni jedan properti. Builder sadrzi metode koje omogucavaju setovanje propertija paketa po potrebi i na osnovu setovanih vrednosti on moze da odredi kog je tipa neki paket (tv,internet,kombinovani).  Ovo nam je omogucilo da nemamo dodatno izvedene klase za svaku vrstu paketa vec sve mozemo da ih predstavljamo pomocu jedne klase.
Packet.cs:<br>
	&nbsp;- public enum PacketType<br>
	&nbsp;- public int ID<br>
	&nbsp;- public string Name<br>
	&nbsp;- public double Price<br>
	&nbsp;- public string Description<br>
	&nbsp;- public int NumOfChannels<br>
	&nbsp;- public int InternetSpeed<br>
	&nbsp;- public int InternetPacketID<br>
	&nbsp;- public int TvPacketID<br>
	&nbsp;- public PacketType Type.<br>
Svi seteri su postavljeni na internal kako bi vrednosti mogao da setuje samo Builder.<br>

# Strukturni patterni

1. **Proxy** - ovaj pattern je posrednik izmedju korisnika i ciljnog objekta, ima ulogu izvrsavanja nekih operacija pre ili posle izvrsavanja nekog specificnog zahteva. Time se omogucava kontrolu pristupa i manipulaciju objektima. U nasem projektu ovaj pattern je realizovan u fajlovima IOperations.cs, OperationsDB.cs i ProxyOperations.cs.
Interfejs IOperations u sebi sadrzi deklaracije specificnih zahteva koji treba da se izvrse nad bazom (dodavanje ugovora, brisanje korisnika i brisanje paketa) i njega implementiraju klase OperationsDB i ProxyOperations. <br>
U nasem projektu Proxy(ProxyOperations.cs) je vrsio provere:
	&nbsp;- da li postoji aktivan ugovor za odredjenog korisnika usluga,<br>
	&nbsp;- da li postoji aktivan ugovor za odredjeni paket<br>
	&nbsp;- da li je paket (pojedinacni) sastavni deo kombinovanog paketa<br>
	&nbsp;- da li postoji aktivan ugovor za odredjenog korisnika usluga sa tim paketom,<br>
i u zavisnosti od rezultata provere nad podacima, specificni zahtev bi se izvrsio. Zahteve koje je izvrsava implementirani su u OperationDB.cs, kao i instanca baze nad kojom zahtevi treba da budu izvrseni.


2. **Facade** - Facade patern smo koristili iz visi razloga:
	&nbsp;- sakrivanje slozenosti sistema koji upravlja logikom aplikacije,
	&nbsp;- Lakše održavanje i promene sistema zbog struktuiranog koda. Kroz jednostavan poziv metoda iz Facade paterna, mogu se procitati i
	upisati svi neophodni podaci u bazu podataka.
	&nbsp;- Smanjenje zavisnosti između klijenta i sistema: Fasada obezbeđuje jednostavan interfejs klijentima, čime se smanjuje zavisnost između 		klijenta i kompleksnih sistema.
U nasoj aplikaciji klasa **DatabaseCommands.cs** implementira Facade. Takodje kroz njega se primenjuje logika patterna Builder i Proxy.


# Patterni ponasanja

1. **Observer** - Ovaj pattern se korsti sa komunikaciju medju objektima. Osnovna ideja jeste da postoji jedan objekat,tzv. "subjekat" koji u sebi ima listu zainteresovanih objekata,tj. "Observera". Kada se stanje subjekta promeni dolazi do automatskog obavestenja ostalih objekata, a koji su predstavljeni u toj listi. Kod ove aplikacije observer je iskljucivo upotrebljen za funkcionalnost loggera, naime realizovan je:
	&nbsp; - interfejsom **IObserver**
	&nbsp; - klasi **EventLogger** 
	&nbsp; - klasi **FileLogger** koje implementiraju pomenuti interfejs.
**EventLogger** ovde igra ulogu tzv. "subjekta" koji vrsi logovanje akcija, a **FileLogger** u ovom slucaju jeste tzv. "observer", iliti posmatrac koji dobijene funckije dalje salje u txt fajl u kom se taj dogadjaj upisuje. Naime kod ovog programa postoje dva log-a:
	&nbsp; - jedan koji vodi racuna o upitima prosledjenim bazi u vidu "SQL" koda,
	&nbsp; - drugi koji vodi racuna o korisnickim akcijama,poput dodavanja/brisanja paketa i korisnika,zakljucenih ugovora itd. i to u vidu prirodnog jezika (srpskog). 

Loggeri su postavljeni na 2 mesta u programu,u **Database** klasi gde vodi belezi **SQL** upite i to u txt fajl pod nazivom _database_logs.txt_ i u Formi, gde se beleze korisnicki logovi u fajl _user_logs.txt_, oba ova fajla se nalaze u folderu "**Logs**", a koji se nalazi u izvrsnom folderu aplikacije.

2. **TemplateMethod** -  je pattern koji omogucava da se u osnovnoj klasi definise opsti okvir algoritma, a da se specificnosti algoritma implementiraju u podklasama. Time se ne narusava zajednicki deo algoritma pa se podstice na flesibilnost i prosirivost samog koga.
U nasoj bibloteci smo implementirali na sledeci nacin:
	 &nbsp; - **_public abstract class PaketDetaljiTemplate_**, implementira Template metode koje postavljaju osnovne okvire algoritama, i abstraktnu metode koje se implemetiraju u podklasama.<br>
	 		&nbsp; -  public string[] ispisDetalji(int brUgovora, Packet paket)<br>
			&nbsp; -  protected abstract string ispisSpecificniPodaci(Packet paket)<br>
			
Template Method je pojednostavio algoritam za ispis detalja paketa i ugovora. Nakon pazljive analize slucaja, utvrdili smo da bi kompleksnost koda i testiranja za svaki tip paketa bila velika, što bi otezalo buduce prosirenje. Upotreba Template Method-a omogucava nam da jednostavno ispisujemo opste podatke i specifisne informacije u istim labelama, nezavisno od vrste paketa. Konkretno u Formi na tab1 i tab2.
			
