# 💰 MoneyApp — Groupe MONEY

MoneyApp est une application **Blazor Server** développée par le **groupe MONEY**.  
Elle a pour objectif de **générer et visualiser des écritures comptables** à partir
de flux JSON fournis par d’autres groupes du projet ERP.

---

## 🧩 Rôles des groupes

### 🔹 Groupe 2 — BACK (Facturation)
Le groupe 2 fournit les **factures clients**.

➡️ Fichier attendu : `invoices.json`

---

### 🔹 Groupe 4 — BANK (Paiements)
Le groupe 4 fournit les **paiements bancaires**.

➡️ Fichier attendu : `payments.json`

---

### 🔹 Groupe MONEY (ce projet)
- Génère les **écritures comptables**
- Affiche les données sous forme de **tables**
- Gère les **cas d’anomalies de paiement**
- Ne modifie **jamais** les données reçues

---

## 📁 Formats de fichiers attendus

### 1 `invoices.json` — (fourni par le groupe 2)

```json
[
  {"billingDate" : "2026-06-29",
   "invoiceRef" : "Facture1",
   "userId" : "C001",
   "firstName" : "Tintin",
   "lastName" : "Toto"
   "amountInclVat" : 7.50
  },
  {"billingDate" : "2026-06-30",
   "invoiceRef" : "Facture2",
   "userId" : "C001",
   "firstName" : "Titi",
   "lastName" : "Castafiore"
   "amountInclVat" : 7.50
  }
]
```

### 2 `payments.json` — (fourni par le groupe 4)

```json
[
  {"executionDate" : "2026-07-01",
   "invoiceId" : "F001",
   "amount" : 7.50,
   "paymentMethod" : SEPA
  },
  {
  "executionDate" : "2026-07-01",
   "invoiceId" : "F002",
   "amount" : 7.50,
   "paymentMethod" : CARD
  }
]
```

## ⚙️ Fonctionnalités

- Connexion simulée
- Consultation comptable
- Écritures comptables générales
- Écritures comptables auxiliaires
- Relevé des opérations bancaires
- Gestion des anomalies de paiement
- Filtres par client, facture et date


##  ▶️ Lancer le projet
Prérequis

- .NET SDK 8.0 ou supérieur

---

```bash
dotnet restore
dotnet build
dotnet run
```

Accès à l’application

Une fois lancée, l’application est accessible sur :

- http://localhost:5293

- https://localhost:7114

### Docker

```bash
docker build -t moneyapp .
docker run --rm -p 8080:8080 moneyapp
```

Accès via Docker

- http://localhost:8080


## 📌 Remarques importantes

Les données sont chargées depuis des fichiers JSON

Aucune base de données n’est utilisée

La TVA est fixe et simulée

Le projet est prêt à être connecté à une API centrale (CONNECT)
