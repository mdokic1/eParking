# Grupa5-eParking

## Članovi tima:
  
1. Faris Demir
2. Amar Bešlagić
3. Milica Đokić
  
## Opis teme

eParking je aplikacija koja omogućava korisniku pronalaženje najbližeg slobodnog parking mjesta korištenjem Google mape, kao i prikazivanje rute do tog parking mjesta.  Osim toga omogućena je i online registracija korisnika, koju odobrava vlasnik. Registracijom korisnici stiču članstvo i rezervisano parking mjesto. Prilikom ulaska korisnika na parking pokreće se timer koji se zaustavlja kada korisnik napušta parking, i na osnovu toga se formira račun. Omogućeno je i unošenje novih parkinga u sistem.

## Procesi

### Login
Korisnik unosi svoje pristupne podatke ili bira opciju Gost. Nakon toga se otvara novi prozor sa mogućim akcijama koje su mu na raspolaganju unutar aplikacije.

### Registracija
Korisnik na formu za registraciju unosi svoje podatke, podatke o vozilu i parkingu. Nakon predavanja forme, vlasniku se šalje zahtjev za registraciju, koju on potom može odbiti ili prihvatiti te obavijestiti korisnika o odluci putem email-a. 

### Plaćanje
Korisnik bira da li želi platiti mjesečnu ili godišnju članarinu ili želi da se račun formira na osnovu vremena provedenog na parkingu. Također ima opciju da bira između tri opcije plaćanja: gotovina, kartica i PayPal. 

### Registracija novih parkinga
Administrator registruje nove parkinge i unosi informacije o njima (lokacija parkinga, kapacitet parkinga, cijena članarine, cijena po satu).

### Parkiranje
Korisnik preko Google mape pronalazi najbliže slobodno parking mjesto. Pri ulasku na parking se pokreće timer. Nakon što korisnik napusti parking mjesto, timer se zaustavlja, i na osnovu provedenog vremena na parkingu  formira se račun, i korisnik, ukoliko nema aktivnu članarinu, pristupa plaćanju.

## Funkcionalnosti

* Mogućnost online registracije
* Mogućnost pronalaženja najbližeg slobodnog parking mjesta preko Google mape, kao i računanje rute do tog parking mjesta
* Mogućnost dobijanja informacija o svim parkinzima na mapi (lokacija, broj slobodnih mjesta)
* Mogućnost registracije novih parkinga
* Pregled podataka i akcija unutar korisničkog računa 
* Mogućnost plaćanja gotovinom, karticom ili PayPal-om

## Akteri

1. Korisnik - vlasnik prevoznog sredstva koji želi da koristi eParking. Podjela korisnika:
* registrovani korisnici(članovi) – korisnici koji imaju plaćenu mjesečnu ili godišnju članarinu i imaju rezervisano parking mjesto na izabranom parkingu
* gosti – korisnici koji nisu registrovani i koji vrše uplatu svaki put kada dođu na parking
2. Vlasnik – nadgleda rad eParkinga, odobrava ili odbija zahtjeve za registraciju novih korisnika
3. Administrator – registruje nove parkinge i briše neke već postojeće, mijenja podatke o parkinzima, i po potrebi ažurira i ostale podatke
4. Sistem za autorizaciju kartice – kontroliše korisničke uplate, te vrši njihovu validaciju
5. Sistem za Google Maps – prikazuje mapu sa registrovanim parkinzima, te računa rutu do izabranog parkinga i prikazuje vrijeme potrebno vozilu da stigne do odredišta
6. Sistem za PayPal – kontroliše korisničke uplate, te vrši njihovu validaciju

## Aplikacija eParking

Pri pokretanju aplikacije otvara se login forma, gdje korisnik ima pravo da se prijavi kao registrovani član sa svojim pristupnim podacima, kao administrator, vlasnik ili kao gost. Osim toga, ima mogućnost da, ukoliko nije član a želi to postati, popuni svoje podatke i pošalje zahtjev za registraciju. Nakon login forme, za korisnika koji je registrovani član, otvara se nova forma na kojoj može vidjeti detalje o svojoj članarini (svoje podatke, podatke o rezervisanom parking mjestu), kao i mogućnost da uđe na Google mapu. Korisniku koji je gost nakon login forme otvara se nova forma preko koje ulazi na Google mapu i pronalazi najbliže slobodno parking mjesto. Na glavnoj formi sa mapom, nalazi se opcija za plaćanje članarine (ukoliko nije već plaćena) i izbor plaćanja parkinga po satu.
