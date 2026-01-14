CREATE DATABASE IF NOT EXISTS moneyapp
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_unicode_ci;

USE moneyapp;

CREATE TABLE IF NOT EXISTS invoices (
  id INT AUTO_INCREMENT PRIMARY KEY,
  facture_date DATE NOT NULL,
  ref_facture VARCHAR(64) NOT NULL,
  client_id VARCHAR(32) NOT NULL,
  client_nom VARCHAR(128) NOT NULL,
  facture_montant DECIMAL(10,2) NOT NULL,
  UNIQUE KEY uq_invoices_ref_facture (ref_facture)
);

CREATE TABLE IF NOT EXISTS payments (
  id INT AUTO_INCREMENT PRIMARY KEY,
  paiement_date DATE NOT NULL,
  facture_ref VARCHAR(64) NOT NULL,
  facture_montant DECIMAL(10,2) NOT NULL,
  moyen_paiement VARCHAR(64) NOT NULL,
  KEY idx_payments_facture_ref (facture_ref)
);

INSERT INTO invoices (facture_date, ref_facture, client_id, client_nom, facture_montant)
VALUES
  ('2026-06-30', 'Facture1', 'C001', 'Tintin', 7.50),
  ('2026-06-30', 'Facture2', 'C002', 'Castafiore', 7.50);

INSERT INTO payments (paiement_date, facture_ref, facture_montant, moyen_paiement)
VALUES
  ('2026-07-01', 'Facture1', 7.50, 'PRELEVEMENT'),
  ('2026-07-01', 'Facture2', 2.00, 'CB');
