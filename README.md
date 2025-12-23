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
