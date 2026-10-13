# Αρχικό Frontend setup με Angular και DevExtreme

## Στόχος

Να προστεθεί στο repository ένα νέο Angular frontend με DevExtreme, περιορισμένο
αυστηρά στο αρχικό scaffolding και στη βασική ρύθμιση του theme. Δεν θα
υλοποιηθούν οθόνες, routing flows ή συνδέσεις με το backend σε αυτό το task.

## Προτεινόμενα βήματα

1. Από το root του repository, δημιούργησε το Angular project με την εντολή:
   `npx @angular/cli new Aviation.Spa --routing --style=scss --skip-git --defaults`.
   Το project πρέπει να δημιουργηθεί στον φάκελο `Aviation.Spa` και να
   χρησιμοποιεί SCSS και Angular routing.
2. Από τον νέο φάκελο `Aviation.Spa`, εγκατέστησε αποκλειστικά ως εξαρτήσεις
   του frontend τα `devextreme` και `devextreme-angular`. Επιβεβαίωσε ότι τα
   σχετικά package manifests και lock file βρίσκονται μέσα στο
   `Aviation.Spa` και ότι δεν τροποποιούνται τα manifests του root ή του API.
3. Ρύθμισε το DevExtreme theme στο `Aviation.Spa/angular.json`, προσθέτοντας
   στο array `styles` του build target το CSS theme
   `node_modules/devextreme/dist/css/dx.material.blue.light.css` (ή άλλο
   αντίστοιχο διαθέσιμο theme, αν απαιτείται από την εγκατεστημένη έκδοση).
   Διατήρησε τα υπάρχοντα styles που δημιουργεί το Angular CLI.
4. Ενημέρωσε το `README.md` με ενότητα **Frontend** που εξηγεί πώς
   εγκαθίστανται και εκκινούνται οι εξαρτήσεις:

   ```bash
   cd Aviation.Spa
   npm install
   npm start
   ```

   Πρόσθεσε τυχόν απαραίτητη προαπαίτηση Node.js/npm, εφόσον δεν καλύπτεται
   ήδη από τις οδηγίες του README.
5. Ενημέρωσε το `CHANGELOG.md` με νέα καταχώριση `[0.10.0]` και ημερομηνία
   `2026-10-13`, εξηγώντας ότι προστέθηκε το αρχικό Angular/DevExtreme
   scaffolding, το theme και οι οδηγίες εκκίνησης. Αύξησε το `VERSION` από
   `0.9.0` σε `0.10.0` ως MINOR bump για τη νέα δυνατότητα frontend.
6. Επαλήθευσε ότι:
   - το Angular CLI project έχει δημιουργηθεί μέσα στο `Aviation.Spa`,
   - τα `devextreme` και `devextreme-angular` είναι δηλωμένα και
     εγκατεστημένα μόνο στο frontend,
   - το CSS theme υπάρχει στην εγκατεστημένη έκδοση και είναι καταχωρισμένο
     στο σωστό `styles` array του `angular.json`,
   - οι Angular build και start εντολές ολοκληρώνονται επιτυχώς,
   - το README περιλαμβάνει τις οδηγίες frontend και τα `CHANGELOG.md` και
     `VERSION` αναφέρουν την έκδοση `0.10.0`.

## Πεδίο εφαρμογής και παραδοχές

- Η αλλαγή αφορά αποκλειστικά το αρχικό frontend setup. Δεν προστίθενται
  σελίδες εφαρμογής, API clients, authentication ή επιχειρησιακή
  λειτουργικότητα.
- Το Angular project έχει δικό του `package.json`, lock file και
  `node_modules` κάτω από το `Aviation.Spa`. Δεν προστίθενται εξαρτήσεις
  Node.js στο root του repository.
- Η εκτέλεση του `npx` μπορεί να χρειαστεί πρόσβαση στο npm registry για
  λήψη Angular CLI και πακέτων· η πραγματική έκδοση των πακέτων θα καθοριστεί
  από τις συμβατές τρέχουσες εκδόσεις κατά την υλοποίηση.
- Η υλοποίηση και τα checks θα ακολουθήσουν τις ρυθμίσεις που παράγει το
  Angular CLI, χωρίς πρόσθετες αλλαγές στο backend.
