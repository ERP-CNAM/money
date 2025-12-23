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

### 1️⃣ `payments.json` — (fourni par le groupe 4)

```json
[
    {
        "paiement_date": "2026-07-01",
        "facture_ref": "Facture1",
        "facture_montant": 7.50,
        "moyen_paiement": "PRELEVEMENT"
    },
    {
        "paiement_date": "2026-07-01",
        "facture_ref": "Facture2",
        "facture_montant": 2.00,
        "moyen_paiement": "CB"
    }
]
```

### 1️⃣ `payments.json` — (fourni par le groupe 4)

```json
[
    {
        "facture_date": "2026-06-30",
        "ref_facture": "Facture1",
        "client_id": "C001",
        "client_nom": "Tintin",
        "facture_montant": 7.50
    },
    {
        "facture_date": "2026-06-30",
        "ref_facture": "Facture2",
        "client_id": "C002",
        "client_nom": "Castafiore",
        "facture_montant": 7.50
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

.NET SDK 8.0 ou supérieur

Commandes
dotnet restore
dotnet build
dotnet run

Accès à l’application

Une fois lancée, l’application est accessible sur :

http://localhost:5293

https://localhost:7114


📌 Remarques importantes

Les données sont chargées depuis des fichiers JSON

Aucune base de données n’est utilisée

La TVA est fixe et simulée

Le projet est prêt à être connecté à une API centrale (CONNECT)