# Publisher
1. Vytvorte jednoduchú ASP.NET Web API aplikáciu.
2. Vytvorte docker-compose.yaml pre kontajner, ktorý bude spúšťať nasledovné images:
• vytvorenú Web API aplikáciu 
• RabbitMQ
• Postgres
3. Aplikácia musí obsahovať nasledovný REST API endpoint:
Method: POST
Route: /Calculation/{key:int}
Body: {"input": decimal}
Body media type: text/json
4. Vstupy vhodným spôsobom validujte.
5. Aplikácia bude obsahovať interný globálny key-value storage na úrovni aplikácie, t.j. nie Redis alebo iné externé 
key-value storage systémy. Využite natívne možnosti platformy .NET. Key-value storage musí byť dostupný iba 
počas behu aplikácie, nie je potrebná žiadna perzistencia.
6. Aplikácia musí spĺňať nasledovné:
• ak key poskytnutý cez API nie je nájdený v globálnom key-value storage tak tento <key; value> vytvorte a 
nastavte value na 2
• ak key už v globálnom storage existuje a je starší ako 15 sekúnd od posledného zápisu, nastavte value na 
hodnotu 2
• v ostatných prípadoch uložte do globálneho storage pre daný key hodnotu vypočítanú nasledovne:
▪ output hodnota je tretia odmocnina z prirodzeného logaritmu hodnoty input zo vstupu podelenej 
hodnotou z globálneho storage
• výsledok výpočtu pošlite na vami vytvorenú queue v RabbitMQ a následne ju vráťte ako response z 
endpointu vo formáte JSON nasledovne:
{
"computed_value": vypočítaná hodnota zapísaná do global storage
"input_value": vložená hodnota input
"previous_value": predošlá hodnota v global storage pred výpočtom
}
7. Vytvorte hosted servicu, ktorá bude na pozadí prijímať správy z RabbitMQ a bude ich vypisovať do konzoly.
Servica sa spustí pri štarte aplikácie.
8. Pre zadaný model vytvorte konfiguráciu pre EF Core 7 nasledovne:
• použite fluent konfiguráciu, nie atribúty
• vzťahy medzi entitami nakonfigurujte explicitne
• vytvorte DbContext, ktorý bude vytvorené konfigurácie aplikovať; s vhodným providerom ho v 
Program.cs zaregistrujte
• pre vytvorenú konfiguráciu vytvorte migráciu, ktorá zmeny premietne do DB; migrácia sa má 
aplikovať automaticky po spustení aplikácie
• samotná funkcionalita (CRUD) nad DB modelom nie je vyžadovaná
